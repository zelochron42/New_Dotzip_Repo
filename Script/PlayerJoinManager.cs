using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerJoinManager : MonoBehaviour
{
    [SerializeField] PlayerRoleManager roleManager;
    PhotonView view;
    Player thisPlayer;
    void Start()
    {
        thisPlayer = PhotonNetwork.LocalPlayer;
        view = PhotonView.Get(roleManager);
    }

    public void ClientReady() {
        
        view.RPC("PlayerReady", RpcTarget.All, thisPlayer);
    }

    private void OnApplicationQuit() {
        view.RPC("PlayerUnready", RpcTarget.All, thisPlayer);
    }
}
