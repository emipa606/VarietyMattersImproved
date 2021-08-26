using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace VarietyMatters
{
    // Token: 0x02000003 RID: 3
    [HarmonyPatch]
    public class VarietyCooking : WorkGiver_DoBill
    {
        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000004 RID: 4 RVA: 0x000021EC File Offset: 0x000003EC
        // (set) Token: 0x06000005 RID: 5 RVA: 0x000021F3 File Offset: 0x000003F3
        private static Pawn Chef { get; set; }

        // Token: 0x06000006 RID: 6 RVA: 0x000021FC File Offset: 0x000003FC
        [HarmonyPatch(typeof(WorkGiver_DoBill), "TryFindBestBillIngredients")]
        [HarmonyPrefix]
        private static void GetChef(Pawn pawn)
        {
            if (!(ModSettings_VarietyMatters.ignoreIngredients || !ModSettings_VarietyMatters.preferVariety))
            {
                Chef = pawn;
            }
        }

        // Token: 0x06000007 RID: 7 RVA: 0x0000222C File Offset: 0x0000042C
        [HarmonyPatch(typeof(WorkGiver_DoBill), "TryFindBestBillIngredientsInSet")]
        [HarmonyPrefix]
        private static bool BestVarietyInSet(ref bool __result, List<Thing> availableThings, Bill bill,
            List<ThingCount> chosen, IntVec3 rootCell, ref bool alreadySorted)
        {
            bool result;
            if (ModSettings_VarietyMatters.ignoreIngredients || !ModSettings_VarietyMatters.preferVariety)
            {
                result = __result;
            }
            else
            {
                if (bill.recipe.workSkill != SkillDefOf.Cooking || bill.recipe.ProducedThingDef == null ||
                    bill.recipe.specialProducts != null)
                {
                    result = true;
                }
                else
                {
                    var varietyRecord = VarietyRecord.GetVarietyRecord(Chef);
                    if (varietyRecord == null || varietyRecord.recentlyConsumed == null)
                    {
                        result = true;
                    }
                    else
                    {
                        SortIngredients(availableThings, rootCell, varietyRecord.recentlyConsumed);
                        var allowMixingIngredients = bill.recipe.allowMixingIngredients;
                        if (allowMixingIngredients)
                        {
                            __result = BestVariety_AllowMix(availableThings, bill, chosen);
                            result = false;
                        }
                        else
                        {
                            alreadySorted = true;
                            result = true;
                        }
                    }
                }
            }

            return result;
        }

        // Token: 0x06000008 RID: 8 RVA: 0x000022F0 File Offset: 0x000004F0
        private static void SortIngredients(List<Thing> availableThings, IntVec3 rootCell, List<string> curVarieties)
        {
            int Comparison(Thing t1, Thing t2)
            {
                var num = (t1.Position - rootCell).LengthHorizontal;
                var num2 = (t2.Position - rootCell).LengthHorizontal;
                if (t1.def.label != t2.def.label)
                {
                    if (!curVarieties.Contains(t1.def.label))
                    {
                        num -= GenMath.Sqrt(curVarieties.Count);
                    }

                    if (!curVarieties.Contains(t2.def.label))
                    {
                        num2 -= GenMath.Sqrt(curVarieties.Count);
                    }
                }

                var preferSpoiling = ModSettings_VarietyMatters.preferSpoiling;
                if (!preferSpoiling)
                {
                    return num.CompareTo(num2);
                }

                var compRottable = t1.TryGetComp<CompRottable>();
                var compRottable2 = t1.TryGetComp<CompRottable>();
                if (compRottable != null)
                {
                    num += (1f - compRottable.RotProgressPct) * 10f;
                }

                if (compRottable2 != null)
                {
                    num2 += (1f - compRottable2.RotProgressPct) * 10f;
                }

                return num.CompareTo(num2);
            }

            availableThings.Sort(Comparison);
        }

        // Token: 0x06000009 RID: 9 RVA: 0x00002328 File Offset: 0x00000528
        private static bool BestVariety_AllowMix(List<Thing> availableThings, Bill bill, List<ThingCount> chosen)
        {
            chosen.Clear();
            foreach (var ingredientCount in bill.recipe.ingredients)
            {
                var num = ingredientCount.GetBaseCount();
                foreach (var thing in availableThings)
                {
                    if (!ingredientCount.filter.Allows(thing) ||
                        !ingredientCount.IsFixedIngredient && !bill.ingredientFilter.Allows(thing))
                    {
                        continue;
                    }

                    var num2 = bill.recipe.IngredientValueGetter.ValuePerUnitOf(thing.def);
                    var num3 = Mathf.Min(Mathf.CeilToInt(num / num2), thing.stackCount);
                    ThingCountUtility.AddToList(chosen, thing, num3);
                    num -= num3 * num2;
                    if (num <= 0.0001f)
                    {
                        break;
                    }
                }

                if (num > 0.0001f)
                {
                    return false;
                }
            }

            return true;
        }
    }
}