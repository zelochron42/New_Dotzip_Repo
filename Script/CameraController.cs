using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
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
    }


    private void Update() {
        float step = 5;

        var cameraPosition = Camera.main.gameObject.transform.position;
        cameraPosition.x += step;
        Camera.main.gameObject.transform.position = cameraPosition;
    }    
}

