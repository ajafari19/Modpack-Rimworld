using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;
using Multiplayer.API;

namespace rjw
{
	public class JobDriver_BestialityForMale : JobDriver
	{
		private int duration;
		private int ticks_between_hearts;
		private int ticks_between_hits = 50;
		private int ticks_between_thrusts;
		protected TargetIndex target_animal = TargetIndex.A;

		protected Pawn animal => (Pawn)(job.GetTarget(target_animal));

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return pawn.Reserve(animal, job, 1, 0, null, errorOnFailed);
		}

		[SyncMethod]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			//--Log.Message("[RJW] JobDriver_BestialityForMale::MakeNewToils() called");
			//Rand.PopState();
			//Rand.PushState(RJW_Multiplayer.PredictableSeed());
			duration = (int)(2500.0f * Rand.Range(0.50f, 0.90f));
			ticks_between_hearts = Rand.RangeInclusive(70, 130);
			ticks_between_hits = Rand.Range(xxx.config.min_ticks_between_hits, xxx.config.max_ticks_between_hits);
			ticks_between_thrusts = 100;

			if (xxx.is_bloodlust(pawn))
				ticks_between_hits = (int)(ticks_between_hits * 0.75);
			if (xxx.is_brawler(pawn))
				ticks_between_hits = (int)(ticks_between_hits * 0.90);

			//this.FailOn (() => (!animal.health.capacities.CanBeAwake) || (!comfort_prisoners.is_designated (animal)));
			// Fail if someone else reserves the prisoner before the pawn arrives or colonist can't reach animal
			this.FailOn(() => !pawn.CanReserveAndReach(animal, PathEndMode.Touch, Danger.Deadly));
			this.FailOn(() => animal.HostileTo(pawn));
			this.FailOnDespawnedNullOrForbidden(target_animal);
			this.FailOn(() => pawn.Drafted);
			yield return Toils_Reserve.Reserve(target_animal, 1, 0);
			//Log.Message("[RJW] JobDriver_BestialityForMale::MakeNewToils() - moving towards animal");
			yield return Toils_Goto.GotoThing(target_animal, PathEndMode.Touch);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(pawn);
			yield return Toils_Interpersonal.GotoInteractablePosition(target_animal);
			if (xxx.is_kind(pawn)
				|| (xxx.CTIsActive && xxx.has_traits(pawn) && pawn.story.traits.HasTrait(TraitDef.Named("RCT_AnimalLover"))))
			{
				yield return TalkToAnimal(pawn, animal);
				yield return TalkToAnimal(pawn, animal);
			}
			if (Rand.Chance(0.6f))
				yield return TalkToAnimal(pawn, animal);
			yield return Toils_Goto.GotoThing(target_animal, PathEndMode.OnCell);

			SexUtility.RapeAttemptAlert(pawn, animal);

			Toil rape = new Toil();
			rape.initAction = delegate
			{
				//--Log.Message("[RJW] JobDriver_BestialityForMale::MakeNewToils() - reserving animal");

				//--Log.Message("[RJW] JobDriver_BestialityForMale::MakeNewToils() - Setting animal job driver");
				if (!(animal.jobs.curDriver is JobDriver_GettinRaped dri))
				{
					//wild animals may flee or attack
					if (pawn.Faction != animal.Faction && animal.RaceProps.wildness > Rand.Range(0.22f, 1.0f)
						&& !(pawn.TicksPerMoveCardinal < (animal.TicksPerMoveCardinal / 2) && !animal.Downed && xxx.is_not_dying(animal)))
					{
						animal.jobs.StopAll(); // Wake up if sleeping.

						float aggro = animal.kindDef.RaceProps.manhunterOnTameFailChance;
						if (animal.kindDef.RaceProps.predator)
							aggro += 0.2f;
						else
							aggro -= 0.1f;

						if (Rand.Chance(aggro) && animal.CanSee(pawn))
						{
							animal.rotationTracker.FaceTarget(pawn);
							LifeStageUtility.PlayNearestLifestageSound(animal, (ls) => ls.soundAngry, 1.4f);
							MoteMaker.ThrowMetaIcon(animal.Position, animal.Map, ThingDefOf.Mote_IncapIcon);
							MoteMaker.ThrowMetaIcon(pawn.Position, pawn.Map, ThingDefOf.Mote_ColonistFleeing); //red '!'
							animal.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter);
							if (animal.kindDef.RaceProps.herdAnimal && Rand.Chance(0.2f))
							{ // 20% chance of turning the whole herd hostile...
								List<Pawn> packmates = animal.Map.mapPawns.AllPawnsSpawned.Where(x =>
									x != animal && x.def == animal.def && x.Faction == animal.Faction &&
									x.Position.InHorDistOf(animal.Position, 24f) && x.CanSee(animal)).ToList();

								foreach (Pawn packmate in packmates)
								{
									packmate.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter);
								}
							}
							Messages.Message(pawn.Name.ToStringShort + " is being attacked by " + xxx.get_pawnname(animal) + ".", pawn, MessageTypeDefOf.ThreatSmall);
						}
						else
						{
							MoteMaker.ThrowMetaIcon(animal.Position, animal.Map, ThingDefOf.Mote_ColonistFleeing);
							LifeStageUtility.PlayNearestLifestageSound(animal, (ls) => ls.soundCall);
							animal.mindState.StartFleeingBecauseOfPawnAction(pawn);
							animal.mindState.mentalStateHandler.TryStartMentalState(DefDatabase<MentalStateDef>.GetNamed("PanicFlee"));
						}
						pawn.jobs.EndCurrentJob(JobCondition.Incompletable);
					}
					else
					{
						Job gettin_bred = new Job(xxx.gettin_bred, pawn, animal);
						animal.jobs.StartJob(gettin_bred, JobCondition.InterruptForced, null, true);
						(animal.jobs.curDriver as JobDriver_GettinRaped)?.increase_time(duration);
					}
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
					xxx.sexTick(pawn, animal, false);
				/*
				if (pawn.IsHashIntervalTick (ticks_between_hits))
					roll_to_hit (pawn, animal);
					*/
				xxx.reduce_rest(animal, 1);
				xxx.reduce_rest(pawn, 2);

			};
			rape.AddFinishAction(delegate
			{
				//--Log.Message("[RJW] JobDriver_BestialityForMale::MakeNewToils() - finished violating");
				if (animal.jobs?.curDriver is JobDriver_GettinRaped)
					(animal.jobs.curDriver as JobDriver_GettinRaped).rapist_count -= 1;

				if (xxx.is_human(pawn))
					pawn.Drawer.renderer.graphics.ResolveApparelGraphics();
			});
			rape.defaultCompleteMode = ToilCompleteMode.Delay;
			rape.defaultDuration = duration;
			yield return rape;

			yield return new Toil
			{
				initAction = delegate
				{
					//Log.Message("[RJW] JobDriver_BestialityForMale::MakeNewToils() - creating aftersex toil");
					SexUtility.ProcessSex(pawn, animal);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}

		[SyncMethod]
		private Toil TalkToAnimal(Pawn pawn, Pawn animal)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				pawn.interactions.TryInteractWith(animal, SexUtility.AnimalSexChat);
			};
			//Rand.PopState();
			//Rand.PushState(RJW_Multiplayer.PredictableSeed());
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = Rand.Range(120, 220);
			return toil;
		}
	}
}