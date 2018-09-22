using RimWorld;
using Verse;

namespace HumanlikeLifeStages
{
    public class Mate
    {
        public static void Mated(Pawn donor, Pawn hasWomb)
        {
            if (ChildrenCrossMod.isChildrenModOn())
            {
                //We gotta side load some fun
                ChildrenCrossMod.Mated(donor, hasWomb);
            }
            else
            {
                DefaultMate(donor, hasWomb);
            }
        }

        public static void DefaultMate(Pawn donor, Pawn womb)
        {
            PawnUtility.Mated(donor, womb);
        }
    }
}