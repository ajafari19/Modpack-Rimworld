using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI.Group;
using System.Linq;
using UnityEngine;


namespace rjw
{
	///<summary>
	///This hediff class simulates pregnancy with mechanoids, mother may be human. It is not intended to be reasonable.
	///Differences from bestial pregnancy are that ... it is lethal
	///TODO: extend with something "friendlier"? than Mech_Scyther.... two Mech_Scyther's? muhahaha
	///</summary>	
	class Hediff_MechanoidPregnancy : Hediff_BasePregnancy
	{
		public override void PregnancyMessage()
		{
			string message_title = "RJW_PregnantTitle".Translate(pawn.LabelIndefinite());
			string message_text1 = "RJW_PregnantText".Translate(pawn.LabelIndefinite());
			string message_text2 = "RJW_PregnantMechStrange".Translate();
			Find.LetterStack.ReceiveLetter(message_title, message_text1 + "\n" + message_text2, LetterDefOf.ThreatBig, pawn);
		}

		public void Hack()
		{
			is_hacked = true;
		}

		public override void Notify_PawnDied()
		{
			base.Notify_PawnDied();
			GiveBirth();
		}

		//Handles the spawning of pawns
		public override void GiveBirth()
		{
			Pawn mother = pawn;
			if (mother == null)
				return;
			try
			{
				//fail if hediff added through debug, since babies not initialized
				if (babies.Count > 9999)
					Log.Message("RJW mech pregnancy birthing pawn count: " + babies.Count);
			}
			catch
			{
				Initialize(mother, father);
			}
			foreach (Pawn baby in babies)
			{
				Faction spawn_faction = null;
				if (!is_hacked)
					spawn_faction = Faction.OfMechanoids;

				Pawn baby1 = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDef.Named("Mech_Scyther"), spawn_faction));
				PawnUtility.TrySpawnHatchedOrBornPawn(baby1, mother);
				if (!is_hacked)
				{
					LordJob_MechanoidsDefendShip lordJob = new LordJob_MechanoidsDefendShip(mother, baby1.Faction, 50f, mother.Position);
					Lord lord = LordMaker.MakeNewLord(baby1.Faction, lordJob, baby1.Map);
					lord.AddPawn(baby1);
				}
				FilthMaker.MakeFilth(baby1.PositionHeld, baby1.MapHeld, mother.RaceProps.BloodDef, mother.LabelIndefinite());
			}

			IEnumerable<BodyPartRecord> source = from x in mother.health.hediffSet.GetNotMissingParts()
												 where x.IsInGroup(BodyPartGroupDefOf.Torso)
												 && !x.IsCorePart
												 //someday include depth filter
												 //so it doesnt cut out external organs (breasts)?
												 //vag  is genital part and genital is external
												 //anal is internal
												 //make sep part of vag?
												 //&& x.depth == BodyPartDepth.Inside
												 select x;
			if (source.Any())
			{
				foreach (BodyPartRecord part in source)
				{
					Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, mother, part);
					hediff_MissingPart.lastInjury = HediffDefOf.Cut;
					hediff_MissingPart.IsFresh = true;
					mother.health.AddHediff(hediff_MissingPart);

					//idk blood doesnt drop
					//mother.health.DropBloodFilth();
					//FilthMaker.MakeFilth(corpse.PositionHeld, corpse.MapHeld, mother.RaceProps.BloodDef, mother.LabelIndefinite());
				}
			}
			mother.health.RemoveHediff(this);
		}

		///This method should be the only one to create the hediff
		//Hediffs are made HediffMaker.MakeHediff which returns hediff of class different than the one needed, so we do the cast and then do all the same operations as in parent class
		//I don't know whether it'd be possible to use standard constructor instead of this retardation
		public static void Create(Pawn mother, Pawn father)
		{
			if (mother == null)
				return;

			BodyPartRecord torso = mother.RaceProps.body.AllParts.Find(x => x.def.defName == "Torso");
			//Log.Message("RJW beastial "+ mother + " " + father);

			Hediff_MechanoidPregnancy hediff = (Hediff_MechanoidPregnancy)HediffMaker.MakeHediff(HediffDef.Named("RJW_pregnancy_mech"), mother, torso);
			hediff.Initialize(mother, father);
		}
	}
}
