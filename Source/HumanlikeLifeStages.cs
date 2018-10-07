using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

//Humanlike
namespace HumanlikeLifeStages
{
    public static class Resources
    {
        public static IEnumerable<BodyPartRecord> LifeStages_ReproductiveOrgans(this Pawn pawn)
        {
            return pawn.RaceProps.body.LifeStages_ReproductiveOrgans();
        }

        public static IEnumerable<BodyPartRecord> LifeStages_ReproductiveOrgans(this BodyDef body)
        {
            return body.AllParts.Where(part => part.def == BodyPartDefOf.LifeStages_ReproductiveOrgans);
        }
    }

    [DefOf]
    public class BodyPartTagDefOf
    {
        public static BodyPartTagDef FertilitySource;
    }

    [DefOf]
    public class HediffDefOf
    {
        public static HediffDef LifeStages_Womb;
        public static HediffDef LifeStages_Testes;

        public static HediffDef LifeStages_PubicHair;
        public static HediffDef LifeStages_BodyHair;
        public static HediffDef LifeStages_NormalChest;
        public static HediffDef LifeStages_Pecs;
        public static HediffDef LifeStages_Breasts;

        public static HediffDef LifeStages_Puberty;
        public static HediffDef LifeStages_Youth;
        public static HediffDef LifeStages_Adult;
        
        public static HediffDef LifeStages_Transgendered;
        public static HediffDef LifeStages_Infertile_BirthDefect;
        public static HediffDef LifeStages_Neutered;

        public static HediffDef LifeStages_Scales;
        public static HediffDef LifeStages_FloweringBody;
        public static HediffDef LifeStages_Leaves;
        public static HediffDef LifeStages_Bark;
        public static HediffDef LifeStages_Goo;
        public static HediffDef LifeStages_SporingBody;
        public static HediffDef LifeStages_Plumage;
        
        public static HediffDef LifeStages_GenericMale;
        public static HediffDef LifeStages_GenericFemale;

        public static HediffDef LifeStages_IncectFemale;
        public static HediffDef LifeStages_IncectFemaleAlt;
        public static HediffDef LifeStages_IncectMale;
        
        public static HediffDef LifeStages_ShrimpMale;
        
        public static HediffDef LifeStages_Simple;
        public static HediffDef LifeStages_Ovotestis;

        public static HediffDef LifeStages_SimplePlant;
        public static HediffDef LifeStages_PlantFemale;
        public static HediffDef LifeStages_PlantMale;
    }

    [DefOf]
    public class BodyPartDefOf
    {
        public static BodyPartDef Groin;
        public static BodyPartDef Chest;
        public static BodyPartDef LifeStages_ReproductiveOrgans;
        public static BodyPartDef Maturity;


        public static IEnumerable<BodyPartRecord> NewOrgans => new List<BodyPartRecord>
        {
            New_LifeStages_ReproductiveOrgans, New_Chest, New_Groin, New_Maturity
        };

        static BodyPartRecord New_LifeStages_ReproductiveOrgans => new BodyPartRecord
        {
            coverage = 0.0001f,
            def = LifeStages_ReproductiveOrgans,
            depth = BodyPartDepth.Undefined,
            groups = new List<BodyPartGroupDef>(new[] {BodyPartGroupDefOf.Torso}),
            height = BodyPartHeight.Middle
        };

        static BodyPartRecord New_Chest => new BodyPartRecord
        {
            coverage = 0.005f,
            def = Chest,
            depth = BodyPartDepth.Undefined,
            groups = new List<BodyPartGroupDef>(new[] {BodyPartGroupDefOf.Torso}),
            height = BodyPartHeight.Middle
        };

        static BodyPartRecord New_Groin => new BodyPartRecord
        {
            coverage = 0.005f,
            def = Groin,
            depth = BodyPartDepth.Outside,
            groups = new List<BodyPartGroupDef>(new[] {BodyPartGroupDefOf.Torso}),
            height = BodyPartHeight.Middle
        };

        static BodyPartRecord New_Maturity => new BodyPartRecord
        {
            coverage = 0f,
            def = Maturity,
            depth = BodyPartDepth.Undefined,
            groups = new List<BodyPartGroupDef>(new[] {BodyPartGroupDefOf.Torso}),
            height = BodyPartHeight.Undefined,
            
        };
    }
}