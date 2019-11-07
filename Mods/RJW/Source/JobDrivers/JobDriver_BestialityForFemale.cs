using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;
using Multiplayer.API;

namespace rjw
{
	public class JobDriver_BestialityForFemale : JobDriver
	{
		private readonly TargetIndex PartnerInd = TargetIndex.A;
		private readonly TargetIndex BedInd = TargetIndex.B;
		private readonly TargetIndex SlotInd = TargetIndex.C;
		private int ticks_left = 200;
		private const int ticks_between_hearts = 100;

		public Pawn Actor => GetActor();
		public Pawn Partner => (Pawn)(job.GetTarget(PartnerInd));
		public Building_Bed Bed => (Building_Bed)(job.GetTarget(BedInd));
		public IntVec3 SleepSpot => (IntVec3)job.GetTarget(SlotInd);

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref ticks_left, "ticksLeft", 0, false);
		}

		private static bool IsInOrByBed(Building_Bed b, Pawn p)
		{
			for (int i = 0; i < b.SleepingSlotsCount; i++)
			{
				if (b.GetSleepingSlotPos(i).InHorDistOf(p.Position, 1f))
				{
					return true;
				}
			}
			return false;
		}
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return pawn.Reserve(Partner, job, 1, 0, null, errorOnFailed);
		}

		[SyncMethod]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(PartnerInd);
			this.FailOnDespawnedNullOrForbidden(BedInd);
			this.FailOn(() => Actor is null || !Actor.CanReserveAndReach(Partner, PathEndMode.Touch, Danger.Deadly));
			this.FailOn(() => pawn.Drafted);
			yield return Toils_Reserve.Reserve(PartnerInd, 1, 0);
			//yield return Toils_Reserve.Reserve(BedInd, Bed.SleepingSlotsCount, 0);
			Toil gotoAnimal = Toils_Goto.GotoThing(PartnerInd, PathEndMode.Touch);
			yield return gotoAnimal;

			bool partnerHasPenis = Genital_Helper.has_penis(Partner) || Genital_Helper.has_penis_infertile(Partner);

			Toil gotoBed = new Toil
			{
				initAction = delegate
				{
					Actor.pather.StartPath(SleepSpot, PathEndMode.OnCell);
					Partner.pather.StartPath(SleepSpot, PathEndMode.OnCell);
				},
				defaultCompleteMode = ToilCompleteMode.PatherArrival
			};
			gotoBed.FailOnBedNoLongerUsable(BedInd);
			gotoBed.AddFailCondition(() => Partner.Downed);
			yield return gotoBed;
			gotoBed.AddFinishAction(delegate
			{
				var gettin_loved = new Job(xxx.gettin_loved, Actor, Bed);
				Partner.jobs.StartJob(gettin_loved, JobCondition.InterruptForced, null, false, true, null);
			});

			Toil waitInBed = new Toil
			{
				initAction = delegate
				{
					//Rand.PopState();
					//Rand.PushState(RJW_Multiplayer.PredictableSeed());
					ticksLeftThisToil = 5000;
					ticks_left = (int)(2000.0f * Rand.Range(0.30f, 1.30f));
				},
				tickAction = delegate
				{
					Actor.GainComfortFromCellIfPossible();
					if (IsInOrByBed(Bed, Partner))
					{
						ticksLeftThisToil = 0;
					}
				},
				defaultCompleteMode = ToilCompleteMode.Delay,
			};
			waitInBed.FailOn(() => pawn.GetRoom(RegionType.Set_Passable) == null);
			yield return waitInBed;

			Toil loveToil = new Toil
			{
				initAction = delegate
				{
					if (!partnerHasPenis)
						Actor.rotationTracker.Face(Partner.DrawPos);
				},
				defaultCompleteMode = ToilCompleteMode.Never, //Changed from Delay
			};
			loveToil.AddPreTickAction(delegate
			{
				//Actor.Reserve(Partner, 1, 0);
				--ticks_left;
				xxx.reduce_rest(Actor, 1);
				xxx.reduce_rest(Partner, 2);
				if (ticks_left <= 0)
					ReadyForNextToil();
				else if (pawn.IsHashIntervalTick(ticks_between_hearts))
				{
					MoteMaker.ThrowMetaIcon(Actor.Position, Actor.Map, ThingDefOf.Mote_Heart);
				}
				Actor.GainComfortFromCellIfPossible();
				Partner.GainComfortFromCellIfPossible();
			});
			loveToil.AddFailCondition(() => Partner.Dead || !IsInOrByBed(Bed, Partner));
			loveToil.socialMode = RandomSocialMode.Off;
			yield return loveToil;

			Toil afterSex = new Toil
			{
				initAction = delegate
				{
					//Log.Message("JobDriver_BestialityForFemale::MakeNewToils() - Calling aftersex");
					// Trying to add some interactions and social logs
					SexUtility.ProcessSex(Partner, pawn);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield return afterSex;
		}
	}
}