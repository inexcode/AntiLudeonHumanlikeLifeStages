using Verse;

namespace HumanlikeLifeStages
{
    public class HediffYouth : HediffWithComps
    {
        public override void Tick()
        {
            base.Tick();
            if (!pawn.IsHashIntervalTick(20000) || pawn.Drawer == null) return;
            pawn.Drawer.renderer.graphics.ResolveAllGraphics();
            
        }
    }
}