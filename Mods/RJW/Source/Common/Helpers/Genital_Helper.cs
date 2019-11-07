using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using System;
using Multiplayer.API;

namespace rjw
{
	public static class Genital_Helper
	{
		public static HediffDef generic_anus = HediffDef.Named("GenericAnus");
		public static HediffDef generic_penis = HediffDef.Named("GenericPenis");
		public static HediffDef generic_vagina = HediffDef.Named("GenericVagina");
		public static HediffDef generic_breasts = HediffDef.Named("GenericBreasts");

		public static HediffDef micro_penis = HediffDef.Named("MicroPenis");
		public static HediffDef small_penis = HediffDef.Named("SmallPenis");
		public static HediffDef average_penis = HediffDef.Named("Penis");
		public static HediffDef big_penis = HediffDef.Named("BigPenis");
		public static HediffDef huge_penis = HediffDef.Named("HugePenis");
		public static HediffDef hydraulic_penis = HediffDef.Named("HydraulicPenis");
		public static HediffDef bionic_penis = HediffDef.Named("BionicPenis");
		public static HediffDef archotech_penis = HediffDef.Named("ArchotechPenis");

		public static HediffDef micro_vagina = HediffDef.Named("MicroVagina");
		public static HediffDef tight_vagina = HediffDef.Named("TightVagina");
		public static HediffDef average_vagina = HediffDef.Named("Vagina");
		public static HediffDef loose_vagina = HediffDef.Named("LooseVagina");
		public static HediffDef gaping_vagina = HediffDef.Named("GapingVagina");
		public static HediffDef hydraulic_vagina = HediffDef.Named("HydraulicVagina");
		public static HediffDef bionic_vagina = HediffDef.Named("BionicVagina");
		public static HediffDef archotech_vagina = HediffDef.Named("ArchotechVagina");

		public static HediffDef flat_breasts = HediffDef.Named("FlatBreasts");
		public static HediffDef small_breasts = HediffDef.Named("SmallBreasts");
		public static HediffDef average_breasts = HediffDef.Named("Breasts");
		public static HediffDef large_breasts = HediffDef.Named("LargeBreasts");
		public static HediffDef huge_breasts = HediffDef.Named("HugeBreasts");
		public static HediffDef hydraulic_breasts = HediffDef.Named("HydraulicBreasts");
		public static HediffDef bionic_breasts = HediffDef.Named("BionicBreasts");
		public static HediffDef archotech_breasts = HediffDef.Named("ArchotechBreasts");
		public static HediffDef featureless_chest = HediffDef.Named("FeaturelessChest");
		public static HediffDef udder = HediffDef.Named("Udder");

		public static HediffDef micro_anus = HediffDef.Named("MicroAnus");
		public static HediffDef tight_anus = HediffDef.Named("TightAnus");
		public static HediffDef loose_anus = HediffDef.Named("LooseAnus");
		public static HediffDef average_anus = HediffDef.Named("Anus");
		public static HediffDef gaping_anus = HediffDef.Named("GapingAnus");
		public static HediffDef hydraulic_anus = HediffDef.Named("HydraulicAnus");
		public static HediffDef bionic_anus = HediffDef.Named("BionicAnus");
		public static HediffDef archotech_anus = HediffDef.Named("ArchotechAnus");

		public static HediffDef peg_penis = HediffDef.Named("PegDick");

		public static HediffDef insect_anus = HediffDef.Named("InsectAnus");
		public static HediffDef ovipositorM = HediffDef.Named("OvipositorM");
		public static HediffDef ovipositorF = HediffDef.Named("OvipositorF");

		public static HediffDef demonT_penis = HediffDef.Named("DemonTentaclePenis");
		public static HediffDef demon_penis = HediffDef.Named("DemonPenis");
		public static HediffDef demon_vagina = HediffDef.Named("DemonVagina");
		public static HediffDef demon_anus = HediffDef.Named("DemonAnus");

		public static HediffDef slime_breasts = HediffDef.Named("SlimeBreasts");
		public static HediffDef slime_penis = HediffDef.Named("SlimeTentacles");
		public static HediffDef slime_vagina = HediffDef.Named("SlimeVagina");
		public static HediffDef slime_anus = HediffDef.Named("SlimeAnus");

		public static HediffDef feline_penis = HediffDef.Named("CatPenis");
		public static HediffDef feline_vagina = HediffDef.Named("CatVagina");

		public static HediffDef canine_penis = HediffDef.Named("DogPenis");
		public static HediffDef canine_vagina = HediffDef.Named("DogVagina");

		public static HediffDef equine_penis = HediffDef.Named("HorsePenis");
		public static HediffDef equine_vagina = HediffDef.Named("HorseVagina");

		public static HediffDef dragon_penis = HediffDef.Named("DragonPenis");
		public static HediffDef dragon_vagina = HediffDef.Named("DragonVagina");

		public static HediffDef raccoon_penis = HediffDef.Named("RaccoonPenis");

		public static HediffDef hemipenis = HediffDef.Named("Hemipenis");

		public static HediffDef crocodilian_penis = HediffDef.Named("CrocodilianPenis");

		public static List<HediffDef> penise = new List<HediffDef> { generic_penis,
			feline_penis, canine_penis, equine_penis, dragon_penis, raccoon_penis, hemipenis,
			ovipositorM, crocodilian_penis, average_penis, micro_penis, small_penis, big_penis,
			huge_penis, peg_penis, hydraulic_penis, bionic_penis, archotech_penis };

		public static List<HediffDef> vaginae = new List<HediffDef> { generic_vagina,
			feline_vagina, canine_vagina, equine_vagina, dragon_vagina, ovipositorF,
			average_vagina, micro_vagina, tight_vagina, loose_vagina, gaping_vagina,
			hydraulic_vagina, bionic_vagina, archotech_vagina };

		public static BodyPartRecord get_genitals(Pawn pawn)
		{
			//--Log.Message("Genital_Helper::get_genitals( " + xxx.get_pawnname(pawn) + " ) called");
			BodyPartRecord Part = pawn?.RaceProps.body.AllParts.Find(bpr => bpr.def.defName == "Genitals");

			if (Part == null)
			{
				//--Log.Message("[RJW] get_genitals( " + xxx.get_pawnname(pawn) + " ) - Part is null");
				return null;
			}
			return Part;
		}

