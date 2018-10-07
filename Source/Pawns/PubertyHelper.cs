using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace HumanlikeLifeStages
{
    public static class PubertyHelper
    {
        public static void applyPubertyDay(Pawn pawn, float severity)
        {
            if (!Recipe_Neuter.PartsToApplyOn(pawn).Any())
            {
                return;
            }

            RacePubertySetting pubertySettings = pawn.RacePubertySetting();

            var sexOrgans = pubertySettings.RelaventHeDiffs(pawn.health.hediffSet);

            if (sexOrgans.Any())
            {
                ChestManager.pubertyChest(pawn, severity, pubertySettings);
                pubertySettings.DecideTooAddHair(pawn);
            }
            else
            {
                pubertySettings.roleOrganMaturity(pawn, severity);
            }
        }

        private static void roleOrganMaturity(this RacePubertySetting that, Pawn pawn, float severity)
        {
            //delay puberty onset
            if (Rand.Value < SettingHelper.latest.EarlyPubertyChance ||
                severity < (1f - SettingHelper.latest.PubertyDelay))
            {
                if (Rand.Value < SettingHelper.latest.IntersexInfertileChance)
                    pawn.health.AddHediff(HediffDefOf.LifeStages_Infertile_BirthDefect, null);

                bool intersex = Rand.Value < SettingHelper.latest.IntersexChance;
                bool cis = Rand.Value > SettingHelper.latest.TransgenderChance;

                if (!cis)
                    pawn.health.AddHediff(HediffDefOf.LifeStages_Transgendered, null);

                if (intersex)
                {
                    that.AddOtherParts(pawn);
                }
                else
                {
                    switch (pawn.gender)
                    {
                        case Gender.Male:
                            
                            that.AddMaleParts(pawn);
                            break;
                        case Gender.Female:
                            that.AddFemaleParts(pawn);
                            break;
                    }
                }
            }
        }

        private static void AddOtherParts(this RacePubertySetting that, Pawn pawn)
        {
            pawn.health.AddHediff(HediffDefOf.LifeStages_Testes, BodyCache.Groin(pawn));
            pawn.health.AddHediff(HediffDefOf.LifeStages_Womb, BodyCache.LifeStages_ReproductiveOrgans(pawn));
        }

        private static void AddFemaleParts(this RacePubertySetting that, Pawn pawn)
        {
            pawn.health.AddHediff(HediffDefOf.LifeStages_Womb, BodyCache.LifeStages_ReproductiveOrgans(pawn));
        }

        private static void AddMaleParts(this RacePubertySetting that, Pawn pawn)
        {
            pawn.health.AddHediff(HediffDefOf.LifeStages_Testes, BodyCache.Groin(pawn));
        }

        public static float isPostPubescence(HediffSet diffSet)
        {
            var heDiffs = diffSet.hediffs.Where(x => x.def == HediffDefOf.LifeStages_Youth);

            if (!heDiffs.Any())
            {
                heDiffs = diffSet.pawn.health.hediffSet.hediffs.Where(x => x.def == HediffDefOf.LifeStages_Youth);
            }

            return !heDiffs.Any() ? 1f : 0f;
        }

        public static float getFactorFertility(this RacePubertySetting that, HediffSet diffSet)
        {
            //look for wombs
            var heDiffs = that.RelaventHeDiffs(diffSet);

            if (!heDiffs.Any()) return 0f;

            return 1f;
        }

        public static IEnumerable<Hediff> RelaventHeDiffs(this RacePubertySetting that, HediffSet diffSet)
        {
            var heDiffs = diffSet.pawn.health.hediffSet.hediffs.Where(x =>
                    that.hasBioOrgan(x) || x.def?.defName?.Contains("Womb") == true);

            return heDiffs;
        }

        public static bool hasBioOrgan(this RacePubertySetting that, Hediff x)
        {
            return that.hasWomb(x) || that.hasTestes(x);
        }

        public static bool hasWomb(this RacePubertySetting that, Hediff x)
        {
            return that.Organs(BodyPartsByRace.Female).First(y => y == x.def) != null;

        }

        public static bool hasTestes(this RacePubertySetting that, Hediff x)
        {
            return that.Organs(BodyPartsByRace.Male).First(y => y == x.def) != null;
        }

        public static bool AnyTestes(this RacePubertySetting that, Pawn pawn)
        {
            return pawn.health.hediffSet.hediffs.Any(that.hasTestes);
        }
        public static bool AnyWomb(this RacePubertySetting that, Pawn pawn)
        {
            return pawn.health.hediffSet.hediffs.Any(that.hasWomb);
        }
        
        public static List<HediffDef> Organs(this RacePubertySetting that, int? genderRoleIndex = null, int? where = null)
        {
            return that.list.Where(
                x => genderRoleIndex == null || genderRoleIndex == x.genderRoleIndex 
                && where == null || where == x.where).Select(x=>x.Which()).ToList();
        }
        
        public static List<HediffDef> Secondary(this RacePubertySetting that, int? genderRoleIndex = null, int? where = null)
        {
            return that.list.Where(x => genderRoleIndex == null || genderRoleIndex == x.secondaryGenderRoleIndex)
                .Where(x => where == null || where == x.where).Select(x=>x.Which()).ToList();
        }
        
        public static RacePubertySetting RacePubertySetting(this Pawn pawn) => SettingHelper.latest.GetPubertySettingsFor(pawn.def);
    }
}