using System.Collections.Generic;
using Verse;

namespace VarietyMatters
{
    // Token: 0x02000011 RID: 17
    public class VarietyRecord : GameComponent
    {
        // Token: 0x0400002A RID: 42
        public static List<Pawn_VarietyTracker> pawnRecords = new List<Pawn_VarietyTracker>();

        // Token: 0x0400002B RID: 43
        private static List<Pawn> trackedPawns = new List<Pawn>();

        // Token: 0x0400002C RID: 44
        private static Dictionary<Pawn, Pawn_VarietyTracker> varietyRecord;

        // Token: 0x06000036 RID: 54 RVA: 0x000041A0 File Offset: 0x000023A0
        public VarietyRecord(Game game)
        {
            varietyRecord = new Dictionary<Pawn, Pawn_VarietyTracker>();
        }

        // Token: 0x06000037 RID: 55 RVA: 0x000041B4 File Offset: 0x000023B4
        public static Pawn_VarietyTracker GetVarietyRecord(Pawn trackedPawn)
        {
            var result = varietyRecord.TryGetValue(trackedPawn, out var pawn_VarietyTracker)
                ? pawn_VarietyTracker
                : null;

            return result;
        }

        // Token: 0x06000038 RID: 56 RVA: 0x000041E0 File Offset: 0x000023E0
        public static void UpdateVarietyRecord(Pawn trackedPawn, Thing foodSource)
        {
            var value = new Pawn_VarietyTracker();
            if (varietyRecord.ContainsKey(trackedPawn))
            {
                value = GetVarietyRecord(trackedPawn);
            }
            else
            {
                varietyRecord.Add(trackedPawn, value);
            }

            Pawn_VarietyTracker.TrackRecentlyConsumed(ref value, trackedPawn, foodSource);
            varietyRecord[trackedPawn] = value;
        }

        // Token: 0x06000039 RID: 57 RVA: 0x00004234 File Offset: 0x00002434
        public static void RemoveTrackedPawn(Pawn trackedPawn)
        {
            if (varietyRecord.ContainsKey(trackedPawn))
            {
                varietyRecord.Remove(trackedPawn);
            }
        }

        // Token: 0x0600003A RID: 58 RVA: 0x00004260 File Offset: 0x00002460
        public override void FinalizeInit()
        {
            if (varietyRecord != null)
            {
                foreach (var pawn in varietyRecord.Keys)
                {
                    var dead = pawn.Dead;
                    if (dead)
                    {
                        RemoveTrackedPawn(pawn);
                    }
                }
            }

            CompVariety.FoodsAvailable();
            base.FinalizeInit();
        }

        // Token: 0x0600003B RID: 59 RVA: 0x000042E4 File Offset: 0x000024E4
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref varietyRecord, "VarietyRecord", LookMode.Reference, LookMode.Deep,
                ref trackedPawns, ref pawnRecords);
        }
    }
}