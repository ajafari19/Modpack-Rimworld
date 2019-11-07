using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Multiplayer.API;

namespace rjw
{
	class RaceGroupDef_Helper
	{
		public static IDictionary<SexPartType, BodyPartDef> BodyPartDefBySexPartType = new Dictionary<SexPartType, BodyPartDef>();

		static RaceGroupDef_Helper()
		{
			BodyPartDefBySexPartType.Add(SexPartType.Anus, xxx.anusDef);
			BodyPartDefBySexPartType.Add(SexPartType.FemaleBreast, xxx.breastsDef);
			BodyPartDefBySexPartType.Add(SexPartType.FemaleGenital, xxx.genitalsDef);
			BodyPartDefBySexPartType.Add(SexPartType.MaleBreast, xxx.breastsDef);
			BodyPartDefBySexPartType.Add(SexPartType.MaleGenital, xxx.genitalsDef);
		}

		public static bool TryGetRaceGroupDef(Pawn pawn, out RaceGroupDef raceGroupDef)
		{
			var raceName = pawn.kindDef.race.defName;
			var pawnKindName = pawn.kindDef.defName;
			var groups = DefDatabase<RaceGroupDef>.AllDefs;
			// Not sure searching by pawnKindName is a good idea but it is needed to reproduce previous functionality 100%.
			raceGroupDef = groups.FirstOrDefault(group => group.pawnKindNames?.Contains(pawnKindName) ?? false)
				?? groups.FirstOrDefault(group => group.raceNames?.Contains(raceName) ?? false);
			return raceGroupDef != null;
		}

		public static List<HediffDef> GetParts(RaceGroupDef raceGroupDef, SexPartType sexPartType)
		{
			switch(sexPartType)
			{
				case SexPartType.Anus:
					return raceGroupDef.anuses;
				case SexPartType.FemaleBreast:
					return raceGroupDef.femaleBreasts;
				case SexPartType.FemaleGenital:
					return raceGroupDef.femaleGenitals;
				case SexPartType.MaleBreast:
					return raceGroupDef.maleBreasts;
				case SexPartType.MaleGenital:
					return raceGroupDef.maleGenitals;
				default:
					throw new ApplicationException($"Unrecognized sexPartType: {sexPartType}");
			}
		}
		public static List<float> GetPartsChances(RaceGroupDef raceGroupDef, SexPartType sexPartType)
		{
			switch(sexPartType)
			{
				case SexPartType.Anus:
					return raceGroupDef.chanceanuses;
				case SexPartType.FemaleBreast:
					return raceGroupDef.chancefemaleBreasts;
				case SexPartType.FemaleGenital:
					return raceGroupDef.chancefemaleGenitals;
				case SexPartType.MaleBreast:
					return raceGroupDef.chancemaleBreasts;
				case SexPartType.MaleGenital:
					return raceGroupDef.chancemaleGenitals;
				default:
					throw new ApplicationException($"Unrecognized sexPartType: {sexPartType}");
			}
		}

		/// <summary>
		/// Returns true if a sex part was chosen (even if that part is "no part").
		/// </summary>
		[SyncMethod]
		public static bool TryAddSexPart(Pawn pawn, SexPartType sexPartType)
		{
			if (!TryGetRaceGroupDef(pawn, out var raceGroupDef))
			{
				// No race, so nothing was chosen.
				return false;
			}

			var parts = GetParts(raceGroupDef, sexPartType);
			if (parts == null)
			{
				// Missing list, so nothing was chosen.
				return false;
			}
			else if (!parts.Any())
			{
				// Empty list, so "no part" was chosen.
				return true;
			}

			var target = BodyPartDefBySexPartType[sexPartType];
			var part = pawn.RaceProps.body.AllParts.Find(bpr => bpr.def == target);
			HediffDef hediffDef = parts.RandomElement();

			if (parts.Count() > 1)
			{
				List<float> partschances = GetPartsChances(raceGroupDef, sexPartType);
				if (partschances.Count() > 1)
				{
					float chance = Rand.Value;
					List<HediffDef> filteredparts = null;
					for (int i = 0;  i < partschances.Count(); i++)
						if (chance <= partschances[i])
							filteredparts.Add(parts[i]);

					if (filteredparts.Any())
						hediffDef = filteredparts.RandomElement();
				}
			}
			pawn.health.AddHediff(hediffDef, part);
			// A part was chosen and added.
			return true;
		}
	}
}
