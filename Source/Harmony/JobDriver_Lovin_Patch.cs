using System.Collections.Generic;
using System.Linq;
using Harmony;
using RimWorld;
using Verse;
using Verse.AI;

namespace HumanlikeLifeStages
{
    [HarmonyPatch(typeof(JobDriver_Lovin), "MakeNewToils")]
    public static class JobDriver_Lovin_Patch
    {
        
        [HarmonyPostfix]
        static void Postfix(ref IEnumerable<Toil> __result, JobDriver_Lovin __instance )
        {
                   
            __result.Last().AddFinishAction(delegate
            {
                    var mate = (Pawn)(__instance.job.GetTarget(TargetIndex.A));
                    var pawn = __instance.pawn;
                    
                    //check fertility then ensemenate wombs
                    if (!FertilityChecker.is_fertile(pawn)) return;
                    if (!FertilityChecker.is_fertile(mate)) return;
                    //for each womb make pregnant
                    if (FertilityChecker.is_FemaleForBabies(pawn) && Rand.Value < SettingHelper.latest.woohooChildChance)
                    {
                        //(donor , has womb)
                        Mate.Mated(mate, pawn);
                    }

                    if (FertilityChecker.is_FemaleForBabies(mate) && Rand.Value < SettingHelper.latest.woohooChildChance)
                    {
                        //(donor , has womb)
                        Mate.Mated(pawn, mate);
                    }
            });
        }
    }
    
    static class FertilityChecker
    {
        public static readonly PawnCapacityDef Fertility = DefDatabase<PawnCapacityDef>.GetNamedSilentFail("Fertility");

        public static bool is_fertile(Pawn pawn)
        {
            return GetFertility(pawn) > 0.001;
        }

        public static float GetFertility(Pawn pawn)
        {
            float val;
            if (alreadyPregnant(pawn))
            {
                return 0f;
            }
            else if (hasBionicWomb(pawn))
            {
                return 1f;
            }
            else if (Fertility != null && (val = pawn.health.capacities.GetLevel(Fertility)) >= 0)
            {
                return val;
            }

            return pawn.ageTracker.CurLifeStage.reproductive ? 1f : 0f;
        }

        public static bool alreadyPregnant(Pawn pawn)
        {
            return pawn.health.hediffSet.HasHediff(RimWorld.HediffDefOf.Pregnant);
        }

        public static bool is_FemaleForBabies(Pawn pawn)
        {
            return pawn.gender == Gender.Female || pawn.health.hediffSet.hediffs.Any(PubertyHelper.hasWomb) || hasBionicWomb(pawn);
        }

        public static bool hasBionicWomb(Pawn pawn)
        {
            return BionicWomb!= null && pawn.health.hediffSet.HasHediff(BionicWomb);
        }

        public static HediffDef BionicWomb = DefDatabase<HediffDef>.GetNamedSilentFail("BionicWomb");
    }
}