using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace HumanlikeLifeStages
{
    class SettingsUIMod : Mod
    {
        private ModSettings settings;

        public SettingsUIMod(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<ModSettings>();
            SettingHelper.latest = this.settings;

            this.settings?.update();
        }


        public override string SettingsCategory() => "Humanlike Life Stages!";

        public override void DoSettingsWindowContents(Rect inRect)
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
//            
            Widgets.Label(inRect.BottomHalf().BottomHalf().BottomHalf(),
                "That's all, thanks for playing. -Alice.\nSource Code Available at https://github.com/alycecil");

            this.settings.Write();
            this.settings.update();
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
        }

        public void update()
        {
            if (HediffDefOf.LifeStages_Youth != null)
                foreach (HediffCompProperties_SeverityPerDay comp in HediffDefOf.LifeStages_Youth.comps
                    .Select(x => x as HediffCompProperties_SeverityPerDay)
                    .Where(x => x != null)
                )
                {
                    comp.severityPerDay = - 1f / (60f * PubertyOnset);
                }


            var fleshRaces = DefDatabase<ThingDef>
                .AllDefsListForReading
                .Where(t => t?.race?.IsFlesh ?? false); // return __instance.FleshType != FleshTypeDefOf.Mechanoid;

            var fs = new float[] {0, 1.2f, 4, 13, 18, 80};
            foreach (var humanoidRace in fleshRaces.Where(td => td.race.Humanlike))
            {
                for (var index = 0; index < humanoidRace.race.lifeStageAges.Count; index++)
                {
                    var raceLifeStageAge = humanoidRace.race.lifeStageAges[index];

                    var age = raceLifeStageAge.minAge;
                    raceLifeStageAge.minAge = fs[index] / 13f * PubertyOnset;

                    Log.Message(
                        "Setting [" + raceLifeStageAge + "] to have min age of " + raceLifeStageAge.minAge + " from "+age);
                }
            }
        }
    }
}