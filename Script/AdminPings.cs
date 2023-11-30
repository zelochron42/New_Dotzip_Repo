using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// script for creating admin pings wherever the user clicks
/// currently usable by all users, need to update to make it only usable by admins later in development
/// </summary>

public class AdminPings : MonoBehaviourPunCallbacks {
    [SerializeField] GameObject[] pingObjects;
    [SerializeField] GameObject currentPingObject;
    [SerializeField] float pingCooldown = 1f;
    float timeSincePing = 0f;
    Camera mainCam;
    UIManager gameOverCanvas;
    void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) {
            currentPingObject = pingObjects[0];
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            currentPingObject = pingObjects[1];
        }
        if (!gameOverCanvas) {
            gameOverCanvas = FindObjectOfType<UIManager>();
            if (gameOverCanvas) {
                gameOverCanvas.gameObject.SetActive(false);
            }
        }

        timeSincePing += Time.deltaTime;
        if (Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject() && timeSincePing > pingCooldown) {
            timeSincePing = 0f;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject newPing = PhotonNetwork.Instantiate(currentPingObject.name, mousePos, currentPingObject.transform.rotation);

        }
    }
    public void SetPingObject(GameObject newObject) {
        currentPingObject = newObject;
    }
}
