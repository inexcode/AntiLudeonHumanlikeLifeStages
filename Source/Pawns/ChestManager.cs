using System.Collections.Generic;
using System.Linq;
using Verse;

namespace HumanlikeLifeStages
{
    public class ChestManager

    {
        public static void intialChest(Pawn pawn)
        {
            if (!HasChestPart(pawn))
            {
                pawn.health.AddHediff(HediffDefOf.LifeStages_NormalChest, BodyCache.Chest(pawn));
            }
        }
 
        private static bool HasChestPart(Pawn pawn)
        {
            return pawn.health.hediffSet.hediffs.Any(x =>
                x.def == HediffDefOf.LifeStages_Breasts
                || x.def == HediffDefOf.LifeStages_Pecs
                || x.def == HediffDefOf.LifeStages_NormalChest);
        }

        public static void pubertyChest(Pawn pawn, float severity)
        {
            intialChest(pawn);

            if (Rand.Value < .1f)
            {
                Log.Message("Chests Click");
                if (pawn.health.hediffSet.hediffs.Any(PubertyHelper.hasTestes) )
                {
                    //more pec
                    MoreChest(pawn, HediffDefOf.LifeStages_Pecs);
                }
                else
                {
                    float boobGrowthChance = 0.05f;
                    if (PubertyHelper.relaventHeDiffs(pawn.health.hediffSet).Any())
                    {
                        //ovaries keep pumping
                        boobGrowthChance = 1f;
                    }

                    if (Rand.Value < boobGrowthChance)
                    {
                        //more boob
                        MoreChest(pawn, HediffDefOf.LifeStages_Breasts);

                    }
                }
            }
        }

        private static void MoreChest(Pawn pawn, HediffDef chestThing)
        {
            if (pawn.health.hediffSet.HasHediff(chestThing)) return;
            
            pawn.health.AddHediff(chestThing, BodyCache.Chest(pawn));

            IEnumerable<Hediff> enumerable = pawn.health.hediffSet.hediffs.Where(x =>
                x.def == HediffDefOf.LifeStages_NormalChest).ToList();
            foreach (var hediff in enumerable)
            {
                pawn.health.RemoveHediff(hediff);
            }
        }
    }
}