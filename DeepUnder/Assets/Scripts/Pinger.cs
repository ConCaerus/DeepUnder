using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pinger : MonoBehaviour {
    InputMaster controls;
    [SerializeField] GameObject pingObj;
    [SerializeField] Transform pingHolder;

    Queue<GameObject> pings = new Queue<GameObject>();
    Queue<GameObject> activePings = new Queue<GameObject>();

    float maxPingSize = 100f;
    [SerializeField] float maxPingLifetime = 2f;
    int pingPoolCount = 10;    //  pp count

    Coroutine cooldown = null;

    private void Start() {
        controls = new InputMaster();
        controls.Enable();
        controls.Player.Ping.performed += ctx => ping();

        //  does the thing
        for(int i = 0; i < pingPoolCount; i++) {
            var p = Instantiate(pingObj.gameObject, transform.parent);  //  lets it be parented close so that when it pings it gets unparented
            p.transform.localPosition = Vector3.zero;
            pings.Enqueue(p);
        }
    }

    private void OnDisable() {
        controls.Disable();
    }

    void ping() {
        if(cooldown != null)
            return;

        var obj = pings.Dequeue();
        activePings.Enqueue(obj);
        obj.transform.parent = pingHolder;
        obj.transform.DOScale(maxPingSize, maxPingLifetime);
        var sc = obj.GetComponent<SphereCollider>();

        float temp = 0f;
        DOTween.To(() => temp, x => temp = x, maxPingSize, maxPingLifetime).OnUpdate(() => { //  increases the radius of the collider of the ping
            sc.radius = temp;
        });


        var mat = obj.GetComponent<Renderer>().material;
        temp = 1f;
        DOTween.To(() => temp, x => temp = x, 0f, maxPingLifetime).OnUpdate(() => { //  decreases the alpha over the lifetime of the ping
            mat.SetFloat("_alpha", temp);
        });
        Invoke("returnPingHome", maxPingLifetime);
        cooldown = StartCoroutine(pingCooldown());
    }

    IEnumerator pingCooldown() {
        yield return new WaitForSeconds(2.0f);
        cooldown = null;
    }

    void returnPingHome() {
        var obj = activePings.Dequeue();
        obj.transform.localScale = Vector3.zero;
        obj.transform.parent = transform.parent;
        obj.transform.localPosition = Vector3.zero;
        obj.GetComponent<SphereCollider>().radius = 0f;
        pings.Enqueue(obj);
    }
}
