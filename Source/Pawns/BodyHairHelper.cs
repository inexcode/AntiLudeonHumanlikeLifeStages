using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace HumanlikeLifeStages
{
    public class BodyHairHelper
    {
        
        public static void DecideTooAddHair(Pawn pawn)
        {
            if (!HasPubicHair(pawn, BodyCache.Groin(pawn)))
            {
                if (Rand.Value < chanceForHair(pawn) * 5)
                {
                    AddPubicHair(pawn);
                }
            } else if (Rand.Value < chanceForHair(pawn)) 
            {
                Log.Message("Add Hair!");
                
                AddHair(pawn, whatPart(pawn));
            }
        }


        private static void AddPubicHair(Pawn pawn)
        {
            BodyPartRecord groin = BodyCache.Groin(pawn);

            //always get public hair first
            pawn.health.AddHediff(HediffDefOf.LifeStages_PubicHair, groin);

            Log.Message("Adding pubic hair");
        }


        private static float chanceForHair(Pawn pawn)
        {
            return PubertyHelper.AnyTestes(pawn) ? SettingHelper.latest.maleHairGrowthRate : SettingHelper.latest.otherHairGrowthRate;
        }

        public static void AddHair(Pawn pawn, BodyPartRecord whereHair)
        {
           
            if (whereHair != null)
            {
                pawn.health.AddHediff(HediffDefOf.LifeStages_BodyHair, whereHair);
            }
        }


        private static BodyPartRecord whatPart(Pawn pawn)
        {
            var validParts = BodyCache.ValidFurryParts(pawn);
            return validParts.OrderByDescending(x => Rand.Value).First();
        }

        public static IEnumerable<BodyPartDef> whatCanGetHairy(Pawn pawn)
        {
            yield return BodyPartDefOf.Chest;
            yield return RimWorld.BodyPartDefOf.Torso;
            yield return RimWorld.BodyPartDefOf.Body;
            yield return RimWorld.BodyPartDefOf.Arm;
            yield return RimWorld.BodyPartDefOf.Leg;
            yield return RimWorld.BodyPartDefOf.Jaw;
            yield return RimWorld.BodyPartDefOf.Neck;
        }

        private static bool HasPubicHair(Pawn pawn, BodyPartRecord bodyPartRecord = null)
        {
            return pawn.health.hediffSet.HasHediff(HediffDefOf.LifeStages_PubicHair, bodyPartRecord);
        }
    }
}