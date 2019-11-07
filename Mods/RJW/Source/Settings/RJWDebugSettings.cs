using System;
using UnityEngine;
using Verse;

namespace rjw
{
	public class RJWDebugSettings : ModSettings
	{
		public static void DoWindowContents(Rect inRect)
		{
			Listing_Standard listingStandard = new Listing_Standard();
			listingStandard.ColumnWidth = inRect.width / 2.05f;
			listingStandard.Begin(inRect);
				listingStandard.Gap(4f);
				listingStandard.CheckboxLabeled("submit_button_enabled".Translate(), ref RJWSettings.submit_button_enabled, "submit_button_enabled_desc".Translate());
				listingStandard.Gap(5f);
				listingStandard.CheckboxLabeled("RJW_designation_box".Translate(), ref RJWSettings.show_RJW_designation_box, "RJW_designation_box_desc".Translate());
				listingStandard.Gap(5f);
				listingStandard.CheckboxLabeled("StackRjwParts_name".Translate(), ref RJWSettings.StackRjwParts, "StackRjwParts_desc".Translate());
				listingStandard.Gap(5f);
				listingStandard.Label("maxDistancetowalk_name".Translate() + ": " + (RJWSettings.maxDistancetowalk), -1f, "maxDistancetowalk_desc".Translate());
				RJWSettings.maxDistancetowalk = listingStandard.Slider((int)RJWSettings.maxDistancetowalk, 0, 5000);
			//listingStandard.Gap(30f);

			listingStandard.NewColumn();
				listingStandard.Gap(4f);
				GUI.contentColor = Color.yellow;
				listingStandard.CheckboxLabeled("WildMode_name".Translate(), ref RJWSettings.WildMode, "WildMode_desc".Translate());
				listingStandard.Gap(5f);
				listingStandard.CheckboxLabeled("override_RJW_designation_checks_name".Translate(), ref RJWSettings.override_RJW_designation_checks, "override_RJW_designation_checks_desc".Translate());
				listingStandard.Gap(5f);
				listingStandard.CheckboxLabeled("GenderlessAsFuta_name".Translate(), ref RJWSettings.GenderlessAsFuta, "GenderlessAsFuta_desc".Translate());
				listingStandard.Gap(5f);
				listingStandard.CheckboxLabeled("DevMode_name".Translate(), ref RJWSettings.DevMode, "DevMode_desc".Translate());
				listingStandard.Gap(5f);
				listingStandard.CheckboxLabeled("DebugLogJoinInBed".Translate(), ref RJWSettings.DebugLogJoinInBed, "DebugLogJoinInBed_desc".Translate());
				listingStandard.Gap(5f);
				GUI.contentColor = Color.white;
				
			listingStandard.End();
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref RJWSettings.submit_button_enabled, "submit_button_enabled", true, true);
			Scribe_Values.Look(ref RJWSettings.show_RJW_designation_box, "show_RJW_designation_box", true, true);
			Scribe_Values.Look(ref RJWSettings.StackRjwParts, "StackRjwParts", false, true);
			Scribe_Values.Look(ref RJWSettings.maxDistancetowalk, "maxDistancetowalk", 250, true);

			Scribe_Values.Look(ref RJWSettings.GenderlessAsFuta, "GenderlessAsFuta", false, true);
			Scribe_Values.Look(ref RJWSettings.WildMode, "Wildmode", false, true);
			Scribe_Values.Look(ref RJWSettings.override_RJW_designation_checks, "override_RJW_designation_checks", false, true);
			Scribe_Values.Look(ref RJWSettings.DevMode, "DevMode", false, true);
			Scribe_Values.Look(ref RJWSettings.DebugLogJoinInBed, "DebugLogJoinInBed", false, true);
		}
	}
}
