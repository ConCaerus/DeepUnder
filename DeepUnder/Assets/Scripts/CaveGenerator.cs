using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CaveGenerator : MonoBehaviour {
    [SerializeField] GameObject playerObj;
    [SerializeField] GameObject ropePrefab;
    [SerializeField] Transform minimapHolder;
    [SerializeField] List<GameObject> cavePieces = new List<GameObject>();
    [SerializeField] List<GameObject> boulders = new List<GameObject>();
    List<GameObject> createdPieces = new List<GameObject>();

    List<RopeInstance> ropes = new List<RopeInstance>();

    int cavePathLength = 10;
    int minCaveLength = 7;
    int caveCount;

    int minMonsterCount = 3;

    /*
    private void Start() {
        generateCave();
        GetComponent<MonsterSpawner>().spawnMonsters();
        FindAnyObjectByType<Minimap>().setup(transform.GetChild(0).gameObject); //  sends the cave to the minimap
    }

    void generateCave() {
        caveCount = 1;

        //  checks if there are elegeble spawning segments
        bool valid = false;
        foreach(var i in cavePieces) {
            if(i.GetComponent<CaveSegment>().playerSpawnPos != null) {
                valid = true;
                break;
            }
        }
        if(!valid) {
            Debug.LogError("No valid starting segments!");
            return;
        }

        //  spawns the first segment
        int rand = Random.Range(0, cavePieces.Count);
        while(cavePieces[rand].GetComponent<CaveSegment>().playerSpawnPos == null)  //  finds a valid spawning segment
            rand = Random.Range(0, cavePieces.Count);

        var p = Instantiate(cavePieces[rand], transform.GetChild(0));
        p.transform.position = Vector3.zero;
        playerObj.transform.position = p.GetComponent<CaveSegment>().playerSpawnPos.position;
        createdPieces.Add(p.gameObject);

        //  spawns the boulder at the beginning of the cave
        //  NOTE: eventually replace this with a hatch or some sort of enterance
        var b = Instantiate(boulders[Random.Range(0, boulders.Count)], transform.GetChild(0));
        b.transform.position = p.transform.GetChild(0).position + new Vector3(0.0f, 2f, 0.0f);

        //  starts a path for each exit point (except for the starting point)
        var pcs = p.GetComponent<CaveSegment>();
        for(int i = 1; i < pcs.exitPoints.Count; i++)
            generatePath(p.gameObject, pcs.exitPoints[i], cavePathLength);



        //  if the cave isn't long enough, start over
        //return;
        if(caveCount <= minCaveLength) {
            //  destroys everything
            foreach(var i in transform.GetChild(0).GetComponentsInChildren<Renderer>())
                Destroy(i.gameObject);
            foreach(var i in ropes)
                Destroy(i.gameObject);
            ropes.Clear();
            createdPieces.Clear();
            generateCave();
        }

        //  creates extra bounds for ropes to hit
        foreach(var i in createdPieces) {
            var temp = Instantiate(i.gameObject, transform);
            temp.transform.position = i.transform.position;
            temp.transform.localScale = i.transform.localScale * 1.01f;
        }

        //  drops all ropes
        foreach(var i in ropes)
            i.dropRope();
    }

    void generatePath(GameObject parentSegment, Transform parentEndPoint, int length) {
        int rand = Random.Range(0, cavePieces.Count);

        //  spawns the next segment
        var p = Instantiate(cavePieces[rand], parentEndPoint);
        var offset = -p.GetComponent<CaveSegment>().exitPoints[0].localPosition;    //  offsets it by the negative local pos of the starting position
        p.transform.localPosition = offset;

        //  unparents it
        p.transform.parent = transform.GetChild(0);


        //  checks if colliding with an already created path
        var pcs = p.GetComponent<CaveSegment>();
        parentSegment.layer = LayerMask.NameToLayer("Default");
        p.layer = LayerMask.NameToLayer("Default");
        for(int i = 1; i < pcs.exitPoints.Count; i++) {
            //  checks for collision around the end point
            if(Physics.CheckSphere(pcs.exitPoints[i].position, p.GetComponent<CaveSegment>().isRoom ? 30f : 10f, LayerMask.GetMask("Ground"))) { 
                //  destroys the segment
                Destroy(p.gameObject);


                //  ends the path and places a boulder
                var b = Instantiate(boulders[Random.Range(0, boulders.Count)], transform.GetChild(0));
                b.transform.position = parentEndPoint.position + new Vector3(0.0f, 2f, 0.0f);
                parentSegment.layer = LayerMask.NameToLayer("Ground");
                return;
            }
        }
        p.layer = LayerMask.NameToLayer("Ground");
        parentSegment.layer = LayerMask.NameToLayer("Ground");


        //  adds it to the cave
        createdPieces.Add(p.gameObject);
        caveCount++;


        //  checks if needs to check for a needed rope
        //  NOTE: rope destroys itself if it determines that it's not needed
        if(p.GetComponent<CaveSegment>().ropePos != null && Mathf.Abs(p.GetComponent<CaveSegment>().exitPoints[0].position.y - p.GetComponent<CaveSegment>().exitPoints[1].position.y) > 1) {
            //  checks if a rope is needed
            var r = Instantiate(ropePrefab.gameObject);
            r.transform.position = p.GetComponent<CaveSegment>().ropePos.position;
            ropes.Add(r.GetComponent<RopeInstance>());
        }


        //  checks if there needs to be more segments in this path
        if(length > 0) {
            for(int i = 1; i < pcs.exitPoints.Count; i++)
                generatePath(p.gameObject, pcs.exitPoints[i], length - 1);
        }

        //  if the path has ended, spawn a bouldy
        else {
            //  spawns bouldy
            for(int i = 1; i < pcs.exitPoints.Count; i++) {
                var b = Instantiate(boulders[Random.Range(0, boulders.Count)], pcs.exitPoints[i]);
                b.transform.position = p.transform.GetChild(i).position + new Vector3(0.0f, 2f, 0.0f);
            }
        }
    }

    public Vector3 getRandomCavePos() {
        return createdPieces[Random.Range(0, createdPieces.Count)].transform.position;
    }
    public List<GameObject> getSegments() {
        return createdPieces;
    }*/
}
