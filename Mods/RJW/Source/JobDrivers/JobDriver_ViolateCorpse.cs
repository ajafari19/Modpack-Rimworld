using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.Sound;
using Multiplayer.API;

namespace rjw
{
	public class JobDriver_ViolateCorpse : JobDriver
	{
		private int duration;
		private int ticks_between_hearts;
		private int ticks_between_hits = 50;
		private int ticks_between_thrusts;

		protected TargetIndex icorpse = TargetIndex.A;
		protected Corpse Target => (Corpse)(job.GetTarget(icorpse));

		public static void sexTick(Pawn pawn, Thing Target)
		{
			if (!xxx.has_quirk(pawn, "Endytophile"))
			{
				xxx.DrawNude(pawn, true);
			}

			if (RJWSettings.sounds_enabled)
				SoundDef.Named("Sex").PlayOneShot(new TargetInfo(pawn.Position, pawn.Map));

			pawn.Drawer.Notify_MeleeAttackOn(Target);
			pawn.rotationTracker.FaceCell(Target.Position);
		}

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return pawn.Reserve(Target, job, 1, -1, null, errorOnFailed);
		}

		[SyncMethod]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			//--Log.Message("[RJW] JobDriver_ViolateCorpse::MakeNewToils() called");
			//Rand.PopState();
			//Rand.PushState(RJW_Multiplayer.PredictableSeed());
			duration = (int)(2000.0f * Rand.Range(0.50f, 0.90f));
			ticks_between_hearts = Rand.RangeInclusive(70, 130);
			ticks_between_hits = Rand.Range(xxx.config.min_ticks_between_hits, xxx.config.max_ticks_between_hits);
			ticks_between_thrusts = 100;

			if (xxx.is_bloodlust(pawn))
				ticks_between_hits = (int)(ticks_between_hits * 0.75);
			if (xxx.is_brawler(pawn))
				ticks_between_hits = (int)(ticks_between_hits * 0.90);

			this.FailOnDespawnedNullOrForbidden(icorpse);
			this.FailOn(() => !pawn.CanReserve(Target, 1, 0));  // Fail if someone else reserves the prisoner before the pawn arrives
			this.FailOn(() => pawn.IsFighting());
			this.FailOn(() => pawn.Drafted);
			this.FailOn(Target.IsBurning);

			//--Log.Message("[RJW] JobDriver_ViolateCorpse::MakeNewToils() - moving towards Target");
			yield return Toils_Goto.GotoThing(icorpse, PathEndMode.OnCell);

			var alert = RJWPreferenceSettings.rape_alert_sound == RJWPreferenceSettings.RapeAlert.Disabled ? 
				MessageTypeDefOf.SilentInput : MessageTypeDefOf.NeutralEvent;
			Messages.Message(pawn.Name + " is trying to rape a Target.", pawn, alert);

			var rape = new Toil();
			rape.initAction = delegate
			{
				//--Log.Message("[RJW] JobDriver_ViolateCorpse::MakeNewToils() - reserving Target");
				//pawn.Reserve(Target, 1, 0); // Target rapin seems like a solitary activity

				//--Log.Message("[RJW] JobDriver_ViolateCorpse::MakeNewToils() - stripping Target");
				Target.Strip();
			};
			rape.tickAction = delegate
			{
				if (pawn.IsHashIntervalTick(ticks_between_hearts))
					MoteMaker.ThrowMetaIcon(pawn.Position, pawn.Map, ThingDefOf.Mote_Heart);
				if (pawn.IsHashIntervalTick(ticks_between_thrusts))
					sexTick(pawn, Target);
				/*
				if (pawn.IsHashIntervalTick (ticks_between_hits))
					roll_to_hit (pawn, Corpse);
					*/
				xxx.reduce_rest(pawn, 2);
			};
			rape.AddFinishAction(delegate
			{
				if (xxx.is_human(pawn))
					pawn.Drawer.renderer.graphics.ResolveApparelGraphics();
			});
			rape.defaultCompleteMode = ToilCompleteMode.Delay;
			rape.defaultDuration = duration;
			yield return rape;

			yield return new Toil
			{
				initAction = delegate
				{
					//--Log.Message("[RJW] JobDriver_ViolateCorpse::MakeNewToils() - creating aftersex toil");
					//Addded by nizhuan-jjr: Try to apply an aftersex process for the pawn and the Target
					if (Target.InnerPawn != null)
					{
						SexUtility.ProcessSex(pawn, Target.InnerPawn, true);
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}
	}
}