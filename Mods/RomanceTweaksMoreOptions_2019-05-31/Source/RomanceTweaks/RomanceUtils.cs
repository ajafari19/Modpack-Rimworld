using RimWorld;
using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RomanceTweaks
{
    public static class RomanceUtils
    {
        public static bool IsSingle(Pawn pawn)
        {
            try
            {
                Pawn_RelationsTracker pawn_relations_tracker = pawn.relations;
                PawnRelationDef relationship_lover = PawnRelationDefOf.Lover;
                Predicate<Pawn> isNotDead = (Pawn p) => { return !p.Dead; };
                if (pawn_relations_tracker.GetFirstDirectRelationPawn(relationship_lover, isNotDead) == null)
                {
                    PawnRelationDef relationship_fiance = PawnRelationDefOf.Fiance;
                    if (pawn_relations_tracker.GetFirstDirectRelationPawn(relationship_fiance, isNotDead) == null)
                    {
                        PawnRelationDef relationship_spouse = PawnRelationDefOf.Spouse;
                        return pawn_relations_tracker.GetFirstDirectRelationPawn(relationship_spouse, isNotDead) == null;
                    }
                }
                return false;
            }
            catch(NullReferenceException e)
            {
                // I don't know man, wave a magic wand?
            }
            catch
            {
                // I don't know man, wave a magic wand?
            }
            finally
            {
                // I don't know man, wave a magic wand?
            }
            return false;
        }
    }
}