		public static BodyPartRecord get_breasts(Pawn pawn)
		{
			//--Log.Message("[RJW] get_breasts( " + xxx.get_pawnname(pawn) + " ) called");
			BodyPartRecord Part = pawn?.RaceProps.body.AllParts.Find(bpr => bpr.def.defName == "Chest");

			if (Part == null)
			{
				//--Log.Message("[RJW] get_breasts( " + xxx.get_pawnname(pawn) + " ) - Part is null");
				return null;
			}
			return Part;
		}

		public static BodyPartRecord get_anus(Pawn pawn)
		{
			//--Log.Message("[RJW] get_anus( " + xxx.get_pawnname(pawn) + " ) called");
			BodyPartRecord Part = pawn?.RaceProps.body.AllParts.Find(bpr => bpr.def.defName == "Anus");

			if (Part == null)
			{
				//--Log.Message("[RJW] get_anus( " + xxx.get_pawnname(pawn) + " ) - Part is null");
				return null;
			}
			return Part;
		}

		public static bool genitals_blocked(Pawn pawn)
		{
			if (pawn.apparel != null)
			{
				foreach (var app in pawn.apparel.WornApparel)
				{
					if ((app.def is bondage_gear_def gear_def) && (gear_def.blocks_genitals))
					{
						return true;
					}
				}
			}
			return false;
		}

		public static bool breasts_blocked(Pawn pawn)
		{
			if (pawn.apparel != null)
			{
				foreach (var app in pawn.apparel.WornApparel)
				{
					if ((app.def is bondage_gear_def gear_def) && (gear_def.blocks_breasts))
						return true;
				}
			}
			return false;
		}

		public static bool anus_blocked(Pawn pawn)
		{
			if (pawn.apparel != null)
			{
				foreach (var app in pawn.apparel.WornApparel)
				{
					if ((app.def is bondage_gear_def gear_def) && (gear_def.blocks_anus))
						return true;
				}
			}
			return false;
		}

		public static bool has_genitals(Pawn pawn)
		{
			BodyPartRecord Part = get_genitals(pawn);
			if (Part is null)
				return false;

			return pawn.health.hediffSet.hediffs.Any((Hediff hed) =>
												  (hed.Part == Part) &&
												  (hed is Hediff_Implant || hed is Hediff_AddedPart));
		}

		public static bool has_breasts(Pawn pawn)
		{
			BodyPartRecord Part = get_breasts(pawn);
			if (Part is null)
				return false;

			return pawn.health.hediffSet.hediffs.Any((Hediff hed) =>
				(hed.Part == Part) &&
				(hed is Hediff_Implant || hed is Hediff_AddedPart) &&
				!hed.def.defName.ToLower().Contains("udder") &&
				!hed.def.defName.ToLower().Contains("featureless"));
		}

		public static bool has_male_breasts(Pawn pawn)
		{
			BodyPartRecord Part = get_breasts(pawn);
			if (Part is null)
				return false;

			return pawn.health.hediffSet.hediffs.Any((Hediff hed) =>
				(hed.Part == Part) &&
				(hed is Hediff_Implant || hed is Hediff_AddedPart) && hed.def == flat_breasts);
		}

		public static bool has_anus(Pawn pawn)
		{
			BodyPartRecord Part = get_anus(pawn);
			if (Part is null)
				return false;

			return pawn.health.hediffSet.hediffs.Any((Hediff hed) =>
						   (hed.Part == Part) && (hed is Hediff_Implant || hed is Hediff_AddedPart));
		}

		public static bool has_penis(Pawn pawn)
		{
			BodyPartRecord Part = get_genitals(pawn);
			if (Part is null)
				return false;

			return pawn.health.hediffSet.hediffs.Any((Hediff hed) =>
						   (hed.Part == Part) &&
						   (hed.def.defName.ToLower().Contains("penis") || hed.def.defName.ToLower().Contains("ovipositorm")));
		}


		public static bool has_multipenis(Pawn pawn)
		{
			BodyPartRecord Part = get_genitals(pawn);
			if (Part is null)
				return false;

			return (pawn.health.hediffSet.hediffs.Any((Hediff hed) =>
						   (hed.Part == Part) &&
						   (hed.def.defName.ToLower().Contains("hemipenis")))
						   ||
					pawn.health.hediffSet.hediffs.Count((Hediff hed) =>
						   (hed.Part == Part) && (
						   (hed.def.defName.ToLower().Contains("penis")) ||
						   (hed.def.defName.ToLower().Contains("pegdick")) ||
						   (hed.def.defName.ToLower().Contains("tentacle"))
						   )) > 1);
		}

		public static bool has_penis_infertile(Pawn pawn)
		{
			BodyPartRecord Part = get_genitals(pawn);
			if (Part is null)
				return false;

			return pawn.health.hediffSet.hediffs.Any((Hediff hed) =>
						   (hed.Part == Part) &&
						   (hed.def.defName.ToLower().Contains("pegdick") || hed.def.defName.ToLower().Contains("tentacle")));
		}

		public static bool has_ovipositorF(Pawn pawn)
		{
			BodyPartRecord Part = get_genitals(pawn);
			if (Part is null)
				return false;

			return pawn.health.hediffSet.hediffs.Any((Hediff hed) =>
						   (hed.Part == Part) &&
						   (hed.def.defName.ToLower().Contains("ovipositorf")));

		}
		public static bool has_ovipositorM(Pawn pawn)
		{
			BodyPartRecord Part = get_genitals(pawn);
			if (Part is null)
				return false;

			return pawn.health.hediffSet.hediffs.Any((Hediff hed) =>
						   (hed.Part == Part) &&
						   (hed.def.defName.ToLower().Contains("ovipositorm")));
		}

		public static bool has_vagina(Pawn pawn)
		{
			BodyPartRecord Part = get_genitals(pawn);
			if (Part is null)
				return false;

			return pawn.health.hediffSet.hediffs.Any((Hediff hed) =>
						   (hed.Part == Part) &&
						   (hed.def.defName.ToLower().Contains("vagina") || hed.def.defName.ToLower().Contains("ovipositorf")));
		}

