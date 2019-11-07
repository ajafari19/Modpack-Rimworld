using Verse;
using Verse.AI;
using System.Collections.Generic;
using Multiplayer.API;

namespace rjw
{
	/// <summary>
	/// Attempts to give a breeding job to an eligible animal.
	/// </summary>
	public class JobGiver_Breed : ThinkNode_JobGiver
	{
		[SyncMethod]
		protected override Job TryGiveJob(Pawn animal)
		{
			//Log.Message("[RJW] JobGiver_Breed::TryGiveJob( " + xxx.get_pawnname(animal) + " ) called0" + (SexUtility.ReadyForLovin(animal)));

			if (!SexUtility.ReadyForLovin(animal))
				return null;

			if(xxx.is_healthy(animal) && xxx.can_rape(animal))
			{
				//Log.Message("[RJW] JobGiver_Breed::TryGiveJob( " + xxx.get_pawnname(animal) + " ) called2");
				List<Pawn> valid_targets = new List<Pawn>();

				//search for desiganted target to sex
				if (animal.IsDesignatedBreedingAnimal())
				{
					Pawn designated_target = BreederHelper.find_designated_breeder(animal, animal.Map);
					if (designated_target != null)
					{
						valid_targets.Add(designated_target);
					}
				}

				//some weird shit happens, animal tries to rape and fails, needs investigation someday
				/*
				//search for animal to sex
				if (RJWSettings.animal_on_animal_enabled)
				{
					//Using bestiality target finder, since it works best for this.
					//search for same race mate
					if (!valid_targets.Any())
					{
						Pawn animal_target = BreederHelper.find_breeder_animal(animal, animal.Map);
						if (animal_target != null)
						{
							valid_targets.Add(animal_target);
						}
					}

					//search for any other animal/human to sex
					if (!valid_targets.Any())
					{
						Pawn animal_target = BreederHelper.find_breeder_animal(animal, animal.Map, false);
						if (animal_target != null)
						{
							valid_targets.Add(animal_target);
						}
					}

				}
				*/

				//Log.Message("[RJW] JobGiver_Breed::TryGiveJob( " + xxx.get_pawnname(animal) + " ) called3 - (" + ((target == null) ? "no target found" : xxx.get_pawnname(target))+") is the prisoner");

				if (valid_targets != null && valid_targets.Any())
				{
					//Rand.PopState();
					//Rand.PushState(RJW_Multiplayer.PredictableSeed());
					var target = valid_targets.RandomElement();
					//Log.Message("Target: " + xxx.get_pawnname(target));
					return new Job(DefDatabase<JobDef>.GetNamed("Breed"), target, animal);
				}
			}
			return null;
		}
	}
}