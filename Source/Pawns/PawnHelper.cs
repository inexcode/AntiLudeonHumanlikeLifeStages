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
    }
}