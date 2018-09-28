using System;
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
                AddHair(pawn, whatPart(pawn));
            }
        }


        private static void AddPubicHair(Pawn pawn)
        {
            BodyPartRecord groin = BodyCache.Groin(pawn);

            //always get public hair first
            var diff = pawn.health.AddHediff(HediffDefOf.LifeStages_PubicHair, groin);
            diff.Severity = .1f;
        }


        private static float chanceForHair(Pawn pawn)
        {
            return PubertyHelper.AnyTestes(pawn) ? SettingHelper.latest.maleHairGrowthRate : SettingHelper.latest.otherHairGrowthRate;
        }

        public static void AddHair(Pawn pawn, BodyPartRecord whereHair)
        {
            if (whereHair == null) return;


            var hediff = GetHediff(pawn, HediffDefOf.LifeStages_BodyHair, whereHair, false);
            if (hediff == null)
            {
                hediff = pawn.health.AddHediff(HediffDefOf.LifeStages_BodyHair, whereHair);
                hediff.Severity = 0.05f;
            }
            else
            {
                hediff.Severity = Math.Min(hediff.Severity + 0.1f, 1f);
            }
        }
        
        


        public static Hediff GetHediff(Pawn pawn, HediffDef def, BodyPartRecord bodyPart, bool mustBeVisible = false)
        {
            var hediffs = pawn.health.hediffSet.hediffs;
            for (int index = 0; index < hediffs.Count; ++index)
            {
                if (hediffs[index].def == def && hediffs[index].Part == bodyPart &&
                    (!mustBeVisible || hediffs[index].Visible))
                    return hediffs[index];
            }

            return null;
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