		public static Hediff get_penis(Pawn pawn)
		{
			BodyPartRecord Part = get_genitals(pawn);
			if (Part is null)
				return null;

			return pawn.health.hediffSet.hediffs.Find((Hediff hed) =>
						   (hed.Part == Part) && penise.Contains(hed.def));
		}

		public static Hediff get_penis_all(Pawn pawn)
		{
			BodyPartRecord Part = get_genitals(pawn);
			if (Part is null)
				return null;

			return pawn.health.hediffSet.hediffs.Find((Hediff hed) =>
							(hed.Part == Part) &&
							(!hed.def.defName.ToLower().Contains("vagina")) &&
							(penise.Contains(hed.def) ||
							hed.def.defName.ToLower().Contains("penis") ||
							hed.def.defName.ToLower().Contains("pegdick") ||
							hed.def.defName.ToLower().Contains("ovipositorm") ||
							hed.def.defName.ToLower().Contains("tentacle")));
		}
		public static Hediff get_vagina(Pawn pawn)
		{
			BodyPartRecord Part = get_genitals(pawn);
			if (Part is null)
				return null;

			return pawn.health.hediffSet.hediffs.Find((Hediff hed) =>
						   (hed.Part == Part) && vaginae.Contains(hed.def));
		}

		public static Hediff get_vagina_all(Pawn pawn)
		{
			BodyPartRecord Part = get_genitals(pawn);
			if (Part is null)
				return null;

			return pawn.health.hediffSet.hediffs.Find((Hediff hed) =>
							(hed.Part == Part) &&
							(vaginae.Contains(hed.def) ||
							hed.def.defName.ToLower().Contains("ovipositorf")));
		}

		public static bool is_futa(Pawn pawn)
		{
			return (has_penis(pawn) && has_vagina(pawn));
		}

		[SyncMethod]
		public static double GenderTechLevelCheck(Pawn pawn)
		{
			if (pawn == null)
				return 0;
			//--Log.Message("[RJW] check pawn tech level for bionics ( " + xxx.get_pawnname(pawn) + " )");

			bool lowtechlevel = true;

			//Rand.PopState();
			//Rand.PushState(RJW_Multiplayer.PredictableSeed());
			double value = Rand.Value;

			if (pawn.Faction != null)
				lowtechlevel = (int)pawn.Faction.def.techLevel < 5;

			//--save savages from inventing hydraulic and bionic genitals
			while (lowtechlevel && value >= 0.90)
			{
				value = Rand.Value;
			}
			return value;
		}

		// return True if going to set penis
		// return False for vagina
		public static bool privates_gender(Pawn pawn, Gender gender)
		{
			return (pawn.gender == Gender.Male) ? (gender != Gender.Female) : (gender == Gender.Male);
		}

