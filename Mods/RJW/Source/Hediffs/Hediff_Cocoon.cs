using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace rjw
{
	public class Cocoon : HediffWithComps
	{
		public int tickNext;

		public override void PostMake()
		{
			Severity = 1.0f;
			SetNextTick();
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref tickNext, "tickNext", 1000, true);
		}

		public override void Tick()
		{
			if (Find.TickManager.TicksGame >= tickNext)
			{
				//Log.Message("Cocoon::Tick() " + base.xxx.get_pawnname(pawn));
				TryHealWounds();
				TryFeed();
				SetNextTick();
			}
		}

		public void TryHealWounds()
		{
			IEnumerable<Hediff> enumerable = from hd in pawn.health.hediffSet.hediffs
											 where !hd.IsTended()
											 select hd;
			if (enumerable != null)
			{
				foreach (Hediff item in enumerable)
				{
					HediffWithComps val = item as HediffWithComps;
					if (val != null && val.TendableNow())
						if (val.Bleeding)
						{
							//Log.Message("TrySealWounds " + xxx.get_pawnname(pawn) + ", Bleeding " + item.Label);
							HediffComp_TendDuration val2 = HediffUtility.TryGetComp<HediffComp_TendDuration>(val);
							val2.tendQuality = 2f;
							val2.tendTicksLeft = Find.TickManager.TicksGame;
							pawn.health.Notify_HediffChanged(item);
						}
						// infections  etc
						else// if (val.def.lethalSeverity > 0f)
						{
							//Log.Message("TryHeal " + xxx.get_pawnname(pawn) + ", infection(?) " + item.Label);
							HediffComp_TendDuration val2 = HediffUtility.TryGetComp<HediffComp_TendDuration>(val);
							val2.tendQuality = 2f;
							val2.tendTicksLeft = Find.TickManager.TicksGame;
							pawn.health.Notify_HediffChanged(item);
						}
				}
			}
		}

		public void TryFeed()
		{
			Need_Food need = pawn.needs.TryGetNeed<Need_Food>();
			if (need == null)
			{
				return;
			}

			if (need.CurLevel < 0.10f)
			{
				//Log.Message("Cocoon::TryFeed() " + xxx.get_pawnname(pawn) + " need to be fed");
				float nutrition_amount = need.MaxLevel / 5f;
				pawn.needs.food.CurLevel += nutrition_amount;
			}
		}

		public void SetNextTick()
		{
			//make actual tick every 16.6 sec
			tickNext = Find.TickManager.TicksGame + 1000;
			//Log.Message("Cocoon::SetNextTick() " + tickNext);
		}
	}
}
