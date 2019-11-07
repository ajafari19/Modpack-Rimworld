using System;
using Verse;
using RimWorld;
using System.Linq;
using System.Collections.Generic;

namespace rjw
{
	/// <summary>
	/// Looks up and returns a BodyPartTagDef defined in the XML
	/// </summary>
	public static class BodyPartTagDefOf {
		public static BodyPartTagDef RJW_FertilitySource
		{
			get
			{
				if (a == null) a = (BodyPartTagDef)GenDefDatabase.GetDef(typeof(BodyPartTagDef), "RJW_FertilitySource");
				return a;
			}
		}
		private static BodyPartTagDef a;
	}
	
}