		[SyncMethod]
		public static void add_genitals(Pawn pawn, Pawn parent = null, Gender gender = Gender.None)
		{
			//--Log.Message("Genital_Helper::add_genitals( " + xxx.get_pawnname(pawn) + " ) called");
			BodyPartRecord Part = get_genitals(pawn);
			//--Log.Message("Genital_Helper::add_genitals( " + xxx.get_pawnname(pawn) + " ) - checking genitals");
			if (Part == null)
			{
				//--Log.Message("[RJW] add_genitals( " + xxx.get_pawnname(pawn) + " ) doesn't have a genitals");
				return;
			}
			else if (pawn.health.hediffSet.PartIsMissing(Part))
			{
				//--Log.Message("[RJW] add_genitals( " + xxx.get_pawnname(pawn) + " ) had a genital but was removed.");
				return;
			}
			if (has_genitals(pawn) && gender == Gender.None)//allow to add gender specific genitals(futa)
			{
				//--Log.Message("[RJW] add_genitals( " + xxx.get_pawnname(pawn) + " ) already has genitals");
				return;
			}
			var temppawn = pawn;
			if (parent != null)
				temppawn = parent;

			HediffDef privates;
			double value = GenderTechLevelCheck(pawn);
			string racename = temppawn.kindDef.race.defName.ToLower();

			// maybe add some check based on bodysize of pawn for genitals size limit
			//Log.Message("Genital_Helper::add_genitals( " + pawn.RaceProps.baseBodySize + " ) - 1");
			//Log.Message("Genital_Helper::add_genitals( " + pawn.kindDef.race.size. + " ) - 2");

			privates = (privates_gender(pawn, gender)) ? generic_penis : generic_vagina;

			if (has_vagina(pawn) && privates == generic_vagina)
			{
				//--Log.Message("[RJW] add_genitals( " + xxx.get_pawnname(pawn) + " ) already has vagina");
				return;
			}
			if ((has_penis(pawn) || has_penis_infertile(pawn)) && privates == generic_penis)
			{
				//--Log.Message("[RJW] add_genitals( " + xxx.get_pawnname(pawn) + " ) already has penis");
				return;
			}

			//override race genitals
			if (privates == generic_vagina && pawn.TryAddSexPart(SexPartType.FemaleGenital))
			{
				return;
			}
			if (privates == generic_penis && pawn.TryAddSexPart(SexPartType.MaleGenital))
			{
				return;
			}

			//Log.Message("Genital_Helper::add_genitals( " + xxx.get_pawnname(pawn));
			//Log.Message("Genital_Helper::add_genitals( " + pawn.kindDef.race.defName);
			//Log.Message("Genital_Helper::is male( " + privates_gender(pawn, gender));
			//Log.Message("Genital_Helper::is male1( " + pawn.gender);
			//Log.Message("Genital_Helper::is male2( " + gender);
			if (xxx.is_mechanoid(pawn))
			{
				return;
			}
			//insects
			else if (xxx.is_insect(temppawn)
				 || racename.Contains("apini")
				 || racename.Contains("mantodean")
				 || racename.Contains("insect")
				 || racename.Contains("bug"))
			{
				privates = (privates_gender(pawn, gender)) ? ovipositorM : ovipositorF;
				//override for Better infestations, since queen is male at creation
				if (racename.Contains("Queen"))
					privates = ovipositorF;
			}
			//space cats pawns
			else if ((racename.Contains("orassan") || racename.Contains("neko")) && !racename.ContainsAny("akaneko"))
			{
				if ((value < 0.70) || (pawn.ageTracker.AgeBiologicalYears < 2) || !pawn.RaceProps.Humanlike)
					privates = (privates_gender(pawn, gender)) ? feline_penis : feline_vagina;
				else if (value < 0.90)
					privates = (privates_gender(pawn, gender)) ? hydraulic_penis : hydraulic_vagina;
				else
					privates = (privates_gender(pawn, gender)) ? bionic_penis : bionic_vagina;
			}
			//space dog pawns
			else if (racename.Contains("fennex")
				 || racename.Contains("xenn")
				 || racename.Contains("leeani")
				 || racename.Contains("ferian")
				 || racename.Contains("callistan"))
			{
				if ((value < 0.70) || (pawn.ageTracker.AgeBiologicalYears < 2) || !pawn.RaceProps.Humanlike)
					privates = (privates_gender(pawn, gender)) ? canine_penis : canine_vagina;
				else if (value < 0.90)
					privates = (privates_gender(pawn, gender)) ? hydraulic_penis : hydraulic_vagina;
				else
					privates = (privates_gender(pawn, gender)) ? bionic_penis : bionic_vagina;
			}
			//space horse pawns
			else if (racename.Contains("equium"))
			{
				if ((value < 0.70) || (pawn.ageTracker.AgeBiologicalYears < 2) || !pawn.RaceProps.Humanlike)
					privates = (privates_gender(pawn, gender)) ? equine_penis : equine_vagina;
				else if (value < 0.90)
					privates = (privates_gender(pawn, gender)) ? hydraulic_penis : hydraulic_vagina;
				else
					privates = (privates_gender(pawn, gender)) ? bionic_penis : bionic_vagina;
			}
			//space raccoon pawns
			else if (racename.Contains("racc") && !racename.Contains("raccoon"))
			{
				if ((value < 0.70) || (pawn.ageTracker.AgeBiologicalYears < 2) || !pawn.RaceProps.Humanlike)
				{
					privates = (privates_gender(pawn, gender)) ? raccoon_penis : generic_vagina;
				}
				else if (value < 0.90)
				{
					privates = (privates_gender(pawn, gender)) ? hydraulic_penis : hydraulic_vagina;
				}
				else
				{
					privates = (privates_gender(pawn, gender)) ? bionic_penis : bionic_vagina;
				}
			}
			//alien races - ChjDroid, ChjAndroid
			else if (racename.Contains("droid"))
			{
				if (pawn.story.GetBackstory(BackstorySlot.Childhood) != null)
				{
					if (pawn.story.childhood.untranslatedTitleShort.ToLower().Contains("bishojo"))
						privates = (privates_gender(pawn, gender)) ? bionic_penis : bionic_vagina;
					else if (pawn.story.childhood.untranslatedTitleShort.ToLower().Contains("pleasure"))
						privates = (privates_gender(pawn, gender)) ? bionic_penis : bionic_vagina;
					else if (pawn.story.childhood.untranslatedTitleShort.ToLower().Contains("idol"))
						privates = (privates_gender(pawn, gender)) ? bionic_penis : bionic_vagina;
					else if (pawn.story.childhood.untranslatedTitleShort.ToLower().Contains("social"))
						privates = (privates_gender(pawn, gender)) ? hydraulic_penis : hydraulic_vagina;
					else if (pawn.story.childhood.untranslatedTitleShort.ToLower().Contains("substitute"))
					{
						if (value < 0.05)
							privates = (privates_gender(pawn, gender)) ? micro_penis : micro_vagina;
						else if (value < 0.20)
							privates = (privates_gender(pawn, gender)) ? small_penis : tight_vagina;
						else if (value < 0.70)
							privates = (privates_gender(pawn, gender)) ? average_penis : average_vagina;
						else if (value < 0.80)
							privates = (privates_gender(pawn, gender)) ? big_penis : loose_vagina;
						else if (value < 0.90)
							privates = (privates_gender(pawn, gender)) ? huge_penis : gaping_vagina;
						else if (value < 0.95)
							privates = (privates_gender(pawn, gender)) ? hydraulic_penis : hydraulic_vagina;
						else
							privates = (privates_gender(pawn, gender)) ? bionic_penis : bionic_vagina;
					}
					else if (pawn.story.GetBackstory(BackstorySlot.Adulthood) != null)
					{
						if (pawn.story.adulthood.untranslatedTitleShort.ToLower().Contains("courtesan"))
							privates = (privates_gender(pawn, gender)) ? bionic_penis : bionic_vagina;
						else if (pawn.story.adulthood.untranslatedTitleShort.ToLower().Contains("social"))
							privates = (privates_gender(pawn, gender)) ? hydraulic_penis : hydraulic_vagina;
					}
					else
						return;
				}
				else if (pawn.story.GetBackstory(BackstorySlot.Adulthood) != null)
				{
					if (pawn.story.adulthood.untranslatedTitleShort.ToLower().Contains("courtesan"))
						privates = (privates_gender(pawn, gender)) ? bionic_penis : bionic_vagina;
					else if (pawn.story.adulthood.untranslatedTitleShort.ToLower().Contains("social"))
						privates = (privates_gender(pawn, gender)) ? hydraulic_penis : hydraulic_vagina;
				}
				if (privates == generic_penis || privates == generic_vagina)
					return;
			}
			//animal cats
			else if (racename.ContainsAny("cat", "cougar", "lion", "leopard", "cheetah", "panther", "tiger", "lynx", "smilodon", "akaneko"))
			{
				privates = (privates_gender(pawn, gender)) ? feline_penis : feline_vagina;
			}
			//animal canine/dogs
			else if (racename.ContainsAny("husky", "warg", "terrier", "collie", "hound", "retriever", "mastiff", "wolf", "fox",
				"vulptex", "dachshund", "schnauzer", "corgi", "pug", "doberman", "chowchow", "borzoi", "saintbernard", "newfoundland",
				"poodle", "dog", "coyote") && !racename.Contains("lf_foxia"))
			{
				privates = (privates_gender(pawn, gender)) ? canine_penis : canine_vagina;
			}
			//animal horse - MoreMonstergirls
			else if (racename.ContainsAny("horse", "centaur", "zebra", "donkey", "dryad"))
			{
				privates = (privates_gender(pawn, gender)) ? equine_penis : equine_vagina;
			}
			//animal raccoon
			else if (racename.Contains("racc"))
			{
				privates = (privates_gender(pawn, gender)) ? raccoon_penis : generic_vagina;
			}
			//animal crocodilian (alligator, crocodile, etc)
			else if (racename.ContainsAny("alligator", "crocodile", "caiman", "totodile", "croconaw", "feraligatr", "quinkana", "purussaurus", "kaprosuchus", "sarcosuchus"))
			{
				privates = (privates_gender(pawn, gender)) ? crocodilian_penis : generic_vagina;
			}
			//hemipenes - mostly reptiles and snakes
			else if (racename.ContainsAny("guana", "cobra", "gecko", "snake", "boa", "quinkana", "megalania", "gila", "gigantophis", "komodo", "basilisk", "thorny", "onix", "lizard", "slither") && !racename.ContainsAny("boar"))
			{
				privates = (privates_gender(pawn, gender)) ? hemipenis : generic_vagina;
			}
			//animal dragon - MoreMonstergirls
			else if (racename.ContainsAny("dragon", "thrumbo", "drake", "charizard", "saurus") && !racename.Contains("lf_dragonia"))
			{
				privates = (privates_gender(pawn, gender)) ? dragon_penis : dragon_vagina;
			}
			//animal slime - MoreMonstergirls
			else if (racename.Contains("slime"))
			{
				//privates = (privates_gender(pawn, gender)) ? slime_penis : slime_vagina;
				pawn.health.AddHediff((privates_gender(pawn, gender)) ? slime_penis : slime_vagina, Part);
				pawn.health.AddHediff((privates_gender(pawn, gender)) ? slime_vagina : slime_penis, Part);
				return;
			}
			//animal demons - MoreMonstergirls
			else if (racename.Contains("impmother") || racename.Contains("demon"))
			{
				pawn.health.AddHediff((privates_gender(pawn, gender)) ? demon_penis : demon_vagina, Part);
				//Rand.PopState();
				//Rand.PushState(RJW_Multiplayer.PredictableSeed());
				if (Rand.Value < 0.25f)
					pawn.health.AddHediff((privates_gender(pawn, gender)) ? demon_penis : demonT_penis, Part);
				return;
			}
			//animal demons - MoreMonstergirls
			else if (racename.Contains("baphomet"))
			{
				//Rand.PopState();
				//Rand.PushState(RJW_Multiplayer.PredictableSeed());
				if (Rand.Value < 0.50f)
					pawn.health.AddHediff((privates_gender(pawn, gender)) ? demon_penis : demon_vagina, Part);
				else
					pawn.health.AddHediff((privates_gender(pawn, gender)) ? equine_penis : demon_vagina, Part);
				return;
			}
			//humanoid monstergirls - LostForest
			else if (racename.ContainsAny("lf_dragonia", "lf_flammelia", "lf_foxia", "lf_glacia", "lf_lefca", "lf_wolvern"))
			{
				if (value < 0.05)
					privates = (privates_gender(pawn, gender)) ? micro_penis : micro_vagina;
				else if (value < 0.20)
					privates = (privates_gender(pawn, gender)) ? small_penis : tight_vagina;
				else if (value < 0.70)
					privates = (privates_gender(pawn, gender)) ? average_penis : average_vagina;
				else if (value < 0.90)
					privates = (privates_gender(pawn, gender)) ? big_penis : loose_vagina;
				else
					privates = (privates_gender(pawn, gender)) ? huge_penis : loose_vagina;
			}
			else if (pawn.RaceProps.Humanlike)
			{
				//--Log.Message("Genital_Helper::add_genitals( " + xxx.get_pawnname(pawn) + " ) - race is humanlike");
				if (pawn.ageTracker.AgeBiologicalYears < 5)
				{
					if (value < 0.05)
						privates = (privates_gender(pawn, gender)) ? micro_penis : micro_vagina;
					else if (value < 0.20)
						privates = (privates_gender(pawn, gender)) ? small_penis : tight_vagina;
					else if (value < 0.70)
						privates = (privates_gender(pawn, gender)) ? average_penis : average_vagina;
					else if (value < 0.90)
						privates = (privates_gender(pawn, gender)) ? big_penis : loose_vagina;
					else
						privates = (privates_gender(pawn, gender)) ? huge_penis : loose_vagina;
				}
				else
				{
					if (value < 0.02)
						privates = (privates_gender(pawn, gender)) ? peg_penis : micro_vagina;
					else if (value < 0.05)
						privates = (privates_gender(pawn, gender)) ? micro_penis : micro_vagina;
					else if (value < 0.20)
						privates = (privates_gender(pawn, gender)) ? small_penis : tight_vagina;
					else if (value < 0.70)
						privates = (privates_gender(pawn, gender)) ? average_penis : average_vagina;
					else if (value < 0.80)
						privates = (privates_gender(pawn, gender)) ? big_penis : loose_vagina;
					else if (value < 0.90)
						privates = (privates_gender(pawn, gender)) ? huge_penis : gaping_vagina;
					else if (value < 0.95)
						privates = (privates_gender(pawn, gender)) ? hydraulic_penis : hydraulic_vagina;
					else
						privates = (privates_gender(pawn, gender)) ? bionic_penis : bionic_vagina;
				}
			}
			//--Log.Message("Genital_Helper::add_genitals final ( " + xxx.get_pawnname(pawn) + " ) " + privates.defName);
			pawn.health.AddHediff(privates, Part);
		}

