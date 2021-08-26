using Verse;

namespace VarietyMatters
{
    // Token: 0x0200000B RID: 11
    [StaticConstructorOnStartup]
    public class AddCompVariety
    {
        // Token: 0x06000020 RID: 32 RVA: 0x00003404 File Offset: 0x00001604
        static AddCompVariety()
        {
            var allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
            foreach (var thingDef in allDefsListForReading)
            {
                if (!thingDef.IsMeat && !thingDef.IsCorpse)
                {
                    continue;
                }

                if (thingDef.GetCompProperties<CompProperties_Variety>() != null)
                {
                    continue;
                }

                var item = new CompProperties_Variety();
                thingDef.comps.Add(item);
            }

            ModSettings_VarietyMatters.GenerateRaces();
        }
    }
}