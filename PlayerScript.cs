using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerScript : MonoBehaviourPunCallbacks
{
   
   
    //public static GameObject LocalPlayerInstance;

    void Awake()
    {
        if (!photonView.IsMine)
        {
            PlayerMovement pm = GetComponent<PlayerMovement>();
            if (pm)
                pm.enabled = false;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        
    }
}
