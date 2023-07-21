using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour {
    [SerializeField] float mouseSens = 100f;
    [SerializeField] Transform playerBody, minimapTransform;

    bool looking = true;
    bool lockLooking = false;

    float xRot = 0f;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        if(lockLooking)
            return;
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        if(looking)
            xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -85f, 85f);

        playerBody.Rotate(Vector3.up * mouseX);
        //minimapTransform.Rotate(Vector3.forward * mouseX);
        transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }

    public bool toggleLooking() {
        looking = !looking;
        return looking;
    }

    public void setLockLooking(bool b) {
        lockLooking = b;
    }
}
