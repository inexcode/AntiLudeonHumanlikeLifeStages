using System.Linq;
using RimWorld;
using Verse;

namespace HumanlikeLifeStages
{
    public class ThoughtWorker_Puberty_Hediff : ThoughtWorker
    {
        
        //protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
        //{   return ThoughtState.ActiveAtStage(puberty(pawn, other) ? 1 : 0);  }

        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (p == null) return false;
            
            if (PawnHelper.isHaveHediff(p, HediffDefOf.LifeStages_Transgendered))
                return ThoughtState.ActiveAtStage(1);

            if (!PawnHelper.isHaveHediff(p, HediffDefOf.LifeStages_Puberty))
                return false;

            return ThoughtState.ActiveAtStage(pubertyFeels(p) ? 1 : 0);
        }


        private bool lastStatus;
        
        private bool pubertyFeels(Pawn pawn)
        {
            if (pawn.IsHashIntervalTick(20000))
            {
                lastStatus = Rand.Bool;
                
                //puberty tick time
                PubertyHelper.applyPubertyDay(pawn,
                    pawn.health.hediffSet.hediffs.First(x => x.def == HediffDefOf.LifeStages_Puberty).Severity);
            }

            return lastStatus;
        }
    }
}