using System.Reflection;
using Harmony;
using Verse;

namespace HumanlikeLifeStages.Harmony
{

    [StaticConstructorOnStartup]
    class Main
    {
        static Main()
        {
            var harmony = HarmonyInstance.Create("humanlike.life.stages");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Log.Message("Welcome to Human Life Stages");

            DefGenerator_GenerateImpliedDefs_PreResolve.Postfix();
        }
    }


    

}
