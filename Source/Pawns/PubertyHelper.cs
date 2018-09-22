using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace HumanlikeLifeStages
{
    public class PubertyHelper
    {
        public static void applyPubertyDay(Pawn pawn, float severity)
        {
            if (pawn?.gender == Gender.None || pawn?.gender == null ||
                !Recipe_Neuter.PartsToApplyOn(pawn).Any())
            {
                Log.Message("Skipping Puberty Click");
                return;
            }

            var sexOrgans = relaventHeDiffs(pawn.health.hediffSet);

            if (sexOrgans.Any())
            {
                ChestManager.pubertyChest(pawn, severity);
                BodyHairHelper.DecideTooAddHair(pawn);
            }
            else
            {
                roleOrganMaturity(pawn, severity);
            }
        }

        private static void roleOrganMaturity(Pawn pawn, float severity)
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
                    pawn.health.AddHediff(HediffDefOf.LifeStages_Testes, BodyCache.Groin(pawn));
                    pawn.health.AddHediff(HediffDefOf.LifeStages_Womb, BodyCache.ReproductiveOrgans(pawn));
                }
                else
                {
                    switch (pawn.gender)
                    {
                        case Gender.Male:
                            pawn.health.AddHediff(HediffDefOf.LifeStages_Testes, BodyCache.Groin(pawn));
                            break;
                        case Gender.Female:
                            pawn.health.AddHediff(HediffDefOf.LifeStages_Womb, BodyCache.ReproductiveOrgans(pawn));
                            break;
                    }
                }
            }
            else
            {
                Log.Message("Delaying Puberty...");
            }
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

        public static float getFactorFertility(HediffSet diffSet)
        {
            //look for wombs
            var heDiffs = relaventHeDiffs(diffSet);

            if (!heDiffs.Any()) return 0f;

            return 1f;
        }

        public static IEnumerable<Hediff> relaventHeDiffs(HediffSet diffSet)
        {
            var heDiffs = diffSet.hediffs.Where(hasBioOrgan);

            if (!heDiffs.Any())
            {
                heDiffs = diffSet.pawn.health.hediffSet.hediffs.Where(x =>
                    hasBioOrgan(x) || x.def?.defName?.Contains("Womb") == true);
            }

            return heDiffs;
        }

        public static bool hasBioOrgan(Hediff x)
        {
            return x.def == HediffDefOf.LifeStages_Womb || x.def == HediffDefOf.LifeStages_Testes;
        }


        public static bool hasTestes(Hediff x)
        {
            return x.def == HediffDefOf.LifeStages_Testes;
        }

        public static bool AnyTestes(Pawn pawn)
        {
            return pawn.health.hediffSet.hediffs.Any(PubertyHelper.hasTestes);
        }
    }
}