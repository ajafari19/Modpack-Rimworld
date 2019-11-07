using RimWorld;
using System;
using Verse;
using System.Collections.Generic;

namespace rjw
{
    public class Recipe_DeterminePregnancy : RecipeWorker
    {
        public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
        {
			/* Males can be impregnated by mechanoids, probably
			if (!xxx.is_female(pawn))
			{
				yield break;
			}
			*/
			BodyPartRecord part = pawn.RaceProps.body.corePart;
            if (recipe.appliedOnFixedBodyParts[0] != null)
                part = pawn.RaceProps.body.AllParts.Find(x => x.def == recipe.appliedOnFixedBodyParts[0]);
			if (part != null && (pawn.ageTracker.CurLifeStage.reproductive)
				|| pawn.health.hediffSet.HasHediff(HediffDef.Named("RJW_pregnancy"), true)
				|| pawn.health.hediffSet.HasHediff(HediffDef.Named("RJW_pregnancy_beast"), true)
				|| pawn.health.hediffSet.HasHediff(HediffDef.Named("RJW_pregnancy_mech"), true)
				)
			{
				yield return part;
			}
        }

        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            if (pawn.health.hediffSet.HasHediff(HediffDef.Named("RJW_pregnancy")))
            {
                Hediff_HumanlikePregnancy pregnancy = (Hediff_HumanlikePregnancy)pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("RJW_pregnancy"));
				pregnancy.CheckPregnancy();
            }

            else if (pawn.health.hediffSet.HasHediff(HediffDef.Named("RJW_pregnancy_beast")))
            {
                Hediff_BestialPregnancy pregnancy = (Hediff_BestialPregnancy)pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("RJW_pregnancy_beast"));
				pregnancy.CheckPregnancy();
            }
			
            else if (pawn.health.hediffSet.HasHediff(HediffDef.Named("RJW_pregnancy_mech")))
            {
                Hediff_MechanoidPregnancy pregnancy = (Hediff_MechanoidPregnancy)pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("RJW_pregnancy_mech"));
				pregnancy.CheckPregnancy();
            }
           
            else
            {
                Messages.Message(xxx.get_pawnname(billDoer) + " has determined " + xxx.get_pawnname(pawn) + " is not pregnant.", MessageTypeDefOf.NeutralEvent);
            }
        }
    }
}
