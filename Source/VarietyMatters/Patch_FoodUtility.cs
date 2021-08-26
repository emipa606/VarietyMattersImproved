using HarmonyLib;
using RimWorld;
using Verse;

namespace VarietyMatters
{
    // Token: 0x0200000A RID: 10
    [HarmonyPatch]
    public static class Patch_FoodUtility
    {
        // Token: 0x0600001F RID: 31 RVA: 0x0000321C File Offset: 0x0000141C
        [HarmonyPatch(typeof(FoodUtility), "FoodOptimality")]
        [HarmonyPostfix]
        private static void Postfix_FoodOptimality(ref float __result, Pawn eater, Thing foodSource)
        {
            if (VarietyRecord.GetVarietyRecord(eater) == null ||
                VarietyRecord.GetVarietyRecord(eater).recentlyConsumed == null ||
                foodSource.TryGetComp<CompVariety>() == null ||
                eater.needs.TryGetNeed<Need_FoodVariety>() == null ||
                eater.needs.TryGetNeed<Need_FoodVariety>().Disabled || eater.Starving() ||
                eater.health.hediffSet.HasHediff(HediffDefOf.FoodPoisoning, true) ||
                ModSettings_VarietyMatters.sickPawns && HealthAIUtility.ShouldSeekMedicalRest(eater) ||
                eater.IsPrisoner)
            {
                return;
            }

            var recentlyConsumed = VarietyRecord.GetVarietyRecord(eater).recentlyConsumed;
            var compIngredients = foodSource.TryGetComp<CompIngredients>();
            var ignoreIngredients = ModSettings_VarietyMatters.ignoreIngredients;
            if (ignoreIngredients)
            {
                if (!recentlyConsumed.Contains(foodSource.def.label) ||
                    foodSource.def.ingestible.preferability == FoodPreferability.RawTasty &&
                    !recentlyConsumed.Contains("Raw" + foodSource.def.label))
                {
                    __result += 4 * recentlyConsumed.Count;
                    return;
                }
            }

            if (compIngredients == null || ModSettings_VarietyMatters.maxVariety)
            {
                if (!recentlyConsumed.Contains(foodSource.def.label) ||
                    foodSource.def.ingestible.preferability == FoodPreferability.RawTasty &&
                    !recentlyConsumed.Contains("Raw" + foodSource.def.label))
                {
                    __result += recentlyConsumed.Count;
                }
            }

            if (compIngredients == null || ModSettings_VarietyMatters.ignoreIngredients)
            {
                return;
            }

            foreach (var thingDef in compIngredients.ingredients)
            {
                if (recentlyConsumed.Contains(thingDef.label))
                {
                    continue;
                }

                __result += recentlyConsumed.Count;
                break;
            }
        }
    }
}