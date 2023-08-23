using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeInstance : MonoBehaviour {
    [SerializeField] Transform origin;
    [SerializeField] GameObject rope;

    float ropeRadius = .15f;


    public void dropRope() {
        RaycastHit hit;
        if(Physics.Raycast(origin.position, Vector3.down, out hit, 1000f, LayerMask.GetMask("Ground"))) {
            float length = hit.distance;

            //  checks if the rope is too short
            if(length < 2f) {
                Destroy(gameObject);
                return;
            }

            //  stretches the rope
            rope.transform.localScale = new Vector3(ropeRadius, length / 2f, ropeRadius);

            //  positions the rope
            var moveDist = length / 2f;
            rope.transform.localPosition = new Vector3(0f, -moveDist, 0f);
        }
    }
}
