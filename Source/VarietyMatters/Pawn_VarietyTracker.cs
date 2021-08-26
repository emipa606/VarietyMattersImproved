using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace VarietyMatters
{
    // Token: 0x0200000C RID: 12
    public class Pawn_VarietyTracker : IExposable
    {
        // Token: 0x04000022 RID: 34
        public List<string> lastVariety;

        // Token: 0x04000021 RID: 33
        public List<string> recentlyConsumed;

        // Token: 0x04000023 RID: 35
        public int recentVarieties;

        // Token: 0x06000026 RID: 38 RVA: 0x00003A34 File Offset: 0x00001C34
        public void ExposeData()
        {
            Scribe_Collections.Look(ref lastVariety, "lastVariety", LookMode.Value, Array.Empty<object>());
            Scribe_Collections.Look(ref recentlyConsumed, "recentlyConsumed", LookMode.Value, Array.Empty<object>());
            Scribe_Values.Look(ref recentVarieties, "recentVarieties");
        }

        // Token: 0x06000022 RID: 34 RVA: 0x000034A4 File Offset: 0x000016A4
        public static void TrackRecentlyConsumed(ref Pawn_VarietyTracker pawnRecord, Pawn ingester, Thing foodSource)
        {
            if (pawnRecord.recentlyConsumed == null)
            {
                pawnRecord.recentlyConsumed = new List<string> { "First food" };
                pawnRecord.lastVariety = new List<string>();
            }

            var baseVarietyExpectation = VarietyExpectation.GetBaseVarietyExpectation(ingester);
            var varietyExpectation = VarietyExpectation.GetVarietyExpectation(ingester);
            TrackVarieties(ref pawnRecord, ingester, foodSource, varietyExpectation);
            RemoveVarieties(ref pawnRecord.recentlyConsumed, varietyExpectation, (int)baseVarietyExpectation);
            pawnRecord.recentVarieties = pawnRecord.recentlyConsumed.Distinct().Count();
            if (pawnRecord.recentlyConsumed[0] == "BadVariety")
            {
                pawnRecord.recentVarieties--;
            }

            if (ingester.needs.TryGetNeed<Need_FoodVariety>() != null &&
                !ingester.needs.TryGetNeed<Need_FoodVariety>().Disabled)
            {
                AdjustVarietyLevel(pawnRecord.recentVarieties, ingester, varietyExpectation, baseVarietyExpectation);
            }
        }

        // Token: 0x06000023 RID: 35 RVA: 0x00003594 File Offset: 0x00001794
        public static void TrackVarieties(ref Pawn_VarietyTracker pawnRecord, Pawn ingester, Thing foodSource,
            int varietyExpectation)
        {
            var compIngredients = foodSource.TryGetComp<CompIngredients>();
            var preferability = foodSource.def.ingestible.preferability;
            var label = foodSource.def.label;
            pawnRecord.lastVariety.Clear();
            if (foodSource.IsNotFresh())
            {
                pawnRecord.recentlyConsumed.Insert(0, "BadVariety");
                pawnRecord.lastVariety.Add("rotten food");
            }
            else
            {
                if (preferability <= FoodPreferability.RawBad)
                {
                    if (FoodUtility.IsHumanlikeCorpseOrHumanlikeMeat(foodSource, foodSource.def) &&
                        ingester.story.traits.HasTrait(TraitDefOf.Cannibal))
                    {
                        pawnRecord.recentlyConsumed.Add("Raw humanlike flesh");
                        pawnRecord.lastVariety.Add("tasty raw humanlike flesh");
                    }
                    else
                    {
                        if (foodSource.def.ingestible.joyKind != null)
                        {
                            pawnRecord.recentlyConsumed.Add(label);
                            pawnRecord.lastVariety.Add(label);
                        }
                        else
                        {
                            pawnRecord.recentlyConsumed.Add(label);
                            pawnRecord.recentlyConsumed.Insert(0, "BadVariety");
                            pawnRecord.lastVariety.Add("some disgusting " + label);
                        }
                    }
                }
                else
                {
                    if (preferability == FoodPreferability.RawTasty)
                    {
                        pawnRecord.recentlyConsumed.Add("Raw" + label);
                        pawnRecord.lastVariety.Add("raw " + label);
                    }
                    else
                    {
                        if (compIngredients == null || ModSettings_VarietyMatters.ignoreIngredients ||
                            ModSettings_VarietyMatters.maxVariety)
                        {
                            pawnRecord.recentlyConsumed.Add(label);
                            pawnRecord.lastVariety.Add(label);
                        }

                        if (compIngredients == null || ModSettings_VarietyMatters.ignoreIngredients)
                        {
                            return;
                        }

                        if (compIngredients.ingredients.Count == 0 &&
                            !ModSettings_VarietyMatters.maxVariety)
                        {
                            var num = (int)VarietyExpectation.GetBaseVarietyExpectation(ingester);
                            if (preferability >= FoodPreferability.MealLavish)
                            {
                                pawnRecord.recentlyConsumed.Add("Mystery lavish" + Rand.Range(0, num / 2));
                                pawnRecord.lastVariety.Add("mystery lavish food");
                            }

                            if (preferability >= FoodPreferability.MealFine)
                            {
                                pawnRecord.recentlyConsumed.Add("Mystery meat" + Rand.Range(0, num / 2));
                                pawnRecord.lastVariety.Add("mystery meat");
                            }

                            pawnRecord.recentlyConsumed.Add("Mystery ingredient" +
                                                            Rand.Range(0, ((varietyExpectation / 2) + num) / 2));
                            pawnRecord.lastVariety.Add("mystery ingredient");
                        }
                        else
                        {
                            foreach (var thingDef in compIngredients.ingredients)
                            {
                                label = thingDef.label;
                                if (!thingDef.IsNutritionGivingIngestible)
                                {
                                    continue;
                                }

                                if (preferability == FoodPreferability.MealLavish &&
                                    (ModSettings_VarietyMatters.maxVariety ||
                                     !pawnRecord.recentlyConsumed.Contains("Lavish" + label)))
                                {
                                    pawnRecord.recentlyConsumed.Add("Lavish" + label);
                                    pawnRecord.lastVariety.Add("lavishly prepared " + label);
                                }
                                else
                                {
                                    pawnRecord.recentlyConsumed.Add(label);
                                    pawnRecord.lastVariety.Add(label);
                                }
                            }
                        }
                    }
                }
            }
        }

        // Token: 0x06000024 RID: 36 RVA: 0x00003950 File Offset: 0x00001B50
        public static void RemoveVarieties(ref List<string> recentlyConsumed, int varietyExpectation,
            int baseVarietyExpectation)
        {
            var num = Mathf.Max(baseVarietyExpectation, varietyExpectation) * 2;
            for (var i = recentlyConsumed.Count; i > num; i--)
            {
                recentlyConsumed.RemoveAt(Rand.Range(0, num - 1));
            }
        }

        // Token: 0x06000025 RID: 37 RVA: 0x00003994 File Offset: 0x00001B94
        public static void AdjustVarietyLevel(int distinctVarieties, Pawn ingester, int varietyExpectation,
            float baseExpectation)
        {
            var need_FoodVariety = ingester.needs.TryGetNeed<Need_FoodVariety>();
            float num = Mathf.Clamp(distinctVarieties - varietyExpectation, 0 - ((varietyExpectation / 2) + 1),
                (varietyExpectation / 2) + 1);
            if (need_FoodVariety.CurLevel > 0.6f && distinctVarieties <= varietyExpectation)
            {
                num -= baseExpectation / 2f;
            }

            if (need_FoodVariety.CurLevel < 0.4f && distinctVarieties >= varietyExpectation)
            {
                num += baseExpectation / 2f;
            }

            need_FoodVariety.CurLevel = Mathf.Clamp(need_FoodVariety.CurLevel + (num / 100f), 0f, 1f);
        }
    }
}