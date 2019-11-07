using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;
using Multiplayer.API;

namespace rjw
{
	public class JobDriver_GettinRaped : JobDriver
	{
		private int ticks_between_hearts;

		private int ticks_remaining = 10;

		public int rapist_count = 1; // Defaults to 1 so the first rapist doesn't have to add themself

		private Pawn Initiator => (Pawn)(job.GetTarget(TargetIndex.A));
		private Pawn Receiver => (Pawn)(job.GetTarget(TargetIndex.B));

		//private bool was_laying_down;

		private Building_Bed Bed;

		public void increase_time(int min_ticks_remaining)
		{
			if (min_ticks_remaining > ticks_remaining)
				ticks_remaining = min_ticks_remaining;
		}

		public void set_bed(Building_Bed newBed)
		{
			Bed = newBed;
		}

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		private void RapeAlert(bool silent = false)
		{
			if (!silent)
				Messages.Message(xxx.get_pawnname(Receiver) + " is getting raped.", Receiver, MessageTypeDefOf.NegativeEvent);
			else
				Messages.Message(xxx.get_pawnname(Receiver) + " is getting raped.", Receiver, MessageTypeDefOf.SilentInput);
		}

		public float CalculateSatisfactionPerTick()
		{
			return 1.0f;
		}

		[SyncMethod]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			//Rand.PopState();
			//Rand.PushState(RJW_Multiplayer.PredictableSeed());
			ticks_between_hearts = Rand.RangeInclusive(70, 130);
			//was_laying_down = pawn.GetPosture() != PawnPosture.Standing;
			//Log.Message(xxx.get_pawnname(Initiator) + ": was_laying_down:" + was_laying_down + " LayingInBed:" + Initiator.GetPosture());
			//Log.Message(xxx.get_pawnname(Receiver) + ": was_laying_down:" + was_laying_down + " LayingInBed:" + Receiver.GetPosture());
			//was_laying_down = (pawn.jobs.curDriver != null) && pawn.GetPosture() != PawnPosture.Standing;
			//Log.Message(xxx.get_pawnname(pawn) + ": bed:" + Bed);

			var get_raped = new Toil();
			get_raped.defaultCompleteMode = ToilCompleteMode.Never;
			get_raped.initAction = delegate
			{
				pawn.pather.StopDead();
				//pawn.jobs.posture = PawnPosture.Standing;
				pawn.jobs.curDriver.asleep = false;

				switch (RJWPreferenceSettings.rape_alert_sound)
				{
					case RJWPreferenceSettings.RapeAlert.Enabled:
						RapeAlert();
						break;
					case RJWPreferenceSettings.RapeAlert.Humanlikes:
						if (xxx.is_human(Receiver))
							RapeAlert();
						else
							RapeAlert(true);
						break;
					case RJWPreferenceSettings.RapeAlert.Colonists:
						if (Receiver.Faction == Faction.OfPlayer)
							RapeAlert();
						else
							RapeAlert(true);
						break;
					default:
						RapeAlert(true);
						break;
				}

				//Messages.Message("GetinRapedNow".Translate(new object[] { pawn.LabelIndefinite() }).CapitalizeFirst(), pawn, MessageTypeDefOf.NegativeEvent);

				if (Initiator == null || Receiver == null) return;
				bool partnerHasHands = Receiver.health.hediffSet.GetNotMissingParts().Any(part => part.IsInGroup(BodyPartGroupDefOf.RightHand) || part.IsInGroup(BodyPartGroupDefOf.LeftHand));

				// Hand check is for monstergirls and other bipedal 'animals'.
				if ((!xxx.is_animal(Initiator) && partnerHasHands) || Rand.Chance(0.3f)) // 30% chance of face-to-face regardless, for variety.
				{ // Face-to-face
					Initiator.rotationTracker.Face(Receiver.DrawPos);
					Receiver.rotationTracker.Face(Initiator.DrawPos);
				}
				else
				{ // From behind / animal stuff should mostly use this
					Initiator.rotationTracker.Face(Receiver.DrawPos);
					Receiver.Rotation = Initiator.Rotation;
				}
				// TODO: The above works, but something is forcing the partners to face each other during sex. Need to figure it out.

				//prevent Receiver standing up and interrupting rape, probably
				if (Receiver.health.hediffSet.HasHediff(HediffDef.Named("Hediff_Submitting")))
					Receiver.health.AddHediff(HediffDef.Named("Hediff_Submitting"));
			};
			get_raped.tickAction = delegate
			{
				--ticks_remaining;
				/*
				if ((ticks_remaining <= 0) || (rapist_count <= 0))
					ReadyForNextToil();
				*/
				if ((rapist_count > 0) && (pawn.IsHashIntervalTick(ticks_between_hearts / rapist_count)))
					MoteMaker.ThrowMetaIcon(pawn.Position, pawn.Map, xxx.mote_noheart);

			};
			get_raped.AddEndCondition(new Func<JobCondition>(() =>
			{
				if ((ticks_remaining <= 0) || (rapist_count <= 0))
					return JobCondition.Succeeded;
				return JobCondition.Ongoing;
			}));
			get_raped.AddFinishAction(delegate
			{
				if (Bed != null && pawn.Downed)
				{
					Job tobed = new Job(JobDefOf.Rescue, pawn, Bed);
					tobed.count = 1;
					Initiator.jobs.jobQueue.EnqueueFirst(tobed);
					//Log.Message(xxx.get_pawnname(Initiator) + ": job tobed:" + tobed);
				}
			});
			get_raped.socialMode = RandomSocialMode.Off;
			yield return get_raped;
		}
	}
}