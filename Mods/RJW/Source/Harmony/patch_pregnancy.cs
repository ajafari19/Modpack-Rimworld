using Harmony;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Multiplayer.API;

namespace rjw
{
	[HarmonyPatch(typeof(Hediff_Pregnant), "DoBirthSpawn")]
	internal static class PATCH_Hediff_Pregnant_DoBirthSpawn
	{
		/// <summary>
		/// This one overrides vanilla pregnancy hediff behavior.
		/// 0 - try to find suitable father for debug pregnancy
		/// 1st part if character pregnant and rjw pregnancies enabled - creates rjw pregnancy and instantly births it instead of vanilla
		/// 2nd part if character pregnant with rjw pregnancy - birth it
		/// 3rd part - debug - create rjw/vanila pregnancy and birth it
		/// </summary>
		/// <param name="mother"></param>
		/// <param name="father"></param>
		/// <returns></returns>
		[HarmonyPrefix]
		[SyncMethod]
		private static bool on_begin_DoBirthSpawn(ref Pawn mother, ref Pawn father)
		{
			//--Log.Message("patches_pregnancy::PATCH_Hediff_Pregnant::DoBirthSpawn() called");

			if (mother == null)
			{
				Log.Error("Hediff_Pregnant::DoBirthSpawn() - no mother defined -> exit");
				return false;
			}

			//vanilla debug?
			if (mother.gender == Gender.Male)
			{
				Log.Error("Hediff_Pregnant::DoBirthSpawn() - mother is male -> exit");
				return false;
			}

			//Rand.PopState();
			//Rand.PushState(RJW_Multiplayer.PredictableSeed());

			// get a reference to the hediff we are applying
			//do birth for vanilla pregnancy Hediff
			//if using rjw pregnancies - add RJW pregnancy Hediff and birth it instead
			Hediff_Pregnant self = (Hediff_Pregnant)mother.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("Pregnant"));
			if (self != null)
			{
				if (father == null)
				{
					father = Hediff_BasePregnancy.Trytogetfather(ref mother);
				}
				Log.Message("patches_pregnancy::PATCH_Hediff_Pregnant::DoBirthSpawn():Vanilla_pregnancy birthing:" + xxx.get_pawnname(mother));
				if (RJWPregnancySettings.animal_pregnancy_enabled && ((father == null || xxx.is_animal(father)) && xxx.is_animal(mother)))
				{
					//RJW Bestial pregnancy animal-animal
					Log.Message(" override as Bestial birthing(animal-animal): Father-" + xxx.get_pawnname(father) + " Mother-" + xxx.get_pawnname(mother));
					Hediff_BestialPregnancy.Create(mother, father);
					Hediff_BestialPregnancy hediff = (Hediff_BestialPregnancy)mother.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("RJW_pregnancy_beast"));
					hediff.Initialize(mother, father);
					hediff.GiveBirth();
					if (self != null)
						mother.health.RemoveHediff(self);

					return false;
				}
				else if (RJWPregnancySettings.bestial_pregnancy_enabled && ((xxx.is_animal(father) && xxx.is_human(mother)) || (xxx.is_human(father) && xxx.is_animal(mother))))
				{
					//RJW Bestial pregnancy human-animal
					Log.Message(" override as Bestial birthing(human-animal): Father-" + xxx.get_pawnname(father) + " Mother-" + xxx.get_pawnname(mother));
					Hediff_BestialPregnancy.Create(mother, father);
					Hediff_BestialPregnancy hediff = (Hediff_BestialPregnancy)mother.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("RJW_pregnancy_beast"));
					hediff.Initialize(mother, father);
					hediff.GiveBirth();
					if (self != null)
						mother.health.RemoveHediff(self);

					return false;
				}
				else if (RJWPregnancySettings.humanlike_pregnancy_enabled && (xxx.is_human(father) && xxx.is_human(mother)))
				{
					//RJW Humanlike pregnancy
					Log.Message(" override as Humanlike birthing: Father-" + xxx.get_pawnname(father) + " Mother-" + xxx.get_pawnname(mother));
					Hediff_HumanlikePregnancy.Create(mother, father);
					Hediff_HumanlikePregnancy hediff = (Hediff_HumanlikePregnancy)mother.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("RJW_pregnancy"));
					hediff.Initialize(mother, father);
					hediff.GiveBirth();
					if (self != null)
						mother.health.RemoveHediff(self);

