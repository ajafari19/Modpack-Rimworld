using System.Linq;
using Verse;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Harmony;
using Harmony.ILCopying;
using RimWorld;
using UnityEngine;
using Verse.AI;
using Verse.Grammar;
using OpCodes = System.Reflection.Emit.OpCodes;

namespace rjw
{
	/// <summary>
	/// Patch all races
	/// 1) into rjw parts recipes 
	/// 2) remove bodyparts from non animals and human likes
	/// </summary>

	[StaticConstructorOnStartup]
	public static class HarmonyPatches
	{
		static HarmonyPatches()
		{
			//summons carpet bombing

			//inject races into rjw recipes
			foreach (RecipeDef x in	DefDatabase<RecipeDef>.AllDefsListForReading.Where(x => x.IsSurgery && (x.targetsBodyPart || !x.appliedOnFixedBodyParts.NullOrEmpty())))
			{
				if (x.appliedOnFixedBodyParts.Contains(xxx.genitalsDef)
					|| x.appliedOnFixedBodyParts.Contains(xxx.breastsDef)
					|| x.appliedOnFixedBodyParts.Contains(xxx.anusDef)
					)

					foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs.Where(thingDef =>
							thingDef.race != null && (
							thingDef.race.Humanlike ||
							thingDef.race.Animal
							)))
					{
						//filter out something, probably?
						//if (thingDef.race. == "Human")
						//	continue;

						if (!x.recipeUsers.Contains(thingDef))
							x.recipeUsers.Add(item: thingDef);
					}
			}

			//TODO: fix errors?
			/*
			//remove rjw bodyparts from non animals and non humanlikes
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs.Where(thingDef =>
					thingDef.race != null && !(
					thingDef.race.Humanlike ||
					thingDef.race.Animal
					)))
			{
					if (thingDef.race.body.AllParts.Exists(x => x.def == xxx.genitalsDef))
						thingDef.race.body.AllParts.Remove(thingDef.race.body.AllParts.Find(bpr => bpr.def.defName == "Genitals"));

					if (thingDef.race.body.AllParts.Exists(x => x.def == xxx.breastsDef))
						thingDef.race.body.AllParts.Remove(thingDef.race.body.AllParts.Find(bpr => bpr.def.defName == "Chest"));

					if (thingDef.race.body.AllParts.Exists(x => x.def == xxx.anusDef))
						thingDef.race.body.AllParts.Remove(thingDef.race.body.AllParts.Find(bpr => bpr.def.defName == "Anus"));
			}
			*/
		}
	}
}
