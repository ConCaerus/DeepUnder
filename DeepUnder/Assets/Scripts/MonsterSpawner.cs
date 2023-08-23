using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour {
    [SerializeField] List<GameObject> monsterPrefabs = new List<GameObject>();

    /*
    public void spawnMonsters() {
        //  creates the thing
        var m = Instantiate(monsterPrefabs[Random.Range(0, monsterPrefabs.Count)]);

        //  positions it as far away from the player as possible
        Vector3 pPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector3 furthest = pPos;
        foreach(var i in GetComponent<CaveGenerator>().getSegments()) {
            if(Vector3.Distance(pPos, i.transform.position) > Vector3.Distance(pPos, furthest)) {
                furthest = i.transform.position;
            }
        }
        m.transform.position = furthest;
    }*/
}
