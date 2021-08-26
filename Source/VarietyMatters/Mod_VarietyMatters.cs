using System.Reflection;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace VarietyMatters
{
    // Token: 0x02000008 RID: 8
    internal class Mod_VarietyMatters : Mod
    {
        // Token: 0x04000020 RID: 32
        private static Vector2 scrollPosition = Vector2.zero;

        // Token: 0x06000011 RID: 17 RVA: 0x000027B0 File Offset: 0x000009B0
        public Mod_VarietyMatters(ModContentPack content) : base(content)
        {
            GetSettings<ModSettings_VarietyMatters>();
            var harmony = new Harmony("rimworld.varietymatters");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        // Token: 0x06000012 RID: 18 RVA: 0x000027E4 File Offset: 0x000009E4
        public override void WriteSettings()
        {
            var maxVariety = ModSettings_VarietyMatters.maxVariety;
            if (maxVariety)
            {
                ModSettings_VarietyMatters.ignoreIngredients = false;
            }

            base.WriteSettings();
        }

        // Token: 0x06000013 RID: 19 RVA: 0x0000280C File Offset: 0x00000A0C
        public override void DoSettingsWindowContents(Rect inRect)
        {
            var listing_Standard = new Listing_Standard();
            var rect = new Rect(10f, 50f, inRect.width * 0.5f, inRect.height);
            listing_Standard.Begin(rect);
            listing_Standard.Label("Variety Options:");
            listing_Standard.CheckboxLabeled("     Use ingredients and meals when tracking variety: ",
                ref ModSettings_VarietyMatters.maxVariety);
            if (!ModSettings_VarietyMatters.maxVariety)
            {
                listing_Standard.CheckboxLabeled("     Ignore ingredients when tracking variety: ",
                    ref ModSettings_VarietyMatters.ignoreIngredients);
            }

            if (ModSettings_VarietyMatters.maxVariety || !ModSettings_VarietyMatters.ignoreIngredients)
            {
                listing_Standard.CheckboxLabeled("     Cooks use different ingredients: ",
                    ref ModSettings_VarietyMatters.preferVariety);
                listing_Standard.CheckboxLabeled("     Cooks prefer spoiling ingredients: ",
                    ref ModSettings_VarietyMatters.preferSpoiling);
                listing_Standard.CheckboxLabeled("     Stack meals by ingredients: ",
                    ref ModSettings_VarietyMatters.stackByIngredients);
                var label = "     Ingredients When Stacking (vanilla = 3):";
                var text = ModSettings_VarietyMatters.numIngredients.ToString();
                LabeledIntEntry(listing_Standard.GetRect(24f), label, ref ModSettings_VarietyMatters.numIngredients,
                    ref text, 1, 1, 1, 10);
            }

            listing_Standard.CheckboxLabeled("     Sick pawns ignore variety thoughts: ",
                ref ModSettings_VarietyMatters.sickPawns);
            listing_Standard.GapLine();
            if (listing_Standard.ButtonTextLabeled("Expectation Level Base Varieties:", "Reset"))
            {
                ModSettings_VarietyMatters.extremelyLowVariety = 2;
                ModSettings_VarietyMatters.veryLowVariety = 4;
                ModSettings_VarietyMatters.lowVariety = 6;
                ModSettings_VarietyMatters.moderateVariety = 8;
                ModSettings_VarietyMatters.highVariety = 10;
                ModSettings_VarietyMatters.skyHighVariety = 12;
                ModSettings_VarietyMatters.nobleVariety = 15;
            }

            listing_Standard.Gap();
            var label2 = "     Extremely Low (default 2):";
            var text2 = ModSettings_VarietyMatters.extremelyLowVariety.ToString();
            LabeledIntEntry(listing_Standard.GetRect(24f), label2, ref ModSettings_VarietyMatters.extremelyLowVariety,
                ref text2, 1, 5, 1, 50);
            var label3 = "     Very Low (default 4):";
            var text3 = ModSettings_VarietyMatters.veryLowVariety.ToString();
            LabeledIntEntry(listing_Standard.GetRect(24f), label3, ref ModSettings_VarietyMatters.veryLowVariety,
                ref text3, 1, 5, 1, 50);
            var label4 = "     Low (default 6):";
            var text4 = ModSettings_VarietyMatters.lowVariety.ToString();
            LabeledIntEntry(listing_Standard.GetRect(24f), label4, ref ModSettings_VarietyMatters.lowVariety, ref text4,
                1, 5, 1, 50);
            var label5 = "     Moderate (default 8):";
            var text5 = ModSettings_VarietyMatters.moderateVariety.ToString();
            LabeledIntEntry(listing_Standard.GetRect(24f), label5, ref ModSettings_VarietyMatters.moderateVariety,
                ref text5, 1, 5, 1, 50);
            var label6 = "     High (default 10):";
            var text6 = ModSettings_VarietyMatters.highVariety.ToString();
            LabeledIntEntry(listing_Standard.GetRect(24f), label6, ref ModSettings_VarietyMatters.highVariety,
                ref text6, 1, 5, 1, 50);
            var label7 = "     Sky High (default 12):";
            var text7 = ModSettings_VarietyMatters.skyHighVariety.ToString();
            LabeledIntEntry(listing_Standard.GetRect(24f), label7, ref ModSettings_VarietyMatters.skyHighVariety,
                ref text7, 1, 5, 1, 50);
            var label8 = "     Noble and Royal (default 15):";
            var text8 = ModSettings_VarietyMatters.nobleVariety.ToString();
            LabeledIntEntry(listing_Standard.GetRect(24f), label8, ref ModSettings_VarietyMatters.nobleVariety,
                ref text8, 1, 5, 1, 50);
            listing_Standard.GapLine();
            listing_Standard.Label("Optional Variety Expectation Adjustments Factors:");
            listing_Standard.CheckboxLabeled("     Current need level:",
                ref ModSettings_VarietyMatters.curNeedAdjustments);
            listing_Standard.CheckboxLabeled("     Mod-added varieties (reload required): ",
                ref ModSettings_VarietyMatters.foodModAdjustments);
            listing_Standard.CheckboxLabeled("     Seasonal temperature: ",
                ref ModSettings_VarietyMatters.tempAdjustments);
            listing_Standard.End();
            var raceList = ModSettings_VarietyMatters.GenerateRaces();
            var rect2 = new Rect(50f + (inRect.width * 0.5f), 0f, inRect.width * 0.4f, 50f);
            listing_Standard.Begin(rect2);
            if (listing_Standard.ButtonTextLabeled("Enable/Disable Variety:", "Reset Current Races"))
            {
                foreach (var key in raceList)
                {
                    ModSettings_VarietyMatters.raceVariety[key] = true;
                }
            }

            listing_Standard.End();
            var outRect = new Rect(50f + (inRect.width * 0.5f), 50f, (inRect.width * 0.5f) - 50f, inRect.height - 10f);
            var rect3 = new Rect(0f, 0f, rect2.width - 30f, raceList.Count * 24f);
            Widgets.BeginScrollView(outRect, ref scrollPosition, rect3);
            listing_Standard.Begin(rect3);
            foreach (var key in raceList)
            {
                var value = ModSettings_VarietyMatters.raceVariety[key];
                listing_Standard.CheckboxLabeled(key.CapitalizeFirst(), ref value);
                ModSettings_VarietyMatters.raceVariety[key] = value;
            }

            listing_Standard.End();
            Widgets.EndScrollView();
            base.DoSettingsWindowContents(inRect);
        }

        // Token: 0x06000014 RID: 20 RVA: 0x00002CCC File Offset: 0x00000ECC
        public void LabeledIntEntry(Rect rect, string label, ref int value, ref string editBuffer, int multiplier,
            int largeMultiplier, int min, int max)
        {
            var num = (int)rect.width / 15;
            Widgets.Label(rect, label);
            if (multiplier != largeMultiplier)
            {
                if (Widgets.ButtonText(new Rect(rect.xMax - (num * 5f), rect.yMin, num, rect.height),
                    (-1 * largeMultiplier).ToString()))
                {
                    value -= largeMultiplier * GenUI.CurrentAdjustmentMultiplier();
                    editBuffer = value.ToString();
                    SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera();
                }

                if (Widgets.ButtonText(new Rect(rect.xMax - num, rect.yMin, num, rect.height),
                    "+" + largeMultiplier))
                {
                    value += largeMultiplier * multiplier * GenUI.CurrentAdjustmentMultiplier();
                    editBuffer = value.ToString();
                    SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera();
                }
            }

            if (Widgets.ButtonText(new Rect(rect.xMax - (num * 4f), rect.yMin, num, rect.height),
                (-1 * multiplier).ToString()))
            {
                value -= GenUI.CurrentAdjustmentMultiplier();
                editBuffer = value.ToString();
                SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera();
            }

            if (Widgets.ButtonText(new Rect(rect.xMax - (num * 2), rect.yMin, num, rect.height),
                "+" + multiplier))
            {
                value += multiplier * GenUI.CurrentAdjustmentMultiplier();
                editBuffer = value.ToString();
                SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera();
            }

            Widgets.TextFieldNumeric(new Rect(rect.xMax - (num * 3), rect.yMin, num, rect.height), ref value,
                ref editBuffer, min, max);
        }

        // Token: 0x06000015 RID: 21 RVA: 0x00002EBC File Offset: 0x000010BC
        public override string SettingsCategory()
        {
            return "VarietyMatters";
        }
    }
}