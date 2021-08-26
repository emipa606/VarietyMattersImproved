using System;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace VarietyMatters
{
    // Token: 0x0200000D RID: 13
    public class ThoughtWorker_FoodVariety : ThoughtWorker
    {
        // Token: 0x06000028 RID: 40 RVA: 0x00003A8C File Offset: 0x00001C8C
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            var need_FoodVariety = p.needs.TryGetNeed<Need_FoodVariety>();
            ThoughtState result;
            if (need_FoodVariety == null || p.Starving() ||
                p.health.hediffSet.HasHediff(HediffDefOf.FoodPoisoning, true) || p.IsPrisoner ||
                p.IsCaravanMember() || ModSettings_VarietyMatters.sickPawns &&
                HealthAIUtility.ShouldSeekMedicalRest(p))
            {
                result = ThoughtState.Inactive;
            }
            else
            {
                switch (need_FoodVariety.CurCategory)
                {
                    case MenuCategory.None:
                        result = ThoughtState.Inactive;
                        break;
                    case MenuCategory.Empty:
                        result = ThoughtState.ActiveAtStage(0);
                        break;
                    case MenuCategory.Sparse:
                        result = ThoughtState.ActiveAtStage(1);
                        break;
                    case MenuCategory.Limited:
                        result = ThoughtState.ActiveAtStage(2);
                        break;
                    case MenuCategory.Simple:
                        result = ThoughtState.ActiveAtStage(3);
                        break;
                    case MenuCategory.Fine:
                        result = ThoughtState.ActiveAtStage(4);
                        break;
                    case MenuCategory.Lavish:
                        result = ThoughtState.ActiveAtStage(5);
                        break;
                    case MenuCategory.Gourmet:
                        result = ThoughtState.ActiveAtStage(6);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            return result;
        }
    }
}