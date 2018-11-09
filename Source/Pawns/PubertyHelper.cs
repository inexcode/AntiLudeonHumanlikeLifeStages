using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
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

                that.AddAllParts(pawn);
                
                if (intersex)
                {
                    that.AddParts(pawn);
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
                        default:
                            that.AddOtherParts(pawn);
                            break;
                    }
                }
            }
        }

        private static void AddParts(this RacePubertySetting that, Pawn pawn)
        {
            that.AddOtherParts(pawn);
            that.AddMaleParts(pawn);
            that.AddFemaleParts(pawn);
            
        }
        
        private static void AddAllParts(this RacePubertySetting that, Pawn pawn)
        {
            foreach (var config in that.list.Where(x => x.IsAssigned() && x.IsAll()))
            {
                pawn.health.AddHediff(config.Which(), pawn.Where(config.Where()));
            }
        }

        private static void AddOtherParts(this RacePubertySetting that, Pawn pawn)
        {
            foreach (var config in that.list.Where(x => x.genderRoleIndex == BodyPartsByRace.Other))
            {
                pawn.health.AddHediff(config.Which(), pawn.Where(config.Where()));
            }
        }

        private static void AddMaleParts(this RacePubertySetting that, Pawn pawn)
        {
            foreach (var config in that.list.Where(x => x.genderRoleIndex == BodyPartsByRace.Male))
            {
                pawn.health.AddHediff(config.Which(), pawn.Where(config.Where()));
            }
        }
        
        private static void AddFemaleParts(this RacePubertySetting that, Pawn pawn)
        {
            foreach (var config in that.list.Where(x => x.genderRoleIndex == BodyPartsByRace.Female))
            {
                pawn.health.AddHediff(config.Which(), pawn.Where(config.Where()));
            }
        }

        public static BodyPartRecord Where(this Pawn pawn, BodyPartDef where)
        {
            if (where == BodyPartDefOf.Chest)
            {
                return BodyCache.Chest(pawn);
            }
            else if (where == BodyPartDefOf.Groin)
            {
                return BodyCache.Groin(pawn);
            }
            else if (where == BodyPartDefOf.LifeStages_ReproductiveOrgans)
            {
                return BodyCache.LifeStages_ReproductiveOrgans(pawn);
            }
            else
            {
                return BodyHairHelper.WhatPart(pawn);
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
            return First(that.Organs(), y => y == x.def) != null;
        }

        public static HediffDef First(List<HediffDef> organs, Func<HediffDef, bool> func)
        {
            if(organs!=null)
                foreach (var hediffDef in organs)
                {
                    if (func(hediffDef)) return hediffDef;
                }

            return null;
        }

        public static bool hasWomb(this RacePubertySetting that, Hediff x)
        {
            return First(that.Organs(BodyPartsByRace.Female),y => y == x.def) != null;
        }

        public static bool hasTestes(this RacePubertySetting that, Hediff x)
        {
            return First(that.Organs(BodyPartsByRace.Male), y => y == x.def) != null;
        }

        public static bool AnyTestes(this RacePubertySetting that, Pawn pawn)
        {
            return pawn.health.hediffSet.hediffs.Any(that.hasTestes);
        }

        public static bool AnyWomb(this RacePubertySetting that, Pawn pawn)
        {
            return pawn.health.hediffSet.hediffs.Any(that.hasWomb);
        }

        public static List<HediffDef> Organs(this RacePubertySetting that, int? genderRoleIndex = null,
            int? where = null)
        {
            List<HediffDef> list = new List<HediffDef>();
            foreach (var x in that.list)
            {
                if ((genderRoleIndex == null && x.genderRoleIndex != BodyPartsByRace.Off ||
                     genderRoleIndex == x.genderRoleIndex) &&
                    (@where == null || @where == x.@where)) list.Add(x.Which());
            }

            return list;
        }

        public static RacePubertySetting RacePubertySetting(this Pawn pawn) =>
            SettingHelper.latest.GetPubertySettingsFor(pawn.def);
    }
}