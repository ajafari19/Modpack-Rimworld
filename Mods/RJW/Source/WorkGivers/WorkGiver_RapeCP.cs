using RimWorld;
using Verse;
using Verse.AI;

namespace rjw
{
	/// <summary>
	/// Assigns a pawn to rape a comfort prisoner
	/// </summary>
	public class WorkGiver_RapeCP : WorkGiver_Rape
	{
		public override bool WorkGiverChecks(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn target = t as Pawn;
			if (!target.IsDesignatedComfort())
			{
				if (RJWSettings.DevMode) JobFailReason.Is("not designated as comfort", null);
					return false;
			}
			return true;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(xxx.comfort_prisoner_rapin, t);
		}
	}
}