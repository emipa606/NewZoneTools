using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld;

public class Designator_CancelChequer : Designator
{
    private static readonly HashSet<Thing> seenThings = [];

    private IntVec3 startPoint;

    public Designator_CancelChequer()
    {
        defaultLabel = "DesignatorCancelChequer".Translate();
        defaultDesc = "DesignatorCancelChequerDesc".Translate();
        icon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel");
        useMouseIcon = true;
        soundDragSustain = SoundDefOf.Designate_DragStandard;
        soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
        soundSucceeded = SoundDefOf.Designate_Cancel;
        hotKey = KeyBindingDefOf.Misc11;
        tutorTag = "Cancel";
    }

    public override int DraggableDimensions => 2;

    public override AcceptanceReport CanDesignateCell(IntVec3 c)
    {
        checked
        {
            AcceptanceReport result;
            if (!c.InBounds(Map))
            {
                result = false;
            }
            else if ((c.x + c.z + startPoint.x + startPoint.z) % 2 == 1)
            {
                result = false;
            }
            else if (CancelableDesignationsAt(c).Any())
            {
                result = true;
            }
            else
            {
                var thingList = c.GetThingList(Map);
                foreach (var thing in thingList)
                {
                    if (CanDesignateThing(thing).Accepted)
                    {
                        return true;
                    }
                }

                result = false;
            }

            return result;
        }
    }

    public override void DesignateSingleCell(IntVec3 c)
    {
        foreach (var designation in CancelableDesignationsAt(c).ToList())
        {
            if (designation.def.designateCancelable)
            {
                Map.designationManager.RemoveDesignation(designation);
            }
        }

        var thingList = c.GetThingList(Map);
        checked
        {
            for (var i = thingList.Count - 1; i >= 0; i--)
            {
                if (CanDesignateThing(thingList[i]).Accepted)
                {
                    DesignateThing(thingList[i]);
                }
            }
        }
    }

    public override AcceptanceReport CanDesignateThing(Thing t)
    {
        if (Map.designationManager.DesignationOn(t) != null)
        {
            foreach (var designation in Map.designationManager.AllDesignationsOn(t))
            {
                if (designation.def.designateCancelable)
                {
                    return true;
                }
            }
        }

        AcceptanceReport result;
        if (t.def.mineable && Map.designationManager.DesignationAt(t.Position, DesignationDefOf.Mine) != null)
        {
            result = true;
        }
        else
        {
            result = t.Faction == Faction.OfPlayer && t is Frame or Blueprint;
        }

        return result;
    }

    public override void DesignateThing(Thing t)
    {
        if (t is Frame or Blueprint)
        {
            t.Destroy(DestroyMode.Cancel);
        }
        else
        {
            Map.designationManager.RemoveAllDesignationsOn(t, true);
            if (!t.def.mineable)
            {
                return;
            }

            var designation = Map.designationManager.DesignationAt(t.Position, DesignationDefOf.Mine);
            if (designation != null)
            {
                Map.designationManager.RemoveDesignation(designation);
            }
        }
    }

    public override void SelectedUpdate()
    {
        GenUI.RenderMouseoverBracket();
        base.SelectedUpdate();
        if (Input.GetMouseButtonDown(0))
        {
            startPoint = UI.MouseCell();
        }
    }

    private IEnumerable<Designation> CancelableDesignationsAt(IntVec3 c)
    {
        return from x in Map.designationManager.AllDesignationsAt(c)
            where x.def != DesignationDefOf.Plan
            select x;
    }

    public override void RenderHighlight(List<IntVec3> dragCells)
    {
        seenThings.Clear();
        checked
        {
            for (var i = 0; i < dragCells.Count; i++)
            {
                if (Map.designationManager.HasMapDesignationAt(dragCells[i]))
                {
                    Graphics.DrawMesh(MeshPool.plane10,
                        dragCells[i].ToVector3ShiftedWithAltitude(AltitudeLayer.FogOfWar.AltitudeFor()),
                        Quaternion.identity, DesignatorUtility.DragHighlightCellMat, 0);
                }

                var thingList = dragCells[i].GetThingList(Map);
                foreach (var thing in thingList)
                {
                    if (seenThings.Contains(thing) || !CanDesignateThing(thing).Accepted)
                    {
                        continue;
                    }

                    var drawPos = thing.DrawPos;
                    drawPos.y = AltitudeLayer.FogOfWar.AltitudeFor();
                    Graphics.DrawMesh(MeshPool.plane10, drawPos, Quaternion.identity,
                        DesignatorUtility.DragHighlightThingMat, 0);
                    seenThings.Add(thing);
                }
            }

            seenThings.Clear();
        }
    }
}