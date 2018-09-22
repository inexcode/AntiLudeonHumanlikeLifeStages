using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

//Humanlike
namespace HumanlikeLifeStages
{
    public static class Resources
    {
        public static IEnumerable<BodyPartRecord> ReproductiveOrgans(this Pawn pawn)
        {
            return pawn.RaceProps.body.ReproductiveOrgans();
        }

        public static IEnumerable<BodyPartRecord> ReproductiveOrgans(this BodyDef body)
        {
            return body.AllParts.Where(part => part.def == BodyPartDefOf.ReproductiveOrgans);
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
        public static HediffDef LifeStages_Transgendered;
        public static HediffDef LifeStages_Infertile_BirthDefect;
        public static HediffDef LifeStages_Neutered;

    }

    [DefOf]
    public class BodyPartDefOf
    {
        public static BodyPartDef Groin;
        public static BodyPartDef Chest;
        public static BodyPartDef ReproductiveOrgans;
        public static BodyPartDef Maturity;


        public static IEnumerable<BodyPartRecord> NewOrgans => new List<BodyPartRecord>
        {
            New_ReproductiveOrgans, New_Chest, New_Groin, New_Maturity
        };

        static BodyPartRecord New_ReproductiveOrgans => new BodyPartRecord
        {
            coverage = 0f,
            def = ReproductiveOrgans,
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