using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using Harmony;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;
using Multiplayer.API;
//using static RimWorld.Planet.CaravanInventoryUtility;
//using RimWorldChildren;

namespace rjw
{
	public static class Logger
	{
		private static readonly LogMessageQueue messageQueueRJW = new LogMessageQueue();
		public static void Message(string text)
		{
			bool DevModeEnabled = RJWSettings.DevMode;
			if (!DevModeEnabled) return;
			UnityEngine.Debug.Log(text);
			messageQueueRJW.Enqueue(new LogMessage(LogMessageType.Message, text, StackTraceUtility.ExtractStackTrace()));
		}
		public static void Warning(string text)
		{
			bool DevModeEnabled = RJWSettings.DevMode;
			if (!DevModeEnabled) return;
			UnityEngine.Debug.Log(text);
			messageQueueRJW.Enqueue(new LogMessage(LogMessageType.Warning, text, StackTraceUtility.ExtractStackTrace()));
		}
		public static void Error(string text)
		{
			bool DevModeEnabled = RJWSettings.DevMode;
			if (!DevModeEnabled) return;
			UnityEngine.Debug.Log(text);
			messageQueueRJW.Enqueue(new LogMessage(LogMessageType.Error, text, StackTraceUtility.ExtractStackTrace()));
		}

