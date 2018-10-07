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
            var forHair = pubertySettings.chanceForHair(pawn);
            if (!pubertySettings.HasPubicHair(pawn, BodyCache.Groin(pawn)))
            {
                if (Rand.Value < forHair * 5)
                {
                    pubertySettings.AddPubicHair(pawn);
                }
            } else if (Rand.Value < forHair) 
            {
                pubertySettings.AddHair(pawn, whatPart(pawn));
            }
        }


        private static void AddPubicHair(this RacePubertySetting pubertySettings, Pawn pawn)
        {
            BodyPartRecord groin = BodyCache.Groin(pawn);

            //always get public hair first
            var diff = pawn.health.AddHediff(HediffDefOf.LifeStages_PubicHair, groin);
            diff.Severity = .1f;
        }


        private static float chanceForHair(this RacePubertySetting pubertySettings, Pawn pawn)
        {
            return pubertySettings.AnyTestes(pawn) ? SettingHelper.latest.maleHairGrowthRate : SettingHelper.latest.otherHairGrowthRate;
        }

        public static void AddHair(this RacePubertySetting pubertySettings, Pawn pawn, BodyPartRecord whereHair)
        {
            if (whereHair == null) return;


            var hediff = PawnHelper.GetHediff(pawn, HediffDefOf.LifeStages_BodyHair, whereHair, false);
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


        private static BodyPartRecord whatPart(Pawn pawn)
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

        private static bool HasPubicHair(this RacePubertySetting pubertySettings, Pawn pawn, BodyPartRecord bodyPartRecord = null)
        {
            var set = pubertySettings.Secondary(BodyPartsByRace.Groin);
            return pawn.health.hediffSet.HasHediff(HediffDefOf.LifeStages_PubicHair, bodyPartRecord);
        }
    }
}