using RimWorld;
using Verse;
using Verse.AI;
using System.Linq;

namespace rjw
{
	/// <summary>
	/// Assigns a pawn to fap
	/// </summary>
	public class WorkGiver_Fap : WorkGiver_Sexchecks
	{
		public override bool MoreChecks(Pawn pawn, Thing t, bool forced = false)
		{
			//Log.Message("[RJW]" + this.GetType().ToString() + " base checks: pass");
			Pawn target = t as Pawn;
			if (target != pawn)
			{
				return false;
			}

			if (!pawn.CanReserve(target, xxx.max_rapists_per_prisoner, 0))
				return false;

			if (!pawn.IsDesignatedHero())
				if (!RJWSettings.WildMode)
				{
					if (!xxx.is_nympho(pawn))
						if ((xxx.need_some_sex(pawn) < 2f))
						{
							if (RJWSettings.DevMode) JobFailReason.Is("not horny enough");
							return false;
						}

					//TODO: more exhibitionsts checks?
					bool canbeseen = false;
					foreach (Pawn bystander in pawn.Map.mapPawns.AllPawnsSpawned.Where(x => xxx.is_human(x) && x != pawn))
					{
						// dont see through walls, dont see whole map, only 15 cells around
						if (pawn.CanSee(bystander) && pawn.Position.DistanceToSquared(bystander.Position) < 15)
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

			//Log.Message("[RJW]" + this.GetType().ToString() + " extended checks: can start sex");
			return true;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(xxx.quickfap, t.Position);
		}
	}
}