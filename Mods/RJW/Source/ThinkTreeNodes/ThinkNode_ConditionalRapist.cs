using Verse;
using Verse.AI;

namespace rjw
{
	/// <summary>
	/// Rapist, chance to trigger random rape
	/// </summary>
	public class ThinkNode_ConditionalRapist : ThinkNode_Conditional
	{
		protected override bool Satisfied(Pawn p)
		{
			if (!RJWSettings.rape_enabled)
				return false;
			
			if (xxx.is_animal(p))
				return false;

			if (DebugSettings.alwaysDoLovin || RJWSettings.WildMode)
				return true;

			// No free will while designated for rape.
			if (p.IsDesignatedComfort())
				return false;

			if (!xxx.is_rapist(p))
				return false;

			if (!xxx.isSingleOrPartnerNotHere(p))
			{
				return false;
			}
			else
				return true;
		}
	}
}