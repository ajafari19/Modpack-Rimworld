using RimWorld;
using Verse;

namespace rjw
{
	public class ThoughtWorker_FeelingBroken : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			var brokenstages = p.health.hediffSet.GetFirstHediffOfDef(xxx.feelingBroken);
			if (brokenstages != null && brokenstages.CurStageIndex != 0)
			{
				return ThoughtState.ActiveAtStage(brokenstages.CurStageIndex - 1);
			}
			return ThoughtState.Inactive;
		}
	}
}