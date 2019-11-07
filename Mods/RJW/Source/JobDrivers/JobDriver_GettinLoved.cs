using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace rjw
{
	public class JobDriver_GettinLoved : JobDriver
	{
		private TargetIndex ipartner = TargetIndex.A;
		private TargetIndex ibed = TargetIndex.B;

		private int tick_interval = 100;

		protected Pawn Partner => (Pawn)(job.GetTarget(ipartner));
		protected Building_Bed Bed => (Building_Bed)(job.GetTarget(ibed));

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
			//return this.pawn.Reserve(this.Partner, this.job, 1, 0, null) && this.pawn.Reserve(this.Bed, this.job, 1, 0, null);
		}

		public float CalculateSatisfactionPerTick()
		{
			return 1.0f;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			//--Log.Message("[RJW]JobDriver_GettinLoved::MakeNewToils is called");

			float partner_ability = xxx.get_sex_ability(Partner);

			// More/less hearts based on partner ability.
			if (partner_ability < 0.8f)
				tick_interval += 100;
			else if (partner_ability > 2.0f)
				tick_interval -= 25;

			// More/less hearts based on opinion.
			if (pawn.relations.OpinionOf(Partner) < 0)
				tick_interval += 50;
			else if (pawn.relations.OpinionOf(Partner) > 60)
				tick_interval -= 25;

			if (Partner.CurJob.def == xxx.casual_sex)
			{
				this.FailOnDespawnedOrNull(ipartner);
				this.FailOn(() => !Partner.health.capacities.CanBeAwake);
				this.FailOn(() => pawn.Drafted);
				this.KeepLyingDown(ibed);
				yield return Toils_Reserve.Reserve(ipartner, 1, 0);
				yield return Toils_Reserve.Reserve(ibed, Bed.SleepingSlotsCount, 0);
				Toil get_loved = Toils_LayDown.LayDown(ibed, true, false, false, false);
				get_loved.FailOn(() => Partner.CurJob == null || Partner.CurJob.def != xxx.casual_sex);
				get_loved.defaultCompleteMode = ToilCompleteMode.Never;
				get_loved.AddPreTickAction(delegate
				{
					if (pawn.IsHashIntervalTick(tick_interval))
					{
						MoteMaker.ThrowMetaIcon(pawn.Position, pawn.Map, ThingDefOf.Mote_Heart);
						xxx.sexTick(pawn, Partner);
					}
				});
				get_loved.socialMode = RandomSocialMode.Off;
				yield return get_loved;
			}
			else if (Partner.CurJob.def == xxx.whore_is_serving_visitors)
			{
				this.FailOnDespawnedOrNull(ipartner);
				this.FailOn(() => !Partner.health.capacities.CanBeAwake || Partner.CurJob == null);
				yield return Toils_Goto.GotoThing(ipartner, PathEndMode.OnCell);
				yield return Toils_Reserve.Reserve(ipartner, 1, 0);
				Toil get_loved = new Toil();
				get_loved.FailOn(() => (Partner.CurJob.def != xxx.whore_is_serving_visitors));
				get_loved.defaultCompleteMode = ToilCompleteMode.Never;
				get_loved.initAction = delegate
				{
					//--Log.Message("[RJW]JobDriver_GettinLoved::MakeNewToils - whore section is called");
				};
				get_loved.AddPreTickAction(delegate
				{
					if (pawn.IsHashIntervalTick(tick_interval))
					{
						MoteMaker.ThrowMetaIcon(pawn.Position, pawn.Map, ThingDefOf.Mote_Heart);
						xxx.sexTick(pawn, Partner);
					}
				});
				get_loved.socialMode = RandomSocialMode.Off;
				yield return get_loved;
			}
			else if (Partner.CurJob.def == xxx.bestialityForFemale)
			{
				this.FailOnDespawnedOrNull(ipartner);
				this.FailOn(() => !Partner.health.capacities.CanBeAwake || Partner.CurJob == null);
				yield return Toils_Goto.GotoThing(ipartner, PathEndMode.OnCell);
				yield return Toils_Reserve.Reserve(ipartner, 1, 0);
				Toil get_loved = new Toil();
				get_loved.FailOn(() => (Partner.CurJob.def != xxx.bestialityForFemale));
				get_loved.defaultCompleteMode = ToilCompleteMode.Never;
				get_loved.initAction = delegate
				{
					//--Log.Message("[RJW]JobDriver_GettinLoved::MakeNewToils - bestialityForFemale section is called");
				};
				get_loved.AddPreTickAction(delegate
				{
					if (pawn.IsHashIntervalTick(tick_interval))
					{
						MoteMaker.ThrowMetaIcon(pawn.Position, pawn.Map, ThingDefOf.Mote_Heart);
						xxx.sexTick(pawn, Partner);
					}
				});
				get_loved.socialMode = RandomSocialMode.Off;
				yield return get_loved;
			}
		}
	}
}