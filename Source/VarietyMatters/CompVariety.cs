using RimWorld;
using Verse;

namespace VarietyMatters
{
    // Token: 0x0200000E RID: 14
    public class CompVariety : ThingComp
    {
        // Token: 0x17000006 RID: 6
        // (get) Token: 0x0600002A RID: 42 RVA: 0x00003B88 File Offset: 0x00001D88
        public CompProperties_Variety Props => (CompProperties_Variety)props;

        // Token: 0x0600002B RID: 43 RVA: 0x00003BA8 File Offset: 0x00001DA8
        public override void PostIngested(Pawn ingester)
        {
            if (!ingester.NonHumanlikeOrWildMan() && (ingester.IsColonist || ingester.IsPrisoner))
            {
                VarietyRecord.UpdateVarietyRecord(ingester, parent);
            }
        }

        // Token: 0x0600002C RID: 44 RVA: 0x00003BE8 File Offset: 0x00001DE8
        public override bool AllowStackWith(Thing other)
        {
            if (ModSettings_VarietyMatters.ignoreIngredients || !ModSettings_VarietyMatters.stackByIngredients)
            {
                return true;
            }

            var comp = parent.GetComp<CompIngredients>();
            var comp2 = ((ThingWithComps)other).GetComp<CompIngredients>();
            if (comp == null && comp2 == null)
            {
                return true;
            }

            if (comp != null && comp.ingredients.Count == 0 && comp2.ingredients.Count == 0)
            {
                return true;
            }

            if (comp != null && comp.ingredients.Count != comp2.ingredients.Count)
            {
                return false;
            }

            if (comp == null)
            {
                return true;
            }

            foreach (var thingDef in comp.ingredients)
            {
                if (!comp2.ingredients.Contains(thingDef))
                {
                    return false;
                }
            }

            return true;
        }

        // Token: 0x0600002D RID: 45 RVA: 0x00003CE0 File Offset: 0x00001EE0
        public static void FoodsAvailable()
        {
            var num = 0;
            var num2 = 0;
            foreach (var thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
            {
                if (!thingDef.IsNutritionGivingIngestible || !thingDef.ingestible.HumanEdible || thingDef.IsCorpse)
                {
                    continue;
                }

                if (thingDef.HasComp(typeof(CompIngredients)) &&
                    thingDef.ingestible.preferability >= FoodPreferability.RawTasty)
                {
                    num++;
                }
                else
                {
                    if (thingDef.ingestible.joyKind != null)
                    {
                        num++;
                        num2++;
                    }
                    else
                    {
                        if (!thingDef.HasComp(typeof(CompHatcher)))
                        {
                            num2++;
                        }
                    }
                }
            }

            if (num <= 22)
            {
                VarietyExpectation.moddedMeals = (num - 24f) / (36f - num);
            }
            else
            {
                VarietyExpectation.moddedMeals = (num - 24f) / (24f + (num * 2f));
            }

            if (num2 <= 66)
            {
                VarietyExpectation.moddedIngredients = (num2 - 66f) / (132f - num2);
            }
            else
            {
                VarietyExpectation.moddedIngredients = (num2 - 66f) / num2;
            }
        }
    }
}