using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] bool debugMode = false;
    [SerializeField] float adminCamSize = 10f;
    // Start is called before the first frame update
    void Start()
    {
        Camera cam = GetComponent<Camera>();
        if (!cam)
            return;
        if (FindObjectOfType<AdminPings>()) {
            cam.orthographicSize = adminCamSize;
        }
        else if (!debugMode) {
            Animator anim = GetComponent<Animator>();
            if (anim)
                anim.enabled = false;
        }
    }


    private void Update() {
        /*float step = 5;

        var cameraPosition = Camera.main.gameObject.transform.position;
        cameraPosition.x += step;
        Camera.main.gameObject.transform.position = cameraPosition;*/
    }    
}

