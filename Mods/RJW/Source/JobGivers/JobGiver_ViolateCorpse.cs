using RimWorld;
using Verse;
using Verse.AI;
using System.Collections.Generic;
using System.Linq;

namespace rjw
{
	public class JobGiver_ViolateCorpse : ThinkNode_JobGiver
	{
		public static Corpse find_corpse(Pawn pawn, Map m)
		{
			//Log.Message("JobGiver_ViolateCorpse::find_corpse( " + xxx.get_pawnname(pawn) + " ) called");
			Corpse found = null;
			float best_fuckability = 0.1f;

			IEnumerable<Thing> targets = m.spawnedThings.Where(x 
				=> x is Corpse 
				&& pawn.CanReserveAndReach(x, PathEndMode.OnCell, Danger.Some)
				&& !x.IsForbidden(pawn)
				);

			foreach (Corpse target in targets)
			{
				if (!xxx.can_path_to_target(pawn, target.Position))
					continue;// too far

				// Filter out rotters if not necrophile.
				if (!xxx.is_necrophiliac(pawn) && target.CurRotDrawMode != RotDrawMode.Fresh)
					continue;

				float fuc = xxx.would_fuck(pawn, target, false, false);
				//--Log.Message("   " + xxx.get_pawnname(corpse.Inner) + " =  " + fuc + ",  best =  " + best_fuckability);
				if (!(fuc > best_fuckability)) continue;

				found = target;
				best_fuckability = fuc;
			}
			return found;
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			// Most checks are done in ThinkNode_ConditionalNecro.

			// filter out necro for nymphs
			if (!RJWSettings.necrophilia_enabled) return null;

			if (pawn.Drafted) return null;

			//--Log.Message("[RJW] JobGiver_ViolateCorpse::TryGiveJob for ( " + xxx.get_pawnname(pawn) + " )");
			if (SexUtility.ReadyForLovin(pawn) || xxx.need_some_sex(pawn) > 1f)
			{
				//--Log.Message("[RJW] JobGiver_ViolateCorpse::TryGiveJob, can love ");
				if (!xxx.can_rape(pawn)) return null;

				var target = find_corpse(pawn, pawn.Map);
				//--Log.Message("[RJW] JobGiver_ViolateCorpse::TryGiveJob - target is " + (target == null ? "NULL" : "Found"));
				if (target != null)
				{
					return new Job(xxx.violate_corpse, target);
				}
				// Ticks should only be increased after successful sex.
			}

			return null;
		}
	}
}