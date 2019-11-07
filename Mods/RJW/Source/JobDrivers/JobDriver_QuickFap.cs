using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;
using Multiplayer.API;

namespace rjw
{
	public class JobDriver_QuickFap : JobDriver
	{
		private const int ticks_between_hearts = 100;
		private int ticks_left;

		public IntVec3 cell => (IntVec3)job.GetTarget(TargetIndex.A);
		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true; // No reservations needed.
		}

		[SyncMethod]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			//this.FailOn(() => PawnUtility.PlayerForcedJobNowOrSoon(pawn));
			this.FailOn(() => pawn.health.Downed);
			this.FailOn(() => pawn.IsBurning());
			this.FailOn(() => pawn.IsFighting());
			this.FailOn(() => pawn.Drafted);

			//Rand.PopState();
			//Rand.PushState(RJW_Multiplayer.PredictableSeed());
			// Faster fapping when frustrated.
			ticks_left = (int)(xxx.need_some_sex(pawn) > 2f ? 2500.0f * Rand.Range(0.2f, 0.7f) : 2500.0f * Rand.Range(0.2f, 0.4f));

			Toil findfapspot = new Toil
			{
				initAction = delegate
				{
					pawn.pather.StartPath(cell, PathEndMode.OnCell);
				},
				defaultCompleteMode = ToilCompleteMode.PatherArrival
			};
			yield return findfapspot;

			//Log.Message("[RJW] Making new toil for QuickFap.");

			Toil fap = Toils_General.Wait(ticks_left);
			fap.tickAction = delegate
			{
				--ticks_left;
				xxx.reduce_rest(pawn, 1);
				if (ticks_left <= 0)
					ReadyForNextToil();
				else if (pawn.IsHashIntervalTick(ticks_between_hearts))
					MoteMaker.ThrowMetaIcon(pawn.Position, pawn.Map, ThingDefOf.Mote_Heart);
			};
			fap.AddFinishAction(delegate
			{
				SexUtility.Aftersex(pawn, xxx.rjwSextype.Masturbation);
				if (!SexUtility.ConsiderCleaning(pawn)) return;

				LocalTargetInfo own_cum = pawn.PositionHeld.GetFirstThing<Filth>(pawn.Map);

				Job clean = new Job(JobDefOf.Clean);
				clean.AddQueuedTarget(TargetIndex.A, own_cum);

				pawn.jobs.jobQueue.EnqueueFirst(clean);
			});
			yield return fap;
		}
	}
}