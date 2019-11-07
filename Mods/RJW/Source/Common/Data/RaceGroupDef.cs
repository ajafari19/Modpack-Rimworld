using System.Collections.Generic;
using Verse;

namespace rjw
{
	class RaceGroupDef : Def
	{
		public List<string> raceNames = null;
		public List<string> pawnKindNames = null;
		public List<HediffDef> anuses = null;
		public List<float> chanceanuses = null;
		public List<HediffDef> femaleBreasts = null;
		public List<float> chancefemaleBreasts = null;
		public List<HediffDef> femaleGenitals = null;
		public List<float> chancefemaleGenitals = null;
		public List<HediffDef> maleBreasts = null;
		public List<float> chancemaleBreasts = null;
		public List<HediffDef> maleGenitals = null;
		public List<float> chancemaleGenitals = null;
		public bool hasSingleGender = false;
		// Not sure this is actually used? There was a StringListDef for it so I added it.
		public bool hasSexNeed = true;
		public bool hasFertility = true;
		public bool hasPregnancy = true;
		public bool oviPregnancy = false;
		public bool ImplantEggs = false;
	}
}