					return false;
				}
				else
				{
					Log.Warning("Hediff_Pregnant::DoBirthSpawn() - rjw checks failed, vanilla pregnancy birth");
					Log.Warning("Hediff_Pregnant::DoBirthSpawn(): Father-" + xxx.get_pawnname(father) + " Mother-" + xxx.get_pawnname(mother));
					//vanilla pregnancy code, no effects on rjw
					int num = (mother.RaceProps.litterSizeCurve == null) ? 1 : Mathf.RoundToInt(Rand.ByCurve(mother.RaceProps.litterSizeCurve));
					if (num < 1)
					{
						num = 1;
					}
					PawnGenerationRequest request = new PawnGenerationRequest(mother.kindDef, mother.Faction, PawnGenerationContext.NonPlayer, -1, forceGenerateNewPawn: false, newborn: true);
					Pawn pawn = null;
					for (int i = 0; i < num; i++)
					{
						pawn = PawnGenerator.GeneratePawn(request);
						if (PawnUtility.TrySpawnHatchedOrBornPawn(pawn, mother))
						{
							if (pawn.playerSettings != null && mother.playerSettings != null)
							{
								pawn.playerSettings.AreaRestriction = mother.playerSettings.AreaRestriction;
							}
							if (pawn.RaceProps.IsFlesh)
							{
								pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, mother);
								if (father != null)
								{
									pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, father);
								}
							}
						}
						else
						{
							Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
						}
						TaleRecorder.RecordTale(TaleDefOf.GaveBirth, mother, pawn);
					}
					if (mother.Spawned)
					{
						FilthMaker.MakeFilth(mother.Position, mother.Map, ThingDefOf.Filth_AmnioticFluid, mother.LabelIndefinite(), 5);
						if (mother.caller != null)
						{
							mother.caller.DoCall();
						}
						if (pawn.caller != null)
						{
							pawn.caller.DoCall();
						}
					}
					if (self != null)
						mother.health.RemoveHediff(self);

					return false;
				}
			}

			// do birth for existing RJW pregnancies

			Hediff_HumanlikePregnancy rjwH = (Hediff_HumanlikePregnancy)mother.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("RJW_pregnancy"));
			if (rjwH != null)
			{
				//RJW Bestial pregnancy
				Log.Message("patches_pregnancy::PATCH_Hediff_Pregnant::DoBirthSpawn():RJW_pregnancy birthing:" + xxx.get_pawnname(mother));
				rjwH.GiveBirth();
				if (self == null)
					return false;
			}

			Hediff_BestialPregnancy rjwB = (Hediff_BestialPregnancy)mother.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("RJW_pregnancy_beast"));
			if (rjwB != null)
			{
				//RJW Humanlike pregnancy
				Log.Message("patches_pregnancy::PATCH_Hediff_Pregnant::DoBirthSpawn():RJW_pregnancy_beast birthing:" + xxx.get_pawnname(mother));
				rjwB.GiveBirth();
				if (self == null)
					return false;
			}

			//debug, add RJW pregnancy and birth it
			Log.Message("patches_pregnancy::PATCH_Hediff_Pregnant::DoBirthSpawn():Debug_pregnancy birthing:" + xxx.get_pawnname(mother));
			if (father == null)
			{
				father = Hediff_BasePregnancy.Trytogetfather(ref mother);
			}
			/*
			if (true)
			{
				//RJW Mech pregnancy
				Log.Message(" override as mech birthing:" + xxx.get_pawnname(mother));
				Hediff_MechanoidPregnancy.Create(mother, father);
				Hediff_MechanoidPregnancy hediff = (Hediff_MechanoidPregnancy)mother.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("RJW_pregnancy_mech"));
				hediff.GiveBirth();
				return false;
			}
			*/

			if (RJWPregnancySettings.bestial_pregnancy_enabled && ((xxx.is_animal(father) || xxx.is_animal(mother)))
				|| (xxx.is_animal(mother) && RJWPregnancySettings.animal_pregnancy_enabled))
			{
				//RJW Bestial pregnancy
				Log.Message(" override as Bestial birthing, mother: " + xxx.get_pawnname(mother));
				Hediff_BestialPregnancy.Create(mother, father);
				Hediff_BestialPregnancy hediff = (Hediff_BestialPregnancy)mother.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("RJW_pregnancy_beast"));
				hediff.GiveBirth();
			}
			else if (RJWPregnancySettings.humanlike_pregnancy_enabled && ((father == null ||xxx.is_human(father)) && xxx.is_human(mother)))
			{
				//RJW Humanlike pregnancy
				Log.Message(" override as Humanlike birthing, mother: " + xxx.get_pawnname(mother));
				Hediff_HumanlikePregnancy.Create(mother, father);
				Hediff_HumanlikePregnancy hediff = (Hediff_HumanlikePregnancy)mother.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("RJW_pregnancy"));
				hediff.GiveBirth();
			}
			else
			{
				Log.Warning("Hediff_Pregnant::DoBirthSpawn() - debug vanilla pregnancy birth");
				//vanilla code
				int num = (mother.RaceProps.litterSizeCurve == null) ? 1 : Mathf.RoundToInt(Rand.ByCurve(mother.RaceProps.litterSizeCurve));
				if (num < 1)
				{
					num = 1;
				}
				PawnGenerationRequest request = new PawnGenerationRequest(mother.kindDef, mother.Faction, PawnGenerationContext.NonPlayer, -1, forceGenerateNewPawn: false, newborn: true);
				Pawn pawn = null;
				for (int i = 0; i < num; i++)
				{
					pawn = PawnGenerator.GeneratePawn(request);
					if (PawnUtility.TrySpawnHatchedOrBornPawn(pawn, mother))
					{
						if (pawn.playerSettings != null && mother.playerSettings != null)
						{
							pawn.playerSettings.AreaRestriction = mother.playerSettings.AreaRestriction;
						}
						if (pawn.RaceProps.IsFlesh)
						{
							pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, mother);
							if (father != null)
							{
								pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, father);
							}
						}
					}
					else
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
					}
					TaleRecorder.RecordTale(TaleDefOf.GaveBirth, mother, pawn);
				}
				if (mother.Spawned)
				{
					FilthMaker.MakeFilth(mother.Position, mother.Map, ThingDefOf.Filth_AmnioticFluid, mother.LabelIndefinite(), 5);
					if (mother.caller != null)
					{
						mother.caller.DoCall();
					}
					if (pawn.caller != null)
					{
						pawn.caller.DoCall();
					}
				}
				if (self != null)
					mother.health.RemoveHediff(self);
			}
			return false;
		}
	}

	
	[HarmonyPatch(typeof(Hediff_Pregnant), "Tick")]
	class PATCH_Hediff_Pregnant_Tick
	{
		[HarmonyPrefix]
		static bool on_begin_Tick( Hediff_Pregnant __instance )
		{
			if (__instance.pawn.IsHashIntervalTick(1000))
			{
				if (!Genital_Helper.has_vagina(__instance.pawn))
				{
					__instance.pawn.health.RemoveHediff(__instance);
				}
			}
			return true;
		}
	}
}