using System;
using Verse;

namespace rjw
{
	/// <summary>
	/// Utility data object and a collection of extension methods for Pawn
	/// </summary>
	public class PawnData : IExposable
	{
		//Should probably mix but not shake it with RJW designation classes
		public Pawn Pawn = null;
		public bool Comfort = false;
		public bool Service = false;
		public bool Breeding = false;
		public bool Milking = false;
		public bool Hero = false;
		public bool Ironman = false;
		public string HeroOwner = "";
		public bool BreedingAnimal = false;
		public bool CanChangeDesignationColonist = false;
		public bool CanChangeDesignationPrisoner = false;
		public bool CanDesignateService = false;
		public bool CanDesignateMilking = false;
		public bool CanDesignateComfort = false;
		public bool CanDesignateBreedingAnimal = false;
		public bool CanDesignateBreeding = false;
		public bool CanDesignateHero = false;

		public PawnData() { }

		public PawnData(Pawn pawn)
		{
			//Log.Message("Creating pawndata for " + pawn);
			Pawn = pawn;
			//Log.Message("This data is valid " + this.IsValid);
		}

		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.Pawn, "Pawn");
			Scribe_Values.Look<bool>(ref Comfort, "Comfort", false, true);
			Scribe_Values.Look<bool>(ref Service, "Service", false, true);
			Scribe_Values.Look<bool>(ref Breeding, "Breeding", false, true);
			Scribe_Values.Look<bool>(ref Milking, "Milking", false, true);
			Scribe_Values.Look<bool>(ref Hero, "Hero", false, true);
			Scribe_Values.Look<bool>(ref Ironman, "Ironman", false, true);
			Scribe_Values.Look<String>(ref HeroOwner, "HeroOwner", "", true);
			Scribe_Values.Look<bool>(ref BreedingAnimal, "BreedingAnimal", false, true);
			Scribe_Values.Look<bool>(ref CanChangeDesignationColonist, "CanChangeDesignationColonist", false, true);
			Scribe_Values.Look<bool>(ref CanChangeDesignationPrisoner, "CanChangeDesignationPrisoner", false, true);
			Scribe_Values.Look<bool>(ref CanDesignateService, "CanDesignateService", false, true);
			Scribe_Values.Look<bool>(ref CanDesignateMilking, "CanDesignateMilking", false, true);
			Scribe_Values.Look<bool>(ref CanDesignateComfort, "CanDesignateComfort", false, true);
			Scribe_Values.Look<bool>(ref CanDesignateBreedingAnimal, "CanDesignateBreedingAnimal", false, true);
			Scribe_Values.Look<bool>(ref CanDesignateBreeding, "CanDesignateBreeding", false, true);
			Scribe_Values.Look<bool>(ref CanDesignateHero, "CanDesignateHero", false, true);
		}

		public bool IsValid { get { return Pawn != null; } }
	}
}
