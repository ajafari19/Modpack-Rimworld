using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;

namespace rjw
{
	class Hediff_HumanlikePregnancy : Hediff_BasePregnancy
	///<summary>
	///This hediff class simulates pregnancy resulting in humanlike childs.
	///</summary>	
	{
		//Handles the spawning of pawns and adding relations
		public override void GiveBirth()
		{
			Pawn mother = pawn;
			if (mother == null)
				return;
			try
			{
				//fail if hediff added through debug, since babies not initialized
				if (babies.Count > 9999)
					Log.Message("RJW humanlike pregnancy birthing pawn count: " + babies.Count);
			}
			catch
			{
				Initialize(mother, father);
			}
			List<Pawn> siblings = new List<Pawn>();
			foreach (Pawn baby in babies)
			{
				PawnUtility.TrySpawnHatchedOrBornPawn(baby, mother);

				var sex_need = mother.needs.TryGetNeed<Need_Sex>();
				if (mother.Faction != null && !(mother.Faction?.IsPlayer ?? false) && sex_need != null)
				{
					sex_need.CurLevel = 1.0f;
				}
				if (mother.Faction != null)
				{
					baby.SetFaction(mother.Faction);
				}
				if (mother.IsPrisonerOfColony)
				{
					baby.guest.CapturedBy(Faction.OfPlayer);
				}

				baby.relations.AddDirectRelation(PawnRelationDefOf.Parent, mother);
				if (father != null)
				{
					baby.relations.AddDirectRelation(PawnRelationDefOf.Parent, father);
				}

				foreach (Pawn sibling in siblings)
				{
					baby.relations.AddDirectRelation(PawnRelationDefOf.Sibling, sibling);
				}
				siblings.Add(baby);

				PostBirth(mother, father, baby);

				mother.health.RemoveHediff(this);
			}
		}
		
		///This method should be the only one to create the hediff
		public static void Create(Pawn mother, Pawn father)
		{
			if (mother == null)
				return;

			var torso = mother.RaceProps.body.AllParts.Find(x => x.def.defName == "Torso");
			//Log.Message("[RJW]Humanlike pregnancy " + mother + " is bred by " + father);

			var hediff = (Hediff_HumanlikePregnancy)HediffMaker.MakeHediff(HediffDef.Named("RJW_pregnancy"), mother, torso);
			hediff.Initialize(mother, father);
		}
	}
}
