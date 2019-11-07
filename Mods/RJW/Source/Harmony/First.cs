using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Harmony;
using RimWorld;
using Verse;

namespace rjw
{
    // Add genitalia to pawns that have the comp.
	[HarmonyPatch(typeof(PawnGenerator), "GenerateTraits")]
	public static class PawnGenerator_GenitaliaPatch
	{
		[HarmonyPostfix]
		public static void AddGenitalia(ref Pawn pawn, ref PawnGenerationRequest request)
		{
			if (CompRJW.Comp(pawn) == null) return;
			if (CompRJW.Comp(pawn).orientation == Orientation.None)
			{
				CompRJW.Comp(pawn).Sexualize(pawn);
			}
		}
	}

	[StaticConstructorOnStartup]
	internal static class First
	{
		/*private static void show_bpr(String body_part_record_def_name)
		{
			var bpr = BodyDefOf.Human.AllParts.Find((BodyPartRecord can) => String.Equals(can.def.defName, body_part_record_def_name));
			--Log.Message(body_part_record_def_name + " BPR internals:");
			--Log.Message("  def: " + bpr.def.ToString());
			--Log.Message("  parts: " + bpr.parts.ToString());
			--Log.Message("  parts.count: " + bpr.parts.Count.ToString());
			--Log.Message("  height: " + bpr.height.ToString());
			--Log.Message("  depth: " + bpr.depth.ToString());
			--Log.Message("  coverage: " + bpr.coverage.ToString());
			--Log.Message("  groups: " + bpr.groups.ToString());
			--Log.Message("  groups.count: " + bpr.groups.Count.ToString());
			--Log.Message("  parent: " + bpr.parent.ToString());
			--Log.Message ("  fleshCoverage: " + bpr.fleshCoverage.ToString ());
			--Log.Message ("  absoluteCoverage: " + bpr.absoluteCoverage.ToString ());
			--Log.Message ("  absoluteFleshCoverage: " + bpr.absoluteFleshCoverage.ToString ());
		}*/

		//Children mod use same defname. but not has worker class. so overriding here.
		public static void inject_Reproduction()
		{
			PawnCapacityDef reproduction = DefDatabase<PawnCapacityDef>.GetNamed("Reproduction");
			reproduction.workerClass = typeof(PawnCapacityWorker_Fertility);
		}

