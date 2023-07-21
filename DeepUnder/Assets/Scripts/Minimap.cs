using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Minimap : MonoBehaviour {
    GameObject map, pDot;
    [SerializeField] GameObject playerDot, monsterDot;
    [SerializeField] GameObject thing;
    [SerializeField] Transform holder;
    [SerializeField] float yOffset;

    [SerializeField] Material mipMat;
    [SerializeField] float minimapScale = .01f;
    [SerializeField] float transparentCutoff = 1f;

    float dotNormScale;
    bool showing = false;

    Vector3 startingPos;

    List<GameObject> foundMonsters = new List<GameObject>();
    List<GameObject> monsterDots = new List<GameObject>();

    GameObject playerObj;

    public void setup(GameObject cave) {
        StartCoroutine(realSetup(cave));
    }
    IEnumerator realSetup(GameObject cave) {
        yield return new WaitForSeconds(.1f);
        //      CREATES THE MAP
        map = Instantiate(cave.gameObject, holder);

        //  corrects position and scale
        //map.transform.localPosition = Vector3.zero;
        map.transform.localScale = new Vector3(minimapScale, minimapScale, minimapScale);

        foreach(var i in map.GetComponentsInChildren<Renderer>()) {
            i.material = mipMat;    //  changes the material
            i.gameObject.layer = gameObject.layer;  //  changes the layer
            if(i.GetComponent<Collider>() != null)
                i.GetComponent<Collider>().enabled = false; //  removes the collider
        }


        //  CREATES THE DOTS
        //  player dot
        playerObj = GameObject.FindGameObjectWithTag("Player");
        pDot = Instantiate(playerDot.gameObject, holder);
        var pPos = playerObj.transform.position;
        pDot.transform.localPosition = pPos * minimapScale;
        startingPos = pDot.transform.localPosition;
        pDot.transform.localScale = new Vector3(minimapScale, minimapScale, minimapScale) * 2f;
        dotNormScale = minimapScale * 2f;
        StartCoroutine(dotAnim(pDot.gameObject));
        toggleMinimap(false);
    }

    private void LateUpdate() {
        if(showing) {
            movePlayerDot();
            moveMonsterDots();
            var target = pDot.transform.localPosition - startingPos;
            var realTarget = new Vector3(-target.x, -target.z, yOffset);
            transform.localPosition = realTarget;

        }
    }

    void movePlayerDot() {
        var pPos = playerObj.transform.position;
        pDot.transform.localPosition = pPos * minimapScale;
        pDot.transform.localRotation = playerObj.transform.localRotation;
    }
    void moveMonsterDots() {
        for(int i = 0; i < foundMonsters.Count; i++) {
            monsterDots[i].transform.localPosition = foundMonsters[i].transform.position * minimapScale;
        }
    }

    public void toggleMinimap(bool b) {
        holder.gameObject.SetActive(b);
        showing = b;
    }

    IEnumerator dotAnim(GameObject dot) {
        dot.transform.DOScale(dotNormScale * 1.5f, .15f);
        yield return new WaitForSeconds(.35f);
        dot.transform.DOScale(dotNormScale, .15f);
        yield return new WaitForSeconds(.35f);
    }



    //  found funcs
    public void foundMonster(GameObject monster) {
        //  checks if the player has already found this monster before
        if(foundMonsters.Contains(monster.gameObject))
            return;
        //  creates a dot for the monster
        var md = Instantiate(monsterDot.gameObject, holder);
        StartCoroutine(dotAnim(md.gameObject));
        //  adds the monster to found monsters
        foundMonsters.Add(monster.gameObject);
        monsterDots.Add(md.gameObject);
    }
}
