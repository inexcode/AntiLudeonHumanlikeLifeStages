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
            RecipeDef srs = DefDatabase<RecipeDef>.GetNamed("HumanlikeLifeStages_SRS");
            RecipeDef nullo = DefDatabase<RecipeDef>.GetNamed("HumanlikeLifeStages_Neuter");

            RecipeDef WaxMeBaby = DefDatabase<RecipeDef>.GetNamed("WaxMeBaby");
            RecipeDef ShaveMeBaby = DefDatabase<RecipeDef>.GetNamed("ShaveMeBaby");
            RecipeDef SecondPuberty = DefDatabase<RecipeDef>.GetNamed("SecondPuberty");
            RecipeDef PlasticSurgery = DefDatabase<RecipeDef>.GetNamed("PlasticSurgery");
            
            var humanoidRaces = HumanoidRaces();

            var fleshBodies = FleshBodiedRaces(humanoidRaces);

            // insert reproductive parts
            foreach (BodyDef body in fleshBodies)
            {
                foreach (var bodyPartRecord in BodyPartDefOf.NewOrgans)
                {
                    // insert body part
                    body.corePart.parts.Add(bodyPartRecord);
#if DEBUG
                    Log.Message("Added body part [" + bodyPartRecord.def.defName + "] to [" + body.defName + "]");
                    #endif
                }

                //clear cache
                body.AllParts.Clear();
                body.ResolveReferences();
            }

            foreach (var humanoidRace in humanoidRaces)
            {
                humanoidRace.recipes.Add(srs);
                humanoidRace.recipes.Add(nullo);
                humanoidRace.recipes.Add(WaxMeBaby);
                humanoidRace.recipes.Add(ShaveMeBaby);
                humanoidRace.recipes.Add(SecondPuberty);
                humanoidRace.recipes.Add(PlasticSurgery);
                
            }


            SettingHelper.latest.Update();
        }

        private static IEnumerable<BodyDef> FleshBodiedRaces(IEnumerable<ThingDef> humanoidRaces)
        {
            var fleshBodies = humanoidRaces
                .Select(t => t.race.body)
                .Distinct();
            return fleshBodies;
        }

        public static IEnumerable<ThingDef> HumanoidRaces()
        {
            var fleshRaces = DefDatabase<ThingDef>
                .AllDefsListForReading
                .Where(t => t.race?.IsFlesh ?? false); // return __instance.FleshType != FleshTypeDefOf.Mechanoid;

            var humanoidRaces = fleshRaces.Where(td => td.race.Humanlike);
            return humanoidRaces;
        }
    }
}