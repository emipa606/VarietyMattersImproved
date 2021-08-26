using System.Collections.Generic;
using RimWorld;
using Verse;

namespace VarietyMatters
{
    // Token: 0x02000009 RID: 9
    public class Need_FoodVariety : Need
    {
        // Token: 0x06000019 RID: 25 RVA: 0x00002FA8 File Offset: 0x000011A8
        public Need_FoodVariety(Pawn pawn) : base(pawn)
        {
            threshPercents = new List<float>
            {
                0.15f,
                0.3f,
                0.7f,
                0.85f
            };
        }

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000017 RID: 23 RVA: 0x00002EE0 File Offset: 0x000010E0
        public MenuCategory CurCategory
        {
            get
            {
                var disabled = Disabled;
                MenuCategory result;
                if (disabled)
                {
                    result = MenuCategory.None;
                }
                else
                {
                    switch (CurLevel)
                    {
                        case > 0.99f:
                            result = MenuCategory.Gourmet;
                            break;
                        case > 0.85f:
                            result = MenuCategory.Lavish;
                            break;
                        case > 0.7f:
                            result = MenuCategory.Fine;
                            break;
                        case > 0.3f:
                            result = MenuCategory.Simple;
                            break;
                        case > 0.15f:
                            result = MenuCategory.Limited;
                            break;
                        default:
                            result = CurLevel > 0f ? MenuCategory.Sparse : MenuCategory.Empty;
                            break;
                    }
                }

                return result;
            }
        }

        // Token: 0x17000003 RID: 3
        // (get) Token: 0x06000018 RID: 24 RVA: 0x00002F94 File Offset: 0x00001194
        public override int GUIChangeArrow => 0;

        // Token: 0x17000004 RID: 4
        // (get) Token: 0x0600001A RID: 26 RVA: 0x00003010 File Offset: 0x00001210
        public override bool ShowOnNeedList => !Disabled;

        // Token: 0x17000005 RID: 5
        // (get) Token: 0x0600001B RID: 27 RVA: 0x0000302C File Offset: 0x0000122C
        public bool Disabled
        {
            get
            {
                bool result;
                if (pawn.Dead || !TraitDef_ModExtension.NeedsVariety(pawn) || pawn.needs.food == null ||
                    pawn.needs.mood == null)
                {
                    result = false;
                }
                else
                {
                    result = !(ModSettings_VarietyMatters.raceVariety.ContainsKey(pawn.def.label) &&
                               ModSettings_VarietyMatters.raceVariety[pawn.def.label]);
                }

                return result;
            }
        }

        // Token: 0x0600001C RID: 28 RVA: 0x000030CC File Offset: 0x000012CC
        public override void SetInitialLevel()
        {
            CurLevel = 0.5f;
        }

        // Token: 0x0600001D RID: 29 RVA: 0x000030DC File Offset: 0x000012DC
        public override void NeedInterval()
        {
            var disabled = Disabled;
            if (disabled)
            {
                CurLevel = 0.5f;
            }
        }

        // Token: 0x0600001E RID: 30 RVA: 0x00003108 File Offset: 0x00001308
        public override string GetTipString()
        {
            var tipString = base.GetTipString();
            var text = VarietyExpectation.GetVarietyExpectation(pawn).ToString();
            var text2 = "0";
            var text3 = "I haven't eaten in ages";
            var varietyRecord = VarietyRecord.GetVarietyRecord(pawn);
            if (varietyRecord is not { recentlyConsumed: { } })
            {
                return string.Concat(tipString, "\n\nVarieties Expected: ", text, "\nRecent Varieties: ", text2,
                    "\nLast: ",
                    text3);
            }

            text2 = varietyRecord.recentVarieties.ToString();
            var lastVariety = varietyRecord.lastVariety;
            if (lastVariety.Count > 0)
            {
                text3 = lastVariety[0].CapitalizeFirst();
            }

            if (lastVariety.Count <= 1)
            {
                return string.Concat(tipString, "\n\nVarieties Expected: ", text, "\nRecent Varieties: ", text2,
                    "\nLast: ",
                    text3);
            }

            for (var i = 1; i < lastVariety.Count; i++)
            {
                text3 = text3 + ", " + lastVariety[i];
            }

            return string.Concat(tipString, "\n\nVarieties Expected: ", text, "\nRecent Varieties: ", text2, "\nLast: ",
                text3);
        }
    }
}