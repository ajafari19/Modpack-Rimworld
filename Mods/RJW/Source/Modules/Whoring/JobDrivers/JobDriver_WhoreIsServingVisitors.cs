using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;
using Multiplayer.API;

namespace rjw
{
	public class JobDriver_WhoreIsServingVisitors : JobDriver
	{
		private readonly TargetIndex PartnerInd = TargetIndex.A;
		private readonly TargetIndex BedInd = TargetIndex.B;
		private readonly TargetIndex SlotInd = TargetIndex.C;
		private int ticks_left = 200;
		private const int ticks_between_hearts = 100;
		private static readonly ThoughtDef thought_free = ThoughtDef.Named("Whorish_Thoughts");
		private static readonly ThoughtDef thought_captive = ThoughtDef.Named("Whorish_Thoughts_Captive");

		public Pawn Actor => GetActor();
		public Pawn Partner => (Pawn)(job.GetTarget(PartnerInd));

		public Building_Bed Bed => (Building_Bed)(job.GetTarget(BedInd));
		public IntVec3 WhoreSleepSpot => (IntVec3)job.GetTarget(SlotInd);

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref ticks_left, "ticksLeft");
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
			//Log.Message("[RJW]JobDriver_WhoreIsServingVisitors::MakeNewToils() - making toils");
			this.FailOnDespawnedOrNull(PartnerInd);
			this.FailOnDespawnedNullOrForbidden(BedInd);
			//Log.Message("[RJW]JobDriver_WhoreIsServingVisitors::MakeNewToils() fail conditions check " + (Actor is null) + " " + !xxx.CanUse(Actor, Bed) + " " + !Actor.CanReserve(Partner));
			this.FailOn(() => Actor is null || !xxx.CanUse(Actor, Bed) || !Actor.CanReserve(Partner));
			this.FailOn(() => pawn.Drafted);
			int price = WhoringHelper.PriceOfWhore(Actor);
			yield return Toils_Reserve.Reserve(PartnerInd, 1, 0);
			//yield return Toils_Reserve.Reserve(BedInd, Bed.SleepingSlotsCount, 0);
			bool partnerHasPenis = Genital_Helper.has_penis(Partner) || Genital_Helper.has_penis_infertile(Partner);

			Toil gotoWhoreBed = new Toil
			{
				initAction = delegate
				 {
					 //Log.Message("[RJW]JobDriver_WhoreIsServingVisitors::MakeNewToils() - gotoWhoreBed initAction is called");
					 Actor.pather.StartPath(WhoreSleepSpot, PathEndMode.OnCell);
					 Partner.jobs.StopAll();
					 Partner.pather.StartPath(WhoreSleepSpot, PathEndMode.Touch);
				 },
				tickAction = delegate
				{
					if (Partner.IsHashIntervalTick(150))
					{
						Partner.pather.StartPath(Actor, PathEndMode.Touch);
						//Log.Message(xxx.get_pawnname(Partner) + ": I'm following the whore");
					}
				},
				defaultCompleteMode = ToilCompleteMode.PatherArrival
			};
			gotoWhoreBed.FailOnWhorebedNoLongerUsable(BedInd, Bed);
			yield return gotoWhoreBed;

			Toil waitInBed = new Toil
			{
				initAction = delegate
				{
					//Rand.PopState();
					//Rand.PushState(RJW_Multiplayer.PredictableSeed());
					//Log.Message("JobDriver_WhoreIsServingVisitors::MakeNewToils() - waitInBed, initAction is called");
					ticksLeftThisToil = 5000;
					ticks_left = (int)(2000.0f * Rand.Range(0.30f, 1.30f));
					//Actor.pather.StopDead();  //Let's just make whores standing at the bed
					//JobDriver curDriver = Actor.jobs.curDriver;
					//curDriver.layingDown = LayingDownState.LayingInBed;
					//curDriver.asleep = false;
					var gettin_loved = new Job(xxx.gettin_loved, Actor, Bed);
					Partner.jobs.StartJob(gettin_loved, JobCondition.InterruptForced);
				},
				tickAction = delegate
				{
					Actor.GainComfortFromCellIfPossible();
					if (IsInOrByBed(Bed, Partner))
					{
						//Log.Message("JobDriver_WhoreIsServingVisitors::MakeNewToils() - waitInBed, tickAction pass");
						ticksLeftThisToil = 0;
					}
				},
				defaultCompleteMode = ToilCompleteMode.Delay,
			};
			waitInBed.FailOn(() => pawn.GetRoom() == null);
			yield return waitInBed;