		public static TimeSpan Time(Action action)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			action();
			stopwatch.Stop();
			return stopwatch.Elapsed;
		}
	}

	public static class xxx
	{
		public static readonly BindingFlags ins_public_or_no = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		public static readonly config config = DefDatabase<config>.GetNamed("the_one");

		public const float base_sat_per_fuck = 0.40f;
		public const float base_attraction = 0.60f;
		public const float no_partner_ability = 0.8f;

		//HARDCODED MAGIC USED ACROSS DOZENS OF FILES, this is as bad place to put it as any other
		//Should at the very least be encompassed in the related designation type
		public static readonly int max_rapists_per_prisoner = 6;

		public static readonly TraitDef nymphomaniac = TraitDef.Named("Nymphomaniac");
		public static readonly TraitDef rapist = TraitDef.Named("Rapist");
		public static readonly TraitDef masochist = TraitDef.Named("Masochist");
		public static readonly TraitDef necrophiliac = TraitDef.Named("Necrophiliac");
		public static readonly TraitDef zoophile = TraitDef.Named("Zoophile");

		//CombatExtended Traits
		public static HediffDef MuscleSpasms;
		public static bool CombatExtendedIsActive;

		//RomanceDiversified Traits
		public static TraitDef straight;
		public static TraitDef bisexual;
		public static TraitDef asexual;
		public static TraitDef faithful;
		public static TraitDef philanderer;
		public static TraitDef polyamorous;
		public static bool RomanceDiversifiedIsActive; //A dirty way to check if the mod is active

		//Psychology Traits
		public static TraitDef prude;
		public static TraitDef lecher;
		public static TraitDef polygamous;
		public static bool PsychologyIsActive;

		//[SYR] Individuality
		public static bool IndividualityIsActive;

		//Rimworld of Magic
		public static bool RoMIsActive;

		//Consolidated Traits
		public static bool CTIsActive;

		//SimpleSlavery
		public static bool SimpleSlaveryIsActive;

		//Dubs Bad Hygiene
		public static bool DubsBadHygieneIsActive;

		//Alien Framework Traits
		public static TraitDef xenophobia; // Degrees: 1: xenophobe, -1: xenophile
		public static bool AlienFrameworkIsActive;

		//Children&Pregnancy Hediffs
		public static HediffDef babystate;
		public static bool RimWorldChildrenIsActive; //A dirty way to check if the mod is active

		//The Hediff to prevent reproduction
		public static readonly HediffDef sterilized = HediffDef.Named("Sterilized");

		//The Hediff for broken body(resulted from being raped as CP for too many times)
		public static readonly HediffDef feelingBroken = HediffDef.Named("FeelingBroken");

		public static PawnCapacityDef reproduction = DefDatabase<PawnCapacityDef>.GetNamed("Reproduction");

		public static readonly BodyPartDef genitalsDef = DefDatabase<BodyPartDef>.GetNamed("Genitals");
		public static readonly BodyPartDef breastsDef = DefDatabase<BodyPartDef>.GetNamed("Chest");
		public static readonly BodyPartDef anusDef = DefDatabase<BodyPartDef>.GetNamed("Anus");

		public static readonly ThoughtDef saw_rash_1 = DefDatabase<ThoughtDef>.GetNamed("SawDiseasedPrivates1");
		public static readonly ThoughtDef saw_rash_2 = DefDatabase<ThoughtDef>.GetNamed("SawDiseasedPrivates2");
		public static readonly ThoughtDef saw_rash_3 = DefDatabase<ThoughtDef>.GetNamed("SawDiseasedPrivates3");

		public static readonly ThoughtDef got_raped = DefDatabase<ThoughtDef>.GetNamed("GotRaped");
		public static readonly ThoughtDef got_raped_unconscious = DefDatabase<ThoughtDef>.GetNamed("GotRapedUnconscious");
		public static readonly ThoughtDef got_bred = DefDatabase<ThoughtDef>.GetNamed("GotBredByAnimal");
		public static readonly ThoughtDef got_licked = DefDatabase<ThoughtDef>.GetNamed("GotLickedByAnimal");
		public static readonly ThoughtDef got_groped = DefDatabase<ThoughtDef>.GetNamed("GotGropedByAnimal");

		public static readonly ThoughtDef masochist_got_raped = DefDatabase<ThoughtDef>.GetNamed("MasochistGotRaped");
		public static readonly ThoughtDef masochist_got_bred = DefDatabase<ThoughtDef>.GetNamed("MasochistGotBredByAnimal");
		public static readonly ThoughtDef masochist_got_licked = DefDatabase<ThoughtDef>.GetNamed("MasochistGotLickedByAnimal");
		public static readonly ThoughtDef masochist_got_groped = DefDatabase<ThoughtDef>.GetNamed("MasochistGotGropedByAnimal");
		public static readonly ThoughtDef allowed_animal_to_breed = DefDatabase<ThoughtDef>.GetNamed("AllowedAnimalToBreed");
		public static readonly ThoughtDef allowed_animal_to_lick = DefDatabase<ThoughtDef>.GetNamed("AllowedAnimalToLick");
		public static readonly ThoughtDef allowed_animal_to_grope = DefDatabase<ThoughtDef>.GetNamed("AllowedAnimalToGrope");
		public static readonly ThoughtDef zoophile_got_bred = DefDatabase<ThoughtDef>.GetNamed("ZoophileGotBredByAnimal");
		public static readonly ThoughtDef zoophile_got_licked = DefDatabase<ThoughtDef>.GetNamed("ZoophileGotLickedByAnimal");
		public static readonly ThoughtDef zoophile_got_groped = DefDatabase<ThoughtDef>.GetNamed("ZoophileGotGropedByAnimal");
		public static readonly ThoughtDef hate_my_rapist = DefDatabase<ThoughtDef>.GetNamed("HateMyRapist");
		public static readonly ThoughtDef kinda_like_my_rapist = DefDatabase<ThoughtDef>.GetNamed("KindaLikeMyRapist");
		public static readonly ThoughtDef allowed_me_to_get_raped = DefDatabase<ThoughtDef>.GetNamed("AllowedMeToGetRaped");
		public static readonly ThoughtDef stole_some_lovin = DefDatabase<ThoughtDef>.GetNamed("StoleSomeLovin");
		public static readonly ThoughtDef bloodlust_stole_some_lovin = DefDatabase<ThoughtDef>.GetNamed("BloodlustStoleSomeLovin");
		public static readonly ThoughtDef violated_corpse = DefDatabase<ThoughtDef>.GetNamed("ViolatedCorpse");
		public static readonly ThoughtDef gave_virginity = DefDatabase<ThoughtDef>.GetNamed("FortunateGaveVirginity");
		public static readonly ThoughtDef lost_virginity = DefDatabase<ThoughtDef>.GetNamed("UnfortunateLostVirginity");
		public static readonly ThoughtDef VanillaGotSomeLovin = DefDatabase<ThoughtDef>.GetNamed("GotSomeLovin");
		public static readonly ThoughtDef took_virginity = DefDatabase<ThoughtDef>.GetNamed("TookVirginity");

		public static readonly JobDef fappin = DefDatabase<JobDef>.GetNamed("Fappin");
		public static readonly JobDef quickfap = DefDatabase<JobDef>.GetNamed("QuickFap");
		public static readonly JobDef gettin_loved = DefDatabase<JobDef>.GetNamed("GettinLoved");
		public static readonly JobDef casual_sex = DefDatabase<JobDef>.GetNamed("JoinInBed");
		public static readonly JobDef gettin_raped = DefDatabase<JobDef>.GetNamed("GettinRaped");
		public static readonly JobDef gettin_bred = DefDatabase<JobDef>.GetNamed("GettinBred");
		public static readonly JobDef comfort_prisoner_rapin = DefDatabase<JobDef>.GetNamed("ComfortPrisonerRapin");
		public static readonly JobDef RapeEnemy = DefDatabase<JobDef>.GetNamed("RapeEnemy");
		public static readonly JobDef violate_corpse = DefDatabase<JobDef>.GetNamed("ViolateCorpse");
		public static readonly JobDef bestiality = DefDatabase<JobDef>.GetNamed("Bestiality");
		public static readonly JobDef bestialityForFemale = DefDatabase<JobDef>.GetNamed("BestialityForFemale");
		public static readonly JobDef random_rape = DefDatabase<JobDef>.GetNamed("RandomRape");
		public static readonly JobDef whore_inviting_visitors = DefDatabase<JobDef>.GetNamed("WhoreInvitingVisitors");
		public static readonly JobDef whore_is_serving_visitors = DefDatabase<JobDef>.GetNamed("WhoreIsServingVisitors");
		public static readonly JobDef struggle_in_BondageGear = DefDatabase<JobDef>.GetNamed("StruggleInBondageGear");
		public static readonly JobDef unlock_BondageGear = DefDatabase<JobDef>.GetNamed("UnlockBondageGear");
		public static readonly JobDef give_BondageGear = DefDatabase<JobDef>.GetNamed("GiveBondageGear");
		public static readonly JobDef animalBreed = DefDatabase<JobDef>.GetNamed("Breed");

		public static readonly ThingDef mote_noheart = ThingDef.Named("Mote_NoHeart");

		public static readonly StatDef sex_stat = StatDef.Named("SexAbility");
		public static readonly StatDef vulnerability_stat = StatDef.Named("Vulnerability");
		public static readonly StatDef sex_drive_stat = StatDef.Named("SexFrequency");

		public static readonly RecordDef GetRapedAsComfortPrisoner = DefDatabase<RecordDef>.GetNamed("GetRapedAsComfortPrisoner");
		public static readonly RecordDef CountOfFappin = DefDatabase<RecordDef>.GetNamed("CountOfFappin");
		public static readonly RecordDef CountOfWhore = DefDatabase<RecordDef>.GetNamed("CountOfWhore");
		public static readonly RecordDef EarnedMoneyByWhore = DefDatabase<RecordDef>.GetNamed("EarnedMoneyByWhore");
		public static readonly RecordDef CountOfSex = DefDatabase<RecordDef>.GetNamed("CountOfSex");
		public static readonly RecordDef CountOfSexWithHumanlikes = DefDatabase<RecordDef>.GetNamed("CountOfSexWithHumanlikes");
		public static readonly RecordDef CountOfSexWithAnimals = DefDatabase<RecordDef>.GetNamed("CountOfSexWithAnimals");
		public static readonly RecordDef CountOfSexWithInsects = DefDatabase<RecordDef>.GetNamed("CountOfSexWithInsects");
		public static readonly RecordDef CountOfSexWithOthers = DefDatabase<RecordDef>.GetNamed("CountOfSexWithOthers");
		public static readonly RecordDef CountOfSexWithCorpse = DefDatabase<RecordDef>.GetNamed("CountOfSexWithCorpse");
		public static readonly RecordDef CountOfRapedHumanlikes = DefDatabase<RecordDef>.GetNamed("CountOfRapedHumanlikes");
		public static readonly RecordDef CountOfBeenRapedByHumanlikes = DefDatabase<RecordDef>.GetNamed("CountOfBeenRapedByHumanlikes");
		public static readonly RecordDef CountOfRapedAnimals = DefDatabase<RecordDef>.GetNamed("CountOfRapedAnimals");
		public static readonly RecordDef CountOfBeenRapedByAnimals = DefDatabase<RecordDef>.GetNamed("CountOfBeenRapedByAnimals");
		public static readonly RecordDef CountOfRapedInsects = DefDatabase<RecordDef>.GetNamed("CountOfRapedInsects");
		public static readonly RecordDef CountOfBeenRapedByInsects = DefDatabase<RecordDef>.GetNamed("CountOfBeenRapedByInsects");
		public static readonly RecordDef CountOfRapedOthers = DefDatabase<RecordDef>.GetNamed("CountOfRapedOthers");
		public static readonly RecordDef CountOfBeenRapedByOthers = DefDatabase<RecordDef>.GetNamed("CountOfBeenRapedByOthers");
		public static readonly RecordDef CountOfBirthHuman = DefDatabase<RecordDef>.GetNamed("CountOfBirthHuman");
		public static readonly RecordDef CountOfBirthAnimal = DefDatabase<RecordDef>.GetNamed("CountOfBirthAnimal");
		public static readonly RecordDef CountOfBirthEgg = DefDatabase<RecordDef>.GetNamed("CountOfBirthEgg");

		public enum rjwSextype { None, Vaginal, Anal, Oral, Masturbation, DoublePenetration, Boobjob, Handjob, Footjob, Fingering, Scissoring, MutualMasturbation, Fisting, MechImplant }

		private static readonly SimpleCurve attractiveness_from_age_male = new SimpleCurve
		{
			new CurvePoint(0f,  0.0f),
			new CurvePoint(4f,  0.1f),
			new CurvePoint(5f,  0.2f),
			new CurvePoint(15f, 0.8f),
			new CurvePoint(20f, 1.0f),
			new CurvePoint(32f, 1.0f),
			new CurvePoint(40f, 0.9f),
			new CurvePoint(45f, 0.77f),
			new CurvePoint(50f, 0.7f),
			new CurvePoint(55f, 0.5f),
			new CurvePoint(75f, 0.1f),
			new CurvePoint(100f, 0f)
		};

		//These were way too low and could be increased further. Anything under 0.7f pretty much stops sex from happening.
		private static readonly SimpleCurve attractiveness_from_age_female = new SimpleCurve
		{
			new CurvePoint(0f,  0.0f),
			new CurvePoint(4f,  0.1f),
			new CurvePoint(5f,  0.2f),
			new CurvePoint(14f, 0.8f),
			new CurvePoint(28f, 1.0f),
			new CurvePoint(30f, 1.0f),
			new CurvePoint(45f, 0.7f),
			new CurvePoint(55f, 0.3f),
			new CurvePoint(75f, 0.1f),
			new CurvePoint(100f, 0f)
		};

		private static readonly SimpleCurve fuckability_per_reserved = new SimpleCurve
		{
			new CurvePoint(0f, 1.0f),
			new CurvePoint(0.3f, 0.4f),
			new CurvePoint(1f, 0.2f)
		};

		public static void bootstrap(Map m)
		{
			if (m.GetComponent<MapCom_Injector>() == null)
				m.components.Add(new MapCom_Injector(m));
		}

		//<Summary>Simple method that quickly checks for match from a list.</Summary>
		public static bool ContainsAny(this string haystack, params string[] needles) { return needles.Any(haystack.Contains); }

		public static bool has_traits(Pawn pawn)
		{
			return pawn?.story?.traits != null;
		}

		public static bool has_quirk(Pawn pawn, string quirk)
		{
			return pawn != null && is_human(pawn) && CompRJW.Comp(pawn).quirks.ToString().Contains(quirk);
		}

		[SyncMethod]
		public static string random_pick_a_trait(this Pawn pawn)
		{
			//Rand.PopState();
			//Rand.PushState(RJW_Multiplayer.PredictableSeed());
			return has_traits(pawn) ? pawn.story.traits.allTraits.RandomElement().def.defName : null;
		}

		public static bool is_psychopath(Pawn pawn)
		{
			return has_traits(pawn) && pawn.story.traits.HasTrait(TraitDefOf.Psychopath);
		}

		public static bool is_ascetic(Pawn pawn)
		{
			return has_traits(pawn) && pawn.story.traits.HasTrait(TraitDefOf.Ascetic);
		}

		public static bool is_bloodlust(Pawn pawn)
		{
			return has_traits(pawn) && pawn.story.traits.HasTrait(TraitDefOf.Bloodlust);
		}

		public static bool is_brawler(Pawn pawn)
		{
			return has_traits(pawn) && pawn.story.traits.HasTrait(TraitDefOf.Brawler);
		}

		public static bool is_kind(Pawn pawn)
		{
			return has_traits(pawn) && pawn.story.traits.HasTrait(TraitDefOf.Kind);
		}

		public static bool is_rapist(Pawn pawn)
		{
			return has_traits(pawn) && pawn.story.traits.HasTrait(rapist);
		}

		public static bool is_necrophiliac(Pawn pawn)
		{
			return has_traits(pawn) && pawn.story.traits.HasTrait(necrophiliac);
		}

		public static bool is_zoophile(Pawn pawn)
		{
			return has_traits(pawn) && pawn.story.traits.HasTrait(zoophile);
		}

		public static bool is_masochist(Pawn pawn)
		{
			return has_traits(pawn) && pawn.story.traits.HasTrait(TraitDef.Named("Masochist"));
		}

		public static bool is_nympho(Pawn pawn)
		{
			return has_traits(pawn) && pawn.story.traits.HasTrait(nymphomaniac);
		}
		public static bool is_slime(Pawn pawn)
		{
			string racename = pawn.kindDef.race.defName.ToLower();
			//if (Prefs.DevMode) Log.Message("[RJW] is_slime " + xxx.get_pawnname(pawn) + " " + racename.Contains("slime"));

			return racename.Contains("slime");
		}

		public static bool is_demon(Pawn pawn)
		{
			string racename = pawn.kindDef.race.defName.ToLower();
			//if (Prefs.DevMode) Log.Message("[RJW] is_demon " + xxx.get_pawnname(pawn) + " " + racename.Contains("demon"));

			return racename.Contains("demon");
		}

		public static bool is_asexual(Pawn pawn) => CompRJW.Comp(pawn).orientation == Orientation.Asexual;
		public static bool is_bisexual(Pawn pawn) => CompRJW.Comp(pawn).orientation == Orientation.Bisexual;
		public static bool is_homosexual(Pawn pawn) => (CompRJW.Comp(pawn).orientation == Orientation.Homosexual || CompRJW.Comp(pawn).orientation == Orientation.MostlyHomosexual);
		public static bool is_heterosexual(Pawn pawn) => (CompRJW.Comp(pawn).orientation == Orientation.Heterosexual || CompRJW.Comp(pawn).orientation == Orientation.MostlyHeterosexual);
		public static bool is_pansexual(Pawn pawn) => CompRJW.Comp(pawn).orientation == Orientation.Pansexual;

		public static bool is_slave(Pawn pawn)
		{
			if (SimpleSlaveryIsActive)
				return pawn?.health.hediffSet.HasHediff(HediffDef.Named("Enslaved")) ?? false;
			else
				return false;
		}

		// A quick check on whether the pawn has the two traits
		// It's used in determine the eligibility of CP raping for the non-futa women
		// Before using it, you should make sure the pawn has traits.
		public static bool is_nympho_or_rapist_or_zoophile(Pawn pawn)
		{
			if (!has_traits(pawn)) { return false; }
			return (is_rapist(pawn) || is_nympho(pawn) || is_zoophile(pawn));
		}

		//Humanoid Alien Framework traits
		public static bool is_xenophile(Pawn pawn)
		{
			if (!has_traits(pawn) || !AlienFrameworkIsActive) { return false; }
			return pawn.story.traits.DegreeOfTrait(xenophobia) == -1;
		}

		public static bool is_xenophobe(Pawn pawn)
		{
			if (!has_traits(pawn) || !AlienFrameworkIsActive) { return false; }
			return pawn.story.traits.DegreeOfTrait(xenophobia) == 1;
		}

		public static bool is_whore(Pawn pawn)
		{
			if (!has_traits(pawn)) { return false; }
			return pawn != null && pawn.IsDesignatedService() && (!RomanceDiversifiedIsActive || !pawn.story.traits.HasTrait(asexual));
			//return (pawn != null && pawn.ownership != null && pawn.ownership.OwnedBed is Building_WhoreBed && (!xxx.RomanceDiversifiedIsActive || !pawn.story.traits.HasTrait(xxx.asexual)));
		}

		public static bool is_lecher(Pawn pawn)
		{
			if (!has_traits(pawn)) { return false; }
			return RomanceDiversifiedIsActive && pawn.story.traits.HasTrait(philanderer) || PsychologyIsActive && pawn.story.traits.HasTrait(lecher);
		}

		public static bool is_prude(Pawn pawn)
		{
			if (!has_traits(pawn)) { return false; }
			return RomanceDiversifiedIsActive && pawn.story.traits.HasTrait(faithful) || PsychologyIsActive && pawn.story.traits.HasTrait(prude);
		}

		public static bool is_animal(Pawn pawn)
		{
			//return !pawn.RaceProps.Humanlike;
			//Edited by nizhuan-jjr:to make Misc.Robots not allowed to have sex. This change makes those robots not counted as animals.
			//return (pawn.RaceProps.Animal || (pawn.RaceProps.intelligence == Intelligence.Animal));
			return pawn?.RaceProps?.Animal ?? false;
		}

		public static bool is_insect(Pawn pawn)
		{
			//Added by Hoge: Insects are also animal. you need check is_insect before is_animal.
			//return pawn.RaceProps.FleshType.defName == "Insectoid";
			//Added by Ed86: its better? isnt it?
			if (pawn == null) return false;

			bool isit = pawn.RaceProps.FleshType == FleshTypeDefOf.Insectoid
						//genetic rim
						|| pawn.RaceProps.FleshType.defName.Contains("GR_Insectoid");
			//Log.Message("is_insect " + get_pawnname(pawn) + " - " + isit);
			return isit;
		}

		public static bool is_mechanoid(Pawn pawn)
		{
			//Added by nizhuan-jjr:to make Misc.Robots not allowed to have sex. Note:Misc.MAI is not a mechanoid.
			if (pawn == null) return false;
			if (AndroidsCompatibility.IsAndroid(pawn)) return false;

			bool isit = pawn.RaceProps.IsMechanoid
						|| pawn.RaceProps.FleshType == FleshTypeDefOf.Mechanoid
						//genetic rim
						|| pawn.RaceProps.FleshType.defName.Contains("GR_Mechanoid")
						//android tiers
						|| pawn.RaceProps.FleshType.defName.Contains("MechanisedInfantry")
						|| pawn.RaceProps.FleshType.defName.Contains("Android");
			//Log.Message("is_mechanoid " + get_pawnname(pawn) + " - " + isit);
			return isit;
		}

		public static bool is_tooluser(Pawn pawn)
		{
			return pawn.RaceProps.ToolUser;
		}

		public static bool is_human(Pawn pawn)
		{
			if (pawn == null) return false;

			return pawn.RaceProps.Humanlike;//||pawn.kindDef.race == ThingDefOf.Human
		}

		public static bool is_female(Pawn pawn)
		{
			return pawn.gender == Gender.Female;
		}
		public static bool is_male(Pawn pawn)
		{
			return pawn.gender == Gender.Male;
		}

		public static bool is_healthy(Pawn pawn)
		{
			return !pawn.Dead &&
				pawn.health.capacities.CanBeAwake &&
				pawn.health.hediffSet.BleedRateTotal <= 0.0f &&
				pawn.health.hediffSet.PainTotal < config.significant_pain_threshold;
		}

		public static bool is_healthy_enough(Pawn pawn)
		{
			return !pawn.Dead &&
				pawn.health.capacities.CanBeAwake &&
				pawn.health.hediffSet.BleedRateTotal <= 0.1f;
		}
		
		//pawn can initiate action or respond - whoring, etc
		public static bool IsTargetPawnOkay(Pawn pawn)
		{
			return xxx.is_healthy(pawn) && !pawn.Downed && !pawn.Suspended;
		}

		public static bool is_not_dying(Pawn pawn)
		{
			return !pawn.Dead &&
				pawn.health.hediffSet.BleedRateTotal <= 0.3f;
		}

		public static bool is_starved(Pawn pawn)
		{
			return pawn?.needs?.food != null &&
				pawn.needs.food.Starving;
		}
		public static float bleedingRate(Pawn pawn)
		{
			return pawn?.health?.hediffSet?.BleedRateTotal ?? 0f;
		}

		public static bool is_Virgin(Pawn pawn)
		{
			//if (RJWSettings.DevMode) Log.Message("[RJW]xxx::is_Virgin check:" +get_pawnname(pawn));
			if (pawn.relations != null)
				if (pawn.relations.ChildrenCount > 0)
				{
					//if (RJWSettings.DevMode) Log.Message("[RJW]xxx::is_Virgin " + " ChildrenCount " + pawn.relations.ChildrenCount);
					return false;
				}

			if (!(
				pawn.records.GetValue(GetRapedAsComfortPrisoner) == 0 &&
				pawn.records.GetValue(CountOfSex) == 0 &&
				pawn.records.GetValue(CountOfSexWithHumanlikes) == 0 &&
				pawn.records.GetValue(CountOfSexWithAnimals) == 0 &&
				pawn.records.GetValue(CountOfSexWithInsects) == 0 &&
				pawn.records.GetValue(CountOfSexWithOthers) == 0 &&
				pawn.records.GetValue(CountOfSexWithCorpse) == 0 &&
				pawn.records.GetValue(CountOfWhore) == 0 &&
				pawn.records.GetValue(CountOfRapedHumanlikes) == 0 &&
				pawn.records.GetValue(CountOfBeenRapedByHumanlikes) == 0 &&
				pawn.records.GetValue(CountOfRapedAnimals) == 0 &&
				pawn.records.GetValue(CountOfBeenRapedByAnimals) == 0 &&
				pawn.records.GetValue(CountOfRapedInsects) == 0 &&
				pawn.records.GetValue(CountOfBeenRapedByInsects) == 0 &&
				pawn.records.GetValue(CountOfRapedOthers) == 0 &&
				pawn.records.GetValue(CountOfBeenRapedByOthers) == 0 &&
				pawn.records.GetAsInt(xxx.CountOfBirthHuman) == 0 &&
				pawn.records.GetAsInt(xxx.CountOfBirthAnimal) == 0 &&
				pawn.records.GetAsInt(xxx.CountOfBirthEgg) == 0
				))
			{
				//if (RJWSettings.DevMode) Log.Message("[RJW]xxx::is_Virgin " + "records check fail");
				return false;
			}
			return true;
		}

		public static string get_pawnname(Pawn who)
		{
			//Log.Message("[RJW]xxx::get_pawnname is "+ who.KindLabelDefinite());
			//Log.Message("[RJW]xxx::get_pawnname is "+ who.KindLabelIndefinite());
			if (who == null) return "null";

			string name = who.Label;
			if (name != null)
			{
				if (who.Name?.ToStringShort != null)
					name = who.Name.ToStringShort;
			}
			else
				name = "noname";

			return name;
		}

		public static bool is_gettin_rapeNow(Pawn pawn)
		{
			if (pawn?.jobs?.curDriver != null)
			{
				return pawn.jobs.curDriver.GetType() == typeof(JobDriver_GettinRaped);
			}
			return false;
		}

		public static bool can_path_to_target(Pawn pawn, IntVec3 Position)
		{
			//probably less taxing, but ignores walls
			if (RJWSettings.maxDistancetowalk < 100)
				return pawn.Position.DistanceToSquared(Position) < RJWSettings.maxDistancetowalk;

			//probably more taxing, using real path
			bool canit = true;
			PawnPath pawnPath = pawn.Map.pathFinder.FindPath(pawn.Position, Position, pawn, PathEndMode.OnCell);
			if (pawnPath.TotalCost > RJWSettings.maxDistancetowalk)
				canit = false;// too far
			pawnPath.Dispose();
			return canit;
		}

		public static void reduce_rest(Pawn pawn, int x = 1)
		{
			if (has_quirk(pawn, "Vigorous")) x -= 1;

			Need_Rest need_rest = pawn.needs.TryGetNeed<Need_Rest>();
			if (need_rest == null)
				return;

			need_rest.CurLevel -= need_rest.RestFallPerTick * x;
		}

		public static float need_some_sex(Pawn pawn)
		{
			// 3=> always horny for non humanlikes
			float horniness_degree = 3f;
			Need_Sex need_sex = pawn.needs.TryGetNeed<Need_Sex>();
			if (need_sex == null) return horniness_degree;
			if (need_sex.CurLevel < need_sex.thresh_frustrated()) horniness_degree = 3f;
			else if (need_sex.CurLevel < need_sex.thresh_horny()) horniness_degree = 2f;
			else if (need_sex.CurLevel < need_sex.thresh_satisfied()) horniness_degree = 1f;
			else horniness_degree = 0f;
			return horniness_degree;
		}

		public static bool is_frustrated(Pawn pawn)
		{
			Need_Sex need_sex = pawn.needs.TryGetNeed<Need_Sex>();
			if (need_sex == null) return false;
			return need_sex.CurLevel < need_sex.thresh_frustrated();
		}

		public static bool is_horny(Pawn pawn)
		{
			Need_Sex need_sex = pawn.needs.TryGetNeed<Need_Sex>();
			if (need_sex == null) return false;
			return need_sex.CurLevel < need_sex.thresh_horny();
		}

		/// <summary> Checks to see if the pawn has any partners who don't have a Polyamorous/Polygamous trait; aka someone who'd get mad about sleeping around. </summary>
		/// <returns> True if the pawn has at least one romantic partner who does not have a poly trait. False if no partners or all partners are poly. </returns>
		public static bool HasNonPolyPartnerOnCurrentMap(Pawn pawn)
		{
			// If they don't have a partner at all we can bail right away.
			if (!LovePartnerRelationUtility.HasAnyLovePartner(pawn))
				return false;

			// They have a partner and a mod that adds a poly trait, so check each partner to see if they're poly.
			foreach (DirectPawnRelation relation in pawn.relations.DirectRelations)
			{
				if (relation.def != PawnRelationDefOf.Lover &&
					relation.def != PawnRelationDefOf.Fiance &&
					relation.def != PawnRelationDefOf.Spouse)
				{
					continue;
				}

				// Dead partners don't count.  And stasis'd partners will never find out!
				if (relation.otherPawn.Dead || relation.otherPawn.Suspended)
					continue;

				// Neither does anyone on another map because cheating away from home is obviously never ever discovered
				if (pawn.Map == null || relation.otherPawn.Map == null || relation.otherPawn.Map != pawn.Map)
					continue;

				if ((RomanceDiversifiedIsActive && relation.otherPawn.story.traits.HasTrait(polyamorous)) ||
					(PsychologyIsActive && relation.otherPawn.story.traits.HasTrait(polygamous)))
				{
					// We have a partner who has the poly trait!  But they could have multiple partners so keep checking.
					continue;
				}

				// We found a partner who doesn't have a poly trait.
				return true;
			}

			// If we got here then we checked every partner and all of them had a poly trait, so they don't have a non-poly partner.
			return false;
		}

		public static Gender opposite_gender(Gender g)
		{
			switch (g)
			{
				case Gender.Male:
					return Gender.Female;
				case Gender.Female:
					return Gender.Male;
				default:
					return Gender.None;
			}
		}

		public static float get_sex_ability(Pawn pawn)
		{
			try
			{
				return pawn.GetStatValue(sex_stat, false);
			}
			catch (NullReferenceException)
			//not seeded with stats, error for non humanlikes/corpses
			//this and below should probably be rewriten to do calculations here
			{
				//Log.Warning(e.ToString());
				return 1f;
			}
		}

		public static float get_vulnerability(Pawn pawn)
		{
			try
			{
				return pawn.GetStatValue(vulnerability_stat, false);
			}
			catch (NullReferenceException)
			//not seeded with stats, error for non humanlikes/corpses
			{
				//Log.Warning(e.ToString());
				return 1f;
			}
		}

		public static float get_sex_drive(Pawn pawn)
		{
			try
			{
				return pawn.GetStatValue(sex_drive_stat, false);
			}
			catch (NullReferenceException)
			//not seeded with stats, error for non humanlikes/corpses
			{
				//Log.Warning(e.ToString());
				return 1f;
			}
		}

		public static bool isSingleOrPartnerNotHere(Pawn pawn)
		{
			return LovePartnerRelationUtility.ExistingLovePartner(pawn) == null || LovePartnerRelationUtility.ExistingLovePartner(pawn).Map != pawn.Map;
		}
		//do loving ??
		//oral not included
		public static bool can_do_loving(Pawn pawn)
		{
			if (is_mechanoid(pawn))
				return false;

			if (is_human(pawn))
			{
				int age = pawn.ageTracker.AgeBiologicalYears;

				if (age < RJWSettings.sex_minimum_age)
					return false;

				return get_sex_ability(pawn) > 0.0f;
			}
			if (is_animal(pawn))
			{
				if (!(RJWSettings.bestiality_enabled || RJWSettings.animal_on_animal_enabled))
					return false;

				//CurLifeStageIndex for insects since they are not reproductive
				return (pawn.ageTracker.CurLifeStageIndex >= 2 || pawn.ageTracker.CurLifeStage.reproductive);
			}
			return false;
		}
		
		// Penetrative organ check.
		public static bool can_fuck(Pawn pawn)
		{
			//this may cause problems with human mechanoids, like misc. bots or other custom race mechanoids
			if (is_mechanoid(pawn))
				return true;

			if (!Genital_Helper.has_penis(pawn) && !Genital_Helper.has_penis_infertile(pawn) && !Genital_Helper.has_ovipositorF(pawn)) return false;

			if (is_human(pawn))
			{
				int age = pawn.ageTracker.AgeBiologicalYears;

				if (age < RJWSettings.sex_minimum_age)
					return false;

				return !Genital_Helper.genitals_blocked(pawn);
			}
			if (is_animal(pawn))
			{
				if (!(RJWSettings.bestiality_enabled || RJWSettings.animal_on_animal_enabled))
					return false;

				//CurLifeStageIndex for insects since they are not reproductive
				return (pawn.ageTracker.CurLifeStageIndex >= 2 || pawn.ageTracker.CurLifeStage.reproductive);
			}
			return false;
		}
		
		// Orifice check.
		public static bool can_be_fucked(Pawn pawn)
		{
			if (is_mechanoid(pawn))
				return false;

			if (!Genital_Helper.has_vagina(pawn) && !Genital_Helper.has_anus(pawn)) return false;

			if (is_human(pawn))
			{
				int age = pawn.ageTracker.AgeBiologicalYears;

				if (age < RJWSettings.sex_minimum_age)
					return false;

				return (Genital_Helper.has_vagina(pawn) && !Genital_Helper.genitals_blocked(pawn))
					|| (Genital_Helper.has_anus(pawn) && !Genital_Helper.anus_blocked(pawn));
			}
			if (is_animal(pawn))
			{
				if (!(RJWSettings.bestiality_enabled || RJWSettings.animal_on_animal_enabled))
					return false;

				//CurLifeStageIndex for insects since they are not reproductive
				return (pawn.ageTracker.CurLifeStageIndex >= 2 || pawn.ageTracker.CurLifeStage.reproductive);
			}
			return false;
		}

		public static bool can_rape(Pawn pawn)
		{
			if (!RJWSettings.rape_enabled) return false;

			if (is_mechanoid(pawn))
				return true;

			if (is_human(pawn))
			{
				int age = pawn.ageTracker.AgeBiologicalYears;

				if (age < RJWSettings.sex_minimum_age)
					return false;

				if (RJWSettings.WildMode)
					return true;

				bool has_penis = Genital_Helper.has_penis(pawn) || Genital_Helper.has_penis_infertile(pawn) || Genital_Helper.has_ovipositorF(pawn);
				return need_some_sex(pawn) > 0
					   && !Genital_Helper.genitals_blocked(pawn)
					   && (!is_female(pawn) ? has_penis :
						   has_penis || get_vulnerability(pawn) <= RJWSettings.nonFutaWomenRaping_MaxVulnerability);
			}
			if (is_animal(pawn))
			{
				if (!(RJWSettings.bestiality_enabled || RJWSettings.animal_on_animal_enabled))
					return false;

				//CurLifeStageIndex for insects since they are not reproductive
				return (pawn.ageTracker.CurLifeStageIndex >= 2 || pawn.ageTracker.CurLifeStage.reproductive);
			}
			return false;
		}

		public static bool can_get_raped(Pawn pawn)
		{
			// Pawns can still get raped even if their genitals are destroyed/removed.
			// Animals can always be raped regardless of age
			if (is_human(pawn))
			{
				int age = pawn.ageTracker.AgeBiologicalYears;

				if (age < RJWSettings.sex_minimum_age)
					return false;

				return get_sex_ability(pawn) > 0.0f
					&& !Genital_Helper.genitals_blocked(pawn)
					&& (!(RJWSettings.rapee_MinVulnerability_human < 0)	&& get_vulnerability(pawn) >= RJWSettings.rapee_MinVulnerability_human);
			}

			if (is_animal(pawn))
			{
				if (!(RJWSettings.bestiality_enabled || RJWSettings.animal_on_animal_enabled))
					return false;

				//CurLifeStageIndex for insects since they are not reproductive
				return (pawn.ageTracker.CurLifeStageIndex >= 2 || pawn.ageTracker.CurLifeStage.reproductive);
			}
			return false;
		}

		/// <summary>
		///	Returns boolean, no more messing around with floats.
		/// Just a simple 'Would rape? True/false'.
		/// </summary>
		[SyncMethod]
		public static bool would_rape(Pawn rapist, Pawn rapee)
		{
			float rape_factor = 0.3f; // start at 30%

			float vulnerabilityFucker = get_vulnerability(rapist); //0 to 3
			float vulnerabilityPartner = get_vulnerability(rapee); //0 to 3

			// More inclined to rape someone from another faction.
			if (rapist.HostileTo(rapee) || rapist.Faction != rapee.Faction)
				rape_factor += 0.25f;

			// More inclined to rape if the target is designated as CP.
			if (rapee.IsDesignatedComfort())
				rape_factor += 0.25f;

			// More inclined to rape when horny.
			Need_Sex horniness = rapist.needs.TryGetNeed<Need_Sex>();
			if (!is_animal(rapist) && horniness?.CurLevel <= horniness?.thresh_horny())
			{
				rape_factor += 0.25f;
			}

			if (is_animal(rapist))
			{
				if (vulnerabilityFucker < vulnerabilityPartner)
					rape_factor -= 0.1f;
				else
					rape_factor += 0.25f;
			}
			else if (is_animal(rapee))
			{
				if (is_zoophile(rapist))
					rape_factor += 0.5f;
				else
					rape_factor -= 0.2f;
			}
			else
			{
				rape_factor *= 0.5f + Mathf.InverseLerp(vulnerabilityFucker, 3f, vulnerabilityPartner);
			}

			if (rapist.health.hediffSet.HasHediff(HediffDef.Named("AlcoholHigh")))
				rape_factor *= 1.25f; //too drunk to care

			// Increase factor from traits.
			if (is_rapist(rapist))
				rape_factor *= 1.5f;
			if (is_nympho(rapist))
				rape_factor *= 1.25f;
			if (is_bloodlust(rapist))
				rape_factor *= 1.2f;
			if (is_psychopath(rapist))
				rape_factor *= 1.2f;
			if (is_masochist(rapee))
				rape_factor *= 1.2f;

			// Lower factor from traits.
			if (is_masochist(rapist))
				rape_factor *= 0.8f;

			if (rapist.needs.joy != null && rapist.needs.joy.CurLevel < 0.1f) // The rapist is really bored...
				rape_factor *= 1.2f;

			//Rand.PopState();
			//Rand.PushState(RJW_Multiplayer.PredictableSeed());
			if (rapist.relations == null || is_animal(rapist)) return Rand.Chance(rape_factor);
			int opinion = rapist.relations.OpinionOf(rapee);

			// Won't rape friends, unless rapist or psychopath.
			if (is_kind(rapist))
			{   //<-80: 1f /-40: 0.5f / 0+: 0f
				rape_factor *= 1f - Mathf.Pow(GenMath.InverseLerp(-80, 0, opinion), 2);
			}
			else if (is_rapist(rapist) || is_psychopath(rapist))
			{   //<40: 1f /80: 0.5f / 120+: 0f
				rape_factor *= 1f - Mathf.Pow(GenMath.InverseLerp(40, 120, opinion), 2); // This can never be 0, since opinion caps at 100.
			}
			else
			{   //<-60: 1f /-20: 0.5f / 40+: 0f
				rape_factor *= 1f - Mathf.Pow(GenMath.InverseLerp(-60, 40, opinion), 2);
			}

			//Log.Message("rjw::xxx rape_factor for " + get_pawnname(rapee) + " is " + rape_factor);

			return Rand.Chance(rape_factor);
		}

		public static float would_fuck(Pawn fucker, Corpse fucked, bool invert_opinion = false, bool ignore_bleeding = false, bool ignore_gender = false)
		{
			CompRottable comp = fucked.GetComp<CompRottable>();

			//--Log.Message("rotFactor:" + rotFactor);

			// Things that don't rot, such as mechanoids and weird mod-added stuff such as Rimworld of Magic's elementals.
			if (comp == null)
			{
				// Trying to necro the weird mod-added stuff causes an error, so skipping those for now.
				return 0.0f;
			}

			float maxRot = ((CompProperties_Rottable)comp.props).TicksToDessicated;
			float rotFactor = (maxRot - comp.RotProgress) / maxRot;
			//--Log.Message("rotFactor:" + rotFactor);
			return would_fuck(fucker, fucked.InnerPawn, invert_opinion, ignore_bleeding, ignore_gender) * rotFactor;
		}

		public static float would_fuck_animal(Pawn pawn, Pawn target, bool invert_opinion = false, bool ignore_bleeding = false, bool ignore_gender = false)
		{
			float wildness_modifier = 1.0f;
			List<float> size_preference = new List<float>() { pawn.BodySize * 0.75f, pawn.BodySize * 1.6f };
			float fuc = xxx.would_fuck(pawn, target, invert_opinion, ignore_bleeding, ignore_gender); // 0.0 to ~3.0, orientation checks etc.

			if (fuc < 0.1f)
			{   // Would not fuck
				return 0;
			}

			if (xxx.has_quirk(pawn, "Teratophile"))
			{   // Teratophiles prefer more 'monstrous' partners.
				size_preference[0] = pawn.BodySize * 0.8f;
				size_preference[1] = pawn.BodySize * 2.0f;
				wildness_modifier = 0.3f;
			}
			if (pawn.health.hediffSet.HasHediff(HediffDef.Named("AlcoholHigh")))
			{
				wildness_modifier = 0.5f; //Drunk and making poor judgments.
				size_preference[1] *= 1.5f;
			}
			else if (pawn.health.hediffSet.HasHediff(HediffDef.Named("YayoHigh")))
			{
				wildness_modifier = 0.2f; //This won't end well.
				size_preference[1] *= 2.5f;
			}
			if (!Genital_Helper.has_penis(pawn) && (Genital_Helper.has_vagina(pawn) || Genital_Helper.has_anus(pawn)))
			{
				size_preference[1] = pawn.BodySize * 1.3f;
			}
			if (xxx.is_animal(pawn))
			{
				size_preference[1] = pawn.BodySize * 1.3f;
				wildness_modifier = 0.4f;
			}
			else
			{
				if (pawn.story.traits.HasTrait(TraitDefOf.Tough) || pawn.story.traits.HasTrait(TraitDefOf.Brawler))
				{
					size_preference[1] += 0.2f;
					wildness_modifier -= 0.2f;
				}
				else if (pawn.story.traits.HasTrait(TraitDef.Named("Wimp")))
				{
					size_preference[0] -= 0.2f;
					size_preference[1] -= 0.2f;
					wildness_modifier += 0.25f;
				}
			}

			float wildness = target.RaceProps.wildness; // 0.0 to 1.0
			float petness = target.RaceProps.petness; // 0.0 to 1.0
			float distance = pawn.Position.DistanceToSquared(target.Position);

			//Log.Message("[RJW]would_fuck_animal:: base: " + fuc + ", wildness: " + wildness + ", petness: " + petness + ", distance: " + distance);

			fuc = fuc + fuc * petness - fuc * wildness * wildness_modifier;

			if (fuc < 0.1f)
			{   // Would not fuck
				return 0;
			}

			// Adjust by distance, nearby targets preferred.
			fuc *= 1.0f - Mathf.Max(distance / 10000, 0.1f);

			// Adjust by size difference.
			if (target.BodySize < size_preference[0])
			{
				fuc *= Mathf.Lerp(0.1f, size_preference[0], target.BodySize);
			}
			else if (target.BodySize > size_preference[1])
			{
				fuc *= Mathf.Lerp(size_preference[1] * 10, size_preference[1], target.BodySize);
			}

			if (target.Faction != pawn.Faction)
			{
				//Log.Message("[RJW]would_fuck_animal(NT):: base: " + fuc + ", bound1: " + fuc * 0.75f);
				//Log.Message("[RJW]would_fuck_animal(NT):: base: " + fuc + ", bound2: " + fuc + 0.25f);
				fuc *= 0.75f; // Less likely to target wild animals.
			}
			else if (pawn.relations.DirectRelationExists(PawnRelationDefOf.Bond, target))
			{
				//Log.Message("[RJW]would_fuck_animal(T):: base: " + fuc + ", bound1: " + fuc * 1.25f);
				//Log.Message("[RJW]would_fuck_animal(T):: base: " + fuc + ", bound2: " + fuc + 0.25f);
				fuc *= 1.25f; // Bonded animals preferred.
			}

			return fuc;
		}

		// Returns how fuckable 'fucker' thinks 'p' is on a scale from 0.0 to 1.0
		public static float would_fuck(Pawn fucker, Pawn fucked, bool invert_opinion = false, bool ignore_bleeding = false, bool ignore_gender = false)
		{
			//--Log.Message("[RJW]would_fuck("+xxx.get_pawnname(fucker)+","+xxx.get_pawnname(fucked)+","+invert_opinion.ToString()+") is called");
			if (!is_healthy_enough(fucker) && !is_psychopath(fucker)) // Not healthy enough to have sex, shouldn't have got this far.
				return 0f;
			if ((is_animal(fucker) || is_animal(fucked)) && (!is_animal(fucker) || !is_animal(fucked)) && !RJWSettings.bestiality_enabled)
				return 0f; // Animals disabled.
			if (fucked.Dead && !RJWSettings.necrophilia_enabled)
				return 0f; // Necrophilia disabled.
			if (fucker.Dead || fucker.Suspended || fucked.Suspended)
				return 0f; // Target unreachable. Shouldn't have got this far, but doesn't hurt to double-check.
			if (is_starved(fucked) && fucked.Faction == fucker.Faction && !is_psychopath(fucker) && !is_rapist(fucker))
				return 0f;
			if (!ignore_bleeding && !is_not_dying(fucked) && !is_psychopath(fucker) && !is_rapist(fucker) && !is_bloodlust(fucker))
				return 0f; // Most people wouldn't fuck someone who's dying.

			if (fucker.gender == Gender.Male)
				switch (RJWPreferenceSettings.Malesex)
				{
					case RJWPreferenceSettings.AllowedSex.All:
						break;
					case RJWPreferenceSettings.AllowedSex.Homo:
						if (fucked.gender != Gender.Male)
							return 0f;
						break;
					case RJWPreferenceSettings.AllowedSex.Nohomo:
						if (fucked.gender == Gender.Male)
							return 0f;
						break;
				}
			if (fucker.gender == Gender.Female)
				switch (RJWPreferenceSettings.FeMalesex)
				{
					case RJWPreferenceSettings.AllowedSex.All:
						break;
					case RJWPreferenceSettings.AllowedSex.Homo:
						if (fucked.gender != Gender.Female)
							return 0f;
						break;
					case RJWPreferenceSettings.AllowedSex.Nohomo:
						if (fucked.gender == Gender.Female)
							return 0f;
						break;
				}

			int fucker_age = fucker.ageTracker.AgeBiologicalYears;
			string fucker_quirks = CompRJW.Comp(fucker).quirks.ToString();
			int p_age = fucked.ageTracker.AgeBiologicalYears;

			// --- Age checks ---
			bool age_ok;
			{
				if (is_animal(fucker) && p_age >= RJWSettings.sex_minimum_age)
				{
					age_ok = true;
				}
				else if (is_animal(fucked) && fucker_age >= RJWSettings.sex_minimum_age)
				{
					// don't check the age of animals when they are the victim
					age_ok = true;
				}
				else if (fucker_age >= RJWSettings.sex_free_for_all_age && p_age >= RJWSettings.sex_free_for_all_age)
				{
					age_ok = true;
				}
				else if (fucker_age < RJWSettings.sex_minimum_age || p_age < RJWSettings.sex_minimum_age)
				{
					age_ok = false;
				}
				else
				{
					age_ok = Math.Abs(fucker.ageTracker.AgeBiologicalYearsFloat - fucked.ageTracker.AgeBiologicalYearsFloat) < 2.05f;
				}
			}
			// Age not acceptable, automatic fail.
			//Log.Message("would_fuck() - age_ok = " + age_ok.ToString());
			if (!age_ok) return 0.0f;

			float age_factor;

			//The human age curve needs work. Currently pawns refuse to have sex with anyone over age of ~50 no matter what the other factors are, which is just silly...
			age_factor = fucked.gender == Gender.Male ? attractiveness_from_age_male.Evaluate(SexUtility.ScaleToHumanAge(fucked)) : attractiveness_from_age_female.Evaluate(SexUtility.ScaleToHumanAge(fucked));
			//--Log.Message("would_fuck() - age_factor = " + age_factor.ToString());

			if (is_animal(fucker))
			{
				age_factor = 1.0f;  //using flat factors, since human age is not comparable to animal ages
			}
			else if (is_animal(fucked))
			{
				if (p_age <= 1 && fucked.RaceProps.lifeExpectancy > 8)
					age_factor = 0.5f;
				else
					age_factor = 1.0f;
				//--Log.Message("would_fuck() - animal age_factor = " + age_factor.ToString());
			}
			if (fucker_quirks.Contains("Gerontophile"))
			{
				age_factor = SexUtility.ScaleToHumanAge(fucked) > 55 ? 1.0f : 0.4f;
			}

			// --- Orientation checks ---
			float orientation_factor; //0 or 1
			{
    			orientation_factor = 1.0f;

				if (!ignore_gender && !is_animal(fucker))
				{
					if (is_asexual(fucker))
						return 0.0f;

					if (!CompRJW.CheckPreference(fucker, fucked))
					{
						//Log.Message("would_fuck() - preference fail");
						return 0.0f;
					}
				}
			}
			//Log.Message("would_fuck() - orientation_factor = " + orientation_factor.ToString());

			// --- Body and appearance checks ---
			float body_factor; //0.4 to 1.6
			{
				if (fucker.health.hediffSet.HasHediff(HediffDef.Named("AlcoholHigh")))
				{
					if (!is_zoophile(fucker) && is_animal(fucked))
						body_factor = 0.8f;
					else
						body_factor = 1.25f; //beer lens
				}
				else if (is_zoophile(fucker) && !is_animal(fucked))
				{
					body_factor = 0.7f;
				}
				else if (!is_zoophile(fucker) && is_animal(fucked))
				{
					body_factor = 0.45f;
				}
				else if (fucked.story != null)
				{
					if (fucked.story.bodyType == BodyTypeDefOf.Female) body_factor = 1.25f;
					else if (fucked.story.bodyType == BodyTypeDefOf.Fat) body_factor = fucker_quirks.Contains("Teratophile") ? 1.2f : 1.0f;
					else body_factor = 1.1f;

					if (fucked.story.traits.HasTrait(TraitDefOf.CreepyBreathing))
						body_factor *= fucker_quirks.Contains("Teratophile") ? 1.1f : 0.9f;

					if (fucked.story.traits.HasTrait(TraitDefOf.Beauty))
					{
						switch (fucked.story.traits.DegreeOfTrait(TraitDefOf.Beauty))
						{
							case 2: // Beautiful
								body_factor *= fucker_quirks.Contains("Teratophile") ? 0.4f : 1.25f;
								break;
							case 1: // Pretty
								body_factor *= fucker_quirks.Contains("Teratophile") ? 0.6f : 1.1f;
								break;
							case -1: // Ugly
								body_factor *= fucker_quirks.Contains("Teratophile") ? 1.1f : 0.8f;
								break;
							case -2: // Staggeringly Ugly
								body_factor *= fucker_quirks.Contains("Teratophile") ? 1.25f : 0.5f;
								break;
						}
					}

					if (RelationsUtility.IsDisfigured(fucked))
					{
						body_factor *= fucker_quirks.Contains("Teratophile") ? 1.25f : 0.8f;
					}

					// Nude target is more tempting.
					if (fucked.apparel.PsychologicallyNude && fucker.CanSee(fucked) && !fucker_quirks.Contains("Endytophile"))
						body_factor *= 1.1f;
				}
				else
				{
					body_factor = 1.1f;
				}

				if (!fucked.Awake() && fucker_quirks.Contains("Somnophile"))
					body_factor *= 1.25f;

				if (fucker_quirks.Contains("Pregnancy fetish"))
				{
					if (fucked.health.hediffSet.HasHediff(HediffDefOf.Pregnant, true))
						body_factor *= 1.25f;
					else
						body_factor *= 0.9f;
				}

				if (fucker_quirks.Contains("Impregnation fetish"))
				{
					if (PregnancyHelper.CanImpregnate(fucker, fucked))
						body_factor *= 1.25f;
					else
						body_factor *= 0.9f;
				}

				if (AlienFrameworkIsActive && !is_animal(fucker))
				{
					if (is_xenophile(fucker))
					{
						if (fucker.def.defName == fucked.def.defName)
							body_factor *= 0.5f; // Same species, xenophile less interested.
					}
					else if (is_xenophobe(fucker))
					{
						if (fucker.def.defName != fucked.def.defName)
							body_factor *= 0.25f; // Different species, xenophobe less interested.
					}
				}

				if (fucked.Dead && !is_necrophiliac(fucker))
				{
					body_factor *= 0.5f;
				}
			}
			//Log.Message("would_fuck() - body_factor = " + body_factor.ToString());

			// --- Opinion checks ---
			float opinion_factor;
			{
				if (fucked.relations != null && fucker.relations != null && !is_animal(fucker) && !is_animal(fucked))
				{
					float opi = !invert_opinion ? fucker.relations.OpinionOf(fucked) : 100 - fucker.relations.OpinionOf(fucked); // -100 to 100
					opinion_factor = 0.8f + (opi + 100.0f) * (.45f / 200.0f); // 0.8 to 1.25
				}
				else if ((is_animal(fucker) || is_animal(fucked)) && fucker.relations.DirectRelationExists(PawnRelationDefOf.Bond, fucked))
				{
					opinion_factor = 1.3f;
				}
				else
				{
					opinion_factor = 1.0f;
				}

				// More likely to take advantege of CP.
				if (fucked.IsDesignatedComfort() || (fucked.IsDesignatedBreeding() && is_animal(fucker)))
					opinion_factor += 0.25f;
				else if (fucked.IsDesignatedService())
					opinion_factor += 0.1f;

				// Less picky if designated for whorin'.
				if (fucker.IsDesignatedService())
					opinion_factor += 0.1f;

				if (has_quirk(fucker, "Sapiosexual") && has_traits(fucked))
				{
					if (fucked.story.traits.HasTrait(TraitDefOf.TooSmart)
						|| (CTIsActive && fucked.story.traits.HasTrait(TraitDef.Named("RCT_Savant")))
						|| (IndividualityIsActive && fucked.story.traits.HasTrait(TraitDef.Named("SYR_Scientist"))))
						opinion_factor *= 1.3f;
					else if (fucked.story.traits.HasTrait(TraitDef.Named("FastLearner"))
							 || (CTIsActive && fucked.story.traits.HasTrait(TraitDef.Named("RCT_Inventor")))
							 || fucked.story.traits.HasTrait(TraitDefOf.GreatMemory))
						opinion_factor *= 1.2f;
					else if (fucked.story.traits.HasTrait(TraitDefOf.Transhumanist))
						opinion_factor *= 1.1f;

					if (fucked.story.WorkTagIsDisabled(WorkTags.Intellectual) 
						|| (CTIsActive && fucked.story.traits.HasTrait(TraitDef.Named("RCT_Dunce"))))
						opinion_factor *= 0.7f;
				}
			}
			//Log.Message("would_fuck() - opinion_factor = " + opinion_factor.ToString());

			float horniness_factor; // 1 to 1.6
			{
				float need_sex = need_some_sex(fucker);
				switch (need_sex)
				{
					case 3:
						horniness_factor = 1.6f;
						break;

					case 2:
						horniness_factor = 1.3f;
						break;

					case 1:
						horniness_factor = 1.1f;
						break;

					default:
						horniness_factor = 1f;
						break;
				}
			}
			//Log.Message("would_fuck() - horniness_factor = " + horniness_factor.ToString());

			float reservedPercentage = (fucked.Dead ? 1f : fucked.ReservedCount()) / max_rapists_per_prisoner;
			//Log.Message("would_fuck() reservedPercentage:" + reservedPercentage + "fuckability_per_reserved"+ fuckability_per_reserved.Evaluate(reservedPercentage));
			//Log.Message("would_fuck() - horniness_factor = " + horniness_factor.ToString());

			float prenymph_att = Mathf.InverseLerp(0f, 2.8f, base_attraction * orientation_factor * horniness_factor * age_factor * body_factor * opinion_factor);
			float final_att = !is_nympho(fucker) ? prenymph_att : 0.2f + 0.8f * prenymph_att;
			//Log.Message("would_fuck( " + xxx.get_pawnname(fucker) + ", " + xxx.get_pawnname(fucked) + " ) - prenymph_att = " + prenymph_att.ToString() + ", final_att = " + final_att.ToString());

			return Mathf.Min(final_att, fuckability_per_reserved.Evaluate(reservedPercentage));
		}

		private static int ReservedCount(this Thing pawn)
		{
			int ret = 0;
			if (pawn == null) return 0;
			try
			{
				ReservationManager reserver = pawn.Map.reservationManager;
				IList reservations = (IList)AccessTools.Field(typeof(ReservationManager), "reservations").GetValue(reserver);

				if (reservations.Count == 0) return 0;
				Type reserveType = reservations[0].GetType();
				ret += (from object t in reservations
					where t != null
					let target = (LocalTargetInfo) AccessTools.Field(reserveType, "target").GetValue(t)
					let claimant = (Pawn) AccessTools.Field(reserveType, "claimant").GetValue(t)
					where target != null
					where target.Thing != null
					where target.Thing.ThingID == pawn.ThingID
					select (int) AccessTools.Field(reserveType, "stackCount").GetValue(t)).Count();
			}
			catch (Exception e)
			{
				Log.Warning(e.ToString());
			}
			return ret;
		}

		//Takes the nutrition away from the one penetrating and injects it to the one on the receiving end
		//As with everything in the mod, this could be greatly extended, current focus though is to prevent starvation of those caught in a huge horde of rappers (that may happen with some mods) 
		public static void TransferNutrition(Pawn pawn, Pawn partner, rjwSextype sextype)
		{
			//Log.Message("xxx::TransferNutrition:: " + xxx.get_pawnname(pawn) + " => " + xxx.get_pawnname(partner)); 
			if (partner?.needs == null)
			{
				//Log.Message("xxx::TransferNutrition() failed due to lack of transfer equipment or pawn ");
				return;
			}

			if (sextype == xxx.rjwSextype.Oral && Genital_Helper.has_penis(pawn))
			{
				Need_Food need = pawn.needs.TryGetNeed<Need_Food>();
				if (need == null)
				{
					//Log.Message("xxx::TransferNutrition() " + xxx.get_pawnname(pawn) + " doesn't track nutrition in itself, probably shouldn't feed the others");
					return;
				}
				float nutrition_amount = Math.Min(need.MaxLevel / 15f, need.CurLevel); //body size is taken into account implicitly by need.MaxLevel
				pawn.needs.food.CurLevel = need.CurLevel - nutrition_amount;
				//Log.Message("xxx::TransferNutrition() " + xxx.get_pawnname(pawn) + " sent " + nutrition_amount + " of nutrition");

				if (partner?.needs?.TryGetNeed<Need_Food>() != null)
				{
					//Log.Message("xxx::TransferNutrition() " +  xxx.get_pawnname(partner) + " can receive");
					partner.needs.food.CurLevel += nutrition_amount;
				}

				if (xxx.DubsBadHygieneIsActive)
				{
					Need DBHThirst = partner.needs.AllNeeds.Find(x => x.def == DefDatabase<NeedDef>.GetNamed("DBHThirst"));
					if (DBHThirst != null)
					{
						//Log.Message("xxx::TransferNutrition() " + xxx.get_pawnname(partner) + " decreasing thirst");
						partner.needs.TryGetNeed(DBHThirst.def).CurLevel += 0.1f;
					}
				}
			}
			if (xxx.RoMIsActive && (sextype == xxx.rjwSextype.Oral || sextype == xxx.rjwSextype.Vaginal || sextype == xxx.rjwSextype.Anal || sextype == xxx.rjwSextype.DoublePenetration))
			{
				if (has_traits(partner))
					if ((partner.story.traits.HasTrait(TraitDef.Named("Succubus")) || partner.story.traits.HasTrait(TraitDef.Named("Warlock"))))
					{
						Need TM_Mana = partner.needs.AllNeeds.Find(x => x.def == DefDatabase<NeedDef>.GetNamed("TM_Mana"));
						if (TM_Mana != null)
						{
							//Log.Message("xxx::TransferNutrition() " + xxx.get_pawnname(partner) + " increase mana");
							partner.needs.TryGetNeed(TM_Mana.def).CurLevel += 0.1f;
						}

						Need_Rest need = pawn.needs.TryGetNeed<Need_Rest>();
						if (need != null)
						{
							//Log.Message("xxx::TransferNutrition() " + xxx.get_pawnname(partner) + " increase rest");
							partner.needs.TryGetNeed(need.def).CurLevel += 0.25f;
							//Log.Message("xxx::TransferNutrition() " + xxx.get_pawnname(pawn) + " decrease rest");
							pawn.needs.TryGetNeed(need.def).CurLevel -= 0.25f;
						}
					}

				if (has_traits(pawn))
					if ((pawn.story.traits.HasTrait(TraitDef.Named("Succubus")) || pawn.story.traits.HasTrait(TraitDef.Named("Warlock"))))
					{
						Need TM_Mana = pawn.needs.AllNeeds.Find(x => x.def == DefDatabase<NeedDef>.GetNamed("TM_Mana"));
						if (TM_Mana != null)
						{
							//Log.Message("xxx::TransferNutrition() " + xxx.get_pawnname(pawn) + " increase mana");
							pawn.needs.TryGetNeed(TM_Mana.def).CurLevel += 0.1f;
						}

						Need_Rest need = pawn.needs.TryGetNeed<Need_Rest>();
						if (need != null)
						{
							//Log.Message("xxx::TransferNutrition() " + xxx.get_pawnname(pawn) + " increase rest");
							pawn.needs.TryGetNeed(need.def).CurLevel += 0.25f;
							//Log.Message("xxx::TransferNutrition() " + xxx.get_pawnname(partner) + " decrease rest");
							partner.needs.TryGetNeed(need.def).CurLevel -= 0.25f;
						}
					}
			}
		}

		public static bool bed_has_at_least_two_occupants(Building_Bed bed)
		{
			return bed.CurOccupants.Count() >= 2;
		}


		public static bool in_same_bed(Pawn pawn, Pawn partner)
		{
			if (pawn.InBed() && partner.InBed())
			{
				if (pawn.CurrentBed() == partner.CurrentBed())
					return true;
			}
			return false;
		}

		public static bool is_laying_down_alone(Pawn pawn)
		{
			if (pawn == null || !pawn.InBed()) return false;

			if (pawn.CurrentBed() != null)
				return !bed_has_at_least_two_occupants(pawn.CurrentBed());
			return true;
		}

		// Overrides the current clothing. Defaults to nude, with option to keep headgear on.
		public static void DrawNude(Pawn pawn, bool keep_hat_on = false)
		{
			if (!is_human(pawn)) return;
			if (RJWPreferenceSettings.sex_wear == RJWPreferenceSettings.Clothing.Clothed) return;

			pawn.Drawer.renderer.graphics.ClearCache();
			pawn.Drawer.renderer.graphics.apparelGraphics.Clear();
			if (RJWPreferenceSettings.sex_wear == RJWPreferenceSettings.Clothing.Headgear || keep_hat_on)
			{
				foreach (Apparel current in pawn.apparel.WornApparel.Where(x => x.def.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.UpperHead) || x.def.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.FullHead)))
				{
					ApparelGraphicRecord item;
					if (ApparelGraphicRecordGetter.TryGetGraphicApparel(current, pawn.story.bodyType, out item))
					{
						pawn.Drawer.renderer.graphics.apparelGraphics.Add(item);
					}
				}
			}
			pawn.Draw();
		}

		public static void sexTick(Pawn pawn, Pawn partner, bool enablerotation = true, bool pawnnude = true, bool partnernude = true)
		{
			if (enablerotation)
			{
				pawn.rotationTracker.Face(partner.DrawPos);
				partner.rotationTracker.Face(pawn.DrawPos);
			}

			if (RJWSettings.sounds_enabled)
			{
				SoundDef.Named("Sex").PlayOneShot(new TargetInfo(pawn.Position, pawn.Map));
			}

			pawn.Drawer.Notify_MeleeAttackOn(partner);
			if (enablerotation)
				pawn.rotationTracker.FaceCell(partner.Position);

			// Endytophiles prefer clothed sex, everyone else gets nude.
			if (pawnnude && !has_quirk(pawn, "Endytophile"))
			{
				DrawNude(pawn);
			}

			if (partnernude && !has_quirk(pawn, "Endytophile"))
			{
				DrawNude(partner);
			}
		}

		//violent - mark true when pawn rape partner
		//Note: violent is not reliable, since either pawn could be the rapist. Check jobdrivers instead, they're still active since this is called before ending the job.
		public static void think_about_sex(Pawn pawn, Pawn partner, bool isReceiving, bool violent = false, rjwSextype sextype = rjwSextype.None, bool isCoreLovin = false, bool whoring = false)
		{
			// Partner should never be null, but just in case something gets changed elsewhere..
			if (partner == null)
			{
				Log.Message("xxx::think-after_sex( ERROR: " + get_pawnname(pawn) + " has no partner. This should not be called from solo acts. Sextype: " + sextype);
				return;
			}

			// Both pawns are now checked individually, instead of giving thoughts to the partner.
			//Can just return if the currently checked pawn is dead or can't have thoughts, which simplifies the checks.
			if (pawn.Dead || !is_human(pawn))
				return;

			//--Log.Message("xxx::think_about_sex( " + xxx.get_pawnname(pawn) + ", " + xxx.get_pawnname(partner) + ", " + violent + " ) called");
			//--Log.Message("xxx::think_about_sex( " + xxx.get_pawnname(pawn) + ", " + xxx.get_pawnname(partner) + ", " + violent + " ) - setting pawn thoughts");

			//unconscious pawns has no thoughts
			//and if they has sex, its probably rape, since they have no choice
			//			pawn.Awake();
			if (!pawn.health.capacities.CanBeAwake)
			{
				ThoughtDef pawn_thought = is_masochist(pawn) || BadlyBroken(pawn) ? masochist_got_raped : got_raped_unconscious;
				pawn.needs.mood.thoughts.memories.TryGainMemory(pawn_thought);
				return;
			}

			// Thoughts for animal-on-colonist.
			if (is_animal(partner) && isReceiving)
			{
				if (!is_zoophile(pawn) && !violent)
				{
					if (sextype == rjwSextype.Oral)
						pawn.needs.mood.thoughts.memories.TryGainMemory(allowed_animal_to_lick);
					else if (sextype == rjwSextype.Anal || sextype == rjwSextype.Vaginal)
						pawn.needs.mood.thoughts.memories.TryGainMemory(allowed_animal_to_breed);
					else //Other rarely seen sex types, such as fingering (by primates, monster girls, etc)
						pawn.needs.mood.thoughts.memories.TryGainMemory(allowed_animal_to_grope);
				}
				else
				{
					if (!is_zoophile(pawn))
					{
						if (sextype == rjwSextype.Oral)
						{
							pawn.needs.mood.thoughts.memories.TryGainMemory(is_masochist(pawn) ? masochist_got_licked : got_licked);
						}
						else if (sextype == rjwSextype.Anal || sextype == rjwSextype.Vaginal)
						{
							pawn.needs.mood.thoughts.memories.TryGainMemory(is_masochist(pawn) ? masochist_got_bred : got_bred);
						}
						else //Other types
						{
							pawn.needs.mood.thoughts.memories.TryGainMemory(is_masochist(pawn) ? masochist_got_groped : got_groped);
						}
					}
					else
					{
						if (sextype == rjwSextype.Oral)
							pawn.needs.mood.thoughts.memories.TryGainMemory(zoophile_got_licked);
						else if (sextype == rjwSextype.Anal || sextype == rjwSextype.Vaginal)
							pawn.needs.mood.thoughts.memories.TryGainMemory(zoophile_got_bred);
						else //Other types
							pawn.needs.mood.thoughts.memories.TryGainMemory(zoophile_got_groped);
					}
				}

				if (!partner.Dead && is_zoophile(pawn) && pawn.CurJob.def != gettin_raped && partner.Faction == null && pawn.Faction == Faction.OfPlayer)
				{
					InteractionDef intDef = !partner.AnimalOrWildMan() ? InteractionDefOf.RecruitAttempt : InteractionDefOf.TameAttempt;
					pawn.interactions.TryInteractWith(partner, intDef);
				}
			}

			// Edited by nizhuan-jjr:The two types of stole_sone_lovin are violent due to the description, so I make sure the thought would only trigger after violent behaviors.
			// Edited by hoge: !is_animal is include mech. mech has no mood.
			// Edited by Zaltys: Since this is checked for both pawns, checking violent doesn't work. 
			if (partner.Dead || partner.CurJob.def == gettin_raped)
			{ // Rapist
				ThoughtDef pawn_thought = is_rapist(pawn) || is_bloodlust(pawn) ? bloodlust_stole_some_lovin : stole_some_lovin;
				pawn.needs.mood.thoughts.memories.TryGainMemory(pawn_thought);

				if ((is_necrophiliac(pawn) || is_psychopath(pawn)) && partner.Dead)
				{
					pawn.needs.mood.thoughts.memories.TryGainMemory(violated_corpse);
				}
			}
			else if (pawn.CurJob.def == gettin_raped) // Rape by animals handled earlier.
			{ // Raped
				if (is_human(partner))
				{
					ThoughtDef pawn_thought = is_masochist(pawn) || BadlyBroken(pawn) ? masochist_got_raped : got_raped;
					pawn.needs.mood.thoughts.memories.TryGainMemory(pawn_thought);

					ThoughtDef pawn_thought_about_rapist = is_masochist(pawn) || BadlyBroken(pawn) ? kinda_like_my_rapist : hate_my_rapist;
					pawn.needs.mood.thoughts.memories.TryGainMemory(pawn_thought_about_rapist, partner);
				}

				if (pawn.Faction != null && pawn.Map != null && !is_masochist(pawn) && !(is_animal(partner) && is_zoophile(pawn)))
				{
					foreach (Pawn bystander in pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction).Where(x => !is_animal(x) && x != pawn && x != partner && !x.Downed && !x.Suspended))
					{
						// dont see through walls, dont see whole map, only 15 cells around
						if (pawn.CanSee(bystander) && pawn.Position.DistanceToSquared(bystander.Position) < 15)
						{
							pawn.needs.mood.thoughts.memories.TryGainMemory(allowed_me_to_get_raped, bystander);
						}
					}
				}
			}
			else if (is_human(partner))
			{
				if (!isCoreLovin && !whoring)
				{
					// human partner and not part of rape or necrophilia.  add the vanilla GotSomeLovin thought
					pawn.needs.mood.thoughts.memories.TryGainMemory(VanillaGotSomeLovin, partner);
				}
			}

			//--Log.Message("xxx::think_about_sex( " + xxx.get_pawnname(pawn) + ", " + xxx.get_pawnname(partner) + ", " + violent + " ) - setting disease thoughts");

			ThinkAboutDiseases(pawn, partner);
		}

		private static void ThinkAboutDiseases(Pawn pawn, Pawn partner)
		{
			// Dead and non-humans have no diseases (yet).
			if (partner.Dead || !is_human(partner)) return;

			// check for visible diseases
			// Add negative relation for visible diseases on the genitals
			int pawn_rash_severity = std.genital_rash_severity(pawn) - std.genital_rash_severity(partner);
			ThoughtDef pawn_thought_about_rash;
			if (pawn_rash_severity == 1) pawn_thought_about_rash = saw_rash_1;
			else if (pawn_rash_severity == 2) pawn_thought_about_rash = saw_rash_2;
			else if (pawn_rash_severity >= 3) pawn_thought_about_rash = saw_rash_3;
			else return;
			Thought_Memory memory = (Thought_Memory)ThoughtMaker.MakeThought(pawn_thought_about_rash);
			pawn.needs.mood.thoughts.memories.TryGainMemory(memory, partner);
		}

		// <summary>Updates records for whoring.</summary>
		public static void UpdateRecords(Pawn pawn, int price)
		{
			pawn.records.AddTo(EarnedMoneyByWhore, price);
			pawn.records.Increment(CountOfWhore);
			//this is added by normal outcome
			//pawn.records.Increment(CountOfSex);
		}

		// <summary>Updates records. "Pawn" should be initiator, and "partner" should be the target.</summary>
		public static void UpdateRecords(Pawn pawn, Pawn partner, rjwSextype sextype, bool isRape = false, bool isLoveSex = false)
		{
			if (!pawn.Dead)
				UpdateRecordsInternal(pawn, partner, isRape, isLoveSex, true, sextype);

			if (partner == null || partner.Dead)
				return;

			UpdateRecordsInternal(partner, pawn, isRape, isLoveSex, false, sextype);
		}

		private static void UpdateRecordsInternal(Pawn pawn, Pawn partner, bool isRape, bool isLoveSex, bool pawnIsRaper, rjwSextype sextype)
		{
			if (pawn == null) return;
			if (pawn.health.Dead) return;

			if (sextype == rjwSextype.Masturbation)
			{
				pawn.records.Increment(CountOfFappin);
				return;
			}

			bool isVirginSex = is_Virgin(pawn); //need copy value before count increase.
			ThoughtDef currentThought = null;

			pawn.records.Increment(CountOfSex);

			if (!isRape)
			{
				if (is_human(partner))
				{
					pawn.records.Increment(partner.health.Dead ? CountOfSexWithCorpse : CountOfSexWithHumanlikes);
					currentThought = isLoveSex ? gave_virginity : null;
				}
				else if (is_insect(partner))
				{
					pawn.records.Increment(CountOfSexWithInsects);
				}
				else if (is_animal(partner))
				{
					pawn.records.Increment(CountOfSexWithAnimals);
					currentThought = is_zoophile(pawn) ? gave_virginity : null;
				}
				else
				{
					pawn.records.Increment(CountOfSexWithOthers);
				}
			}
			else
			{
				if (!pawnIsRaper)
				{
					currentThought = is_masochist(pawn) ? gave_virginity : lost_virginity;
				}

				if (is_human(partner))
				{
					pawn.records.Increment(pawnIsRaper ? partner.health.Dead ? CountOfSexWithCorpse : CountOfRapedHumanlikes : CountOfBeenRapedByHumanlikes);
					if (pawnIsRaper && (is_rapist(pawn) || is_bloodlust(pawn)))
						currentThought = gave_virginity;
				}
				else if (is_insect(partner))
				{
					pawn.records.Increment(CountOfSexWithInsects);
					pawn.records.Increment(pawnIsRaper ? CountOfRapedInsects : CountOfBeenRapedByInsects);
				}
				else if (is_animal(partner))
				{
					pawn.records.Increment(CountOfSexWithAnimals);
					pawn.records.Increment(pawnIsRaper ? CountOfRapedAnimals : CountOfBeenRapedByAnimals);
					if (is_zoophile(pawn)) currentThought = gave_virginity;
				}
				else
				{
					pawn.records.Increment(CountOfSexWithOthers);
					pawn.records.Increment(pawnIsRaper ? CountOfRapedOthers : CountOfBeenRapedByOthers);
				}
			}
			
			//TODO: someday only loose virginity only during vaginal sex
			//if (isVirginSex) //&& (sextype == rjwSextype.Vaginal || sextype == rjwSextype.DoublePenetration))
			//{
			//	Log.Message(xxx.get_pawnname(pawn) + " | " + xxx.get_pawnname(partner) + " | " + currentThought);
			//	Log.Message("1");
			//	if (!is_animal(partner))//passive
			//	{
			//		if (currentThought != null)
			//			partner.needs.mood.thoughts.memories.TryGainMemory(currentThought);
			//	}
			//	Log.Message("2");
			//	if (!is_animal(pawn))//active
			//	{
			//		currentThought = took_virginity;
			//		pawn.needs.mood.thoughts.memories.TryGainMemory(currentThought);
			//	}
			//}
		}

		//============↓======Section of processing the whoring system===============↓=============

		public static Building_Bed FindBed(Pawn pawn)
		{
			if (pawn.ownership.OwnedBed != null && pawn.ownership.OwnedBed.MaxAssignedPawnsCount > 0)
			{
				return pawn.ownership.OwnedBed;
			}
			return null;
		}

		public static bool CanUse(Pawn pawn, Building_Bed bed)
		{
			bool flag = pawn.CanReserveAndReach(bed, PathEndMode.InteractionCell, Danger.Unspecified) && !bed.IsForbidden(pawn) && bed.AssignedPawns.Contains(pawn);
			return flag;
		}

		public static void FailOnWhorebedNoLongerUsable(this Toil toil, TargetIndex bedIndex, Building_Bed bed)
		{
			if (toil == null)
			{
				throw new ArgumentNullException(nameof(toil));
			}

			toil.FailOnDespawnedOrNull(bedIndex);
			toil.FailOn(bed.IsBurning);
			toil.FailOn(() => HealthAIUtility.ShouldSeekMedicalRestUrgent(toil.actor));
			toil.FailOn(() => toil.actor.IsColonist && !toil.actor.CurJob.ignoreForbidden && !toil.actor.Downed && bed.IsForbidden(toil.actor));
		}

		public static IntVec3 SleepPosOfAssignedPawn(this Building_Bed bed, Pawn pawn)
		{
			if (!bed.AssignedPawns.Contains(pawn))
			{
				Log.Error("[RJW]xxx::SleepPosOfAssignedPawn - pawn is not an owner of the bed;returning bed.position");
				return bed.Position;
			}

			int slotIndex = 0;
			for (byte i = 0; i < bed.owners.Count; i++)
			{
				if (bed.owners[i] == pawn)
				{
					slotIndex = i;
				}
			}
			return bed.GetSleepingSlotPos(slotIndex);
		}

		//============↓======Section of processing the broken body system===============↓=============
		public static bool BodyIsBroken(Pawn pawn)
		{
			return pawn.health.hediffSet.HasHediff(feelingBroken);
		}

		public static bool BadlyBroken(Pawn pawn)
		{
			if (!BodyIsBroken(pawn))
				return false;

			int stage = pawn.health.hediffSet.GetFirstHediffOfDef(feelingBroken).CurStageIndex;
			if (stage >= 3)
			{
				//when broken make character masochist
				//todo remove/replace social/needs dubuffs
				if (!is_masochist(pawn))
				{
					if (!is_rapist(pawn))
					{
						pawn.story.traits.GainTrait(new Trait(masochist));
						//Log.Message(xxx.get_pawnname(pawn) + " BadlyBroken, not masochist, adding masochist trait");
					}
					else
					{
						pawn.story.traits.allTraits.Remove(pawn.story.traits.GetTrait(rapist));
						pawn.story.traits.GainTrait(new Trait(masochist));
						//Log.Message(xxx.get_pawnname(pawn) + " BadlyBroken, switch rapist -> masochist");
					}
					pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDef(got_raped);
					pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDef(got_licked);
					pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDef(hate_my_rapist);
					pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDef(allowed_me_to_get_raped);
				}
				if (pawn.IsPrisonerOfColony)
				{
					//Log.Message(xxx.get_pawnname(pawn) + " BadlyBroken, reduce prisoner resistance");
					pawn.guest.resistance = Mathf.Max(pawn.guest.resistance - 1f, 0f);
				}
			}
			return stage > 1;
		}
		//add variant for eggs?
		public static void processBrokenPawn(Pawn pawn)
		{
			// Called after rape/breed
			if (pawn is null)
				return;

			if (is_human(pawn) && !pawn.Dead && pawn.records != null)
			{
				if (has_traits(pawn))
				{
					if (xxx.is_slime(pawn))
							return;

					if (!BodyIsBroken(pawn))
						pawn.health.AddHediff(feelingBroken);
					else
					{
						float num = feelingBroken.initialSeverity;
						int feelingBrokenStage = pawn.health.hediffSet.GetFirstHediffOfDef(feelingBroken).CurStageIndex;

						if (xxx.RoMIsActive)
							if (pawn.story.traits.HasTrait(TraitDef.Named("Succubus")))
								num *= 0.25f;

						if (pawn.story.traits.HasTrait(TraitDefOf.Tough))
						{
							num *= 0.5f;
						}
						if (pawn.story.traits.HasTrait(TraitDef.Named("Wimp")))
						{
							num *= 2.0f;
						}
						if (pawn.story.traits.HasTrait(TraitDefOf.Nerves))
						{
							int td = pawn.story.traits.DegreeOfTrait(TraitDefOf.Nerves);
							if (feelingBrokenStage >= 2 && td > -1)
							{
								pawn.story.traits.allTraits.Remove(pawn.story.traits.GetTrait(TraitDefOf.Nerves));
								pawn.story.traits.GainTrait(new Trait(TraitDefOf.Nerves, -1));
							}
							if (feelingBrokenStage >= 3 && td > -2)
							{
								pawn.story.traits.allTraits.Remove(pawn.story.traits.GetTrait(TraitDefOf.Nerves));
								pawn.story.traits.GainTrait(new Trait(TraitDefOf.Nerves, -2));
							}
							switch (td)
							{
								case -2:
									num *= 2.0f;
									break;
								case -1:
									num *= 1.5f;
									break;
								case 1:
									num *= 0.5f;
									break;
								case 2:
									num *= 0.25f;
									break;
							}
						}
						else if (feelingBrokenStage > 1)
						{
							pawn.story.traits.GainTrait(new Trait(TraitDefOf.Nerves, -1));
						}
						pawn.health.hediffSet.GetFirstHediffOfDef(feelingBroken).Severity += num;
					}
					BadlyBroken(pawn);
				}
			}
		}

		public static void ExtraSatisfyForBrokenPawn(Pawn pawn)
		{
			if (!BodyIsBroken(pawn) || pawn.needs?.joy is null)
				return;
			float pawn_satisfaction = 0.2f;
			//Log.Message("Current stage " + pawn.health.hediffSet.GetFirstHediffOfDef(feelingBroken).CurStageIndex);
			switch (pawn.health.hediffSet.GetFirstHediffOfDef(feelingBroken).CurStageIndex)
			{
				case 0:
					break;

				case 1:
					pawn.needs.TryGetNeed<Need_Sex>().CurLevel += pawn_satisfaction;
					pawn.needs.joy.CurLevel += pawn_satisfaction * 0.50f;   // convert half of satisfaction to joy
					break;

				case 2:
					pawn_satisfaction *= 2f;
					pawn.needs.TryGetNeed<Need_Sex>().CurLevel += pawn_satisfaction;
					pawn.needs.joy.CurLevel += pawn_satisfaction * 0.50f;   // convert half of satisfaction to joy
					break;
			}
		}

		//============↑======Section of processing the broken body system===============↑=============
	}
}