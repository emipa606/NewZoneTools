using UnityEngine;
using Verse;

namespace RimWorld;

public class Designator_ZoneAdd_GrowingSame : Designator_ZoneAdd_Growing
{
    private IntVec3 startPoint;

    public Designator_ZoneAdd_GrowingSame()
    {
        zoneTypeToPlace = typeof(Zone_Growing);
        defaultLabel = "GrowingZoneSame".Translate();
        defaultDesc = "DesignatorGrowingZoneSameDesc".Translate();
        icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Growing");
        hotKey = KeyBindingDefOf.Misc12;
        tutorTag = "ZoneAdd_Growing";
    }

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

    public override AcceptanceReport CanDesignateCell(IntVec3 c)
    {
        return base.CanDesignateCell(c).Accepted &&
               Map.fertilityGrid.FertilityAt(c) == Map.fertilityGrid.FertilityAt(startPoint);
    }
}