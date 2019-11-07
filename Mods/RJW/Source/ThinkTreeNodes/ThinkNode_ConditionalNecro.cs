using Verse;
using Verse.AI;

namespace rjw
{
	/// <summary>
	/// Called to determine if the pawn can engage in necrophilia.
	/// </summary>
	public class ThinkNode_ConditionalNecro : ThinkNode_Conditional
	{
		protected override bool Satisfied(Pawn p)
		{
			//Log.Message("[RJW]ThinkNode_ConditionalNecro " + p);

			if (DebugSettings.alwaysDoLovin)
				return true;

			if (!RJWSettings.necrophilia_enabled)
				return false;

			// No necrophilia for animals. At least for now.
			// This would be easy to enable, if we actually want to add it.
			if (xxx.is_animal(p))
				return false;

			// No free will while designated for rape.
			if (p.IsDesignatedComfort() && !RJWSettings.WildMode)
				return false;

			return true;
		}
	}
}