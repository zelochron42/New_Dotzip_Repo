using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

//this code ensures that players are only controlled by their owner client, and not by all other players
public class PlayerScript : MonoBehaviourPunCallbacks
{
   
   
    //public static GameObject LocalPlayerInstance;

    void Awake()
    {
        if (!photonView.IsMine)
        {
            PlayerMovement pm = GetComponent<PlayerMovement>();
            if (pm)
                Destroy(pm);
            PlayerHealthStatus phs = GetComponent<PlayerHealthStatus>();
            if (phs)
                Destroy(phs);
        }
        DontDestroyOnLoad(gameObject);
    }

    [PunRPC]
    void DisableSelf() {
        gameObject.SetActive(false);
    }

    void Update()
    {
        
    }
}
