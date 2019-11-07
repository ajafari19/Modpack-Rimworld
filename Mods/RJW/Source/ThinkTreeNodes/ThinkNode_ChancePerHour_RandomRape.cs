//using System;
//using RimWorld;
//using UnityEngine;
//using Verse;
//using Verse.AI;
//
//namespace rjw
//{
//	/// <summary>
//	/// not  used?
//	/// </summary>
//	public class ThinkNode_ChancePerHour_RandomRape : ThinkNode_ChancePerHour
//	{
//		protected override float MtbHours(Pawn pawn)
//		{
//			var base_mtb = xxx.config.comfort_prisoner_rape_mtbh_mul;
//
//			float desire_factor;
//			{
//				var need_sex = pawn.needs.TryGetNeed<Need_Sex>();
//				if (need_sex != null)
//				{
//					if (need_sex.CurLevel <= need_sex.thresh_frustrated())
//						desire_factor = 0.15f;
//					else if (need_sex.CurLevel <= need_sex.thresh_horny())
//						desire_factor = 0.60f;
//					else
//						desire_factor = 1.00f;
//				}
//				else
//					desire_factor = 1.00f;
//			}
//
//			float personality_factor;
//			{
//				personality_factor = 1.0f;
//				if (pawn.story != null)
//				{
//					foreach (var trait in pawn.story.traits.allTraits)
//					{
//						if (trait.def == TraitDefOf.Bloodlust) personality_factor *= 0.25f;
//						else if (trait.def == TraitDefOf.Brawler) personality_factor *= 0.50f;
//						else if (trait.def == TraitDefOf.Psychopath) personality_factor *= 0.50f;
//						else if (trait.def == TraitDefOf.Kind) personality_factor *= 30.00f;
//					}
//				}
//			}
//
//			float fun_factor;
//			{
//				if ((pawn.needs.joy != null) && (xxx.is_bloodlust(pawn)))
//					fun_factor = Mathf.Clamp01(0.50f + pawn.needs.joy.CurLevel);
//				else
//					fun_factor = 1.00f;
//			}
//
//			var gender_factor = (pawn.gender == Gender.Male) ? 1.0f : 3.0f;
//
//			return base_mtb * desire_factor * personality_factor * fun_factor * gender_factor;
//		}
//
//		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
//		{
//			try
//			{
//				return base.TryIssueJobPackage(pawn, jobParams);
//			}
//			catch (NullReferenceException)
//			{
//				return ThinkResult.NoJob; ;
//			}
//		}
//	}
//}