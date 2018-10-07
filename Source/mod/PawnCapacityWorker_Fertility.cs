using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace HumanlikeLifeStages
{
    public class PawnCapacityWorker_Fertility : PawnCapacityWorker
    {
        public override float CalculateCapacityLevel( HediffSet diffSet,
            List<PawnCapacityUtility.CapacityImpactor> impactors = null )
        {
            var basis = PawnCapacityUtility.CalculateTagEfficiency(diffSet, 
                BodyPartTagDefOf.FertilitySource, 3.40282347E+38f, default(FloatRange), impactors);

            RacePubertySetting pubertySettings = SettingHelper.latest.GetPubertySettingsFor(diffSet.pawn.def);

            return PubertyHelper.getFactorFertility(pubertySettings,diffSet) * basis * PubertyHelper.isPostPubescence(diffSet);
        }
        
        public override bool CanHaveCapacity( BodyDef body )
        {
            return body.HasPartWithTag(BodyPartTagDefOf.FertilitySource);
        }
    }
}