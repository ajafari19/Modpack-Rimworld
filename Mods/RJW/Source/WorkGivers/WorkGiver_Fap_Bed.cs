using RimWorld;
using Verse;
using Verse.AI;
using System.Linq;

namespace rjw
{
	/// <summary>
	/// Assigns a pawn to fap in bed
	/// </summary>
	public class WorkGiver_Fap_bed : WorkGiver_Sexchecks
	{
		public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial);

		public override bool MoreChecks(Pawn pawn, Thing t, bool forced = false)
		{
			//Log.Message("[RJW]" + this.GetType().ToString() + " base checks: pass");
			Building_Bed target = t as Building_Bed;
			if (target == null)
			{
				if (RJWSettings.DevMode) JobFailReason.Is("not a bed");
				return false;
			}
			
			if (!target.AssignedPawns.Contains(pawn))
			{
				if (RJWSettings.DevMode) JobFailReason.Is("not pawn bed");
				return false;
			}
			/*
			if (!target.AssignedPawns.Contains(pawn))
				if (!pawn.CanReserve(target, target.SleepingSlotsCount, 0) && (!target.AnyUnownedSleepingSlot))
				{
					if (RJWSettings.DevMode) JobFailReason.Is("no space in bed");
					return false;
				}
			*/

			if (!pawn.IsDesignatedHero())
				if (!RJWSettings.WildMode)
				{
					if (!xxx.is_nympho(pawn))
						if ((xxx.need_some_sex(pawn) < 2f))
						{
							if (RJWSettings.DevMode) JobFailReason.Is("not horny enough");
							return false;
						}
					if (target.CurOccupants.Count() != 0)
					{
						if (target.CurOccupants.Count() == 1 && !target.CurOccupants.Contains(pawn))
						{
							if (RJWSettings.DevMode) JobFailReason.Is("bed not empty");
							return false;
						}
						if (target.CurOccupants.Count() > 1)
						{
							if (RJWSettings.DevMode) JobFailReason.Is("bed not empty");
							return false;
						}
					}

					//TODO: more exhibitionsts checks?
					bool canbeseen = false;
					foreach (Pawn bystander in pawn.Map.mapPawns.AllPawnsSpawned.Where(x => xxx.is_human(x) && x != pawn))
					{
						// dont see through walls, dont see whole map, only 15 cells around
						if (target.CanSee(bystander) && target.Position.DistanceToSquared(bystander.Position) < 15)
						{
							//if (!LovePartnerRelationUtility.LovePartnerRelationExists(pawn, bystander))
							canbeseen = true;
						}
					}
					if (!xxx.has_quirk(pawn, "Exhibitionist") && canbeseen)
					{
						if (RJWSettings.DevMode) JobFailReason.Is("can be seen");
						return false;
					}
					if (xxx.has_quirk(pawn, "Exhibitionist") && !canbeseen)
					{
						if (RJWSettings.DevMode) JobFailReason.Is("can not be seen");
						return false;
					}
				}


			//experimental change textures of bed to whore bed
			//Log.Message("[RJW] bed " + t.GetType().ToString() + " path " + t.Graphic.data.texPath);
			//t.Graphic.data.texPath = "Things/Building/Furniture/Bed/DoubleBedWhore";
			//t.Graphic.path = "Things/Building/Furniture/Bed/DoubleBedWhore";
			//t.DefaultGraphic.data.texPath = "Things/Building/Furniture/Bed/DoubleBedWhore";
			//t.DefaultGraphic.path = "Things/Building/Furniture/Bed/DoubleBedWhore";
			//Log.Message("[RJW] bed " + t.GetType().ToString() + " texPath " + t.Graphic.data.texPath);
			//Log.Message("[RJW] bed " + t.GetType().ToString() + " drawSize " + t.Graphic.data.drawSize);
			//t.Draw();
			//t.ExposeData();
			//Scribe_Values.Look(ref t.Graphic.data.texPath, t.Graphic.data.texPath, "Things/Building/Furniture/Bed/DoubleBedWhore", true);

			//Log.Message("[RJW]" + this.GetType().ToString() + " extended checks: can start sex");
			return true;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(xxx.fappin, t);
		}
	}
}