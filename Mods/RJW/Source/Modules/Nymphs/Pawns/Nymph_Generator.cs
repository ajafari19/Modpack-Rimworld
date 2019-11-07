using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using System;
using Multiplayer.API;

namespace rjw
{
	public static class nymph_generator
	{
		private static bool is_trait_conflicting_or_duplicate(Pawn pawn, Trait t)
		{
			foreach (var existing in pawn.story.traits.allTraits)
				if ((existing.def == t.def) || (t.def.ConflictsWith(existing)))
					return true;
			return false;
		}

		[SyncMethod]
		public static Gender setnymphgender()
		{
			//with males 100% its still 99%, coz im  to lazy to fix it
			//float rnd = Rand.Value;
			//Rand.PopState();
			//Rand.PushState(RJW_Multiplayer.PredictableSeed());
			float chance = RJWSettings.male_nymph_chance;

			Gender Pawngender = Rand.Chance(chance) ? Gender.Male: Gender.Female;
			//Log.Message("[RJW] setnymphsex: " + (rnd < chance) + " rnd:" + rnd + " chance:" + chance);
			//Log.Message("[RJW] setnymphsex: " + Pawngender);
			return Pawngender;
		}

		// Replaces a pawn's backstory and traits to turn it into a nymph
		[SyncMethod]
		public static void set_story(Pawn pawn)
		{
			var gen_sto = nymph_backstories.generate();

			pawn.story.childhood = gen_sto.child;
			pawn.story.adulthood = gen_sto.adult;
			
			// add broken body to broken nymph
			if (pawn.story.adulthood == nymph_backstories.adult.broken)
			{
				pawn.health.AddHediff(xxx.feelingBroken);
				//Rand.PopState();
				//Rand.PushState(RJW_Multiplayer.PredictableSeed());
				(pawn.health.hediffSet.GetFirstHediffOfDef(xxx.feelingBroken)).Severity = Rand.Range(0.4f, 1.0f);
			}

			//The mod More Trait Slots will adjust the max number of traits pawn can get, and therefore,
			//I need to collect pawns' traits and assign other_traits back to the pawn after adding the nymph_story traits.
			Stack<Trait> other_traits = new Stack<Trait>();
			int numberOfTotalTraits = 0;
			if (!pawn.story.traits.allTraits.NullOrEmpty())
			{
				foreach (Trait t in pawn.story.traits.allTraits)
				{
					other_traits.Push(t);
					++numberOfTotalTraits;
				}
			}

			pawn.story.traits.allTraits.Clear();
			var trait_count = 0;
			foreach (var t in gen_sto.traits)
			{
				pawn.story.traits.GainTrait(t);
				++trait_count;
			}
			while (trait_count < numberOfTotalTraits)
			{
				Trait t = other_traits.Pop();
				if (!is_trait_conflicting_or_duplicate(pawn, t))
					pawn.story.traits.GainTrait(t);
				++trait_count;
			}
		}

		[SyncMethod]
		private static int sum_previous_gains(SkillDef def, Pawn_StoryTracker sto, Pawn_AgeTracker age)
		{
			int total_gain = 0;
			int gain;

			// Gains from backstories
			if (sto.childhood.skillGainsResolved.TryGetValue(def, out gain))
				total_gain += gain;
			if (sto.adulthood.skillGainsResolved.TryGetValue(def, out gain))
				total_gain += gain;

			// Gains from traits
			foreach (var trait in sto.traits.allTraits)
				if (trait.CurrentData.skillGains.TryGetValue(def, out gain))
					total_gain += gain;

			// Gains from age
			//Rand.PopState();
			//Rand.PushState(RJW_Multiplayer.PredictableSeed());
			var rgain = Rand.Value * (float)total_gain * 0.35f;
			var age_factor = Mathf.Clamp01((age.AgeBiologicalYearsFloat - 17.0f) / 10.0f); // Assume nymphs are 17~27
			total_gain += (int)(age_factor * rgain);

			return Mathf.Clamp(total_gain, 0, 20);
		}

