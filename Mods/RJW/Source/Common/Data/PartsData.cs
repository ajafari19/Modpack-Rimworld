using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using HugsLib;

namespace rjw
{
	/// <summary>
	/// Utility rjw data object for storing Parts(genitals, breasts, anuses) data between operations
	/// </summary>
	public class PartsData : IExposable
	{
		public Pawn Pawn = null; // store original owner , maybe better kinddef?
		public Thing Thing = null; // store world thing?
		public float Severity = 0.0f; // store size

		public PartsData() { }

		public PartsData(Thing thing, Pawn pawn = null, float severity = 0.5f)
		{
			//Log.Message("Creating PartsData for " + thing);
			Pawn = pawn;
			Thing = thing;
			Severity = severity;
			//Log.Message("This data is valid " + this.IsValid);
		}

		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref Severity, "severity", 0.5f, true);
			Scribe_References.Look<Pawn>(ref this.Pawn, "Pawn");
			Scribe_References.Look<Thing>(ref this.Thing, "Thing");
		}

		public bool IsValid { get { return Thing != null; } }
	}
}
