using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace VarietyMatters
{
    // Token: 0x02000007 RID: 7
    internal class ModSettings_VarietyMatters : ModSettings
    {
        // Token: 0x0400000B RID: 11
        public static bool maxVariety;

        // Token: 0x0400000C RID: 12
        public static bool ignoreIngredients;

        // Token: 0x0400000D RID: 13
        public static bool sickPawns = true;

        // Token: 0x0400000E RID: 14
        public static bool stackByIngredients;

        // Token: 0x0400000F RID: 15
        public static int numIngredients = 3;

        // Token: 0x04000010 RID: 16
        public static bool curNeedAdjustments = true;

        // Token: 0x04000011 RID: 17
        public static bool foodModAdjustments = true;

        // Token: 0x04000012 RID: 18
        public static bool tempAdjustments = true;

        // Token: 0x04000013 RID: 19
        public static int extremelyLowVariety = 2;

        // Token: 0x04000014 RID: 20
        public static int veryLowVariety = 4;

        // Token: 0x04000015 RID: 21
        public static int lowVariety = 6;

        // Token: 0x04000016 RID: 22
        public static int moderateVariety = 8;

        // Token: 0x04000017 RID: 23
        public static int highVariety = 10;

        // Token: 0x04000018 RID: 24
        public static int skyHighVariety = 12;

        // Token: 0x04000019 RID: 25
        public static int nobleVariety = 15;

        // Token: 0x0400001A RID: 26
        public static bool preferVariety = true;

        // Token: 0x0400001B RID: 27
        public static bool preferSpoiling = true;

        // Token: 0x0400001C RID: 28
        public static bool mealUpdateDisplayed;

        // Token: 0x0400001D RID: 29
        public static Dictionary<string, bool> raceVariety = new Dictionary<string, bool>();

        // Token: 0x0400001E RID: 30
        public static List<string> raceKeys = new List<string>();

        // Token: 0x0400001F RID: 31
        public static List<bool> raceValues = new List<bool>();

        // Token: 0x0600000D RID: 13 RVA: 0x00002478 File Offset: 0x00000678
        public static List<string> GenerateRaces()
        {
            if (raceVariety == null)
            {
                raceVariety = new Dictionary<string, bool>();
            }

            foreach (var thingDef in DefDatabase<ThingDef>.AllDefsListForReading.Where(def =>
                    def.race is { intelligence: Intelligence.Humanlike } && def.race.foodType != FoodTypeFlags.None &&
                    !raceVariety.Keys.Contains(def.label))
                .OrderBy(def => def.label))
            {
                raceVariety[thingDef.label] = true;
            }

            return raceVariety.Keys.OrderBy(s => s).ToList();
        }

        // Token: 0x0600000E RID: 14 RVA: 0x00002598 File Offset: 0x00000798
        public override void ExposeData()
        {
            Scribe_Values.Look(ref maxVariety, "maxVariety");
            Scribe_Values.Look(ref ignoreIngredients, "ignoreIngredients");
            Scribe_Values.Look(ref sickPawns, "sickPawns", true);
            Scribe_Values.Look(ref stackByIngredients, "stackByIngredients");
            Scribe_Values.Look(ref curNeedAdjustments, "curNeedAdjustments", true);
            Scribe_Values.Look(ref foodModAdjustments, "foodModAdjustments", true);
            Scribe_Values.Look(ref tempAdjustments, "tempAdjustments", true);
            Scribe_Values.Look(ref extremelyLowVariety, "extremelyLowVariety", 2);
            Scribe_Values.Look(ref veryLowVariety, "veryLowVariety", 4);
            Scribe_Values.Look(ref lowVariety, "lowVariety", 6);
            Scribe_Values.Look(ref moderateVariety, "moderateVariety", 8);
            Scribe_Values.Look(ref highVariety, "highVariety", 10);
            Scribe_Values.Look(ref skyHighVariety, "skyHighVariety", 12);
            Scribe_Values.Look(ref nobleVariety, "nobleVariety", 15);
            Scribe_Values.Look(ref numIngredients, "numIngredients", 3);
            Scribe_Values.Look(ref mealUpdateDisplayed, "mealupdateDisplayed");
            Scribe_Values.Look(ref preferVariety, "preferVariety", true);
            Scribe_Values.Look(ref preferSpoiling, "preferSpoiling", true);
            Scribe_Collections.Look(ref raceVariety, "raceVariety", LookMode.Value, LookMode.Value, ref raceKeys,
                ref raceValues);
            base.ExposeData();
        }
    }
}