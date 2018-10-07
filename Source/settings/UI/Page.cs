using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace HumanlikeLifeStages
{
    public enum Page
    {
        l1,
        aliens
    }

    public static class PubertySettingHelper
    {
        public static HediffDef Which(this PubertySetting that) =>
            DefDatabase<HediffDef>.GetNamedSilentFail(that.which);
        
    }
    public static class ModSettingRenderer
    {
        public static void aliens(this SettingsUIMod that, Rect inRect)
        {
            if (SettingsUIMod.def == null)
                SettingsUIMod.def = DefGenerator_GenerateImpliedDefs_PreResolve.HumanoidRaces().ToList();

            ThingDef currentDef = SettingsUIMod.def[SettingsUIMod.current];

            var clicked = Widgets.ButtonText(
                inRect.TopHalf().TopHalf().TopHalf().TopHalf().LeftHalf().ContractedBy(4f),
                "Back"
            );

            var previous = Widgets.ButtonText(
                inRect.TopHalf().TopHalf().TopHalf().TopHalf().RightHalf().LeftPart(.1f),
                "<"
            );

            Widgets.Label(
                inRect.TopHalf().TopHalf().TopHalf().TopHalf().RightHalf().ScaledBy(.8f),
                currentDef.label
            );

            var next = Widgets.ButtonText(
                inRect.TopHalf().TopHalf().TopHalf().TopHalf().RightHalf().RightPart(.1f),
                ">"
            );

            var rect = inRect.BottomPart(.85f).ContractedBy(4f);
            
            
            that.RenderOptions(currentDef, rect, previous || next);


            if (previous)
            {
                SettingsUIMod.current--;
            }
            else if (next)
            {
                SettingsUIMod.current++;
            }
            
            SettingsUIMod.current %= SettingsUIMod.def.Count;
            SettingsUIMod.current = Math.Abs(SettingsUIMod.current);

            if (clicked)
            {
                that.Page = Page.l1;
            }
        }

        public static void L1(this SettingsUIMod that, Rect inRect)
        {
            that.Settings.woohooChildChance = Widgets.HorizontalSlider(
                inRect.TopHalf().TopHalf().TopHalf().ContractedBy(4f),
                that.Settings.woohooChildChance, 0f, 1f, true,
                "Risky Lovin Factor " + that.Settings.woohooChildChance * 100f
                , "0%", "100%");

            that.Settings.IntersexChance = Widgets.HorizontalSlider(
                inRect.TopHalf().TopHalf().BottomHalf().LeftHalf().ContractedBy(4f),
                that.Settings.IntersexChance, 0f, 1f, true,
                "Intersex chance : " +
                that.Settings.IntersexChance * 100f + "\n(Functional Ovotestes)",
                "0%", "100%");

            that.Settings.IntersexInfertileChance = Widgets.HorizontalSlider(
                inRect.TopHalf().TopHalf().BottomHalf().RightHalf().ContractedBy(4f),
                that.Settings.IntersexInfertileChance, 0f, 1f, true,
                "Genetic Infertility chance : " +
                that.Settings.IntersexInfertileChance * 100f + "\n(Intersex; AIS, Swyer Syndrome, et. al.)",
                "0%", "100%");

            that.Settings.TransgenderChance = Widgets.HorizontalSlider(
                inRect.TopHalf().BottomHalf().TopHalf().ContractedBy(4f),
                that.Settings.TransgenderChance, 0f, 1f, true,
                "Transgendered chance : " +
                that.Settings.TransgenderChance * 100f,
                "0%", "100%");


            that.Settings.maleHairGrowthRate = Widgets.HorizontalSlider(
                inRect.TopHalf().BottomHalf().BottomHalf().LeftHalf().ContractedBy(4f),
                that.Settings.maleHairGrowthRate, 0f, 1f, true,
                "Testosterone powered hair growth rate : " +
                that.Settings.maleHairGrowthRate * 100f,
                "0%", "100%");

            that.Settings.otherHairGrowthRate = Widgets.HorizontalSlider(
                inRect.TopHalf().BottomHalf().BottomHalf().RightHalf().ContractedBy(4f),
                that.Settings.otherHairGrowthRate, 0f, 1f, true,
                "Hair growth rate for the rest : " +
                that.Settings.otherHairGrowthRate * 100f,
                "0%", "100%");


            that.Settings.EarlyPubertyChance = Widgets.HorizontalSlider(
                inRect.BottomHalf().TopHalf().TopHalf().LeftHalf().ContractedBy(4f),
                that.Settings.EarlyPubertyChance, 0f, .90f, true,
                (that.Settings.PubertyDelay < 0.001
                    ? "No Puberty Delay"
                    : "Early puberty chance " + that.Settings.EarlyPubertyChance * 100)
                , "0%", "90%");

            that.Settings.PubertyDelay = Widgets.HorizontalSlider(
                inRect.BottomHalf().TopHalf().TopHalf().RightHalf().ContractedBy(4f),
                that.Settings.PubertyDelay, 0f, .95f, true,
                "Standard Puberty Delay " + that.Settings.PubertyDelay * 100 +
                "\n(Lower increases hair prevalence on average)"
                , "0%", "100%");

            that.Settings.PubertyOnset = Widgets.HorizontalSlider(inRect.BottomHalf().TopHalf().BottomHalf(),
                that.Settings.PubertyOnset, 1, 18, true,
                "Age of Earliest Puberty " + that.Settings.PubertyOnset + " years."
                , "1", "18");

            that.ThirdGendered(inRect);

            var clicked = Widgets.ButtonText(
                inRect.BottomHalf().BottomHalf().BottomHalf().BottomHalf().LeftHalf(),
                "Aliens Configurations"
            );

            Widgets.Label(inRect.BottomHalf().BottomHalf().BottomHalf().RightHalf(),
                "That's all, thanks for playing. -Alice.\nSource Code Available at https://github.com/alycecil");

            if (clicked)
            {
                that.Page = Page.aliens;
            }
        }

        private static void ThirdGendered(this SettingsUIMod that, Rect inRect)
        {
            var pronounArea = inRect.BottomHalf().BottomHalf().TopHalf();
            var ll = pronounArea.LeftHalf().LeftHalf();
            var lr = pronounArea.LeftHalf().RightHalf();
            var rl = pronounArea.RightHalf().LeftHalf();
            var rr = pronounArea.RightHalf().RightHalf();

            Widgets.TextArea(ll, "Non-Binary Gender", true);
            that.Settings.thirdGenderName = Widgets.TextArea(ll.BottomHalf(), that.Settings.thirdGenderName);

            Widgets.TextArea(lr.TopHalf(), "ProNoun", true);
            that.Settings.thirdGenderProNoun = Widgets.TextArea(lr.BottomHalf(), that.Settings.thirdGenderProNoun);

            Widgets.TextArea(rl.TopHalf(), "Possessive", true);
            that.Settings.thirdGenderPossessive =
                Widgets.TextArea(rl.BottomHalf(), that.Settings.thirdGenderPossessive);

            Widgets.TextArea(rr.TopHalf(), "Objective", true);
            that.Settings.thirdGenderObjective = Widgets.TextArea(rr.BottomHalf(), that.Settings.thirdGenderObjective);
        }
        
        private static void RenderOptions(this SettingsUIMod that, ThingDef currentDef, Rect rect, bool validate)
        {
            if (that.Settings == null)
            {
                throw new Exception("Why doesn't settings exist yet ?");
            }
            var settings = that.Settings.GetPubertySettingsFor(currentDef);
            
            if (settings?.list == null)
            {
                throw new Exception("["+currentDef.defName+"] Race has no special settings? Alice said we should always get things here. Can you send her this?");
            }
            var listCount = settings.list.Count;
            var splits = Split(rect, listCount+1).ToArray();

            Widgets.CheckboxLabeled(new Rect(rect.x,rect.y,rect.height/5f, rect.height/10f),"Has Prepubescence stage?",ref settings.pubertySetting.value );
            
            
            rect = rect.BottomPart(.9f);

            for (int i = 0; i < listCount; i++)
            {
                var setting = settings.list[i];
                var myRect = splits[i+1];

                setting.WidgetDraw(myRect);
            }
        }

        public static IEnumerable<Rect> Split(Rect rect, int count)
        {
            float offset = rect.height / count;
            for (int i = 0; i <= count; i++)
            {
                yield return new Rect(rect.x, rect.y + (i * offset), rect.width, offset);
            }
        }

    }
}