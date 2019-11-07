using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;
using System.Collections.Generic;

namespace rjw
{
	public class JobGiver_RandomRape : ThinkNode_JobGiver
	{
		public Pawn find_victim(Pawn pawn, Map m)
		{
			Pawn victim = null;

			IEnumerable<Pawn> targets = m.mapPawns.AllPawnsSpawned.Where(x 
				=> x != pawn
				&& xxx.is_not_dying(x)
				&& xxx.can_get_raped(x)
				&& !x.Suspended
				&& !x.IsForbidden(pawn)
				&& pawn.CanReserveAndReach(x, PathEndMode.Touch, Danger.Some, xxx.max_rapists_per_prisoner, 0)
				&& !x.HostileTo(pawn)
				);

			float best_fuckability = 0.10f; // Don't rape pawns with <10% fuckability

			//Animal rape
			if (xxx.is_zoophile(pawn) && RJWSettings.bestiality_enabled)
			{
				foreach (Pawn target in targets.Where(x => xxx.is_animal(x)))
				{
					if (!xxx.can_path_to_target(pawn, target.Position))
						continue;// too far

					float fuc = xxx.would_fuck(pawn, target, true, true);
					if (fuc > best_fuckability)
					{
						best_fuckability = fuc;
						victim = target;
					}
				}

				if (victim != null)
					return victim;
			}

			// Humanlike rape - could be prisoner, colonist, or non-hostile outsider
			foreach (Pawn target in targets.Where(x => !xxx.is_animal(x)))
			{
				if (!xxx.can_path_to_target(pawn, target.Position))
					continue;// too far

				float fuc = xxx.would_fuck(pawn, target, true, true);
				if (fuc > best_fuckability)
				{
					best_fuckability = fuc;
					victim = target;
				}
			}

			return victim;
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			//Log.Message("[RJW] JobGiver_RandomRape::TryGiveJob( " + xxx.get_pawnname(pawn) + " ) called");
			if (xxx.can_rape(pawn))
			{
				Pawn victim = find_victim(pawn, pawn.Map);

				if (victim != null)
				{
					//Log.Message("[RJW] JobGiver_RandomRape::TryGiveJob( " + xxx.get_pawnname(pawn) + " ) - found victim " + xxx.get_pawnname(victim));
					return new Job(xxx.random_rape, victim);
				}
			}

			return null;
		}
	}
}