		public static void add_breasts(Pawn pawn, Pawn parent = null, Gender gender = Gender.None)
		{
			//--Log.Message("[RJW] add_breasts( " + xxx.get_pawnname(pawn) + " ) called");
			BodyPartRecord Part = get_breasts(pawn);

			if (Part == null)
			{
				//--Log.Message("[RJW] add_breasts( " + xxx.get_pawnname(pawn) + " ) - pawn doesn't have a breasts");
				return;
			}
			else if (pawn.health.hediffSet.PartIsMissing(Part))
			{
				//--Log.Message("[RJW] add_breasts( " + xxx.get_pawnname(pawn) + " ) had breasts but were removed.");
				return;
			}
			if (has_breasts(pawn))
			{
				//--Log.Message("[RJW] add_breasts( " + xxx.get_pawnname(pawn) + " ) - pawn already has breasts");
				return;
			}

			var temppawn = pawn;
			if (parent != null)
				temppawn = parent;

			HediffDef bewbs;
			double value = GenderTechLevelCheck(pawn);
			string racename = temppawn.kindDef.race.defName.ToLower();

			bewbs = generic_breasts;

			//TODO: figure out how to add (flat) breasts to males
			var sexPartType = (pawn.gender == Gender.Female || gender == Gender.Female)
				? SexPartType.FemaleBreast
				: SexPartType.MaleBreast;
			if (pawn.TryAddSexPart(sexPartType))
			{
				return;
			}
			if (xxx.is_mechanoid(pawn))
			{
				return;
			}
			if (xxx.is_insect(temppawn))
			{
				// this will probably need override in case there are humanoid insect race
				//--Log.Message("[RJW] add_breasts( " + xxx.get_pawnname(pawn) + " ) - is insect,doesnt need breasts");
				return;
			}
			//alien races - MoreMonstergirls
			else if (racename.Contains("slime"))
			{
				//slimes are always females, and idc what anyone else say!
				pawn.health.AddHediff(slime_breasts, Part);
				return;
			}
			else if (pawn.gender == Gender.Female || gender == Gender.Female)
			{
				if (pawn.RaceProps.Humanlike)
				{
					//alien races - ChjDroid, ChjAndroid
					if (racename.ContainsAny("mantis", "rockman", "slug", "zoltan", "engie", "sergal", "cutebold", "dodo", "owl", "parrot",
						"penguin", "cassowary", "chicken", "vulture"))
					{
						pawn.health.AddHediff(featureless_chest, Part);
						return;
					}
					else if (racename.ContainsAny("avali", "khess"))
					{
						return;
					}
					else if (racename.Contains("droid"))
					{
						if (pawn.story.GetBackstory(BackstorySlot.Childhood) != null)
						{
							if (pawn.story.childhood.untranslatedTitleShort.ToLower().Contains("bishojo"))
								bewbs = bionic_breasts;
							else if (pawn.story.childhood.untranslatedTitleShort.ToLower().Contains("pleasure"))
								bewbs = bionic_breasts;
							else if (pawn.story.childhood.untranslatedTitleShort.ToLower().Contains("idol"))
								bewbs = bionic_breasts;
							else if (pawn.story.childhood.untranslatedTitleShort.ToLower().Contains("social"))
								bewbs = hydraulic_breasts;
							else if (pawn.story.childhood.untranslatedTitleShort.ToLower().Contains("substitute"))
							{
								if (value < 0.05)
									bewbs = flat_breasts;
								else if (value < 0.25)
									bewbs = small_breasts;
								else if (value < 0.70)
									bewbs = average_breasts;
								else if (value < 0.80)
									bewbs = large_breasts;
								else if (value < 0.90)
									bewbs = huge_breasts;
								else if (value < 0.95)
									bewbs = hydraulic_breasts;
								else
									bewbs = bionic_breasts;
							}
							else if (pawn.story.GetBackstory(BackstorySlot.Adulthood) != null)
							{
								if (pawn.story.adulthood.untranslatedTitleShort.ToLower().Contains("courtesan"))
									bewbs = bionic_breasts;
								else if (pawn.story.adulthood.untranslatedTitleShort.ToLower().Contains("social"))
									bewbs = hydraulic_breasts;
							}
							else
								return;
						}
						else if (pawn.story.GetBackstory(BackstorySlot.Adulthood) != null)
						{
							if (pawn.story.adulthood.untranslatedTitleShort.ToLower().Contains("courtesan"))
								bewbs = bionic_breasts;
							else if (pawn.story.adulthood.untranslatedTitleShort.ToLower().Contains("social"))
								bewbs = hydraulic_breasts;
						}
						if (bewbs == generic_breasts)
							return;
					}
					//alien races - MoreMonstergirls
					//alien races - Kijin
					else if (racename.Contains("cowgirl") || racename.Contains("kijin"))
					{
						if (value < 0.05)
							bewbs = flat_breasts;
						else if (value < 0.10)
							bewbs = small_breasts;
						else if (value < 0.20)
							bewbs = average_breasts;
						else if (value < 0.50)
							bewbs = large_breasts;
						else if (value < 0.75 && racename.Contains("cowgirl"))
							bewbs = udder;
						else
							bewbs = huge_breasts;
					}
					else if (pawn.ageTracker.AgeBiologicalYears < 5)
					{
						if (value < 0.05)
							bewbs = flat_breasts;
						else if (value < 0.15)
							bewbs = small_breasts;
						else if (value < 0.70)
							bewbs = average_breasts;
						else if (value < 0.80)
							bewbs = large_breasts;
						else
							bewbs = huge_breasts;
					}
					else
					{
						if (value < 0.05)
							bewbs = flat_breasts;
						else if (value < 0.25)
							bewbs = small_breasts;
						else if (value < 0.70)
							bewbs = average_breasts;
						else if (value < 0.80)
							bewbs = large_breasts;
						else if (value < 0.90)
							bewbs = huge_breasts;
						else if (value < 0.95)
							bewbs = hydraulic_breasts;
						else
							bewbs = bionic_breasts;
					}
				}
				//humanoid monstergirls - LostForest
				else if (racename.ContainsAny("lf_dragonia", "lf_flammelia", "lf_foxia", "lf_glacia", "lf_lefca", "lf_wolvern"))
				{
					if (value < 0.10)
						bewbs = flat_breasts;
					else if (value < 0.30)
						bewbs = small_breasts;
					else if (value < 0.70)
						bewbs = average_breasts;
					else if (value < 0.90)
						bewbs = large_breasts;
					else
						bewbs = huge_breasts;
				}
				else if (racename.ContainsAny("mammoth", "elasmotherium", "chalicotherium", "megaloceros", "sivatherium", "deinotherium",
					"aurochs", "zygolophodon", "uintatherium", "gazelle", "ffalo", "boomalope", "cow", "miltank", "elk", "reek", "nerf",
					"bantha", "tauntaun", "caribou", "deer", "ibex", "dromedary", "alpaca", "llama", "goat", "moose"))
				{
					pawn.health.AddHediff(udder, Part);
					return;
				}
				else if (racename.ContainsAny("cassowary", "emu", "dinornis", "ostrich", "turkey", "chicken", "duck", "murkroW", "bustard", "palaeeudyptes",
					"goose", "tukiri", "porg", "yi", "kiwi", "penguin", "quail", "ptarmigan", "doduo", "flamingo", "plup", "empoleon", "meadow ave") && !racename.ContainsAny("duck-billed"))
				{
					return;  // Separate list for birds, to make it easier to add cloaca at some later date.
				}   // Other breastless creatures.
				else if (racename.ContainsAny("titanis", "titanoboa", "guan", "tortoise", "turt", "aerofleet", "quinkana", "megalochelys",
					"purussaurus", "cobra", "dewback", "rancor", "frog", "onyx", "flommel", "lapras", "aron", "chinchou",
					"squirtle", "wartortle", "blastoise", "totodile", "croconaw", "feraligatr", "litwick", "pumpkaboo", "shuppet", "haunter",
					"gastly", "oddish", "hoppip", "tropius", "budew", "roselia", "bellsprout", "drifloon", "chikorita", "bayleef", "meganium",
					"char", "drago", "dratini", "saur", "tyrannus", "carnotaurus", "baryonyx", "minmi", "diplodocus", "phodon", "indominus",
					"raptor", "caihong", "coelophysis", "cephale", "compsognathus", "mimus", "troodon", "dactyl", "tanystropheus", "geosternbergia",
					"deino", "suchus", "dracorex", "cephalus", "trodon", "quetzalcoatlus", "pteranodon", "antarctopelta", "stygimoloch", "rhabdodon",
					"rhamphorhynchus", "ceratops", "ceratus", "zalmoxes", "mochlodon", "gigantophis", "crab", "pulmonoscorpius", "manipulator",
					"meganeura", "euphoberia", "holcorobeus", "protosolpuga", "barbslinger", "blizzarisk", "frostmite", "devourer", "hyperweaver",
					"macrelcana", "acklay", "elemental", "megalania", "gecko", "gator", "komodo", "scolipede", "shuckle", "combee", "shedinja",
					"caterpie", "wurmple", "lockjaw", "needlepost", "needleroll", "squid", "slug", "gila", "pleura"))
				{
					return;
				}
				pawn.health.AddHediff(bewbs, Part);
			}
		}

