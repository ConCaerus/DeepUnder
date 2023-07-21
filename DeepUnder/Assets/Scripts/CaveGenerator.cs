using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CaveGenerator : MonoBehaviour {
    [SerializeField] GameObject playerObj;
    [SerializeField] Transform minimapHolder;
    [SerializeField] List<GameObject> cavePieces = new List<GameObject>();
    [SerializeField] List<GameObject> boulders = new List<GameObject>();
    List<GameObject> createdPieces = new List<GameObject>();

    [SerializeField] List<GameObject> monsters = new List<GameObject>();

    int cavePathLength = 10;
    int minCaveLength = 7;
    int caveCount;

    int minMonsterCount = 3;

    private void Start() {
        generateCave();
        spawnMonsters();
        FindAnyObjectByType<Minimap>().setup(transform.GetChild(0).gameObject); //  sends the cave to the minimap
    }

    void generateCave() {
        caveCount = 1;

        //  spawns the first segment
        int rand = Random.Range(0, cavePieces.Count);
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
        if(caveCount <= minCaveLength) {
            //  destroys everything
            foreach(var i in transform.GetChild(0).GetComponentsInChildren<Renderer>())
                Destroy(i.gameObject);
            createdPieces.Clear();
            generateCave();
        }
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
            //  checks for collision
            if(Physics.CheckCapsule(pcs.exitPoints[i - 1].position, pcs.exitPoints[i].position, 7f, LayerMask.GetMask("Ground"))) {
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

    void spawnMonsters() {
        List<GameObject> usableSegs = new List<GameObject>();
        List<GameObject> spawned = new List<GameObject>();
        var pPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        int mCount = (int)Random.Range(minMonsterCount, caveCount / 2f);
        float minDist = 10f;

        //  finds all of the usable monster poses
        foreach(var i in createdPieces) {
            //  checks if the segment is usable
            if(Vector2.Distance(pPos, i.GetComponent<CaveSegment>().monsterSpawnPos.position) > minDist)
                usableSegs.Add(i.gameObject);
        }

        //  spawns monsters
        for(int i = 0; i < mCount; i++) {
            //  finds a position for the monster
            var segment = usableSegs[Random.Range(0, usableSegs.Count)];
            usableSegs.Remove(segment);

            //  spawns the monster at the position
            var m = Instantiate(monsters[Random.Range(0, monsters.Count)].gameObject, segment.GetComponent<CaveSegment>().monsterSpawnPos);
            m.transform.localPosition = Vector3.zero;
            m.transform.localRotation = Quaternion.identity;
            m.transform.parent = null;

            spawned.Add(m.gameObject);
        }
    }
}
