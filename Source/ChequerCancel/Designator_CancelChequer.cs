using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
    // Token: 0x02000002 RID: 2
    public class Designator_CancelChequer : Designator
    {
        // Token: 0x04000002 RID: 2
        private static readonly HashSet<Thing> seenThings = new HashSet<Thing>();

        // Token: 0x04000001 RID: 1
        private IntVec3 startPoint;

        // Token: 0x06000002 RID: 2 RVA: 0x00002064 File Offset: 0x00000264
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

        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public override int DraggableDimensions => 2;

        // Token: 0x06000003 RID: 3 RVA: 0x000020EC File Offset: 0x000002EC
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

        // Token: 0x06000004 RID: 4 RVA: 0x000021CC File Offset: 0x000003CC
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

        // Token: 0x06000005 RID: 5 RVA: 0x000022AC File Offset: 0x000004AC
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
                result = t.Faction == Faction.OfPlayer && (t is Frame || t is Blueprint);
            }

            return result;
        }

        // Token: 0x06000006 RID: 6 RVA: 0x000023B0 File Offset: 0x000005B0
        public override void DesignateThing(Thing t)
        {
            if (t is Frame || t is Blueprint)
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

        // Token: 0x06000007 RID: 7 RVA: 0x00002448 File Offset: 0x00000648
        public override void SelectedUpdate()
        {
            GenUI.RenderMouseoverBracket();
            base.SelectedUpdate();
            if (Input.GetMouseButtonDown(0))
            {
                startPoint = UI.MouseCell();
            }
        }

        // Token: 0x06000008 RID: 8 RVA: 0x000024A4 File Offset: 0x000006A4
        private IEnumerable<Designation> CancelableDesignationsAt(IntVec3 c)
        {
            return from x in Map.designationManager.AllDesignationsAt(c)
                where x.def != DesignationDefOf.Plan
                select x;
        }

        // Token: 0x06000009 RID: 9 RVA: 0x000024EC File Offset: 0x000006EC
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
}