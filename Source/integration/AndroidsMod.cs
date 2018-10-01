using Verse;

namespace HumanlikeLifeStages
{
    public static class AndroidsMod
    {
        private static ThingDef ChjDroid = DefDatabase<ThingDef>.GetNamedSilentFail("ChjDroid"),
            ChjAndroid = DefDatabase<ThingDef>.GetNamedSilentFail("ChjAndroid");
        
        
        static bool isDroid(Pawn p)
        {
            return p.def == ChjDroid;
        }
        
        
        static bool isAndroid(Pawn p)
        {
            return p.def == ChjAndroid;
        }

        public static bool isRelavent(Pawn pawn)
        {
            return isDroid(pawn) || isAndroid(pawn);
        }
    }
}