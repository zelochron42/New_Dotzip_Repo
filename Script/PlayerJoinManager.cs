using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerJoinManager : MonoBehaviourPunCallbacks
{
    [SerializeField] PlayerRoleManager roleManager;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ClientReady() {
        Player thisPlayer = PhotonNetwork.LocalPlayer;
        PhotonView view = PhotonView.Get(roleManager);
        view.RPC("PlayerReady", PhotonNetwork.MasterClient, thisPlayer);
    }
}
