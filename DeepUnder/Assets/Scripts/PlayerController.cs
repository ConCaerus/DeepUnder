using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour {
    InputMaster controls;
    CharacterController cc;

    [SerializeField] Transform groundCheck, wallCheck, camHolder, roofCheck;
    [SerializeField] float relevantDist = 0.4f;

    [SerializeField] float speed;
    [SerializeField] float gravity;
    [SerializeField] float jumpHeight;
    [SerializeField] float crouchSize, standSize, crawlSize;

    Vector3 velocity = Vector3.zero;
    bool isGrounded = false;
    bool canStand = true;
    bool canClimb = false;
    bool crouching = false;
    bool sprinting = false;
    bool canInteract = false;
    bool lookingAtMap = false;

    float horDir, verDir;

    Coroutine climber = null;
    Coroutine stander = null;
    Coroutine mapper = null;

    Quaternion camHolderOriginal;

    private void Start() {
        DOTween.Init();
        cc = GetComponent<CharacterController>();
        camHolderOriginal = camHolder.transform.rotation;
        controls = new InputMaster();
        controls.Enable();
        controls.Player.Jump.performed += ctx => jumpManager();
        controls.Player.Jump.canceled += ctx => { if(climber != null) StopCoroutine(climber); };
        controls.Player.Crouch.performed += ctx => crouch();
        controls.Player.Crouch.canceled += ctx => unCrouch();
        controls.Player.Sprint.performed += ctx => { sprinting = true; };
        controls.Player.Sprint.canceled += ctx => { sprinting = false; };
        controls.Player.Interact.performed += ctx => interact();
        controls.Player.Minimap.performed += ctx => toggleMinimap();
    }

    private void Update() {
        //  checks physics shit
        isGrounded = Physics.CheckSphere(groundCheck.position, relevantDist, LayerMask.GetMask("Ground"));
        canStand = !Physics.CheckSphere(roofCheck.position, relevantDist, LayerMask.GetMask("Ground"));
        canClimb = Physics.CheckSphere(wallCheck.position, relevantDist, LayerMask.GetMask("Climbable"));
        canInteract = Physics.CheckSphere(wallCheck.position, relevantDist, LayerMask.GetMask("Interactable"));

        if(canClimb || (isGrounded && velocity.y < 0f))
            velocity.y = -2f;

        horDir = Input.GetAxis("Horizontal");
        verDir = Input.GetAxis("Vertical");

        var target = transform.right * horDir + transform.forward * verDir;
        var tempSpeed = (crouching || lookingAtMap) ? speed * .35f : (sprinting ? speed * 1.65f : speed);
        cc.Move(target * tempSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }

    private void OnDisable() {
        controls.Disable();
    }


    void jumpManager() {
        if(canClimb)
            climb();
        else
            jump();
    }
    void jump() {
        if(isGrounded && !crouching)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
    void crouch() {
        transform.DOScaleY(crouchSize, .15f);
        crouching = true;
    }
    void unCrouch() {
        if(canStand) {
            transform.DOScaleY(standSize, .25f);
            crouching = false;
        }
        else if(stander == null) {
            stander = StartCoroutine(standWhenCan());
        }
    }
    IEnumerator standWhenCan() {
        while(!canStand)
            yield return new WaitForEndOfFrame();
        unCrouch();
        stander = null;
    }
    void climb() {
        if(climber != null)
            StopCoroutine(climber);
        climber = StartCoroutine(climbAnim());
    }
    IEnumerator climbAnim() {
        while(canClimb) {
            transform.DOMoveY(transform.position.y + 2f, .25f);
            yield return new WaitForSeconds(.35f);
        }
        climber = null;
    }

    void interact() {
        if(canInteract) {
            Debug.Log("interacted");
        }
    }
    void toggleMinimap() {
        if(mapper != null)
            return;
        mapper = StartCoroutine(minimapAnim());
    }

    IEnumerator minimapAnim() {
        FindAnyObjectByType<MouseLook>().setLockLooking(true);
        yield return new WaitForEndOfFrame();
        //  shows the minimap
        if(!FindAnyObjectByType<MouseLook>().toggleLooking()) {
            lookingAtMap = true;
            camHolder.DOComplete();
            camHolder.transform.DORotate(new Vector3(75f, 0f, 0f), .15f, RotateMode.LocalAxisAdd);
            yield return new WaitForSeconds(.25f);
            FindAnyObjectByType<MouseLook>().setLockLooking(false);
            FindAnyObjectByType<Minimap>().toggleMinimap(true);
        }

        //  hides the minimap
        else {
            FindAnyObjectByType<Minimap>().toggleMinimap(false);
            yield return new WaitForSeconds(.15f);
            lookingAtMap = false;
            camHolder.DOComplete();
            camHolder.transform.DORotate(new Vector3(-75f, 0f, 0f), .25f, RotateMode.LocalAxisAdd);
            yield return new WaitForSeconds(.26f);
            FindAnyObjectByType<MouseLook>().setLockLooking(false);
        }
        mapper = null;
    }
}
