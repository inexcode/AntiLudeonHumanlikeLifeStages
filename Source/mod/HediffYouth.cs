using Verse;

namespace HumanlikeLifeStages
{
    public class HediffYouth : HediffWithComps
    {
        public override void Tick()
        {
            base.Tick();
            if (!pawn.IsHashIntervalTick(20000)) return;
            pawn?.Drawer?.renderer?.graphics?.ResolveAllGraphics();

            this.Severity = (SettingHelper.latest.PubertyOnset - pawn.ageTracker.AgeBiologicalYearsFloat) /
                            SettingHelper.latest.PubertyOnset;
        }
    }
}