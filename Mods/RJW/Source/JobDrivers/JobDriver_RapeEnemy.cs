using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;
using Multiplayer.API;

namespace rjw
{
	internal class JobDef_RapeEnemy : JobDef
	{
		public List<string> TargetDefNames = new List<string>();
		public int priority = 0;

		protected JobDriver_RapeEnemy instance
		{
			get
			{
				if (_tmpInstance == null)
				{
					_tmpInstance = (JobDriver_RapeEnemy)Activator.CreateInstance(driverClass);
				}
				return _tmpInstance;
			}
		}

		private JobDriver_RapeEnemy _tmpInstance;

		public virtual bool CanUseThisJobForPawn(Pawn rapist)
		{
			if (rapist.CurJob != null && rapist.CurJob.def != JobDefOf.LayDown)
				return false;

			return instance.CanUseThisJobForPawn(rapist);// || TargetDefNames.Contains(rapist.def.defName);
		}

		public virtual Pawn FindVictim(Pawn rapist, Map m)
		{
			return instance.FindVictim(rapist, m);
		}
	}



	public class JobDriver_RapeEnemy : JobDriver_Rape
	{
		private static readonly HediffDef is_submitting = HediffDef.Named("Hediff_Submitting");//used in find_victim

		//override can_rape mechanics
		protected bool requierCanRape = true;

		public virtual bool CanUseThisJobForPawn(Pawn rapist)
		{
			return xxx.is_human(rapist);
		}

		// this is probably useseless, maybe there be something in future
		public virtual bool considerStillAliveEnemies => true;

		[SyncMethod]
		public virtual Pawn FindVictim(Pawn rapist, Map m)
		{
			//Log.Message("[RJW]" + this.GetType().ToString() + "::TryGiveJob( " + xxx.get_pawnname(rapist) + " ) map " + m?.ToString());
			if (rapist == null || m == null) return null;
			//Log.Message("[RJW]" + this.GetType().ToString() + "::TryGiveJob( " + xxx.get_pawnname(rapist) + " ) can rape " + xxx.can_rape(rapist));
			if (!xxx.can_rape(rapist) && requierCanRape) return null;
			Pawn best_rapee = null;
			List<Pawn> filteredtargets = new List<Pawn>();
			float best_fuckability = 0.20f; // Don't rape pawns with <20% fuckability

			IEnumerable<Pawn> targets =
				m.mapPawns.AllPawnsSpawned.Where(x => !x.IsForbidden(rapist) && x != rapist && x.HostileTo(rapist));

			//Log.Message("[RJW]" + this.GetType().ToString() + "::TryGiveJob( " + xxx.get_pawnname(rapist) + " ) found " + targets.Count() + "targets");
			foreach (Pawn target in targets)
			{
				if (!xxx.can_path_to_target(rapist, target.Position))
					continue;// too far

				//Log.Message("[RJW]" + this.GetType().ToString() + "::TryGiveJob( " + xxx.get_pawnname(rapist) + " ) target: " + xxx.get_pawnname(target) );

				if (considerStillAliveEnemies)
				{
					if (rapist.CanSee(target) && !target.Downed)
					{
						//Log.Message("[RJW]"+this.GetType().ToString()+"::TryGiveJob( " + xxx.get_pawnname(rapist) +" ) enemies: "+ xxx.get_pawnname(target)+ " still alive" );
						return null; //Enemies still up. Kill them first.
					}
				}

				if (!RJWSettings.bestiality_enabled && xxx.is_animal(target) && !(xxx.is_animal(rapist) && RJWSettings.animal_on_animal_enabled)) continue; //zoo disabled, skip.
				if (target.CurJob.def == xxx.gettin_raped || target.CurJob.def == xxx.gettin_loved) continue; //already having sex with someone, skip, give chance to other victims.

				//Log.Message("[RJW]"+this.GetType().ToString()+"::TryGiveJob( " + xxx.get_pawnname(rapist) + " -> " + xxx.get_pawnname(target) + " ) - checking\nCanReserve:"+ rapist.CanReserve(target, xxx.max_rapists_per_prisoner, 0) + "\nCanReach:" + rapist.CanReach(target, PathEndMode.OnCell, Danger.None)+ "\nCan_rape_Easily:" + Can_rape_Easily(target));
				if (rapist.CanReserveAndReach(target, PathEndMode.OnCell, Danger.Some, xxx.max_rapists_per_prisoner, 0) && Can_rape_Easily(target) )
				{
					if (xxx.is_human(target) || xxx.is_animal(target))
					{
						float fuc = GetFuckability(rapist, target);
						//Log.Message("[RJW]"+this.GetType().ToString()+ "::FindVictim( " + xxx.get_pawnname(rapist) + " -> " + xxx.get_pawnname(target) + " ) - fuckability:" + fuc + " ");
						if (fuc > best_fuckability)
						{
							if (xxx.is_animal(rapist))
							{
								filteredtargets.Add(target);
								continue;
							}
							best_rapee = target;
							best_fuckability = fuc;
						}
						//else { Log.Message("[RJW]"+this.GetType().ToString()+"::TryGiveJob( " + xxx.get_pawnname(rapist) + " -> " + xxx.get_pawnname(target) + " ) - is not good for me "+ "( " + fuc + " )"); }
					}
				}
				//else { Log.Message("[RJW]"+this.GetType().ToString()+"::TryGiveJob( " + xxx.get_pawnname(rapist) + " -> " + xxx.get_pawnname(target) + " ) - is not good"); }
			}
			//Log.Message("[RJW]"+this.GetType().ToString()+"::TryGiveJob( " + xxx.get_pawnname(rapist) + " -> " + xxx.get_pawnname(best_rapee) + " ) - fuckability:" + best_fuckability + " ");
			return filteredtargets.Any() ? filteredtargets.RandomElement() : best_rapee;
		}

		public virtual float GetFuckability(Pawn rapist, Pawn target)
		{
			//Log.Message("[RJW]JobDriver_RapeEnemy::GetFuckability(" + rapist.ToString() + "," + target.ToString() + ")");
			if (target.health.hediffSet.HasHediff(is_submitting))//it's not about attractiveness anymore, it's about showing who's whos bitch
			{
				return 2 * xxx.would_fuck(rapist, target, invert_opinion: true, ignore_bleeding: true, ignore_gender: true);
			}
			return !xxx.would_rape(rapist, target) ? 0f
				: xxx.would_fuck(rapist, target, invert_opinion: true, ignore_bleeding: true, ignore_gender: true);
		}

		protected bool Can_rape_Easily(Pawn pawn)
		{
			return xxx.can_get_raped(pawn) && !pawn.IsBurning();
		}
	}
}