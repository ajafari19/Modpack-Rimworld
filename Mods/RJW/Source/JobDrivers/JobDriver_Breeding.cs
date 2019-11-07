using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;
using Multiplayer.API;

namespace rjw
{
	/// <summary>
	/// This is the driver for animals mounting breeders.
	/// </summary>
	public class JobDriver_Breeding : JobDriver
	{
		protected TargetIndex PartnerIndex = TargetIndex.A;

		public Pawn Target => (Pawn)(job.GetTarget(PartnerIndex));

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return pawn.Reserve(Target, job, BreederHelper.max_animals_at_once, 0);
		}

		[SyncMethod]
		public virtual void roll_to_hit(Pawn pawn, Pawn partner)
		{
			if (!RJWSettings.rape_beating || !xxx.is_human(pawn))
				return;

			//Rand.PopState();
			//Rand.PushState(RJW_Multiplayer.PredictableSeed());
			float rand_value = Rand.Value;
			float victim_pain = partner.health.hediffSet.PainTotal;
			float chance_to_hit = xxx.config.base_chance_to_hit_prisoner/5;
			float threshold = xxx.config.minor_pain_threshold;

			if ((victim_pain < threshold && rand_value < chance_to_hit))
			{
				if (InteractionUtility.TryGetRandomVerbForSocialFight(pawn, out Verb v))
					pawn.meleeVerbs.TryMeleeAttack(partner, v);
			}
		}

		[SyncMethod]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			//Rand.PopState();
			//Rand.PushState(RJW_Multiplayer.PredictableSeed());
			int duration = (int)(2000.0f * Rand.Range(0.50f, 0.90f));
			int ticks_between_hearts = Rand.RangeInclusive(70, 130);
			int ticks_between_hits = Rand.Range(xxx.config.min_ticks_between_hits, xxx.config.max_ticks_between_hits);
			int ticks_between_thrusts = 100;

			//--Log.Message("JobDriver_ComfortPrisonerRapin::MakeNewToils() - setting fail conditions");
			this.FailOnDespawnedNullOrForbidden(PartnerIndex);
			this.FailOn(() => !pawn.CanReserve(Target, BreederHelper.max_animals_at_once, 0)); // Fail if someone else reserves the target before the animal arrives.
			this.FailOn(() => !pawn.CanReach(Target, PathEndMode.Touch, Danger.Some)); // Fail if animal cannot reach target.
			this.FailOn(() => !(Target.IsDesignatedBreeding() || (RJWSettings.animal_on_animal_enabled && xxx.is_animal(Target)))); // Fail if not designated and not animal-on-animal
			this.FailOn(() => Target.CurJob == null);
			this.FailOn(() => pawn.Drafted);

			// Path to target
			yield return Toils_Goto.GotoThing(PartnerIndex, PathEndMode.OnCell);

			SexUtility.RapeAttemptAlert(pawn, Target);

			// Breed target
			var breed = new Toil();
			breed.initAction = delegate
			{
				//Log.Message("JobDriver_ComfortPrisonerRapin::MakeNewToils() - Setting victim job driver");
				Job currentJob = Target.jobs.curJob;

				if (currentJob.def != xxx.gettin_raped)
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

					Target.jobs.StartJob(gettin_raped, JobCondition.InterruptForced);

					//var dri = Target.jobs.curDriver as JobDriver_GettinRaped;
					//if (xxx.is_animal(pawn) && xxx.is_animal(Target)) // No alert spam for animal-on-animal
						//dri.disable_alert = true;

					(Target.jobs.curDriver as JobDriver_GettinRaped).increase_time(duration);
					if (Bed != null)
						(Target.jobs.curDriver as JobDriver_GettinRaped)?.set_bed(Bed);
				}
				else
				{
					if (Target.jobs.curDriver is JobDriver_GettinRaped dri)
					{
						dri.rapist_count += 1;
						dri.increase_time(duration);
					}
				}
			};

			breed.tickAction = delegate
			{
				if (pawn.IsHashIntervalTick(ticks_between_hearts))
					MoteMaker.ThrowMetaIcon(pawn.Position, pawn.Map, ThingDefOf.Mote_Heart);
				if (pawn.IsHashIntervalTick(ticks_between_thrusts))
					xxx.sexTick(pawn, Target);
				if (!xxx.is_zoophile(Target) && pawn.IsHashIntervalTick(ticks_between_hits))
					roll_to_hit(pawn, Target);

				if (!Target.Dead)
					xxx.reduce_rest(Target, 1);
				xxx.reduce_rest(pawn, 2);

				if (Genital_Helper.has_penis(pawn) || Genital_Helper.has_penis_infertile(pawn))
				{
					// Face same direction, most of animal sex is likely doggystyle.
					Target.Rotation = pawn.Rotation;
				}
			};

			breed.AddFinishAction(delegate
			{
				if ((Target.jobs != null) 
				&& (Target.jobs.curDriver != null) 
				&& (Target.jobs.curDriver as JobDriver_GettinRaped != null))
				{
					(Target.jobs.curDriver as JobDriver_GettinRaped).rapist_count -= 1;
				}
				pawn.stances.StaggerFor(Rand.Range(0,50));
				Target.stances.StaggerFor(Rand.Range(10,300));
			});

			breed.defaultCompleteMode = ToilCompleteMode.Delay;
			breed.defaultDuration = duration;
			yield return breed;

			yield return new Toil
			{
				initAction = delegate
				{
					//Log.Message("JobDriver_Breeding::MakeNewToils() - Calling aftersex");
					//// Trying to add some interactions and social logs
					bool violent = !(pawn.relations.DirectRelationExists(PawnRelationDefOf.Bond, Target) ||
					                 (xxx.is_animal(pawn) && (pawn.RaceProps.wildness - pawn.RaceProps.petness + 0.18f) > Rand.Range(0.36f, 1.8f)));
					SexUtility.ProcessSex(pawn, Target, violent);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}
	}
}