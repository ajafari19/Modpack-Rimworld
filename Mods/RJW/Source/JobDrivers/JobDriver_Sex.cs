using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace rjw
{
	public abstract class JobDriver_Sex : JobDriver
	{
		protected float satisfaction = 1.0f;

		public bool shouldreserve = true;

		public int stackCount = 0;

		public int ticks_between_hearts;
		public int ticks_between_hits = 50;
		public int ticks_between_thrusts;

		public Thing Target = null;

		//private Building_Bed Bed;

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			Log.Message("shouldreserve " + shouldreserve);
			if (shouldreserve)
				return pawn.Reserve(Target, job, xxx.max_rapists_per_prisoner, stackCount, null, errorOnFailed);
			else
				return true; // No reservations needed.

			//return this.pawn.Reserve(this.Partner, this.job, 1, 0, null) && this.pawn.Reserve(this.Bed, this.job, 1, 0, null);
		}

		public void CalculateSatisfactionPerTick()
		{
				satisfaction = 1.0f;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			return null;
		}
	}
}