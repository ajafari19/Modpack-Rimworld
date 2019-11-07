using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace rjw
{
	public static class PawnExtensions
	{
		public static bool RaceHasFertility(this Pawn pawn)
		{
			// True by default.
			if (RaceGroupDef_Helper.TryGetRaceGroupDef(pawn, out var raceGroupDef))
				return raceGroupDef.hasFertility;
			return true;
		}

		public static bool RaceHasPregnancy(this Pawn pawn)
		{
			// True by default.
			if (RaceGroupDef_Helper.TryGetRaceGroupDef(pawn, out var raceGroupDef))
				return raceGroupDef.hasPregnancy;
			return true;
		}

		public static bool RaceHasOviPregnancy(this Pawn pawn)
		{
			// False by default.
			if (RaceGroupDef_Helper.TryGetRaceGroupDef(pawn, out var raceGroupDef))
				return raceGroupDef.oviPregnancy;
			return false;
		}

		public static bool RaceImplantEggs(this Pawn pawn)
		{
			// False by default.
			if (RaceGroupDef_Helper.TryGetRaceGroupDef(pawn, out var raceGroupDef))
				return raceGroupDef.ImplantEggs;
			return false;
		}

		public static bool RaceHasSexNeed(this Pawn pawn)
		{
			// True by default.
			if (RaceGroupDef_Helper.TryGetRaceGroupDef(pawn, out var raceGroupDef))
				return raceGroupDef.hasSexNeed;
			return true;
		}

		public static bool TryAddSexPart(this Pawn pawn, SexPartType sexPartType)
		{
			return RaceGroupDef_Helper.TryAddSexPart(pawn, sexPartType);
		}
	}
}
