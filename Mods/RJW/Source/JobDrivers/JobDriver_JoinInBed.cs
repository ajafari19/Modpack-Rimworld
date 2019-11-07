using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;
using Multiplayer.API;

namespace rjw
{
	public class JobDriver_JoinInBed : JobDriver
	{
		private const int ticks_between_hearts = 100;

		private int ticks_left;

		private readonly TargetIndex ipawn = TargetIndex.A;
		private readonly TargetIndex ipartner = TargetIndex.B;
		private readonly TargetIndex ibed = TargetIndex.C;

		protected Pawn Top => (Pawn)(job.GetTarget(ipawn));
		protected Pawn Partner => (Pawn)(job.GetTarget(ipartner));
		protected Building_Bed Bed => (Building_Bed)(job.GetTarget(ibed));

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return Top.Reserve(Partner, job, xxx.max_rapists_per_prisoner, 0, null, errorOnFailed);
		}

		[SyncMethod]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			//--Log.Message("JobDriver_JoinInBed::MakeNewToils() called");
			this.FailOnDespawnedOrNull(ipartner);
			this.FailOnDespawnedOrNull(ibed);
			this.FailOn(() => !Partner.health.capacities.CanBeAwake);
			this.FailOn(() => !(Partner.InBed() || xxx.in_same_bed(Partner, Top)));
			this.FailOn(() => pawn.Drafted);
			yield return Toils_Reserve.Reserve(ipartner, xxx.max_rapists_per_prisoner, 0);
			yield return Toils_Goto.GotoThing(ipartner, PathEndMode.OnCell);
			yield return new Toil
			{
				initAction = delegate
				{
					//--Log.Message("JobDriver_JoinInBed::MakeNewToils() - setting initAction");
					//Rand.PopState();
					//Rand.PushState(RJW_Multiplayer.PredictableSeed());
					ticks_left = (int)(2500.0f * Rand.Range(0.30f, 1.30f));
					Job gettin_loved = new Job(xxx.gettin_loved, Top, Bed);
					Partner.jobs.StartJob(gettin_loved, JobCondition.InterruptForced);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			Toil do_lovin = new Toil {defaultCompleteMode = ToilCompleteMode.Never};
			do_lovin.FailOn(() => (Partner.CurJob == null) || (Partner.CurJob.def != xxx.gettin_loved));
			do_lovin.AddPreTickAction(delegate
			{
				--ticks_left;
				xxx.reduce_rest(Partner, 1);
				xxx.reduce_rest(Top, 2);
				if (ticks_left <= 0)
					ReadyForNextToil();
				else if (Top.IsHashIntervalTick(ticks_between_hearts))
					MoteMaker.ThrowMetaIcon(Top.Position, Top.Map, ThingDefOf.Mote_Heart);
			});
			do_lovin.socialMode = RandomSocialMode.Off;
			yield return do_lovin;
			yield return new Toil
			{
				initAction = delegate
				{
					// Trying to add some interactions and social logs
					SexUtility.ProcessSex(Top, Partner, false /*rape*/, false/*isCoreLovin*/, false /*whoring*/);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}
	}
}