		private static void inject_recipes()
		{
			//--Log.Message("[RJW] First::inject_recipes");
			// Inject the recipes to create the artificial privates into the crafting spot or machining bench.
			// BUT, also dynamically detect if EPOE is loaded and, if it is, inject the recipes into EPOE's
			// crafting benches instead.


			if (IsLoaded("Lord of the Rims - The Third Age"))
			{
				// no idea why it removes MakePegDick recipe
				Log.Warning("[RJW]The Third Age is installed. Unable to inject advanced recipes.");
				return;
			}

			//Vanilla benches
			var cra_spo = DefDatabase<ThingDef>.GetNamed("CraftingSpot");
			var mac_ben = DefDatabase<ThingDef>.GetNamed("TableMachining");
			var fab_ben = DefDatabase<ThingDef>.GetNamed("FabricationBench");
			var tai_ben = DefDatabase<ThingDef>.GetNamed("ElectricTailoringBench");

			//EPOE benches
			var bas_ben = DefDatabase<ThingDef>.GetNamed("TableBasicProsthetic", false);
			var sim_ben = DefDatabase<ThingDef>.GetNamed("TableSimpleProsthetic", false);
			var bio_ben = DefDatabase<ThingDef>.GetNamed("TableBionics", false);

			(bas_ben ?? cra_spo).AllRecipes.Add(DefDatabase<RecipeDef>.GetNamed("MakePegDick"));

			(sim_ben ?? mac_ben).AllRecipes.Add(DefDatabase<RecipeDef>.GetNamed("MakeHydraulicAnus"));
			(sim_ben ?? mac_ben).AllRecipes.Add(DefDatabase<RecipeDef>.GetNamed("MakeHydraulicBreasts"));
			(sim_ben ?? mac_ben).AllRecipes.Add(DefDatabase<RecipeDef>.GetNamed("MakeHydraulicPenis"));
			(sim_ben ?? mac_ben).AllRecipes.Add(DefDatabase<RecipeDef>.GetNamed("MakeHydraulicVagina"));

			(bio_ben ?? fab_ben).AllRecipes.Add(DefDatabase<RecipeDef>.GetNamed("MakeBionicAnus"));
			(bio_ben ?? fab_ben).AllRecipes.Add(DefDatabase<RecipeDef>.GetNamed("MakeBionicBreasts"));
			(bio_ben ?? fab_ben).AllRecipes.Add(DefDatabase<RecipeDef>.GetNamed("MakeBionicPenis"));
			(bio_ben ?? fab_ben).AllRecipes.Add(DefDatabase<RecipeDef>.GetNamed("MakeBionicVagina"));

			// Inject the bondage gear recipes into their appropriate benches
			if (xxx.config.bondage_gear_enabled)
			{
				tai_ben.AllRecipes.Add(DefDatabase<RecipeDef>.GetNamed("MakeHololock"));
				tai_ben.AllRecipes.Add(DefDatabase<RecipeDef>.GetNamed("MakeArmbinder"));
				tai_ben.AllRecipes.Add(DefDatabase<RecipeDef>.GetNamed("MakeChastityBelt"));
				tai_ben.AllRecipes.Add(DefDatabase<RecipeDef>.GetNamed("MakeGag"));
			}
		}

		/*private static void show_bs(Backstory bs)
		{
			--Log.Message("Backstory \"" + bs.Title + "\" internals:");
			--Log.Message("  identifier: " + bs.identifier);
			--Log.Message("  slot: " + bs.slot.ToString());
			--Log.Message("  Title: " + bs.Title);
			--Log.Message("  TitleShort: " + bs.TitleShort);
			--Log.Message("  baseDesc: " + bs.baseDesc);
			--Log.Message("  skillGains: " + ((bs.skillGains == null) ? "null" : bs.skillGains.ToString()));
			--Log.Message("  skillGainsResolved: " + ((bs.skillGainsResolved == null) ? "null" : bs.skillGainsResolved.ToString()));
			--Log.Message("  workDisables: " + bs.workDisables.ToString());
			--Log.Message("  requiredWorkTags: " + bs.requiredWorkTags.ToString());
			--Log.Message("  spawnCategories: " + bs.spawnCategories.ToString());
			--Log.Message("  bodyTypeGlobal: " + bs.bodyTypeGlobal.ToString());
			--Log.Message("  bodyTypeFemale: " + bs.bodyTypeFemale.ToString());
			--Log.Message("  bodyTypeMale: " + bs.bodyTypeMale.ToString());
			--Log.Message("  forcedTraits: " + ((bs.forcedTraits == null) ? "null" : bs.forcedTraits.ToString()));
			--Log.Message("  disallowedTraits: " + ((bs.disallowedTraits == null) ? "null" : bs.disallowedTraits.ToString()));
			--Log.Message("  shuffleable: " + bs.shuffleable.ToString());
		}*/

		//Quick check to see if an another mod is loaded.
		private static bool IsLoaded(string mod)
		{
			return LoadedModManager.RunningModsListForReading.Any(x => x.Name == mod);
		}