			bool canAfford = WhoringHelper.CanAfford(Partner, Actor, price);
			if (canAfford)
			{
				Toil loveToil = new Toil
				{
					initAction = delegate
					{
						//Actor.jobs.curDriver.ticksLeftThisToil = 1200;
						//Using ticks_left to control the time of sex
						//--Log.Message("JobDriver_WhoreIsServingVisitors::MakeNewToils() - loveToil, setting initAction");
						/*
						//Hoge: Whore is just work. no feel cheatedOnMe.
						if (xxx.HasNonPolyPartner(Actor))
						{
							Pawn pawn = LovePartnerRelationUtility.ExistingLovePartner(Actor);
							if (((Partner != pawn) && !pawn.Dead) && ((pawn.Map == Actor.Map) || (Rand.Value < 0.15)))
							{
								pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.CheatedOnMe, Actor);
							}
						}
						*/
						if (xxx.HasNonPolyPartnerOnCurrentMap(Partner))
						{
							Pawn lover = LovePartnerRelationUtility.ExistingLovePartner(Partner);
							//Rand.PopState();
							//Rand.PushState(RJW_Multiplayer.PredictableSeed());
							// We have to do a few other checks because the pawn might have multiple lovers and ExistingLovePartner() might return the wrong one
							if (lover != null && Actor != lover && !lover.Dead && (lover.Map == Partner.Map || Rand.Value < 0.25))
							{
								lover.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.CheatedOnMe, Partner);
							}
						}
						if (!partnerHasPenis)
							Actor.rotationTracker.Face(Partner.DrawPos);
					},
					defaultCompleteMode = ToilCompleteMode.Never, //Changed from Delay
				};
				loveToil.AddPreTickAction(delegate
				{
					//Actor.Reserve(Partner, 1, 0);
					--ticks_left;
					xxx.reduce_rest(Partner);
					xxx.reduce_rest(Actor, 2);

					if (ticks_left <= 0)
						ReadyForNextToil();
					else if (pawn.IsHashIntervalTick(ticks_between_hearts))
					{
						MoteMaker.ThrowMetaIcon(Actor.Position, Actor.Map, ThingDefOf.Mote_Heart);
					}
					Actor.GainComfortFromCellIfPossible();
					Partner.GainComfortFromCellIfPossible();
				});
				loveToil.AddFinishAction(delegate
				{
					//Log.Message("[RJW] JobDriver_WhoreIsServingVisitors::MakeNewToils() - finished loveToil");
					//// Trying to add some interactions and social logs
					//xxx.processAnalSex(Partner, Actor, ref isAnalSex, partnerHasPenis);
				});
				loveToil.AddFailCondition(() => Partner.Dead || !IsInOrByBed(Bed, Partner));
				loveToil.socialMode = RandomSocialMode.Off;
				yield return loveToil;

				Toil afterSex = new Toil
				{
					initAction = delegate
					{
						// Adding interactions, social logs, etc
						SexUtility.ProcessSex(Actor, Partner, false, false, true);

						//--Log.Message("JobDriver_WhoreIsServingVisitors::MakeNewToils() - Partner should pay the price now in afterSex.initAction");
						int remainPrice = WhoringHelper.PayPriceToWhore(Partner, price, Actor);
						/*if (remainPrice <= 0)
						{
							--Log.Message("JobDriver_WhoreIsServingVisitors::MakeNewToils() - Paying price is success");
						}
						else
						{
							--Log.Message("JobDriver_WhoreIsServingVisitors::MakeNewToils() - Paying price failed");
						}*/
						xxx.UpdateRecords(Actor, price-remainPrice);
						var thought = (Actor.IsPrisoner) ? thought_captive : thought_free;
						pawn.needs.mood.thoughts.memories.TryGainMemory(thought);
						if (SexUtility.ConsiderCleaning(pawn))
						{
							LocalTargetInfo cum = pawn.PositionHeld.GetFirstThing<Filth>(pawn.Map);

							Job clean = new Job(JobDefOf.Clean);
							clean.AddQueuedTarget(TargetIndex.A, cum);

							pawn.jobs.jobQueue.EnqueueFirst(clean);
						}
					},
					defaultCompleteMode = ToilCompleteMode.Instant
				};
				yield return afterSex;
			}
		}
	}
}