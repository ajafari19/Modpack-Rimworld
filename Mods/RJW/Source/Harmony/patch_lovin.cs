using System;
using System.Reflection;
using Harmony;
using RimWorld;
using Verse;
using Verse.AI;

namespace rjw
{
	/// <summary>
	/// Patch:
	/// Core Loving, Mating
	/// Rational Romance LovinCasual
	/// CnP, remove pregnancy if rjw human preg enabled
	/// </summary>

	// Add a fail condition to JobDriver_Lovin that prevents pawns from lovin' if they aren't physically able/have genitals
	[HarmonyPatch(typeof(JobDriver_Lovin), "MakeNewToils")]
	internal static class PATCH_JobDriver_Lovin_MakeNewToils
	{
		[HarmonyPrefix]
		private static bool on_begin_lovin(JobDriver_Lovin __instance)
		{
			Pawn pawn = __instance.pawn;
			Pawn partner = null;
			var any_ins = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			partner = (Pawn)(__instance.GetType().GetProperty("Partner", any_ins).GetValue(__instance, null));

			__instance.FailOn(() => (!(xxx.can_fuck(pawn) || xxx.can_be_fucked(pawn))));
			__instance.FailOn(() => (!(xxx.can_fuck(partner) || xxx.can_be_fucked(partner))));

			//this breaks job
			//Log.Message("[RJW]patches_lovin::would_fuck" + xxx.would_fuck(pawn, partner));
			//__instance.FailOn(() => (xxx.would_fuck(pawn, partner) < 0.1f));
			return true;
		}
	}

	// Add a fail condition to JobDriver_Mate that prevents animals from lovin' if they aren't physically able/have genitals
	[HarmonyPatch(typeof(JobDriver_Mate), "MakeNewToils")]
	internal static class PATCH_JobDriver_Mate_MakeNewToils
	{
		[HarmonyPrefix]
		private static bool on_begin_matin(JobDriver_Mate __instance)
		{
			//only reproductive male starts mating job
			__instance.FailOn(() =>
				!(Genital_Helper.has_penis(__instance.pawn) || Genital_Helper.has_penis_infertile(__instance.pawn)));
			return true;
		}
	}

	//Patch for futa animals(vanialla - only male) to initiate mating
	[HarmonyPatch(typeof(JobGiver_Mate), "TryGiveJob")]
	internal static class PATCH_JobGiver_Mate_TryGiveJob
	{
		[HarmonyPostfix]
		public static void Postfix(Job __result, Pawn pawn)
		{
			if (__result == null)
			{

				if (!(Genital_Helper.has_penis(pawn) || Genital_Helper.has_penis_infertile(pawn)) || !pawn.ageTracker.CurLifeStage.reproductive)
				{
					__result = null;
				}
				Predicate<Thing> validator = delegate (Thing t)
				{
					Pawn pawn3 = t as Pawn;
					return !pawn3.Downed && pawn3.CanCasuallyInteractNow(false) && !pawn3.IsForbidden(pawn) && pawn3.Faction == pawn.Faction && PawnUtility.FertileMateTarget(pawn, pawn3);
				};
				Pawn pawn2 = (Pawn)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(pawn.def), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 30f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
				if (pawn2 == null)
				{
					__result = null;
				}
				__result = new Job(JobDefOf.Mate, pawn2);

			}
		}
	}

	//JobDriver_DoLovinCasual from RomanceDiversified should have handled whether pawns can do casual lovin,
	//so I don't bothered to do a check here, unless some bugs occur due to this.
	//this prob needs a patch like above, but who cares

	// Call xxx.aftersex after pawns have finished lovin'
	// You might be thinking, "wouldn't it be easier to add this code as a finish condition to JobDriver_Lovin in the patch above?" I tried that
	// at first but it didn't work because the finish condition is always called regardless of how the job ends (i.e. if it's interrupted or not)
	// and there's no way to find out from within the finish condition how the job ended. I want to make sure not apply the effects of sex if the
	// job was interrupted somehow.
	[HarmonyPatch(typeof(JobDriver), "Cleanup")]
	internal static class PATCH_JobDriver_Loving_Cleanup
	{
		//RomanceDiversified lovin
		//not very good solution, some other mod can have same named jobdrivers, but w/e
		private readonly static Type JobDriverDoLovinCasual = AccessTools.TypeByName("JobDriver_DoLovinCasual");

