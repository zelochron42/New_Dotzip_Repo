using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using Photon.Pun;
using Photon.Realtime;

public class levelCountdown : MonoBehaviour
{

    public TextMeshProUGUI timeCountText;
    private float timeCount = 0;

    private void Start()
    {
        //set the time text in the UI
        timeCountText.text = "Current Time: " + timeCount.ToString("0.0");
    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonView photonView = GetComponent<PhotonView>();
            photonView.RPC("addtime", RpcTarget.All, timeCount);
        }
    }

    [PunRPC]
    public void addtime(float timeCount)
    {
        //add a point every second as the scene is loaded
        timeCount += 1 * Time.deltaTime;
        timeCountText.text = "Current Time: " + timeCount.ToString("0.0");
    }
}