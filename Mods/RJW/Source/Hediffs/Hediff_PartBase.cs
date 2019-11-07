using Verse;
using RimWorld;
using System.Text;

namespace rjw
{
	public class HediffDef_PartBase : Hediff_Implant
	{
		public override void ExposeData()
		{
			base.ExposeData();
		}


		public override string LabelBase
		{
			get
			{
				/*
				 * make patch to make/save capmods?
				if (CapMods.Count < 5)
				{
					PawnCapacityModifier pawnCapacityModifier = new PawnCapacityModifier();
					pawnCapacityModifier.capacity = PawnCapacityDefOf.Moving;
					pawnCapacityModifier.offset += 0.5f;
					CapMods.Add(pawnCapacityModifier);
				}
				*/

				//name/kind
				return this.def.label;
			}
		}

		public override string LabelInBrackets
		{
			get
			{
				/* penis
				string size = "Average";
				if (Severity < 0.1f)
					size = "Micro";
				if (Severity < 0.25f)
					size = "Small";
				if (Severity > 0.75f)
					size = "Big";
				if (Severity > 0.9f)
					size = "Huge";

				return size;
				*/
				return (this.CurStage != null && !this.CurStage.label.NullOrEmpty()) ? this.CurStage.label : null;
			}
		}

		//stack hediff in health tab
		public override int UIGroupKey
		{
			get
			{
				if (RJWSettings.StackRjwParts)
					//(Label x count)
					return this.Label.GetHashCode();
				else
					//dont
					return loadID;
			}
		}

		public override string TipStringExtra
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (StatDrawEntry current in HediffStatsUtility.SpecialDisplayStats(this.CurStage, this))
				{
					if (current.ShouldDisplay)
					{
						stringBuilder.AppendLine(current.LabelCap + ": " + current.ValueString);
					}
				}
				//stringBuilder.AppendLine("1");// size?
				//stringBuilder.AppendLine("2");// erm something?
				return stringBuilder.ToString();
			}
		}

		//do not merge same rjw parts into one
		public override bool TryMergeWith(Hediff other)
		{
			return false;
		}

		public override bool Visible
		{
			get
			{
				//TODO:
				//show parts
				//show discovered parts(naked, etc)
				//dont show parts
				return xxx.config.show_regular_dick_and_vag;
			}
		}
	}
}