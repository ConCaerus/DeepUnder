using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CaveSegment : MonoBehaviour {
    //  NOTE: "starting" point will always be the first in this list
    [SerializeField] List<GameObject> possibleEnvironmentPieces = new List<GameObject>();
    [SerializeField] public List<Transform> exitPoints = new List<Transform>();
    [SerializeField] Transform envPosHolder;

    [SerializeField] List<GameObject> attachedSegments = new List<GameObject>();

    public GameObject prevSegment;
    public int usedExitIndex;


    private void Start() {
        //  finds all of the attached segments
        foreach(var i in FindFirstObjectByType<CaveManager>().getSegments()) {
            //  makes sure that this isn't checking against the same segment
            if(i.gameObject == gameObject)
                continue;

            //  checks if the segment is attached
            for(int j = 0; j < exitPoints.Count; j++) {
                for(int w = 0; w < i.GetComponent<CaveSegment>().exitPoints.Count; w++) {
                    if(i.GetComponent<CaveSegment>().exitPoints[w].transform.position == exitPoints[j].transform.position) {
                        attachedSegments.Add(i.gameObject);
                        break;
                    }
                }
            }
        }

        //  spawns environment pieces
        if(envPosHolder == null)
            return;
        foreach(var i in envPosHolder.GetComponentsInChildren<Transform>()) {
            //  determines if should have piece
            int rand = Random.Range(0, 3);
            if(rand < 10) {
                var thing = Instantiate(possibleEnvironmentPieces[Random.Range(0, possibleEnvironmentPieces.Count)].gameObject, i);
                thing.transform.localPosition = Vector3.zero;

                //  randomizes the height of the thing
                thing.transform.localScale = new Vector3(1f, Random.Range(.75f, 1.25f), 1f);
            }
        }
    }

    public void attachToParentSegment() {
        if(prevSegment == null)
            return;
        if(usedExitIndex == 0)
            usedExitIndex++;

        //  does the thing
        Transform prevParent = gameObject.transform.parent;
        gameObject.transform.parent = prevSegment.GetComponent<CaveSegment>().exitPoints[usedExitIndex];
        gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        gameObject.transform.localPosition = Vector3.zero;

        //  unparents the thing
        gameObject.transform.parent = prevParent;
        gameObject.transform.localScale = Vector3.one;
    }
}