		public static void add_anus(Pawn pawn, Pawn parent = null)
		{
			BodyPartRecord Part = get_anus(pawn);

			if (Part == null)
			{
				//--Log.Message("[RJW] add_anus( " + xxx.get_pawnname(pawn) + " ) doesn't have an anus");
				return;
			}
			else if (pawn.health.hediffSet.PartIsMissing(Part))
			{
				//--Log.Message("[RJW] add_anus( " + xxx.get_pawnname(pawn) + " ) had an anus but was removed.");
				return;
			}
			if (has_anus(pawn))
			{
				//--Log.Message("[RJW] add_anus( " + xxx.get_pawnname(pawn) + " ) already has an anus");
				return;
			}

			var temppawn = pawn;
			if (parent != null)
				temppawn = parent;

			HediffDef asshole;
			double value = GenderTechLevelCheck(pawn);
			string racename = temppawn.kindDef.race.defName.ToLower();

			asshole = generic_anus;

			if (pawn.TryAddSexPart(SexPartType.Anus))
			{
				return;
			}
			else if (xxx.is_mechanoid(pawn))
			{
				return;
			}
			else if (xxx.is_insect(temppawn))
			{
				asshole = insect_anus;
			}
			//alien races - ChjDroid, ChjAndroid
			else if (racename.Contains("droid"))
			{
				if (pawn.story.GetBackstory(BackstorySlot.Childhood) != null)
				{
					if (pawn.story.childhood.untranslatedTitleShort.ToLower().Contains("bishojo"))
						asshole = bionic_anus;
					else if (pawn.story.childhood.untranslatedTitleShort.ToLower().Contains("pleasure"))
						asshole = bionic_anus;
					else if (pawn.story.childhood.untranslatedTitleShort.ToLower().Contains("idol"))
						asshole = bionic_anus;
					else if (pawn.story.childhood.untranslatedTitleShort.ToLower().Contains("social"))
						asshole = hydraulic_anus;
					else if (pawn.story.childhood.untranslatedTitleShort.ToLower().Contains("substitute"))
					{
						if (value < 0.05)
							asshole = micro_anus;
						else if (value < 0.20)
							asshole = tight_anus;
						else if (value < 0.70)
							asshole = average_anus;
						else if (value < 0.80)
							asshole = loose_anus;
						else if (value < 0.90)
							asshole = gaping_anus;
						else if (value < 0.95)
							asshole = hydraulic_anus;
						else
							asshole = bionic_anus;
					}
					else if (pawn.story.GetBackstory(BackstorySlot.Adulthood) != null)
					{
						if (pawn.story.adulthood.untranslatedTitleShort.ToLower().Contains("courtesan"))
							asshole = bionic_anus;
						else if (pawn.story.adulthood.untranslatedTitleShort.ToLower().Contains("social"))
							asshole = hydraulic_anus;
					}
				}
				else if (pawn.story.GetBackstory(BackstorySlot.Adulthood) != null)
				{
					if (pawn.story.adulthood.untranslatedTitleShort.ToLower().Contains("courtesan"))
						asshole = bionic_anus;
					else if (pawn.story.adulthood.untranslatedTitleShort.ToLower().Contains("social"))
						asshole = hydraulic_anus;
				}
				if (asshole == generic_anus)
					return;
			}
			else if (racename.Contains("slime"))
			{
				asshole = slime_anus;
			}
			//animal demons - MoreMonstergirls
			else if (racename.Contains("impmother") || racename.Contains("baphomet") || racename.Contains("demon"))
			{
				asshole = demon_anus;
			}
			//humanoid monstergirls - LostForest
			else if (racename.ContainsAny("lf_dragonia", "lf_flammelia", "lf_foxia", "lf_glacia", "lf_lefca", "lf_wolvern"))
			{
				if (value < 0.05)
					asshole = micro_anus;
				else if (value < 0.20)
					asshole = tight_anus;
				else if (value < 0.90)
					asshole = average_anus;
				else
					asshole = loose_anus;
			}
			else if (pawn.RaceProps.Humanlike)
			{
				if (pawn.ageTracker.AgeBiologicalYears < 5)
				{
					if (value < 0.05)
						asshole = micro_anus;
					else if (value < 0.20)
						asshole = tight_anus;
					else if (value < 0.90)
						asshole = average_anus;
					else
						asshole = loose_anus;
				}
				else
				{
					if (value < 0.05)
						asshole = micro_anus;
					else if (value < 0.20)
						asshole = tight_anus;
					else if (value < 0.70)
						asshole = average_anus;
					else if (value < 0.80)
						asshole = loose_anus;
					else if (value < 0.90)
						asshole = gaping_anus;
					else if (value < 0.95)
						asshole = hydraulic_anus;
					else
						asshole = bionic_anus;
				}
			}
			pawn.health.AddHediff(asshole, Part);
		}

