using RimWorld;
using System;
using Verse;
using System.Collections.Generic;

namespace rjw
{
    public class Recipe_Abortion : Recipe_RemoveHediff
	{

		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			BodyPartRecord part = pawn.RaceProps.body.corePart;
			if (recipe.appliedOnFixedBodyParts[0] != null)
				part = pawn.RaceProps.body.AllParts.Find(x => x.def == recipe.appliedOnFixedBodyParts[0]);
			if (part != null)
			{
				if (pawn.health.hediffSet.HasHediff(HediffDef.Named("RJW_pregnancy"), true) && recipe.removesHediff == HediffDef.Named("RJW_pregnancy"))
				{
					Hediff_HumanlikePregnancy pregnancy = (Hediff_HumanlikePregnancy)pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("RJW_pregnancy"));
					if (pregnancy.is_checked)
						yield return part;
				}

				else if (pawn.health.hediffSet.HasHediff(HediffDef.Named("RJW_pregnancy_beast"), true) && recipe.removesHediff == HediffDef.Named("RJW_pregnancy_beast"))
				{
					Hediff_BestialPregnancy pregnancy = (Hediff_BestialPregnancy)pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("RJW_pregnancy_beast"));
					if (pregnancy.is_checked)
						yield return part;
				}

				else if (pawn.health.hediffSet.HasHediff(HediffDef.Named("RJW_pregnancy_mech"), true) && recipe.removesHediff == HediffDef.Named("RJW_pregnancy_mech"))
				{
					Hediff_MechanoidPregnancy pregnancy = (Hediff_MechanoidPregnancy)pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("RJW_pregnancy_mech"));
					if (pregnancy.is_checked)
						yield return part;
				}
			}
		}
	}
}
