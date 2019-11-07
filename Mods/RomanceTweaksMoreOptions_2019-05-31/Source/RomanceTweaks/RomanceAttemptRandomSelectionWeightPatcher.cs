using Harmony;
using RimWorld;
using System;
using Verse;

namespace RomanceTweaks
{
    [HarmonyAfter(new string[]
    {
        "HugsLib.Psychology"
    }), HarmonyPatch(typeof(InteractionWorker_RomanceAttempt), "RandomSelectionWeight")]
    public class RomanceAttemptRandomSelectionWeightPatcher
    {
        public static float Postfix(float __result, Pawn initiator, Pawn recipient)
        {
            float num = RomanceTweakMod.RomanceChanceModifier;
            if(initiator==null || recipient == null)
            {
                if (RomanceTweakMod.DebugMode)
                {
                    Log.Message(string.Format("[RTMO] At least one pawn is null (your game broken?), only applying general factor"), false);
                }
                return __result * num;
            }
            if (RomanceUtils.IsSingle(initiator) && RomanceUtils.IsSingle(recipient))
            {
                num *= RomanceTweakMod.RomanceChanceModifierSingle;
            }

            if (RomanceTweakMod.RomanceChanceModifierDifferentFaction != 1f)
            {
                if (initiator.Faction == null || initiator.Faction.def == null ||
                    recipient.Faction == null || recipient.Faction.def == null)
                {
                    if (RomanceTweakMod.DebugMode)
                    {
                        Log.Message(string.Format("[RTMO] At least one pawn's faction is null (your game broken?), not applying faction modifier"), false);
                    }
                }
                else if (initiator.Faction.def != recipient.Faction.def)
                {
                    num *= RomanceTweakMod.RomanceChanceModifierDifferentFaction;
                }
            }
            float inc_num = 1f;
            foreach (PawnRelationDef def in initiator.GetRelations(recipient))
            {
                if (def == PawnRelationDefOf.Child ||
                    def == PawnRelationDefOf.HalfSibling ||
                    def == PawnRelationDefOf.Parent ||
                    def == PawnRelationDefOf.Sibling)
                {
                    // close relation, default attraction 0.03
                    inc_num = RomanceTweakMod.IncestModifier_Close;
                }
                else if(def == PawnRelationDefOf.Cousin ||
                    def == PawnRelationDefOf.Grandchild ||
                    def == PawnRelationDefOf.Grandparent ||
                    def == PawnRelationDefOf.NephewOrNiece ||
                    def == PawnRelationDefOf.UncleOrAunt)
                {
                    // medium relation, default attraction 0.25
                    inc_num = RomanceTweakMod.IncestModifier_Medium;
                }
                else if(def == PawnRelationDefOf.GreatGrandchild ||
                    def == PawnRelationDefOf.GreatGrandparent ||
                    def == PawnRelationDefOf.GranduncleOrGrandaunt ||
                    def == PawnRelationDefOf.Kin)
                    // missing (no fields in PawnRelationDefOf:
                    //  - GrandnephewOrGrandniece
                    //  - CousinOnceRemoved
                    //  - SecondCousin
                {
                    // far relation, default attraction 0.5
                    inc_num = RomanceTweakMod.IncestModifier_Far;
                }
            }
            num *= inc_num;
            if (RomanceTweakMod.DebugMode && __result != 0 && num != 1f)
            {
                if (initiator.Name == null || initiator.Name.ToStringShort == null ||
                    recipient.Name == null || recipient.Name.ToStringShort == null)
                {
                    Log.Message(string.Format("[RTMO] Romance Chance [at least one name is null!] : {0} -> {1}", new object[]
                    {
                        __result,
                        __result * num
                    }), false);
                }
                else
                {
                    Log.Message(string.Format("[RTMO] Romance Chance {0} -> {1} : {2} -> {3}", new object[]
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
