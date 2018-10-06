using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace HumanlikeLifeStages
{
    class SettingsUIMod : Mod
    {
        enum Page
        {
            l1, 
            aliens
        }
        private ModSettings settings;
        private Page page = Page.l1;

        public SettingsUIMod(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<ModSettings>();
            SettingHelper.latest = this.settings;

            this.settings?.update();
        }


        public override string SettingsCategory() => "Humanlike Life Stages!";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            if (page == Page.l1)
            {
                L1(inRect);
            }else if (page == Page.aliens)
            {
                aliens(inRect);
            }
            else
            {
                page = Page.l1;
            }

            this.settings.Write();
            this.settings.update();
        }


        private ThingDef current = ThingDefOf.Human;
        
        private void aliens(Rect inRect)
        {    
            var clicked = Widgets.ButtonText(
                inRect.TopHalf().TopHalf().TopHalf().TopHalf().LeftHalf(),
                "Back"
            );
            
           
            if (clicked)
            {
                page = Page.l1;
            }
        }

        private void L1(Rect inRect)
        {
            this.settings.woohooChildChance = Widgets.HorizontalSlider(
                inRect.TopHalf().TopHalf().TopHalf().ContractedBy(4f),
                this.settings.woohooChildChance, 0f, 1f, true,
                "Risky Lovin Factor " + this.settings.woohooChildChance * 100f
                , "0%", "100%");

            this.settings.IntersexChance = Widgets.HorizontalSlider(
                inRect.TopHalf().TopHalf().BottomHalf().LeftHalf().ContractedBy(4f),
                this.settings.IntersexChance, 0f, 1f, true,
                "Intersex chance : " +
                this.settings.IntersexChance * 100f + "\n(Functional Ovotestes)",
                "0%", "100%");

            this.settings.IntersexInfertileChance = Widgets.HorizontalSlider(
                inRect.TopHalf().TopHalf().BottomHalf().RightHalf().ContractedBy(4f),
                this.settings.IntersexInfertileChance, 0f, 1f, true,
                "Genetic Infertility chance : " +
                this.settings.IntersexInfertileChance * 100f + "\n(Intersex; AIS, Swyer Syndrome, et. al.)",
                "0%", "100%");

            this.settings.TransgenderChance = Widgets.HorizontalSlider(
                inRect.TopHalf().BottomHalf().TopHalf().ContractedBy(4f),
                this.settings.TransgenderChance, 0f, 1f, true,
                "Transgendered chance : " +
                this.settings.TransgenderChance * 100f,
                "0%", "100%");


            this.settings.maleHairGrowthRate = Widgets.HorizontalSlider(
                inRect.TopHalf().BottomHalf().BottomHalf().LeftHalf().ContractedBy(4f),
                this.settings.maleHairGrowthRate, 0f, 1f, true,
                "Testosterone powered hair growth rate : " +
                this.settings.maleHairGrowthRate * 100f,
                "0%", "100%");

            this.settings.otherHairGrowthRate = Widgets.HorizontalSlider(
                inRect.TopHalf().BottomHalf().BottomHalf().RightHalf().ContractedBy(4f),
                this.settings.otherHairGrowthRate, 0f, 1f, true,
                "Hair growth rate for the rest : " +
                this.settings.otherHairGrowthRate * 100f,
                "0%", "100%");


            this.settings.EarlyPubertyChance = Widgets.HorizontalSlider(
                inRect.BottomHalf().TopHalf().TopHalf().LeftHalf().ContractedBy(4f),
                this.settings.EarlyPubertyChance, 0f, .90f, true,
                (this.settings.PubertyDelay < 0.001
                    ? "No Puberty Delay"
                    : "Early puberty chance " + this.settings.EarlyPubertyChance * 100)
                , "0%", "90%");

            this.settings.PubertyDelay = Widgets.HorizontalSlider(
                inRect.BottomHalf().TopHalf().TopHalf().RightHalf().ContractedBy(4f),
                this.settings.PubertyDelay, 0f, .95f, true,
                "Standard Puberty Delay " + this.settings.PubertyDelay * 100 +
                "\n(Lower increases hair prevalence on average)"
                , "0%", "100%");

            this.settings.PubertyOnset = Widgets.HorizontalSlider(inRect.BottomHalf().TopHalf().BottomHalf(),
                this.settings.PubertyOnset, 1, 18, true,
                "Age of Earliest Puberty " + this.settings.PubertyOnset + " years."
                , "1", "18");

            ThirdGendered(inRect);

            var clicked = Widgets.ButtonText(
                inRect.BottomHalf().BottomHalf().BottomHalf().BottomHalf().LeftHalf(),
                "Aliens Configurations"
            );

            Widgets.Label(inRect.BottomHalf().BottomHalf().BottomHalf().RightHalf(),
                "That's all, thanks for playing. -Alice.\nSource Code Available at https://github.com/alycecil");

            if (clicked)
            {
                page = Page.aliens;
            }
        }

        private void ThirdGendered(Rect inRect)
        {
            var pronounArea = inRect.BottomHalf().BottomHalf().TopHalf();
            var ll = pronounArea.LeftHalf().LeftHalf();
            var lr = pronounArea.LeftHalf().RightHalf();
            var rl = pronounArea.RightHalf().LeftHalf();
            var rr = pronounArea.RightHalf().RightHalf();

            Widgets.TextArea(ll, "Non-Binary Gender", true);
            this.settings.thirdGenderName = Widgets.TextArea(ll.BottomHalf(), this.settings.thirdGenderName);
            
            Widgets.TextArea(lr.TopHalf(), "ProNoun", true);
            this.settings.thirdGenderProNoun = Widgets.TextArea(lr.BottomHalf(), this.settings.thirdGenderProNoun);

            Widgets.TextArea(rl.TopHalf(), "Possessive", true);
            this.settings.thirdGenderPossessive = Widgets.TextArea(rl.BottomHalf(), this.settings.thirdGenderPossessive);

            Widgets.TextArea(rr.TopHalf(), "Objective", true);
            this.settings.thirdGenderObjective = Widgets.TextArea(rr.BottomHalf(), this.settings.thirdGenderObjective);
        }
    }

    public class ModSettings : Verse.ModSettings
    {
        public float woohooChildChance = 1f,
            IntersexChance = 0.05f,
            IntersexInfertileChance = 0.05f,
            TransgenderChance = 0.05f,
            maleHairGrowthRate = 0.2f,
            otherHairGrowthRate = 0.02f,
            EarlyPubertyChance = 0.02f,
            PubertyDelay = 0.75f,
            PubertyOnset = 3f;

        public String thirdGenderName = "gender-queer";
        public String thirdGenderProNoun = "they";
        public String thirdGenderPossessive = "theirs";
        public String thirdGenderObjective = "them";


        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.woohooChildChance, "woohooChildChance", 0.01f);
            Scribe_Values.Look(ref this.IntersexChance, "IntersexChance", 0.05f);
            Scribe_Values.Look(ref this.IntersexInfertileChance, "IntersexInfertileChance", 0.05f);
            Scribe_Values.Look(ref this.TransgenderChance, "TransgenderChance", 0.05f);
            Scribe_Values.Look(ref this.maleHairGrowthRate, "maleHairGrowthRate", 0.2f);
            Scribe_Values.Look(ref this.otherHairGrowthRate, "otherHairGrowthRate", 0.1f);
            Scribe_Values.Look(ref this.EarlyPubertyChance, "EarlyPubertyChance", 0.02f);
            Scribe_Values.Look(ref this.PubertyDelay, "PubertyDelay", 0.75f);
            Scribe_Values.Look(ref this.PubertyOnset, "PubertyOnset", 3f);

            Scribe_Values.Look(ref this.thirdGenderName, "thirdGenderName", "gender-queer");
            Scribe_Values.Look(ref this.thirdGenderProNoun, "thirdGenderProNoun", "they");
            Scribe_Values.Look(ref this.thirdGenderPossessive, "thirdGenderPossessive", "theirs");
            Scribe_Values.Look(ref this.thirdGenderObjective, "thirdGenderObjective", "their");
        }

        public void update()
        {
            if (HediffDefOf.LifeStages_Youth != null)
                foreach (HediffCompProperties_SeverityPerDay comp in HediffDefOf.LifeStages_Youth?.comps
                    .Select(x => x as HediffCompProperties_SeverityPerDay)
                    .Where(x => x != null)
                )
                {
                    comp.severityPerDay = -1f / (60f * PubertyOnset);
                }


            var fleshRaces = DefDatabase<ThingDef>
                .AllDefsListForReading
                .Where(t => (t?.race?.IsFlesh ?? false)
                            && (t?.race?.Humanlike ?? false)
                            && (t?.race?.lifeStageAges?.Any() ??
                                false)); // return __instance.FleshType != FleshTypeDefOf.Mechanoid;

            var fs = new float[] {0, 1.2f, 4, 13, 18, 80, 8000, 8001, 8002};
            try
            {
                foreach (var humanoidRace in fleshRaces)
                {
                    for (var index = 0; index < humanoidRace.race.lifeStageAges.Count && index < fs.Length; index++)
                    {
                        var raceLifeStageAge = humanoidRace.race.lifeStageAges[index];

                        var age = raceLifeStageAge.minAge;
                        raceLifeStageAge.minAge = Math.Max(age,fs[index]) / 13 * PubertyOnset;

                        Log.Message(
                            "Setting [" + humanoidRace + ", step "+index+"] to have min age of " + raceLifeStageAge.minAge +
                            " from " + age);
                    }
                }
            } catch (Exception) { }
        }
    }
}