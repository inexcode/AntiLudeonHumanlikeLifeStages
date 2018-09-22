using System.Collections.Generic;
using RimWorld;
using Verse;

namespace HumanlikeLifeStages
{
    public class Recipe_SRS : Recipe_Surgery
    {

        public override IEnumerable<BodyPartRecord> GetPartsToApplyOn( Pawn pawn, RecipeDef recipe )
        {
            if (PawnHelper.isHaveHediff(pawn, HediffDefOf.LifeStages_Transgendered) || pawn.gender == Gender.None)
            {
                return pawn.ReproductiveOrgans();
            }

            return new List<BodyPartRecord>();
        }

        public override void ApplyOnPawn( Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            if ( billDoer != null )
            {
                if ( !PawnHelper.isHaveHediff(pawn, HediffDefOf.LifeStages_Youth) && CheckSurgeryFail( billDoer, pawn, ingredients, part, bill ) )
                    return;
                TaleRecorder.RecordTale( TaleDefOf.DidSurgery, billDoer, pawn );
            }

            ResolveTransgender(pawn);
                
            pawn.gender = newGender(pawn);

            resolveSexOrgans(pawn);

        }

        private void resolveSexOrgans(Pawn pawn)
        {
            if (pawn?.health?.hediffSet.hediffs == null) return;
            foreach (var hediff in pawn?.health?.hediffSet.hediffs.ToArray())
            {
                if (HediffDefOf.LifeStages_Testes != null && hediff.def == HediffDefOf.LifeStages_Testes)
                    pawn.health.RemoveHediff(hediff);
                
                
                if (HediffDefOf.LifeStages_Womb != null && hediff.def == HediffDefOf.LifeStages_Womb)
                    pawn.health.RemoveHediff(hediff);
                
                
                if (HediffDefOf.LifeStages_BodyHair != null && hediff.def == HediffDefOf.LifeStages_BodyHair && Rand.Bool)
                    pawn.health.RemoveHediff(hediff);
            }
        }

        private void ResolveTransgender(Pawn pawn)
        {
            if (pawn?.health?.hediffSet.hediffs == null) return;
            foreach (var hediff in pawn?.health?.hediffSet.hediffs.ToArray())
            {
                if (HediffDefOf.LifeStages_Transgendered == null ||
                    hediff.def != HediffDefOf.LifeStages_Transgendered) continue;
                
                pawn.health.RemoveHediff(hediff);
            }
        }

        private Gender newGender(Pawn pawn)
        {
            
            switch (pawn.gender)
            {
                case Gender.None:
                    return  Rand.Bool ? Gender.Male : Gender.Female;
                case Gender.Female:
                    return Gender.Male;
                case Gender.Male:
                    return Gender.Female;
            }
            return Gender.None;
        }
    }
}
