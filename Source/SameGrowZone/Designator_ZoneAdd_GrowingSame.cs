using UnityEngine;
using Verse;

namespace RimWorld
{
    // Token: 0x02000002 RID: 2
    public class Designator_ZoneAdd_GrowingSame : Designator_ZoneAdd_Growing
    {
        // Token: 0x04000001 RID: 1
        private IntVec3 startPoint;

        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public Designator_ZoneAdd_GrowingSame()
        {
            zoneTypeToPlace = typeof(Zone_Growing);
            defaultLabel = "GrowingZoneSame".Translate();
            defaultDesc = "DesignatorGrowingZoneSameDesc".Translate();
            icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Growing");
            hotKey = KeyBindingDefOf.Misc12;
            tutorTag = "ZoneAdd_Growing";
        }

        // Token: 0x06000002 RID: 2 RVA: 0x000020C0 File Offset: 0x000002C0
        public override void SelectedUpdate()
        {
            base.SelectedUpdate();
            if (Find.Selector.SelectedZone != null && Find.Selector.SelectedZone.GetType() != zoneTypeToPlace)
            {
                Find.Selector.Deselect(Find.Selector.SelectedZone);
            }

            if (Input.GetMouseButtonDown(0))
            {
                startPoint = UI.MouseCell();
            }
        }

        // Token: 0x06000003 RID: 3 RVA: 0x00002134 File Offset: 0x00000334
        public override AcceptanceReport CanDesignateCell(IntVec3 c)
        {
            AcceptanceReport result;
            if (!base.CanDesignateCell(c).Accepted)
            {
                result = false;
            }
            else if (Map.fertilityGrid.FertilityAt(c) != Map.fertilityGrid.FertilityAt(startPoint))
            {
                result = false;
            }
            else
            {
                result = true;
            }

            return result;
        }
    }
}