using Verse;

namespace VarietyMatters
{
    // Token: 0x0200000F RID: 15
    internal class TraitDef_ModExtension : DefModExtension
    {
        // Token: 0x04000024 RID: 36
        public float expectationFactor = 1f;

        // Token: 0x04000026 RID: 38
        public int maxVarietyExpectation = 40;

        // Token: 0x04000025 RID: 37
        public int minVarietyExpectation = 2;

        // Token: 0x04000027 RID: 39
        public bool needsVariety = true;

        // Token: 0x0600002F RID: 47 RVA: 0x00003E5C File Offset: 0x0000205C
        public static bool NeedsVariety(Pawn pawn)
        {
            foreach (var trait in pawn.story.traits.allTraits)
            {
                if (trait.def.HasModExtension<TraitDef_ModExtension>())
                {
                    return trait.def.GetModExtension<TraitDef_ModExtension>().needsVariety;
                }
            }

            return true;
        }
    }
}