using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
        private void Update()
{
    float step = 5;

    var cameraPosition = Camera.main.gameObject.transform.position;
    cameraPosition.x += step;
    Camera.main.gameObject.transform.position = cameraPosition;
}
    }

