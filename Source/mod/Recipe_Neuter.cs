using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace HumanlikeLifeStages
{
    public class Recipe_Neuter : Recipe_Surgery
    {

        public override IEnumerable<BodyPartRecord> GetPartsToApplyOn( Pawn pawn, RecipeDef recipe )
        {
            return PartsToApplyOn(pawn);
        }

        public static IEnumerable<BodyPartRecord> PartsToApplyOn(Pawn pawn)
        {
            if (!pawn.health.hediffSet.HasHediff(HediffDefOf.LifeStages_Neutered))
                return pawn.ReproductiveOrgans().Where(x => !pawn.health.hediffSet.PartIsMissing(x));

            return new List<BodyPartRecord>();
        }

        public override void ApplyOnPawn( Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill = null)
        {
            if ( billDoer != null )
            {
                if ( CheckSurgeryFail( billDoer, pawn, ingredients, part, bill ) )
                    return;
                TaleRecorder.RecordTale( TaleDefOf.DidSurgery, billDoer, pawn );
            }
            pawn.health.AddHediff( recipe.addsHediff, part, null );

            pawn.gender = Gender.None;
        }
        
        private void ResolvePuberty(Pawn pawn)
        {
            if (pawn?.health?.hediffSet.hediffs == null) return;
            foreach (var hediff in pawn?.health?.hediffSet.hediffs)
            {
                if (HediffDefOf.LifeStages_Transgendered == null ||
                    hediff.def != HediffDefOf.LifeStages_Transgendered) continue;
                
                pawn.health.RemoveHediff(hediff);
            }
        }
    }
}
