using HugsLib;
using HugsLib.Settings;
using System;
using Verse;

namespace RomanceTweaks
{
    public class RomanceTweakMod : ModBase
    {
        internal static SettingHandle<bool> DebugMode;

        internal static SettingHandle<float> RomanceChanceModifier;
        internal static SettingHandle<float> RomanceChanceModifierSingle;
        internal static SettingHandle<float> RomanceChanceModifierDifferentFaction;
        internal static SettingHandle<float> IncestModifier_Close;
        internal static SettingHandle<float> IncestModifier_Medium;
        internal static SettingHandle<float> IncestModifier_Far;

        internal static SettingHandle<float> RomanceSuccessModifier;

        internal static SettingHandle<float> BreakupChanceModifier;

        public override string ModIdentifier
        {
            get
            {
                return "RomanceTweaksMoreOptions";
            }
        }

        public override void DefsLoaded()
        {
            RomanceTweakMod.DebugMode = Settings.GetHandle<bool>("DebugMode", Translator.Translate("RomanceTweaks.DebugMode"), null, false, null, null);
            RomanceTweakMod.RomanceChanceModifier = Settings.GetHandle<float>("romanceChanceModifier", Translator.Translate("RomanceTweaks.RomanceChanceModifier"), Translator.Translate("RomanceTweaks.RomanceChanceModifierDesc"), 1f, null, null);
            RomanceTweakMod.RomanceChanceModifierSingle = Settings.GetHandle<float>("romanceChanceModifierSingle", Translator.Translate("RomanceTweaks.RomanceChanceModifierSingle"), Translator.Translate("RomanceTweaks.RomanceChanceModifierSingleDesc"), 1f, null, null);
            RomanceTweakMod.RomanceChanceModifierDifferentFaction = Settings.GetHandle<float>("romanceChanceModifierDifferentFaction", Translator.Translate("RomanceTweaks.RomanceChanceModifierDifferentFaction"), Translator.Translate("RomanceTweaks.RomanceChanceModifierDifferentFactionDesc"), 1f, null, null);
            RomanceTweakMod.IncestModifier_Close = Settings.GetHandle<float>("IncestModifier (Close)", Translator.Translate("RomanceTweaks.IncestModifier_Close"), Translator.Translate("RomanceTweaks.IncestModifier_Close_Desc"), 1f, null, null);
            RomanceTweakMod.IncestModifier_Medium = Settings.GetHandle<float>("IncestModifier (Medium)", Translator.Translate("RomanceTweaks.IncestModifier_Medium"), Translator.Translate("RomanceTweaks.IncestModifier_Medium_Desc"), 1f, null, null);
            RomanceTweakMod.IncestModifier_Far = Settings.GetHandle<float>("IncestModifier (Far)", Translator.Translate("RomanceTweaks.IncestModifier_Far"), Translator.Translate("RomanceTweaks.IncestModifier_Far_Desc"), 1f, null, null);
            RomanceTweakMod.RomanceSuccessModifier = Settings.GetHandle<float>("RomanceSuccessModifier", Translator.Translate("RomanceTweaks.RomanceSuccessModifier"), null, 1f, null, null);
            RomanceTweakMod.BreakupChanceModifier = Settings.GetHandle<float>("BreakupChanceModifier", Translator.Translate("RomanceTweaks.BreakupChanceModifier"), null, 1f, null, null);
        }
    }
}
