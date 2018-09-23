using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;
using RimWorld;
using UnityEngine;
using Verse;

namespace HumanlikeLifeStages
{
    [HarmonyPatch(typeof(DefGenerator), "GenerateImpliedDefs_PreResolve")]
    public static class DefGenerator_GenerateImpliedDefs_PreResolve
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            var fleshRaces = DefDatabase<ThingDef>
                .AllDefsListForReading
                .Where(t => t.race?.IsFlesh ?? false); // return __instance.FleshType != FleshTypeDefOf.Mechanoid;

            var humanoidRaces = fleshRaces.Where(td => td.race.Humanlike);

            var fleshBodies = humanoidRaces
                .Select(t => t.race.body)
                .Distinct();

            // insert reproductive parts
            foreach (BodyDef body in fleshBodies)
            {
                foreach (var bodyPartRecord in BodyPartDefOf.NewOrgans)
                {
                    // insert body part
                    body.corePart.parts.Add(bodyPartRecord);
                    Log.Message("Added body part [" + bodyPartRecord.def.defName + "] to [" + body.defName + "]");
                }

                //clear cache
                body.ResolveReferences();
            }
        }
    }

    [HarmonyPatch(typeof(PawnGraphicSet), "ResolveAllGraphics")]
    public static class PawnGraphicSet_ResolveAllGraphics
    {
        [HarmonyPrefix]
        public static void Postfix(PawnGraphicSet __instance)
        {
            if (__instance?.pawn?.RaceProps == null
                || !__instance.pawn.RaceProps.Humanlike ||
                __instance.nakedGraphic == null) return;

            float scale = __instance?.pawn?.ageTracker?.CurLifeStage?.bodySizeFactor ?? 1f;
            scale *= __instance.pawn.RaceProps.baseBodySize * 1.5f;
            
            var vector2 = new Vector2(scale, scale);
            //Not sure iff ill need to do this .Scale(new Vector2(1.5f,1.5f));


            __instance.nakedGraphic = GraphicDatabase.Get<Graphic_Multi>(
                __instance.pawn.story.bodyType.bodyNakedGraphicPath, ShaderDatabase.CutoutSkin, vector2,
                __instance.pawn.story.SkinColor);
            __instance.rottingGraphic = GraphicDatabase.Get<Graphic_Multi>(
                __instance.pawn.story.bodyType.bodyNakedGraphicPath, ShaderDatabase.CutoutSkin, vector2,
                PawnGraphicSet.RottingColor);
            
            __instance.ClearCache();


            Log.Message("Scaling Size of [" + __instance.pawn + "] by [" + scale + "] Updated.");
        }
    }

//  


    // Token: 0x0200002B RID: 43
    [HarmonyPatch(typeof(PawnRenderer), "RenderPawnInternal")]
    [HarmonyPatch(new Type[]
    {
        typeof(Vector3),
        typeof(float),
        typeof(bool),
        typeof(Rot4),
        typeof(Rot4),
        typeof(RotDrawMode),
        typeof(bool),
        typeof(bool)
    })]
    public static class PawnRenderer_RenderPawnInternal_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            FieldInfo  humanlikeBodyInfo = AccessTools.Field(type: typeof(MeshPool), name: nameof(MeshPool.humanlikeBodySet));
            
            int startIndex = -1, endIndex = -1;

            var codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count; i++)
            {
                var codeInstruction = codes[i];
                if (endIndex > 0)
                {
                    break;
                }
                else if (startIndex > 0)
                {
                    if (codeInstruction.opcode == OpCodes.Br)
                    {
                        endIndex = i;
                        break;
                    }
                }
                else if (codeInstruction.opcode == OpCodes.Ldsfld)
                {
                    var value = codeInstruction.operand?.ToString();
                    if (codeInstruction.operand == humanlikeBodyInfo)
                    {
                        startIndex = i; //get that br
                    }
                }
            }

            if (startIndex > 0 && endIndex > 0)
            {
                    codes[endIndex].opcode = OpCodes.Nop;
                    codes[endIndex].operand = null;
                    codes[endIndex].labels = new List<Label>();
                
                Log.Message("Age Matters2 : Op Codes Altered for Child Size");
            }
            else
            {
                Log.Error("AgeMatters2: Unable to alter op codes to render by body size. sorry, going to default.");
            }

            return codes.AsEnumerable();
        }
    }
}