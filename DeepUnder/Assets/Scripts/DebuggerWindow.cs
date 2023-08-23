using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;
#if UNITY_EDITOR
using UnityEditor;


public class DebuggerWindow : EditorWindow {

    [MenuItem("Window/Debugger")]
    public static void showWindow() {
        GetWindow<DebuggerWindow>("Debugger");
    }


    private void OnGUI() {
        if(GUILayout.Button("Reposition All Cave Segments"))
            repositionCave();
    }


    void repositionCave() {
        List<CaveSegment> segs = new List<CaveSegment>();
        foreach(var i in FindObjectsOfType<CaveSegment>())
            segs.Add(i);

        //  finds the starting point of the cave
        var start = segs[0];
        while(start.prevSegment != null)
            start = start.prevSegment.GetComponent<CaveSegment>();

        var temp = start;

        int count = FindObjectsOfType<CaveSegment>().Length;
        for(int i = 0; i < count; i++) {
            temp.attachToParentSegment();
            segs.Remove(temp);

            var prevTemp = temp;
            //  first, checks for sibling segments that also are parented to temp's parent
            foreach(var j in segs) {
                if(j.prevSegment == temp.prevSegment) {
                    temp = j;
                    break;
                }
            }
            if(temp != prevTemp)
                continue;

            //  second, checks for segs that have temp as their parent
            foreach(var j in segs) {
                if(j.prevSegment == temp.gameObject) {
                    temp = j;
                    break;
                }
            }
        }
    }
}

#endif