		private static void CheckingCompatibleMods()
		{
			//RomanceDiversified
			try
			{
				xxx.straight = DefDatabase<TraitDef>.GetNamedSilentFail("Straight");
				xxx.bisexual = DefDatabase<TraitDef>.GetNamedSilentFail("Bisexual");
				xxx.asexual = DefDatabase<TraitDef>.GetNamedSilentFail("Asexual");
				xxx.faithful = DefDatabase<TraitDef>.GetNamedSilentFail("Faithful");
				xxx.philanderer = DefDatabase<TraitDef>.GetNamedSilentFail("Philanderer");
				xxx.polyamorous = DefDatabase<TraitDef>.GetNamedSilentFail("Polyamorous");
				if (xxx.straight is null)
				{
					xxx.RomanceDiversifiedIsActive = false;
					if (Prefs.DevMode) Log.Message("[RJW]RomanceDiversified is not detected");
				}
				else
				{
					xxx.RomanceDiversifiedIsActive = true;
					if (Prefs.DevMode) Log.Message("[RJW]RomanceDiversified is detected.");
				}
			}
			catch (Exception)
			{
				xxx.RomanceDiversifiedIsActive = false;
				xxx.straight = null;
				xxx.bisexual = null;
				xxx.asexual = null;
				xxx.faithful = null;
				xxx.philanderer = null;
				xxx.polyamorous = null;
				if (Prefs.DevMode) Log.Message("[RJW]RomanceDiversified is not detected.error");
			}
			
			if (IsLoaded("[KV] Consolidated Traits - 1.0"))
			{
				Log.Message("[RJW]Consolidated Traits found, adding trait compatibility.");
				xxx.CTIsActive = true;
			}
			else
			{
				xxx.CTIsActive = false;
			}

			if (IsLoaded("Lord of the Rims - The Third Age"))
			{
				Log.Warning("[RJW]The Third Age detected. High tech items removed");
			}

			//Rimworld of Magic - need this for the check to avoid undead pregnancies
			if (IsLoaded("A RimWorld of Magic"))
			{
				xxx.RoMIsActive = true;
				if (Prefs.DevMode) Log.Message("[RJW]Rimworld of Magic is detected.");
			}
			else
			{
				xxx.RoMIsActive = false;
			}

			//[SYR] Individuality
			if (IsLoaded("[SYR] Individuality"))
			{
				xxx.IndividualityIsActive = true;
				if (Prefs.DevMode) Log.Message("[RJW]Individuality is detected.");
			}
			else
			{
				xxx.IndividualityIsActive = false;
			}

			//Humanoid Alien Races Framework 2.0
			if (IsLoaded("Humanoid Alien Races 2.0"))
			{
				xxx.AlienFrameworkIsActive = true;
				xxx.xenophobia = DefDatabase<TraitDef>.GetNamedSilentFail("Xenophobia");
				if (Prefs.DevMode) Log.Message("[RJW]Humanoid Alien Races 2.0 is detected. Xenophile and Xenophobe traits active.");
			}
			else
			{
				xxx.AlienFrameworkIsActive = false;
			}

			//Psychology
			if (IsLoaded("Psychology"))
			{
				xxx.PsychologyIsActive = true;
				xxx.prude = DefDatabase<TraitDef>.GetNamedSilentFail("Prude");
				xxx.lecher = DefDatabase<TraitDef>.GetNamedSilentFail("Lecher");
				xxx.polygamous = DefDatabase<TraitDef>.GetNamedSilentFail("Polygamous");
				if (Prefs.DevMode) Log.Message("[RJW]Psychology is detected. (Note: only partially supported)");
			}
			else
			{
				xxx.PsychologyIsActive = false;
			}

			//SimpleSlavery
			if (IsLoaded("Simple Slavery[1.0]"))
			{
				xxx.SimpleSlaveryIsActive = true;
				if (Prefs.DevMode) Log.Message("[RJW]SimpleSlavery is detected.");
			}
			else
			{
				xxx.SimpleSlaveryIsActive = false;
			}

			//DubsBadHygiene
			if (IsLoaded("Dubs Bad Hygiene"))
			{
				xxx.DubsBadHygieneIsActive = true;
				if (Prefs.DevMode) Log.Message("[RJW]Dubs Bad Hygiene is detected.");
			}
			else
			{
				xxx.DubsBadHygieneIsActive = false;
			}

			//Children and Pregnancy
			try
			{
				//bool CNPActive = ModsConfig.ActiveModsInLoadOrder.FirstIndexOf(m => m.Name == "Children and Pregnancy");//Won't work, name is wrong
				xxx.babystate = DefDatabase<HediffDef>.GetNamedSilentFail("BabyState");
				if (xxx.babystate is null)
				{
					xxx.RimWorldChildrenIsActive = false;
					if (Prefs.DevMode) Log.Message("[RJW]Children&Pregnancy is not detected");
				}
				else
				{
					xxx.RimWorldChildrenIsActive = true;
					if (Prefs.DevMode) Log.Message("[RJW]Children&Pregnancy is detected.");
				}
			}
			catch (Exception)
			{
				xxx.RimWorldChildrenIsActive = false; //A dirty way to check if the mod is active
				xxx.babystate = null;
				if (Prefs.DevMode) Log.Message("[RJW]Children&Pregnancy is not detected.error");
			}

			//Combat Extended
			try
			{
				xxx.MuscleSpasms = DefDatabase<HediffDef>.GetNamedSilentFail("MuscleSpasms");
				if (xxx.MuscleSpasms is null)
				{
					xxx.CombatExtendedIsActive = false;
				}
				else
				{
					xxx.CombatExtendedIsActive = true;
					if (Prefs.DevMode) Log.Message("[RJW]Combat Extended is detected. Current compatibility unknown, use at your own risk. ");
				}
			}
			catch (Exception)
			{
				xxx.CombatExtendedIsActive = false;
				xxx.MuscleSpasms = null;
				if (Prefs.DevMode) Log.Message("[RJW]Combat Extended is not detected.error");
			}
		}

