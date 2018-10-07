using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace HumanlikeLifeStages
{
    public static class BodyPartsByRace
    {
        public static void Update(this ModSettings that)
        {
            if (HediffDefOf.LifeStages_Youth != null)
                foreach (HediffCompProperties_SeverityPerDay comp in HediffDefOf.LifeStages_Youth?.comps
                    .Select(x => x as HediffCompProperties_SeverityPerDay)
                    .Where(x => x != null)
                )
                {
                    comp.severityPerDay = -1f / (60f * that.PubertyOnset);
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
                        raceLifeStageAge.minAge = Math.Max(age, fs[index]) / 13 * that.PubertyOnset;
                    }
                }
            }
            catch (Exception)
            {
            }
        }


        /***** MOD INTEGRATION ****/
        public static RacePubertySetting GetPubertySettingsFor(this ModSettings that, ThingDef currentDef)
        {
            //that.racialSettings = new List<RacePubertySetting>();
            RacePubertySetting mine = null;
            if (that.racialSettings.NullOrEmpty())
            {
                Log.Warning("Initial Settings Loaded!");
                that.racialSettings = new List<RacePubertySetting>();
            }
            
            try
            {
                that.racialSettings = that.racialSettings.ToList();
            }
            catch (Exception e)
            {
                Log.Error("Cleared your settings, sorry!\nDev Reason : "+ e);
                that.racialSettings = new List<RacePubertySetting>();
            }
            
            foreach (RacePubertySetting x in that.racialSettings)
            {
                if (x.IsThatDef(currentDef)) {
                    mine = x;
                    break;
                }
            }
            
            if (mine == null)
            {
                Log.Message("Building new setting for " + currentDef.defName);
                mine = BuildPubertySettings(currentDef);
                that.racialSettings.Add(mine);
            }


            return mine;
        }

        private static bool IsThatDef( this RacePubertySetting x, ThingDef currentDef)
        {
            return x!=null && (x.appliedTo?.value?.EqualsIgnoreCase(currentDef.defName) ?? false);
        }

        private static RacePubertySetting BuildPubertySettings(ThingDef currentDef)
        {
            List<PubertySetting> list;
            bool hasPubertyAtBirth = false;
            if (currentDef.defName.EqualsIgnoreCase("Alien_Cactoid")
                || currentDef.defName.EqualsIgnoreCase("Alien_Tree"))
            {
                list = plantDefaults();
            }
            else if (currentDef.defName.EqualsIgnoreCase("Alien_Vulture")
                     || currentDef.defName.EqualsIgnoreCase("Alien_Parrot")
                     || currentDef.defName.EqualsIgnoreCase("Alien_Owl")
                     || currentDef.defName.EqualsIgnoreCase("Alien_Chicken")
                     || currentDef.defName.EqualsIgnoreCase("Alien_Cassowary")
                     || currentDef.defName.EqualsIgnoreCase("Avali")
            )
            {
                list = birdDefaults();
            }
            else if (currentDef.defName.EqualsIgnoreCase("Alien_ElderThing_Race_Standard")
            )
            {
                list = elderDefaults();
            }else if (AndroidsMod.isRelaventDef(currentDef))
            {
                
                hasPubertyAtBirth = true;

                if (AndroidsMod.isAndroid(currentDef))
                {
                    list = humanDefaults();    
                }
                else
                {
                    list = boringDefault();
                }
            }
            else
            {
                list = humanDefaults();
            }

            return new RacePubertySetting(){appliedTo = currentDef.defName, list = list, instantPubertySetting = hasPubertyAtBirth};
        }

        public static readonly int Off = 0, Male = 1, Female = 2, Other = 3, All = 4;
        public static readonly int  Groin = 1, Internal = 2, Chest = 3, Skin = 4;

        public static List<PubertySetting> humanDefaults() => new List<PubertySetting>()
        {
            new PubertySetting(HediffDefOf.LifeStages_Womb, Female, Off, Internal),
            new PubertySetting(HediffDefOf.LifeStages_Testes, Male, Off, Groin),
            new PubertySetting(HediffDefOf.LifeStages_Pecs, Off, Male, Chest),
            new PubertySetting(HediffDefOf.LifeStages_Breasts, Off, Female, Chest),
            new PubertySetting(HediffDefOf.LifeStages_Ovotestis, Off, Off, Off),

            new PubertySetting(HediffDefOf.LifeStages_GenericMale, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_GenericFemale, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_IncectFemale, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_IncectFemaleAlt, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_IncectMale, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_ShrimpMale, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Simple, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_SimplePlant, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_PlantMale, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_PlantFemale, Off, Off, Off),

            new PubertySetting(HediffDefOf.LifeStages_BodyHair, Off, All, Skin),
            new PubertySetting(HediffDefOf.LifeStages_PubicHair, Off, All, Groin),
            new PubertySetting(HediffDefOf.LifeStages_SporingBody, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Goo, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Bark, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Leaves, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_FloweringBody, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Scales, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Plumage, Off, Off, Off),
        };


        public static List<PubertySetting> birdDefaults() => new List<PubertySetting>()
        {
            new PubertySetting(HediffDefOf.LifeStages_Womb, Female, Off, Internal),
            new PubertySetting(HediffDefOf.LifeStages_Testes, Male, Off, Internal),
            new PubertySetting(HediffDefOf.LifeStages_Pecs, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Breasts, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Ovotestis, Other, Off, Internal),

            new PubertySetting(HediffDefOf.LifeStages_GenericMale, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_GenericFemale, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_IncectFemale, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_IncectFemaleAlt, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_IncectMale, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_ShrimpMale, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Simple, All, Off, Groin),
            new PubertySetting(HediffDefOf.LifeStages_SimplePlant, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_PlantMale, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_PlantFemale, Off, Off, Off),

            new PubertySetting(HediffDefOf.LifeStages_BodyHair, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_PubicHair, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_SporingBody, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Goo, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Bark, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Leaves, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_FloweringBody, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Scales, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Plumage, Off, All, Skin),
        };

        public static List<PubertySetting> elderDefaults() => new List<PubertySetting>()
        {
            new PubertySetting(HediffDefOf.LifeStages_Womb, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Testes, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Pecs, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Breasts, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Ovotestis, Off, Off, Off),

            new PubertySetting(HediffDefOf.LifeStages_GenericMale, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_GenericFemale, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_IncectFemale, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_IncectFemaleAlt, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_IncectMale, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_ShrimpMale, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Simple, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_SimplePlant, All, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_PlantMale, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_PlantFemale, Off, Off, Off),

            new PubertySetting(HediffDefOf.LifeStages_BodyHair, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_PubicHair, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_SporingBody, All, Off, Skin),
            new PubertySetting(HediffDefOf.LifeStages_Goo, Off, All, Skin),
            new PubertySetting(HediffDefOf.LifeStages_Bark, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Leaves, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_FloweringBody, Off, All, Off),
            new PubertySetting(HediffDefOf.LifeStages_Scales, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Plumage, Off, Off, Off),
        };


        public static List<PubertySetting> plantDefaults() => new List<PubertySetting>()
        {
            new PubertySetting(HediffDefOf.LifeStages_Womb, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Testes, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Pecs, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Breasts, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Ovotestis, Off, Off, Off),

            new PubertySetting(HediffDefOf.LifeStages_GenericMale, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_GenericFemale, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_IncectFemale, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_IncectFemaleAlt, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_IncectMale, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_ShrimpMale, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Simple, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_SimplePlant, Other, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_PlantMale, Male, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_PlantFemale, Female, Off, Off),

            new PubertySetting(HediffDefOf.LifeStages_BodyHair, Off, Off, Skin),
            new PubertySetting(HediffDefOf.LifeStages_PubicHair, Off, Off, Skin),
            new PubertySetting(HediffDefOf.LifeStages_SporingBody, Off, Off, Skin),
            new PubertySetting(HediffDefOf.LifeStages_Goo, Off, Off, Skin),
            new PubertySetting(HediffDefOf.LifeStages_Bark, Off, All, Skin),
            new PubertySetting(HediffDefOf.LifeStages_Leaves, Off, All, Skin),
            new PubertySetting(HediffDefOf.LifeStages_FloweringBody, Off, All, Skin),
            new PubertySetting(HediffDefOf.LifeStages_Scales, Off, Off, Skin),
            new PubertySetting(HediffDefOf.LifeStages_Plumage, Off, Off, Skin),
        };
        
        public static List<PubertySetting> boringDefault() => new List<PubertySetting>()
        {
            new PubertySetting(HediffDefOf.LifeStages_Womb, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Testes, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Pecs, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Breasts, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Ovotestis, Off, Off, Off),

            new PubertySetting(HediffDefOf.LifeStages_GenericMale, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_GenericFemale, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_IncectFemale, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_IncectFemaleAlt, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_IncectMale, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_ShrimpMale, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Simple, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_SimplePlant, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_PlantMale, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_PlantFemale, Off, Off, Off),

            new PubertySetting(HediffDefOf.LifeStages_BodyHair, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_PubicHair, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_SporingBody, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Goo, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Bark, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Leaves, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_FloweringBody, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Scales, Off, Off, Off),
            new PubertySetting(HediffDefOf.LifeStages_Plumage, Off, Off, Off),
        };
    }
}