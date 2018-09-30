using System;
using System.Linq;
using RimWorld;
using Verse;

namespace HumanlikeLifeStages
{
    static class PawnHelper
    {
        public static bool is_animal(Pawn pawn)
        {
            return pawn.RaceProps.Animal;
        }

        public static bool is_human(Pawn pawn)
        {
            return pawn.RaceProps.Humanlike; //||pawn.kindDef.race == ThingDefOf.Human
        }

        public static bool isHaveTrait(Pawn pawn, TraitDef what)
        {
            return (pawn?.story?.traits != null && pawn.story.traits.HasTrait(what));
        }

        public static bool isHaveHediff(Pawn pawn, HediffDef what)
        {
            return (pawn?.health?.hediffSet != null && pawn.health.hediffSet.HasHediff(what));
        }

        public static BodyPartRecord MaturityPart(Pawn pawn)
        {
            BodyPartRecord maturityPart = pawn.RaceProps.body.AllParts.Find(b => (b.def == BodyPartDefOf.Maturity));
            return maturityPart;
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

        public static void ResolveTransgender(Pawn pawn)
        {
            if (pawn?.health?.hediffSet.hediffs == null) return;
            foreach (var hediff in pawn?.health?.hediffSet.hediffs.ToArray())
            {
                if (HediffDefOf.LifeStages_Transgendered == null ||
                    hediff.def != HediffDefOf.LifeStages_Transgendered) continue;

                pawn.health.RemoveHediff(hediff);
            }
        }

        public static void ResolvePuberty(Pawn pawn)
        {
            if (pawn?.health?.hediffSet.hediffs == null) return;
            foreach (var hediff in pawn?.health?.hediffSet.hediffs)
            {
                if (HediffDefOf.LifeStages_Transgendered == null ||
                    hediff.def != HediffDefOf.LifeStages_Transgendered) continue;

                pawn.health.RemoveHediff(hediff);
            }
        }
    }
}