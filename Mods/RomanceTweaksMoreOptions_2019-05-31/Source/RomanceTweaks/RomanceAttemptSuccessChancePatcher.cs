using Harmony;
using RimWorld;
using System;
using Verse;

namespace RomanceTweaks
{
    [HarmonyAfter(new string[]
    {
        "HugsLib.Psychology"
    }), HarmonyPatch(typeof(InteractionWorker_RomanceAttempt), "SuccessChance")]
    public class RomanceAttemptSuccessChancePatcher
    {
        public static float Postfix(float __result, Pawn initiator, Pawn recipient)
        {
            float num = RomanceTweakMod.RomanceSuccessModifier;
            if (RomanceTweakMod.DebugMode && __result != 0 && num != 1f)
            {
                if (initiator.Name == null || initiator.Name.ToStringShort == null ||
                    recipient.Name == null || recipient.Name.ToStringShort == null)
                {
                    Log.Message(string.Format("[RTMO] Romance Success [at least one name is null!] : {0} -> {1}", new object[]
                    {
                        __result,
                        __result * num
                    }), false);
                }
                else
                {
                    Log.Message(string.Format("[RTMO] Romance Success {0} -> {1} : {2} -> {3}", new object[]
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
