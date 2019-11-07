using Harmony;
using RimWorld;
using System;
using Verse;

namespace RomanceTweaks
{
    [HarmonyPatch(typeof(InteractionWorker_Breakup), "RandomSelectionWeight")]
    public class BreakupRandomSelectionWeightPatcher
    {
        public static float Postfix(float __result, Pawn initiator, Pawn recipient)
        {
            float num = RomanceTweakMod.BreakupChanceModifier;
            if (RomanceTweakMod.DebugMode && __result != 0 && num != 1f)
            {
                if (initiator.Name == null || initiator.Name.ToStringShort == null ||
                    recipient.Name == null || recipient.Name.ToStringShort == null)
                {
                    Log.Message(string.Format("[RTMO] Breakup Chance [at least one name is null!] : {0} -> {1}", new object[]
                    {
                        __result,
                        __result * num
                    }), false);
                }
                else
                {
                    Log.Message(string.Format("[RTMO] Breakup Chance {0} -> {1} : {2} -> {3}", new object[]
                    {
                        initiator.Name.ToStringShort,
                        recipient.Name.ToStringShort,
                        __result,
                        __result * num
                    }), false);
                }
            }
            return __result * num;
        }
    }
}
