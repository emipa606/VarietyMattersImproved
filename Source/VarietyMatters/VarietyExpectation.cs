using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace VarietyMatters
{
    // Token: 0x02000010 RID: 16
    public class VarietyExpectation
    {
        // Token: 0x04000028 RID: 40
        public static float moddedMeals;

        // Token: 0x04000029 RID: 41
        public static float moddedIngredients;

        // Token: 0x17000007 RID: 7
        // (get) Token: 0x06000031 RID: 49 RVA: 0x00003F0C File Offset: 0x0000210C
        public static float ModVarietyFactor
        {
            get
            {
                var maxVariety = ModSettings_VarietyMatters.maxVariety;
                float result;
                if (maxVariety)
                {
                    result = Math.Max(moddedMeals, moddedIngredients) + 0.2f;
                }
                else
                {
                    var ignoreIngredients = ModSettings_VarietyMatters.ignoreIngredients;
                    result = ignoreIngredients ? moddedMeals : moddedIngredients;
                }

                return result;
            }
        }

        // Token: 0x06000032 RID: 50 RVA: 0x00003F58 File Offset: 0x00002158
        public static int GetVarietyExpectation(Pawn ingester)
        {
            var baseVarietyExpectation = GetBaseVarietyExpectation(ingester);
            var num = ApplyAdjustments(ingester, baseVarietyExpectation);
            num = Mathf.Max(num, Mathf.CeilToInt(baseVarietyExpectation * 0.55f));
            return (int)num;
        }

        // Token: 0x06000033 RID: 51 RVA: 0x00003F90 File Offset: 0x00002190
        public static float GetBaseVarietyExpectation(Pawn ingester)
        {
            var expectationDef = ExpectationsUtility.CurrentExpectationFor(ingester);
            if (expectationDef == ExpectationDefOf.ExtremelyLow)
            {
                return ModSettings_VarietyMatters.extremelyLowVariety;
            }

            if (expectationDef == ExpectationDefOf.VeryLow)
            {
                return ModSettings_VarietyMatters.veryLowVariety;
            }

            if (expectationDef == ExpectationDefOf.Low ||
                ingester.story?.traits?.HasTrait(TraitDefOf.Ascetic) == true || ingester.IsSlaveOfColony ||
                ingester.IsPrisonerOfColony)
            {
                return ModSettings_VarietyMatters.lowVariety;
            }

            if (expectationDef == ExpectationDefOf.Moderate)
            {
                return ModSettings_VarietyMatters.moderateVariety;
            }

            if (expectationDef == ExpectationDefOf.High)
            {
                return ModSettings_VarietyMatters.highVariety;
            }

            return expectationDef == ExpectationDefOf.SkyHigh
                ? ModSettings_VarietyMatters.skyHighVariety
                : ModSettings_VarietyMatters.nobleVariety;
        }

        // Token: 0x06000034 RID: 52 RVA: 0x0000404C File Offset: 0x0000224C
        private static float ApplyAdjustments(Pawn ingester, float baseExpectation)
        {
            var curNeedAdjustments = ModSettings_VarietyMatters.curNeedAdjustments;
            if (curNeedAdjustments)
            {
                var need_FoodVariety = ingester.needs.TryGetNeed<Need_FoodVariety>();
                if (need_FoodVariety.CurLevel >= 0.6f)
                {
                    baseExpectation += (need_FoodVariety.CurLevel - 0.5f) / 0.1f *
                                       Math.Min(baseExpectation * 0.16f, 1f);
                }

                if (need_FoodVariety.CurLevel <= 0.4f)
                {
                    baseExpectation *= 0.75f + (need_FoodVariety.CurLevel / 2f);
                }
            }

            var num = 1f;
            var foodModAdjustments = ModSettings_VarietyMatters.foodModAdjustments;
            if (foodModAdjustments)
            {
                num += ModVarietyFactor;
            }

            baseExpectation *= Math.Min(num, 2f);
            if (!ModSettings_VarietyMatters.tempAdjustments || ingester.MapHeld == null)
            {
                return baseExpectation;
            }

            var outdoorTemp = ingester.MapHeld.mapTemperature.OutdoorTemp;
            if (outdoorTemp < -10f)
            {
                baseExpectation *= 0.75f;
            }

            if (outdoorTemp < 0f)
            {
                baseExpectation *= 0.8f;
            }
            else
            {
                if (outdoorTemp < 10f || outdoorTemp > 44f)
                {
                    baseExpectation *= 0.9f;
                }
            }

            return baseExpectation;
        }
    }
}