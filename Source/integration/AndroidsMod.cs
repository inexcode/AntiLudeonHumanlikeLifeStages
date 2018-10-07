using Verse;

namespace HumanlikeLifeStages
{
    public static class AndroidsMod
    {
        private static ThingDef ChjDroid = DefDatabase<ThingDef>.GetNamedSilentFail("ChjDroid"),
            ChjBattleDroid= DefDatabase<ThingDef>.GetNamedSilentFail("ChjBattleDroid"),
            ChjAndroid = DefDatabase<ThingDef>.GetNamedSilentFail("ChjAndroid");
        
        
        static bool isDroid(ThingDef def)
        {
            return def == ChjDroid || def == ChjBattleDroid;
        }
        
        
        public static bool isAndroid(ThingDef def)
        {
            return def == ChjAndroid;
        }

        public static bool isRelaventDef(ThingDef currentDef)
        {
            return isAndroid(currentDef) || isDroid(currentDef);
        }
    }
}