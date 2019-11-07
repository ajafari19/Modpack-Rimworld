using RimWorld;
using Verse;
using Multiplayer.API;

namespace rjw
{
	public class IncidentWorker_NymphVisitorGroup : IncidentWorker_NeutralGroup
	{

		private static readonly SimpleCurve PointsCurve = new SimpleCurve
		{
			new CurvePoint(45f, 0f),
			new CurvePoint(50f, 1f),
			new CurvePoint(100f, 1f),
			new CurvePoint(200f, 0.25f),
			new CurvePoint(300f, 0.1f),
			new CurvePoint(500f, 0f)
		};

		[SyncMethod]
		protected override void ResolveParmsPoints(IncidentParms parms)
		{
			if (!(parms.points >= 0f))
			{
				parms.points = Rand.ByCurve(PointsCurve);
			}
		}

		[SyncMethod]
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			//--Log.Message("IncidentWorker_NymphVisitorGroup::TryExecute() called");

			if (!RJWSettings.nymphos)
			{
				return false;
			}

			Map map = (Map)parms.target;

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

			Pawn pawn = nymph_generator.spawn_nymph(loc, ref map);

			Find.LetterStack.ReceiveLetter("Nymph wanders in", "A wandering nymph has decided to visit your colony.", LetterDefOf.NeutralEvent, pawn);
			return true;
		}
	}
}