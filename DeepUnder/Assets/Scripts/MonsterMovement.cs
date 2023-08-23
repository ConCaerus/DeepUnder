using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour {
    [SerializeField] float speed, slowSpeed;


    Rigidbody rb;
    Transform playerTrans;
    CaveManager cm;
    Vector3 wanderTarget;

    bool seesPlayer = false;

    [SerializeField] Transform god;

    private void Start() {
        playerTrans = FindFirstObjectByType<PlayerController>().transform;
        rb = GetComponent<Rigidbody>();
        cm = FindFirstObjectByType<CaveManager>();
        setNewWanderTarget();
        StartCoroutine(checkSurroundings());
    }

    private void FixedUpdate() {
        //else if(rb.velocity != Vector3.zero) rb.velocity = Vector3.MoveTowards(rb.velocity, Vector3.zero, slowSpeed * Time.fixedDeltaTime);    //  slow down code
        //  moves towards the player if sees them
        var dir = (seesPlayer ? playerTrans.position : wanderTarget) - transform.position;
        rb.velocity = dir.normalized * speed * Time.fixedDeltaTime;

        //  checks if reached wander target
        if(Vector3.Distance(transform.position, wanderTarget) < .1f)
            setNewWanderTarget();
    }

    void setNewWanderTarget() {
        wanderTarget = god.position;
        return;
        do wanderTarget = cm.getSegments()[Random.Range(0, cm.getSegments().Count)].transform.position;
        while(Vector3.Distance(wanderTarget, transform.position) > 25f);
        wanderTarget.y += 2;
    }

    void findNextPathPoint() {
        var curSeg = cm.getClosestSegment(transform.position);
    }


    IEnumerator checkSurroundings() {
        //  checks if sees player
        bool sawPlayer = seesPlayer;
        seesPlayer = Vector3.Distance(playerTrans.position, transform.position) < 20f && !Physics.CheckCapsule(transform.position, playerTrans.position, .15f, LayerMask.GetMask("Ground"));
        //  NOTE: for realistic shit to happen, the monster needs to have a blind spot (maybe behind it?)

        //  if just now 
        if(!seesPlayer && sawPlayer) {
            setNewWanderTarget();
        }

        yield return new WaitForSeconds(.15f);
        StartCoroutine(checkSurroundings());
    }
}
