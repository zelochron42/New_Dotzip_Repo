using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Victory : MonoBehaviourPunCallbacks
{
    [SerializeField] string levelScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            PhotonView pv = collision.gameObject.GetComponent<PhotonView>();
            if (!pv || pv.Owner != PhotonNetwork.LocalPlayer)
                return;
            pv.RPC("DisableSelf", RpcTarget.All);
            CheckGameover();
        }
        //Debug.Log("You Win");
        //SceneManager.LoadScene(levelScene);

    }

    void CheckGameover() { //checking to make sure all the players have either died or been set inactive before advancing the scene
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players) {
            if (p.activeInHierarchy == true)
                return;
        }
        SceneManager.LoadScene(levelScene); //TODO: add a hard-limit to level time to stop matches from going on indefinitely with one living player
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
