using System.Linq;
using RimWorld;
using Verse;
using Multiplayer.API;

namespace rjw
{
	public class IncidentWorker_NymphJoins : IncidentWorker
	{
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!RJWSettings.nymphos) return false;
			Map map = (Map)parms.target;
			float colonist_count = map.mapPawns.FreeColonistsCount;
			float nymph_count = map.mapPawns.FreeColonists.Count(xxx.is_nympho);

			float nymph_fraction = nymph_count / colonist_count;
			return colonist_count >= 1 && (nymph_fraction < xxx.config.max_nymph_fraction);
		}

		[SyncMethod]
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			//--Log.Message("IncidentWorker_NymphJoins::TryExecute() called");

			if (!RJWSettings.nymphos) return false;
			Map map = (Map) parms.target;

			if (map == null)
			{
				//--Log.Message("IncidentWorker_NymphJoins::TryExecute() - map is null, abort!");
				return false;
			}
			else
			{
				//--Log.Message("IncidentWorker_NymphJoins::TryExecute() - map is ok");
			}

			//Rand.PopState();
			//Rand.PushState(RJW_Multiplayer.PredictableSeed());
			if (!RCellFinder.TryFindRandomPawnEntryCell(out IntVec3 loc, map, CellFinder.EdgeRoadChance_Friendly + 0.2f))
			{
				//--Log.Message("IncidentWorker_NymphJoins::TryExecute() - no entry, abort!");
				return false;
			}

			Pawn pawn = nymph_generator.spawn_nymph(loc, ref map, Faction.OfPlayer);
			Find.LetterStack.ReceiveLetter("Nymph Joins", "A wandering nymph has decided to join your colony.",
				LetterDefOf.PositiveEvent, pawn);

			return true;
		}
	}
}