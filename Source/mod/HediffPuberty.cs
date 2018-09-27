using Verse;

namespace HumanlikeLifeStages
{
    public class HediffPuberty : HediffWithComps
    {
        public override void PostRemoved()
        {
            base.PostRemoved();
            pawn.health.AddHediff(HediffDefOf.LifeStages_Adult, PawnHelper.MaturityPart(pawn));
        }
    }
}