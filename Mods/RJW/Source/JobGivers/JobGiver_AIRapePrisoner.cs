using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;
using System.Collections.Generic;
using Multiplayer.API;

namespace rjw
{
	//Rape to Prisoner of QuestPrisonerWillingToJoin
	class JobGiver_AIRapePrisoner : ThinkNode_JobGiver
	{
		private const float min_fuckability = 0.10f; // Don't rape prisoners with <10% fuckability

		[SyncMethod]
		public static Pawn find_victim(Pawn pawn, Map m)
		{
			IEnumerable<Pawn> targets = m.mapPawns.AllPawns.Where(x
				=> x != pawn
				&& IsPrisonerOf(x, pawn.Faction)
				&& xxx.can_get_raped(x)
				&& !x.Position.IsForbidden(pawn)
				);

			List<Pawn> valid_targets = new List<Pawn>();

			foreach (Pawn target in targets)
			{
				if (!pawn.CanReserve(target, xxx.max_rapists_per_prisoner, 0)) continue;

				//--Log.Message(xxx.get_pawnname(pawn) + "->" + xxx.get_pawnname(target) + ":" + fuc);
				if (xxx.would_fuck(pawn, target, true, true) > min_fuckability)
				{
					valid_targets.Add(target);
				}
			}

			//Rand.PopState();
			//Rand.PushState(RJW_Multiplayer.PredictableSeed());
			return valid_targets.Any() ? valid_targets.RandomElement() : null;
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			//Log.Message("[RJW] JobGiver_AIRapePrisoner::TryGiveJob( " + xxx.get_pawnname(pawn) + " ) called ");

			if (!xxx.can_rape(pawn)) return null;

			if (SexUtility.ReadyForLovin(pawn) || xxx.need_some_sex(pawn) > 1f)
			{
				// don't allow pawns marked as comfort prisoners to rape others
				if (xxx.is_healthy(pawn))
				{
					Pawn prisoner = find_victim(pawn, pawn.Map);

					if (prisoner != null)
					{
						//--Log.Message("[RJW] JobGiver_RandomRape::TryGiveJob( " + xxx.get_pawnname(p) + " ) - found victim " + xxx.get_pawnname(prisoner));
						return new Job(xxx.random_rape, prisoner);
					}
				}
			}

			return null;
		}

		protected static bool IsPrisonerOf(Pawn pawn,Faction faction)
		{
			if (pawn?.guest == null) return false;
			return pawn.guest.HostFaction == faction && pawn.guest.IsPrisoner;
		}
	}
}
