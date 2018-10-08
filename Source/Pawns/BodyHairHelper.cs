using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace HumanlikeLifeStages
{
    public static class BodyHairHelper
    {
        public static void DecideTooAddHair(this RacePubertySetting pubertySettings, Pawn pawn)
        {
            var forHair = pubertySettings.ChanceForHair(pawn);
            if (Rand.Value < forHair*2)
            {
                Log.Message("Hair please");
                pubertySettings.AddHair(pawn);
            }
        }

        private static float ChanceForHair(this RacePubertySetting pubertySettings, Pawn pawn)
        {
            return pubertySettings.AnyTestes(pawn)
                ? SettingHelper.latest.maleHairGrowthRate
                : SettingHelper.latest.otherHairGrowthRate;
        }

        public static void AddHair(this RacePubertySetting pubertySettings, Pawn pawn)
        {
            IEnumerable<PubertySetting> settings = pubertySettings.list.Where(x => x.IsSecondaryAssigned()
                && (x.IsSecondaryAll() || x.GetSecondaryGender() == pawn.gender));
            //HediffDef bodyHair = PubertyHelper.First(hediffDefs);
            foreach (PubertySetting bodyHair in settings)
            {
                BodyPartRecord whereHair = pawn.Where(bodyHair.Where());
                
                var which = bodyHair.Which();
                
                Log.Message("Adding hair to " + whereHair + " of type " + which);
                
                var hediff = PawnHelper.GetHediff(pawn, which, whereHair, false);
                if (hediff == null)
                {
                    hediff = pawn.health.AddHediff(which, whereHair);
                    hediff.Severity = 0.05f;
                }
                else
                {
                    hediff.Severity = Math.Min(hediff.Severity + 0.1f, 1f);
                }
            }
        }


        public static BodyPartRecord WhatPart(Pawn pawn)
        {
            var validParts = BodyCache.ValidFurryParts(pawn);
            return validParts.OrderByDescending(x => Rand.Value).First();
        }

        public static IEnumerable<BodyPartDef> WhatSkinCanGetHairy(Pawn pawn)
        {
            yield return BodyPartDefOf.Chest;
            yield return RimWorld.BodyPartDefOf.Torso;
            yield return RimWorld.BodyPartDefOf.Body;
            yield return RimWorld.BodyPartDefOf.Arm;
            yield return RimWorld.BodyPartDefOf.Leg;
            yield return RimWorld.BodyPartDefOf.Jaw;
            yield return RimWorld.BodyPartDefOf.Neck;
        }
    }
}