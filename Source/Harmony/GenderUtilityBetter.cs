using System;
using Harmony;
using Verse;

namespace HumanlikeLifeStages
{
    public class BetterGenderUtility
    {
        
        public static Gender WhatGender(Pawn pawn)
        {
            var intersex = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.LifeStages_Infertile_BirthDefect) !=
                           null ||
                           PubertyHelper.AnyTestes(pawn) && PubertyHelper.AnyWomb(pawn);

            if (intersex) return Gender.None;

            var sex = pawn.gender;
            var cis = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.LifeStages_Transgendered) == null;

            if (cis)
                return sex;

            switch (sex)
            {
                case Gender.Male:
                    return Gender.Female;
                case Gender.Female:
                    return Gender.Male;
                default:
                    return Gender.None;
            }
        }
        
        public static string Noun(Pawn pawn)
        {
            return WhatGender(pawn).GetLabel(false);
        }

        public static string Possessive(Pawn pawn)
        {
            return WhatGender(pawn).GetPossessive();
        }

        public static string Objective(Pawn pawn)
        {
            return WhatGender(pawn).GetObjective();
        }
    }

    [HarmonyPatch(typeof(GenText), "ProSubjCap")]
    public static class GenLabelBetterKindDef
    {
        public static bool BestKindLabel(ref string __result, Pawn pawn, bool mustNoteGender, bool mustNoteLifeStage,
            bool plural, int pluralCount)
        {
            return true;
        }
    }

    [HarmonyPatch(typeof(GenText), "ProSubj")]
    [HarmonyPatch(typeof(GenText), "ProSubjCap")]
    public static class GenTextBetterGetGenderLabel
    {
        [HarmonyPrefix]
        public static bool GetGenderLabel(Pawn p, ref string __result)
        {
            if (!PawnHelper.is_human(p)) return true;
            __result = BetterGenderUtility.Noun(p);
            return false;
        }
    }


    [HarmonyPatch(typeof(GenderUtility), "GetGenderLabel")]
    public static class GenderUtilityBetterGetGenderLabel
    {
        [HarmonyPrefix]
        public static bool GetGenderLabel(Pawn pawn, ref string __result)
        {
            if (!PawnHelper.is_human(pawn)) return true;
            __result = BetterGenderUtility.Noun(pawn);
            return false;
        }
    }


    [HarmonyPatch(typeof(GenderUtility), "GetLabel")]
    public static class GenderUtilityBetterLabel
    {
        [HarmonyPrefix]
        public static bool GetLabel(ref Gender gender, bool animal, ref string __result)
        {
            if (animal) return true;
            if (gender == Gender.Male || gender == Gender.Female) return true;
            __result = SettingHelper.latest.thirdGenderName;
            return false;
        }
    }
    
    [HarmonyPatch(typeof(GenderUtility), "GetPossessive")]
    public static class GenderUtilityBetterGetPossessive
    {
        [HarmonyPrefix]
        public static bool GetPossessive(Gender gender, ref string __result)
        {
            if (gender == Gender.Male || gender == Gender.Female) return true;
            __result = SettingHelper.latest.thirdGenderPossessive;
            return false;
        }
    }


    [HarmonyPatch(typeof(GenderUtility), "GetObjective")]
    public static class GenderUtilityBetterGetObjective
    {
        [HarmonyPrefix]
        public static bool GetObjective(Gender gender, ref string __result)
        {
            if (gender == Gender.Male || gender == Gender.Female) return true;
            __result = SettingHelper.latest.thirdGenderObjective;
            return false;
        }
    }

    [HarmonyPatch(typeof(GenderUtility), "GetPronoun")]
    public static class GenderUtilityBetterPronoun
    {
        [HarmonyPrefix]
        public static bool GetLabel(ref Gender gender, ref string __result)
        {
            if (gender == Gender.Male || gender == Gender.Female) return true;
            
            __result = SettingHelper.latest.thirdGenderProNoun;
            return false;
        }
    }

    [HarmonyPatch(typeof(GenText), "PossessiveCap")]
    [HarmonyPatch(typeof(GenText), "Possessive")]
    public static class PossessivePatch
    {
        [HarmonyPrefix]
        public static bool Possessive(Pawn p, ref string __result)
        {
            if (!PawnHelper.is_human(p)) return true;
            __result = BetterGenderUtility.Possessive(p);
            return false;
        }
    }

    [HarmonyPatch(typeof(GenText), "ProObj")]
    [HarmonyPatch(typeof(GenText), "ProObjCap")]
    public static class GenTextBetterGetGenderObj
    {
        [HarmonyPrefix]
        public static bool GetObjective(Pawn p, ref string __result)
        {
            if (!PawnHelper.is_human(p)) return true;
            __result = BetterGenderUtility.Objective(p); 
            return false;
        }
    }
}