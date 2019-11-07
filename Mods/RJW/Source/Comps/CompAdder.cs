using Verse;

namespace rjw
{
	[StaticConstructorOnStartup]
	public static class AddComp
	{
		static AddComp()
		{
			AddRJWComp();
		}

		/// <summary>
		/// This automatically adds the comp to all races on startup.
		/// </summary>
		public static void AddRJWComp()
		{
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.race == null )
					continue;

				thingDef.comps.Add(new CompProperties_RJW());
				//Log.Message("Adding def to race " + thingDef.label);
			}
		}
	}
}