using System.Linq;
using RimWorld;
using Verse;

namespace HumanlikeLifeStages
{
    public class ThoughtWorker_Youth_Apply : ThoughtWorker
    {

        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            
            if (p == null) return false;

            if (p.ageTracker.AgeBiologicalYears > SettingHelper.latest.PubertyOnset+1) return false;
            
            if (PawnHelper.isHaveHediff(p, HediffDefOf.LifeStages_Puberty))
                return false;
            
            if (PawnHelper.isHaveHediff(p, HediffDefOf.LifeStages_Youth))
                return false;

            PawnGenerator_GeneratePawnRelations_Patch.Postfix(p);

            return false;
        }
    }
}