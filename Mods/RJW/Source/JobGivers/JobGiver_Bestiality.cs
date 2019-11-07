// #define TESTMODE // Uncomment to enable logging.

using System.Diagnostics;
using RimWorld;
using Verse;
using Verse.AI;

namespace rjw
{
	/// <summary>
	/// Pawn tries to find animal to do loving/raping.
	/// </summary>
	public class JobGiver_Bestiality : ThinkNode_JobGiver
	{
		[Conditional("TESTMODE")]
		private static void DebugText(string msg)
		{
			Log.Message(msg);
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.Drafted) return null;

			// Most checks are now done in ThinkNode_ConditionalBestiality
			DebugText("[RJW] JobGiver_Bestiality::TryGiveJob( " + xxx.get_pawnname(pawn) + " ) called");

			if (!SexUtility.ReadyForLovin(pawn) && !xxx.is_frustrated(pawn))
				return null;

			Pawn target = BreederHelper.find_breeder_animal(pawn, pawn.Map);
			DebugText("[RJW] JobGiver_Bestiality::TryGiveJob - target is " + (target == null ? "no target found" : xxx.get_pawnname(target)));

			if (target == null) return null;
					
			if (xxx.can_rape(pawn))
			{
				return new Job(xxx.bestiality, target);
			}

			Building_Bed bed = pawn.ownership.OwnedBed;
			if (!xxx.can_be_fucked(pawn) || bed == null || !target.CanReach(bed, PathEndMode.OnCell, Danger.Some) || target.Downed) return null;

			// TODO: Should rename this to BestialityInBed or somesuch, since it's not limited to females.
			return new Job(xxx.bestialityForFemale, target, bed, bed.SleepPosOfAssignedPawn(pawn));
		}
	}
}