		static First()
		{
			//--Log.Message("[RJW] First::First() called");

			// check for required mods
			//CheckModRequirements();
			//CheckIncompatibleMods();
			CheckingCompatibleMods();

			inject_recipes();
			inject_Reproduction();

			nymph_backstories.init();
			bondage_gear_tradeability.init();

			var har = HarmonyInstance.Create("rjw");
			har.PatchAll(Assembly.GetExecutingAssembly());
			PATCH_Pawn_ApparelTracker_TryDrop.apply(har);
			//CnPcompatibility.Patch(har);                          //CnP IS NO OUT YET
		}

		internal static void CheckModRequirements()
		{
			//--Log.Message("First::CheckModRequirements() called");
			List<string> required_mods = new List<string> {
				"HugsLib",
			};
			foreach (string required_mod in required_mods)
			{
				bool found = false;
				foreach (ModMetaData installed_mod in ModLister.AllInstalledMods)
				{
					if (installed_mod.Active && installed_mod.Name.Contains(required_mod))
					{
						found = true;
					}

					if (!found)
					{
						ErrorMissingRequirement(required_mod);
					}
				}
			}
		}

		internal static void CheckIncompatibleMods()
		{
			//--Log.Message("First::CheckIncompatibleMods() called");
			List<string> incompatible_mods = new List<string> {
				"Bogus Test Mod That Doesn't Exist"
			};
			foreach (string incompatible_mod in incompatible_mods)
			{
				foreach (ModMetaData installed_mod in ModLister.AllInstalledMods)
				{
					if (installed_mod.Active && installed_mod.Name.Contains(incompatible_mod))
					{
						ErrorIncompatibleMod(installed_mod);
					}
				}
			}
		}

		internal static void ErrorMissingRequirement(string missing)
		{
			Log.Error("Initialization error:  Unable to find required mod '" + missing + "' in mod list");
		}

		internal static void ErrorIncompatibleMod(ModMetaData othermod)
		{
			Log.Error("Initialization Error:  Incompatible mod '" + othermod.Name + "' detected in mod list");
		}
	}
}