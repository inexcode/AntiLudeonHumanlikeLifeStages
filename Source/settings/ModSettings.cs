using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace HumanlikeLifeStages
{
    public class SettingsUIMod : Mod
    {
        public static List<ThingDef> def = null;
        public static int current = 0;
        
        public ModSettings Settings;
        public Page Page = Page.l1;

        public SettingsUIMod(ModContentPack content) : base(content)
        {
            this.Settings = GetSettings<ModSettings>();
            Log.Message("Loaded Settings"+Settings);
            SettingHelper.latest = this.Settings;
        }


        public override string SettingsCategory() => "Humanlike Life Stages!";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            switch (Page)
            {
                case Page.l1:
                    this.L1(inRect);
                    break;
                case Page.aliens:
                    this.aliens(inRect);
                    break;
                default:
                    Page = Page.l1;
                    break;
            }

            this.Settings.Write();
        }
    }

    public class ModSettings : Verse.ModSettings
    {
        public List<RacePubertySetting> racialSettings = new List<RacePubertySetting>();
        
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
            
            Scribe_Collections.Look(ref this.racialSettings, "racialSettings");
        }
    }
}