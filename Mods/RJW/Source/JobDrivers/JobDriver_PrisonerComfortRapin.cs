using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;
using Multiplayer.API;

namespace rjw
{
	public class JobDriver_PrisonerComfortRapin : JobDriver
	{
		protected int duration;
		protected int ticks_between_hearts;
		protected int ticks_between_hits = 50;
		protected int ticks_between_thrusts;

		protected TargetIndex iprisoner = TargetIndex.A;

		protected virtual float chance_to_hit_prisoner => xxx.config.base_chance_to_hit_prisoner;

		protected Pawn Target => (Pawn)(job.GetTarget(iprisoner));

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return pawn.Reserve(Target, job, xxx.max_rapists_per_prisoner, 0, null, errorOnFailed);
		}

		[SyncMethod]
		public virtual void roll_to_hit(Pawn rapist, Pawn p)
		{
			if (!RJWSettings.rape_beating)
			{
				return;
			}

			//Rand.PopState();
			//Rand.PushState(RJW_Multiplayer.PredictableSeed());
			float rand_value = Rand.Value;
			float victim_pain = p.health.hediffSet.PainTotal;
			// bloodlust makes the aggressor more likely to hit the prisoner
			float beating_chance = chance_to_hit_prisoner * (xxx.is_bloodlust(rapist) ? 1.25f : 1.0f);
			// psychopath makes the aggressor more likely to hit the prisoner past the significant_pain_threshold
			float beating_threshold = xxx.is_psychopath(rapist) ? xxx.config.extreme_pain_threshold : xxx.config.significant_pain_threshold;

			//--Log.Message("roll_to_hit:  rand = " + rand_value + ", beating_chance = " + beating_chance + ", victim_pain = " + victim_pain + ", beating_threshold = " + beating_threshold);

			//if ((victim_pain < beating_threshold && rand_value < beating_chance) || (rand_value < (beating_chance / 2)))
			if ((victim_pain < beating_threshold && rand_value < beating_chance) || (rand_value < (beating_chance / 2) && xxx.is_bloodlust(rapist)))
			{
				//--Log.Message("   done told her twice already...");
				if (InteractionUtility.TryGetRandomVerbForSocialFight(rapist, out Verb v))
				{
					rapist.meleeVerbs.TryMeleeAttack(p, v);
				}
			}

			/*
			//if (p.health.hediffSet.PainTotal < xxx.config.significant_pain_threshold)
			if ((Rand.Value < 0.50f) &&
				((Rand.Value < 0.33f) || (p.health.hediffSet.PainTotal < xxx.config.significant_pain_threshold) ||
				 (xxx.is_bloodlust (rapist) || xxx.is_psychopath (rapist)))) {
				Verb v;
				if (InteractionUtility.TryGetRandomVerbForSocialFight (rapist, out v))
					rapist.meleeVerbs.TryMeleeAttack (p, v);
			}
			*/
		}

		[SyncMethod]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			//Rand.PopState();
			//Rand.PushState(RJW_Multiplayer.PredictableSeed());
			duration = (int)(2000.0f * Rand.Range(0.50f, 0.90f));
			ticks_between_hearts = Rand.RangeInclusive(70, 130);
			ticks_between_hits = Rand.Range(xxx.config.min_ticks_between_hits, xxx.config.max_ticks_between_hits);
			ticks_between_thrusts = 100;
			bool pawnHasPenis = Genital_Helper.has_penis(pawn) || Genital_Helper.has_penis_infertile(pawn);

			if (xxx.is_bloodlust(pawn))
				ticks_between_hits = (int)(ticks_between_hits * 0.75);
			if (xxx.is_brawler(pawn))
				ticks_between_hits = (int)(ticks_between_hits * 0.90);

			//--Log.Message("JobDriver_ComfortPrisonerRapin::MakeNewToils() - setting fail conditions");
			this.FailOnDespawnedNullOrForbidden(iprisoner);
			//this.FailOn(() => (!Target.health.capacities.CanBeAwake) || (!comfort_prisoners.is_designated(Target)));//this is wrong
			this.FailOn(() => (!Target.IsDesignatedComfort()));
			this.FailOn(() => !pawn.CanReserve(Target, xxx.max_rapists_per_prisoner, 0)); // Fail if someone else reserves the prisoner before the pawn arrives
			this.FailOn(() => pawn.Drafted);
			yield return Toils_Goto.GotoThing(iprisoner, PathEndMode.OnCell);

			SexUtility.RapeAttemptAlert(pawn, Target);

			Toil rape = new Toil();
			rape.initAction = delegate
			{
				//--Log.Message("JobDriver_ComfortPrisonerRapin::MakeNewToils() - reserving prisoner");
				//pawn.Reserve(Target, comfort_prisoners.max_rapists_per_prisoner, 0);
				if (!pawnHasPenis)
					Target.rotationTracker.Face(pawn.DrawPos);

				//--Log.Message("JobDriver_ComfortPrisonerRapin::MakeNewToils() - Setting victim job driver");
				JobDriver_GettinRaped dri = Target.jobs.curDriver as JobDriver_GettinRaped;
				if (dri == null)
				{
					Job gettin_raped = new Job(xxx.gettin_raped, pawn, Target);
					Building_Bed Bed = null;
					//Log.Message(xxx.get_pawnname(pawn) + " LayingInBed:" + pawn.GetPosture());
					//Log.Message(xxx.get_pawnname(Target) + " LayingInBed:" + Target.GetPosture());
					if (Target.GetPosture() == PawnPosture.LayingInBed)
					{
						Bed = Target.CurrentBed();
						//Log.Message(xxx.get_pawnname(Target) + ": bed:" + Bed);
					}

					Target.jobs.StartJob(gettin_raped, JobCondition.InterruptForced, null, false, true, null);
					(Target.jobs.curDriver as JobDriver_GettinRaped)?.increase_time(duration);
					if (Bed != null)
						(Target.jobs.curDriver as JobDriver_GettinRaped)?.set_bed(Bed);
				}
				else
				{
					dri.rapist_count += 1;
					dri.increase_time(duration);
				}
			};
			rape.tickAction = delegate
			{
				if (pawn.IsHashIntervalTick(ticks_between_hearts))
					MoteMaker.ThrowMetaIcon(pawn.Position, pawn.Map, ThingDefOf.Mote_Heart);
				if (pawn.IsHashIntervalTick(ticks_between_thrusts))
					xxx.sexTick(pawn, Target);
				if (pawn.IsHashIntervalTick(ticks_between_hits))
					roll_to_hit(pawn, Target);
				xxx.reduce_rest(Target, 1);
				xxx.reduce_rest(pawn, 2);
			};
			rape.AddFinishAction(delegate
			{
				//Log.Message("JobDriver_ComfortPrisonerRapin::MakeNewToils() - Clearing victim job");
				if (Target.jobs?.curDriver is JobDriver_GettinRaped)
				{
					//Log.Message("JobDriver_ComfortPrisonerRapin::MakeNewToils() - Victim present");
					(Target.jobs.curDriver as JobDriver_GettinRaped).rapist_count -= 1;
				}
			});
			rape.defaultCompleteMode = ToilCompleteMode.Delay;
			rape.defaultDuration = duration;
			yield return rape;

			yield return new Toil
			{
				initAction = delegate
				{
					// Trying to add some interactions and social logs
					SexUtility.ProcessSex(pawn, Target, true);
					Target.records.Increment(xxx.GetRapedAsComfortPrisoner);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}
	}
}