		public static void SexualizeSingleGenderPawn(Pawn pawn)
		{
			// Single gender is futa without the female gender change.
			add_genitals(pawn, null, Gender.Male);
			add_genitals(pawn, null, Gender.Female);
			add_breasts(pawn, null, Gender.Female);
			add_anus(pawn, null);
		}

		public static void SexulaizeGenderlessPawn(Pawn pawn)
		{
			if (RJWSettings.GenderlessAsFuta && !xxx.is_mechanoid(pawn))
			{
				Log.Message("[RJW] SexulaizeGenderlessPawn() - genderless pawn, treating Genderless pawn As Futa" + xxx.get_pawnname(pawn));
				//set gender to female for futas
				pawn.gender = Gender.Female;
				add_genitals(pawn, null, Gender.Male);
				add_genitals(pawn, null, Gender.Female);
				add_breasts(pawn, null, Gender.Female);
				add_anus(pawn, null);
			}
			else
			{
				Log.Message("[RJW] SexulaizeGenderlessPawn() - unable to sexualize genderless pawn " + xxx.get_pawnname(pawn) + " gender: " + pawn.gender);
			}
		}

		[SyncMethod]
		public static void SexualizeGenderedPawn(Pawn pawn)
		{
			//apply normal gender
			//Log.Message("[RJW] SexualizeGenderedPawn( " + xxx.get_pawnname(pawn) + " ) add genitals");
			add_genitals(pawn, null, pawn.gender);
			//Log.Message("[RJW] SexualizeGenderedPawn( " + xxx.get_pawnname(pawn) + " ) add breasts");
			add_breasts(pawn, null, pawn.gender);
			//Log.Message("[RJW] SexualizeGenderedPawn( " + xxx.get_pawnname(pawn) + " ) add anus");
			add_anus(pawn, null);

			//apply futa gender
			//if (pawn.gender == Gender.Female) // changing male to futa will break pawn generation due to relations

			if (pawn.Faction != null && !xxx.is_animal(pawn)) //null faction throws error
			{
				//Log.Message("[RJW] SexualizeGenderedPawn( " + xxx.get_pawnname(pawn) + " ) techLevel: " + (int)pawn.Faction.def.techLevel);
				//Log.Message("[RJW] SexualizeGenderedPawn( " + xxx.get_pawnname(pawn) + " ) techLevel: " + pawn.Faction.Name);

				//natives/spacer futa
				float chance = (int)pawn.Faction.def.techLevel < 5 ? RJWSettings.futa_natives_chance : RJWSettings.futa_spacers_chance;
				//nymph futa gender
				chance = xxx.is_nympho(pawn) ? RJWSettings.futa_nymph_chance : chance;

				if (Rand.Chance(chance))
				{
					//make futa
					if (pawn.gender == Gender.Female && RJWSettings.FemaleFuta)
						add_genitals(pawn, null, Gender.Male);
					//make trap
					else if (pawn.gender == Gender.Male && RJWSettings.MaleTrap)
						add_breasts(pawn, null, Gender.Female);
				}
			}
		}

