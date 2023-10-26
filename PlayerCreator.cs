using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerCreator : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject PlayerPrefab;

    public override void OnJoinedRoom() {
        base.OnJoinedRoom();
        GameObject thisPlayer = PhotonNetwork.Instantiate(PlayerPrefab.name, PlayerPrefab.transform.position, PlayerPrefab.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
