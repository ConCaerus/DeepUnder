using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingCollider : MonoBehaviour {
    Minimap mm;

    private void OnTriggerEnter(Collider col) {
        if(col.gameObject.tag == "Monster") {
            mm.foundMonster(col.gameObject);
        }
    }

    private void Start() {
        mm = FindAnyObjectByType<Minimap>();
    }
}
