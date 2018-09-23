using System.Linq;
using Harmony;
using RimWorld;
using Verse;

namespace HumanlikeLifeStages
{
    [HarmonyPatch( typeof( DefGenerator ), "GenerateImpliedDefs_PreResolve" )]
    public static class DefGenerator_GenerateImpliedDefs_PreResolve
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            var fleshRaces = DefDatabase<ThingDef>
                .AllDefsListForReading
                .Where(t => t.race?.IsFlesh ?? false); // return this.FleshType != FleshTypeDefOf.Mechanoid;

            var humanoidRaces = fleshRaces.Where( td => td.race.Humanlike );

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
                    Log.Message("Added body part ["+bodyPartRecord+"] to ["+body+"]");
                }

                //clear cache
                body.ResolveReferences();
            }
        }
    }
}