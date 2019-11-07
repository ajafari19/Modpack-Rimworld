using System.Linq;
using RimWorld;
using Verse;

namespace rjw
{
	internal class JobDriver_RapeEnemyByInsect : JobDriver_RapeEnemy
	{
		//override mechanics

		//public JobDriver_RapeEnemyByInsect()
		//{
		//	this.requierCanRape = false;
		//}

		public override bool CanUseThisJobForPawn(Pawn rapist)
		{
			if (rapist.CurJob != null && (rapist.CurJob.def != JobDefOf.LayDown || rapist.CurJob.def != JobDefOf.Wait_Wander || rapist.CurJob.def != JobDefOf.GotoWander))
				return false;

			//someday add check for insect-insect breeding
			return xxx.is_insect(rapist) && RJWSettings.bestiality_enabled;
		}

		public override float GetFuckability(Pawn rapist, Pawn target)
		{
			//Plant Eggs to everyone.
			if (rapist.gender == Gender.Female)
			{
				return 1f; 
			}
			//Feritlize eggs to everyone with planted eggs.
			else
			{
				//if ((from x in target.health.hediffSet.GetHediffs<Hediff_InsectEgg>() where (x.IsParent(rapist.def.defName) && !x.fertilized) select x).Count() > 0)
				if ((from x in target.health.hediffSet.GetHediffs<Hediff_InsectEgg>() where x.IsParent(rapist) select x).Count() > 0)
				{
					return 1f;
				}
			}
			return 0f;
		}
	}
}