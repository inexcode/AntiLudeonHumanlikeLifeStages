using System.Linq;
using Fluffy_BirdsAndBees;
using RimWorld;
using Verse;

namespace HumanlikeLifeStages
{
    public class ThoughtWorker_Puberty_Hediff : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (p == null) return false;
          
            if (!PawnHelper.isHaveHediff(p, HediffDefOf.LifeStages_Puberty))
                return false;
            
            return PawnHelper.isHaveHediff(p, HediffDefOf.LifeStages_Transgendered) ? ThoughtState.ActiveAtStage(1) : ThoughtState.ActiveAtStage(pubertyFeels(p) ? 1 : 0);
        }


        private bool lastStatus;
        
        private bool pubertyFeels(Pawn pawn)
        {
            if (!pawn.IsHashIntervalTick(20000)) return lastStatus;
                
            lastStatus = Rand.Bool;
                
            //puberty tick time
            PubertyHelper.applyPubertyDay(pawn,
                pawn.health.hediffSet.hediffs.First(x => x.def == HediffDefOf.LifeStages_Puberty).Severity);

            return lastStatus;
        }
    }
}