		// Set a nymph's initial skills & passions from backstory, traits, and age
		[SyncMethod]
		public static void set_skills(Pawn pawn)
		{
			foreach (var skill_def in DefDatabase<SkillDef>.AllDefsListForReading)
			{
				var rec = pawn.skills.GetSkill(skill_def);
				if (!rec.TotallyDisabled)
				{
					//Rand.PopState();
					//Rand.PushState(RJW_Multiplayer.PredictableSeed());
					rec.Level = sum_previous_gains(skill_def, pawn.story, pawn.ageTracker);
					rec.xpSinceLastLevel = rec.XpRequiredForLevelUp * Rand.Range(0.10f, 0.90f);

					var pas_cha = nymph_backstories.get_passion_chances(pawn.story.childhood, pawn.story.adulthood, skill_def);
					var rv = Rand.Value;
					if (rv < pas_cha.major) rec.passion = Passion.Major;
					else if (rv < pas_cha.minor) rec.passion = Passion.Minor;
					else rec.passion = Passion.None;
				}
				else
					rec.passion = Passion.None;
			}
		}

		// Apply nymphoverrides for generated pawn
		public static void nymph_override(Pawn pawn)
		{
			set_story(pawn);
			set_skills(pawn);
		}

		[SyncMethod]
		public static Pawn spawn_nymph(IntVec3 around_loc, ref Map map, Faction faction = null)
		{
			PawnKindDef pkd;
			{
				pkd = PawnKindDef.Named("Nymph");
				pkd.minGenerationAge = 18;
				pkd.maxGenerationAge = 27;
			}
			PawnGenerationRequest request = new PawnGenerationRequest(pkd,
												 faction,
												 PawnGenerationContext.NonPlayer,
												 map.Tile,	// tile(default is -1), since Inhabitant is true, then tile should have a value here; otherwise an error will pop
												 false, // Force generate new pawn
												 false, // Newborn
												 false, // Allow dead
												 false, // Allow downed
												 false, // Can generate pawn relations
												 false, // Must be capable of violence
												 0.0f, // Colonist relation chance factor
												 false, // Force add free warm layer if needed
												 true, // Allow gay
												 true, // Allow food
												 true, // Inhabitant
												 false, // Been in Cryosleep
												 false, // ForceRedressWorldPawnIfFormerColonist
												 false, // WorldPawnFactionDoesntMatter
												 c => (c.story.bodyType == BodyTypeDefOf.Female) || (c.story.bodyType == BodyTypeDefOf.Thin), // ValidatorPreGear
												 c => (c.story.bodyType == BodyTypeDefOf.Female) || (c.story.bodyType == BodyTypeDefOf.Thin), // ValidatorPostGear
												 null, // MinChanceToRedressWorldPawn
												 null, // Fixed biological age
												 null, // Fixed chronological age
												 setnymphgender(), // Fixed gender
												 null, // Fixed melanin
												 null); // Fixed last name


			//Rand.PopState();
			//Rand.PushState(RJW_Multiplayer.PredictableSeed());
			IntVec3 spawn_loc = CellFinder.RandomClosewalkCellNear(around_loc, map, 6);//RandomSpawnCellForPawnNear could be an alternative
			
			//Log.Message("[RJW] spawn_nymph1: " + request.FixedGender);

			//generate random pawn
			Pawn pawn = rjw_PawnGenerator.GeneratePawn(request);

			//i have no fucking idea why, but core PawnGenerator always generate females ignoring request settings
			//Pawn pawn = PawnGenerator.GeneratePawn(request); 
			//Log.Message("[RJW] spawn_nymph3: " + pawn.gender);

			//override random generated pawn stuff with nymph stuff
			nymph_override(pawn);
			GenSpawn.Spawn(pawn, spawn_loc, map);
			return pawn;
		}
	}
}