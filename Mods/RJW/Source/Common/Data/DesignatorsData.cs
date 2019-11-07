using Verse;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using HugsLib.Utils;

namespace rjw
{
	/// <summary>
	/// Collection of Designated pawns lists
	/// </summary>
	public class DesignatorsData : UtilityWorldObject
	{
		public static List<Pawn> rjwHero = new List<Pawn>();
		public static List<Pawn> rjwComfort = new List<Pawn>();
		public static List<Pawn> rjwService = new List<Pawn>();
		public static List<Pawn> rjwMilking = new List<Pawn>();
		public static List<Pawn> rjwBreeding = new List<Pawn>();
		public static List<Pawn> rjwBreedingAnimal = new List<Pawn>();

		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				rjwHero = PawnsFinder.All_AliveOrDead.Where(p => p.IsDesignatedHero()).ToList();
				rjwComfort = PawnsFinder.All_AliveOrDead.Where(p => p.IsDesignatedComfort()).ToList();
				rjwService = PawnsFinder.All_AliveOrDead.Where(p => p.IsDesignatedService()).ToList();
				rjwMilking = PawnsFinder.All_AliveOrDead.Where(p => p.IsDesignatedMilking()).ToList();
				rjwBreeding = PawnsFinder.All_AliveOrDead.Where(p => p.IsDesignatedBreeding()).ToList();
				rjwBreedingAnimal = PawnsFinder.All_AliveOrDead.Where(p => p.IsDesignatedBreedingAnimal()).ToList();
			}
			Scribe_Collections.Look(ref rjwHero, "rjwHero", LookMode.Reference, new object[0]);
			Scribe_Collections.Look(ref rjwComfort, "rjwComfort", LookMode.Reference, new object[0]);
			Scribe_Collections.Look(ref rjwService, "rjwService", LookMode.Reference, new object[0]);
			Scribe_Collections.Look(ref rjwMilking, "rjwMilking", LookMode.Reference, new object[0]);
			Scribe_Collections.Look(ref rjwBreeding, "rjwBreeding", LookMode.Reference, new object[0]);
			Scribe_Collections.Look(ref rjwBreedingAnimal, "rjwBreedingAnimal", LookMode.Reference, new object[0]);
		}
	}
}