		[SyncMethod]
		public static void sexualize_pawn(Pawn pawn)
		{
			//Log.Message("[RJW] sexualize_pawn( " + xxx.get_pawnname(pawn) + " ) called");
			if (pawn == null)
			{
				return;
			}

			if (RaceGroupDef_Helper.TryGetRaceGroupDef(pawn, out var raceGroupDef) &&
				raceGroupDef.hasSingleGender)
			{
				Log.Message($"[RJW] sexualize_pawn() - sexualizing single gender pawn {xxx.get_pawnname(pawn)}  race: {raceGroupDef.defName}");
				SexualizeSingleGenderPawn(pawn);
			}
			else if (pawn.RaceProps.hasGenders)
			{
				SexualizeGenderedPawn(pawn);
			}
			else
			{
				if (Current.ProgramState == ProgramState.Playing) // DO NOT run at world generation, throws error
				{
					SexulaizeGenderlessPawn(pawn);
					return;
				}
			}

			if (!pawn.Dead)
			{
				//Add ticks at generation, so pawns don't instantly start lovin' when generated (esp. on scenario start).
				//if (xxx.RoMIsActive && pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_ShapeshiftHD"))) // Rimworld of Magic's polymorph/shapeshift
				//	pawn.mindState.canLovinTick = Find.TickManager.TicksGame + (int)(SexUtility.GenerateMinTicksToNextLovin(pawn) * Rand.Range(0.01f, 0.05f));
				if (!xxx.is_insect(pawn))
					pawn.mindState.canLovinTick = Find.TickManager.TicksGame + (int)(SexUtility.GenerateMinTicksToNextLovin(pawn) * Rand.Range(0.1f, 1.0f));
				else
					pawn.mindState.canLovinTick = Find.TickManager.TicksGame + (int)(SexUtility.GenerateMinTicksToNextLovin(pawn) * Rand.Range(0.01f, 0.2f));

				//Log.Message("[RJW] sexualize_pawn( " + xxx.get_pawnname(pawn) + " ) add sexneed");
				if (pawn.RaceProps.Humanlike)
				{
					var sex_need = pawn.needs.TryGetNeed<Need_Sex>();
					if (pawn.Faction != null && !(pawn.Faction?.IsPlayer ?? false) && sex_need != null)
					{
						sex_need.CurLevel = Rand.Range(0.01f, 0.75f);
					}
				}
			}
		}
	}
}