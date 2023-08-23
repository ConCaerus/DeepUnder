using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveManager : MonoBehaviour {
    List<GameObject> segments = new List<GameObject>();


    private void Start() {
        foreach(var i in FindObjectsOfType<CaveSegment>())
            segments.Add(i.gameObject);
    }

    public List<GameObject> getSegments() {
        var temp = new List<GameObject>();
        foreach(var i in segments)
            temp.Add(i.gameObject);
        return temp;
    }

    public GameObject getClosestSegment(Vector3 pos) {
        var closest = segments[0];
        foreach(var i in segments) {
            if(Vector3.Distance(i.transform.position, pos) < Vector3.Distance(closest.transform.position, pos))
                closest = i;
        }
        return closest.gameObject;
    }
}
