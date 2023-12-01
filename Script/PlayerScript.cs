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
                Destroy(pm);
            PlayerHealthStatus phs = GetComponent<PlayerHealthStatus>();
            if (phs)
                Destroy(phs);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        
    }
}
