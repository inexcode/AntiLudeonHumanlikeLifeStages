using System;
using Harmony;
using RimWorld;
using Verse;

//Thank you to https://github.com/alextd/RimWorld-RandomResearch/blob/70e4e618703178188be5c22e0ff4ba18156a2a13/Source/ResearchPal/HideCurrent.cs#L16
namespace HumanlikeLifeStages.BirdsAndBees
{
    [StaticConstructorOnStartup]
    public static class YouthAreNotNeutered
    {
        
        static YouthAreNotNeutered()
        {
            try
            {
                Log.Message("Patching ");
                Patch();
                Log.Message("Patched Birds and Bees, youth not neutered");
            }
            catch (Exception) { }
        }

        public static void Patch()
        {
            HarmonyInstance harmony = HarmonyInstance.Create("humanlike.life.stages.birds.bees");
            var type = typeof(Fluffy_BirdsAndBees.ThoughtWorker_Neutered);
            var methodName = "CurrentStateInternal";
            harmony.Patch(
                AccessTools.Method(type, methodName), 
                null, null,
                new HarmonyMethod(typeof(YouthAreNotNeutered), "Prefix"));
        }


        public static bool Prefix(ThoughtState __result, Pawn p)
        {
            if (!(p.ageTracker.AgeBiologicalYears <= SettingHelper.latest.PubertyOnset + 1)) return true;
            
            __result = false;
            return false;

        }
        
    }
}