		//vanilla lovin
		private readonly static Type JobDriverLovin = typeof(JobDriver_Lovin);

		//vanilla mate
		private readonly static Type JobDriverMate = typeof(JobDriver_Mate);

		[HarmonyPrefix]
		private static bool on_cleanup_driver(JobDriver __instance, JobCondition condition)
		{
			if (__instance == null)
				return true;

			if (condition == JobCondition.Succeeded)
			{
				Pawn pawn = __instance.pawn;
				Pawn partner = null;

				//Log.Message("[RJW]patches_lovin::on_cleanup_driver" + xxx.get_pawnname(pawn));

				//[RF] Rational Romance [1.0] loving
				if (xxx.RomanceDiversifiedIsActive && __instance.GetType() == JobDriverDoLovinCasual)
				{
					// not sure RR can even cause pregnancies but w/e
					var any_ins = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
					partner = (Pawn)(__instance.GetType().GetProperty("Partner", any_ins).GetValue(__instance, null));
					Log.Message("[RJW]patches_lovin::on_cleanup_driver RomanceDiversified/RationalRomance:" + xxx.get_pawnname(pawn) + "+" + xxx.get_pawnname(partner));
				}
				//Vanilla loving
				else if (__instance.GetType() == JobDriverLovin)
				{
					var any_ins = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
					partner = (Pawn)(__instance.GetType().GetProperty("Partner", any_ins).GetValue(__instance, null));
				//CnP loving
					if (xxx.RimWorldChildrenIsActive && RJWPregnancySettings.humanlike_pregnancy_enabled && xxx.is_human(pawn) && xxx.is_human(partner))
					{
						Log.Message("[RJW]patches_lovin:: RimWorldChildren/ChildrenAndPregnancy pregnancy:" + xxx.get_pawnname(pawn) + "+" + xxx.get_pawnname(partner));
						PregnancyHelper.cleanup_CnP(pawn);
						PregnancyHelper.cleanup_CnP(partner);
					}
				}
				//Vanilla mating
				else if (__instance.GetType() == JobDriverMate)
				{
					var any_ins = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
					partner = (Pawn)(__instance.GetType().GetProperty("Female", any_ins).GetValue(__instance, null));
				}
				else
					return true;

				//vanilla will probably be fucked up for non humanlikes... but only humanlikes do loving, right?
				//if rjw pregnancy enabled, remove vanilla for:
				//human-human
				//animal-animal
				//bestiality
				//always remove when someone is insect or mech
				if (RJWPregnancySettings.humanlike_pregnancy_enabled && xxx.is_human(pawn) && xxx.is_human(partner)
					|| RJWPregnancySettings.animal_pregnancy_enabled && xxx.is_animal(pawn) && xxx.is_animal(partner)
					|| (RJWPregnancySettings.bestial_pregnancy_enabled && xxx.is_human(pawn) && xxx.is_animal(partner)
					|| RJWPregnancySettings.bestial_pregnancy_enabled && xxx.is_animal(pawn) && xxx.is_human(partner))
					|| xxx.is_insect(pawn) || xxx.is_insect(partner) || xxx.is_mechanoid(pawn) || xxx.is_mechanoid(partner)
					)
				{
					Log.Message("[RJW]patches_lovin::on_cleanup_driver vanilla pregnancy:" + xxx.get_pawnname(pawn) + "+" + xxx.get_pawnname(partner));
					PregnancyHelper.cleanup_vanilla(pawn);
					PregnancyHelper.cleanup_vanilla(partner);
				}

				SexUtility.ProcessSex(pawn, partner, false, true);
			}
			return true;
		}
	}
}