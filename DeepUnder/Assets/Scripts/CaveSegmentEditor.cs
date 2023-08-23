using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor(typeof(CaveSegment))]
public class CaveSegmentEditor : Editor {
    GameObject prevPrevSegment;

    int prevUsedExitPoint = 1;


    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        CaveSegment cs = (CaveSegment)target;

        if(GUILayout.Button("Connect"))
            connectSegments(cs);
    }

    void connectSegments(CaveSegment cs) {
        //  checks if the segment doesn't have a connection
        if(cs.prevSegment == null)
            return;

        //  does the thing
        else {
            //  checks if this prevSegment is the same as the previously used segment
            if(prevPrevSegment == cs.prevSegment) {
                //  cycle through the usable exit point indexes
                prevUsedExitPoint++;
                if(prevUsedExitPoint > cs.prevSegment.GetComponent<CaveSegment>().exitPoints.Count - 1)
                    prevUsedExitPoint = 1;
            }
            prevPrevSegment = cs.prevSegment;
            cs.usedExitIndex = prevUsedExitPoint;
            cs.attachToParentSegment();
        }
    }
}

