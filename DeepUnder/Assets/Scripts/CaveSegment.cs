using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveSegment : MonoBehaviour {
    //  NOTE: "starting" point will always be the first in this list
    [SerializeField] public List<Transform> exitPoints = new List<Transform>();
    [SerializeField] public Transform playerSpawnPos;
    [SerializeField] public Transform monsterSpawnPos;

    public bool deadEnd { get; private set; } = false;

    List<GameObject> attachedSegments = new List<GameObject>();

    public void addAttachedSegment(GameObject seg) {
        attachedSegments.Add(seg);
    }
    public void setDeadEnd(bool b) {
        deadEnd = b;
    }
}
