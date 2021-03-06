﻿using System;
using System.Collections.Generic;
using Harmony;
using RimWorld;
using Verse;
using Verse.AI.Group;
using System.Reflection;

namespace rjw
{
	//TODO: Remove this patch for compatibility with other mods and add them to genital helper class.
	/*
	[HarmonyPatch(typeof(PawnGenerator), "GeneratePawn", new Type[] { typeof(PawnGenerationRequest) })]
	internal static class Patches_GenerateRapist
	{
		public static void Postfix(Pawn __result, ref PawnGenerationRequest request)
		{
			if (__result == null) return;

			if (__result.RaceProps.IsMechanoid)
			{
				BodyPartRecord genitalPart = __result.RaceProps.body.AllParts.Find(bpr => bpr.def.defName == "Genitals");
				__result.health.AddHediff(HediffDef.Named("BionicPenis"), genitalPart);
			}

			if (PawnGenerationContext.NonPlayer.Includes(request.Context))
			{
				Need need = __result.needs.TryGetNeed<Need_Sex>();
				if (need != null)
				{
					//need.ForceSetLevel(Rand.Range(0f,1f));
					need.ForceSetLevel(Rand.Range(0.01f, 0.2f));
				}
			}
		}
	}*/

	[HarmonyPatch(typeof(LordJob_AssaultColony), "CreateGraph")]
	internal static class Patches_AssaultColonyForRape
	{
		public static void Postfix(StateGraph __result)
		{
			//--Log.Message("[ABF]AssaultColonyForRape::CreateGraph");
			if (__result == null) return;
			//--Log.Message("[RJW]AssaultColonyForRape::CreateGraph");
			foreach (var trans in __result.transitions)
			{
				if (HasDesignatedTransition(trans))
				{
					foreach (Trigger t in trans.triggers)
					{
						if (t.filters == null)
						{
							t.filters = new List<TriggerFilter>() { new Trigger_SexSatisfy(0.3f) };
						}
						else
						{
							t.filters.Add(new Trigger_SexSatisfy(0.3f));
						}
					}
					//--Log.Message("[ABF]AssaultColonyForRape::CreateGraph Adding SexSatisfyTrigger to " + trans.ToString());
				}
			}
		}

		private static bool HasDesignatedTransition(Transition t)
		{
			if (t.target == null) return false;
			if (t.target.GetType() == typeof(LordToil_KidnapCover)) return true;

			foreach (Trigger ta in t.triggers)
			{
				if (ta.GetType() == typeof(Trigger_FractionColonyDamageTaken)) return true;
			}
			return false;
		}
	}

	/*
	[HarmonyPatch(typeof(JobGiver_Manhunter), "TryGiveJob")]
	static class Patches_ABF_MunHunt
	{
		public static void Postfix(Job __result, ref Pawn pawn)
		{
			//--Log.Message("[RJW]Patches_ABF_MunHunt::Postfix called");
			if (__result == null) return;

			if (__result.def == JobDefOf.Wait || __result.def == JobDefOf.Goto) __result = null;
		}
	}
	*/

	/*
	//temporaly added code.  Adding record will make savedata error...
	[HarmonyPatch(typeof(DefMap<RecordDef, float>), "ExposeData")]
	internal static class Patches_DefMapTweak
	{
		public static bool Prefix(DefMap<RecordDef, float> __instance)
		{
			var field = __instance.GetType().GetField("values", BindingFlags.GetField | BindingFlags.SetField | BindingFlags.NonPublic | BindingFlags.Instance);
			var l = (field.GetValue(__instance) as List<float>);
			while (l.Count < DefDatabase<RecordDef>.DefCount)
			{
				l.Add(0f);
			}
			//field.SetValue(__instance, (int)() + 1);
			return true;
		}
	}
	[HarmonyPatch(typeof(MemoryThoughtHandler), "TryGainMemory", new Type[]{typeof(Thought_Memory),typeof(Pawn)})]
	internal static class Patches_MemoriesDebug
	{
		public static bool Prefix(MemoryThoughtHandler __instance, ref Thought_Memory newThought, ref Pawn otherPawn)
		{
			Log.Message(__instance.xxx.get_pawnname(pawn) + " adding " + newThought.LabelCap);
			return true;
		}
	}
	*/
	[HarmonyPatch(typeof(SkillRecord), "CalculateTotallyDisabled")]
	internal static class Patches_SkillRecordDebug
	{
		public static bool Prefix(SkillRecord __instance,ref bool __result)
		{
			var field = __instance.GetType().GetField("pawn", BindingFlags.GetField | BindingFlags.SetField | BindingFlags.NonPublic | BindingFlags.Instance);
			Pawn pawn = (field.GetValue(__instance) as Pawn);
			if (__instance.def == null)
			{
				Log.Message("no def!");
				__result = false;
				return false;
			}
			return true;
		}
	}
}