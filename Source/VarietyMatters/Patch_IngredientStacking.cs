using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VarietyMatters
{
    // Token: 0x02000002 RID: 2
    [HarmonyPatch(typeof(CompIngredients), "PreAbsorbStack")]
    public class Patch_IngredientStacking
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public static void Prefix(CompIngredients __instance, Thing otherStack, ref List<ThingDef> __state)
        {
            var ignoreIngredients = ModSettings_VarietyMatters.ignoreIngredients;
            if (ignoreIngredients)
            {
                return;
            }

            if (ModSettings_VarietyMatters.numIngredients <= 3 || __instance.ingredients.Count <= 0)
            {
                return;
            }

            __state = otherStack.TryGetComp<CompIngredients>().ingredients;
            foreach (var thingDef in __instance.ingredients)
            {
                if (!__state.Contains(thingDef))
                {
                    __state.Add(thingDef);
                }
            }
        }

        // Token: 0x06000002 RID: 2 RVA: 0x000020E4 File Offset: 0x000002E4
        public static void Postfix(ref CompIngredients __instance, ref List<ThingDef> __state)
        {
            var ignoreIngredients = ModSettings_VarietyMatters.ignoreIngredients;
            if (ignoreIngredients)
            {
                return;
            }

            var num = __instance.ingredients.Count;
            if (num < ModSettings_VarietyMatters.numIngredients && __state != null && __state.Count > num)
            {
                if (__state.Count > ModSettings_VarietyMatters.numIngredients)
                {
                    __state.Shuffle();
                }

                foreach (var thingDef in __state)
                {
                    if (!__instance.ingredients.Contains(thingDef))
                    {
                        __instance.ingredients.Add(thingDef);
                        num++;
                    }

                    if (num == ModSettings_VarietyMatters.numIngredients)
                    {
                        break;
                    }
                }
            }
            else
            {
                while (num > ModSettings_VarietyMatters.numIngredients && num > 0)
                {
                    __instance.ingredients.RemoveAt(num - 1);
                    num--;
                }
            }
        }
    }
}