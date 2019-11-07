using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;
using Multiplayer.API;

namespace rjw
{
	public class JobDriver_Fappin : JobDriver
	{
		private const int ticks_between_hearts = 100;

		private int ticks_left;

		private readonly TargetIndex ibed = TargetIndex.A;
		private Building_Bed Bed => (Building_Bed)((Thing)job.GetTarget(ibed));

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return pawn.Reserve(Bed, job, Bed.SleepingSlotsCount, 0, null, errorOnFailed);
		}

		[SyncMethod]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			// Faster fapping when frustrated.
			//Rand.PopState();
			//Rand.PushState(RJW_Multiplayer.PredictableSeed());
			ticks_left = (int)(xxx.need_some_sex(pawn) > 2f ? 2500.0f * Rand.Range(0.2f, 0.7f) : 2500.0f * Rand.Range(0.2f, 0.4f));

			this.FailOnDespawnedOrNull(ibed);
			this.FailOn(() => pawn.Drafted);
			this.KeepLyingDown(ibed);
			yield return Toils_Bed.ClaimBedIfNonMedical(ibed);
			yield return Toils_Bed.GotoBed(ibed);

			Toil do_fappin = Toils_LayDown.LayDown(ibed, true, false, false, false);
			do_fappin.AddPreTickAction(delegate
			{
				--ticks_left;
				xxx.reduce_rest(pawn, 1);
				if (ticks_left <= 0)
					ReadyForNextToil();
				else if (pawn.IsHashIntervalTick(ticks_between_hearts))
					MoteMaker.ThrowMetaIcon(pawn.Position, pawn.Map, ThingDefOf.Mote_Heart);
			});
			do_fappin.AddFinishAction(delegate
			{
			    //Moved satisfy and tick increase to aftersex, since it works with solo acts now.
				SexUtility.Aftersex(pawn, xxx.rjwSextype.Masturbation);
				if (SexUtility.ConsiderCleaning(pawn))
				{
					LocalTargetInfo own_cum = pawn.PositionHeld.GetFirstThing<Filth>(pawn.Map);

					Job clean = new Job(JobDefOf.Clean);
					clean.AddQueuedTarget(TargetIndex.A, own_cum);

					pawn.jobs.jobQueue.EnqueueFirst(clean);

				}
			});
			do_fappin.socialMode = RandomSocialMode.Off;
			yield return do_fappin;
		}
	}
}