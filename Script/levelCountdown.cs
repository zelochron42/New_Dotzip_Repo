using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using Photon.Pun;
using Photon.Realtime;

public class levelCountdown : MonoBehaviourPunCallbacks, IPunObservable
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
                //add a point every second as the scene is loaded
                timeCount += 1 * Time.deltaTime;

            //commented out RPC code to try OnPhotonSerializeView instead
            //photonView.RPC("addtime", RpcTarget.All, timeCount);
    }
    void Update()
    {
        timeCountText.text = "Current Time: " + timeCount.ToString("0.0");
        if (timeCount >= 130f) {
            RoundScorer rs = FindObjectOfType<RoundScorer>();
            if (!rs)
                return;
            PhotonView pv = rs.GetComponent<PhotonView>();
            if (!pv)
                return;
            pv.RPC("RoundOver", PhotonNetwork.MasterClient);
        }
    }


    /*[PunRPC]
    public void addtime(float timeCount)
    {
        timeCountText.text = "Current Time: " + timeCount.ToString("0.0");
    }*/

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(timeCount);
        }
        else if (stream.IsReading) {
            timeCount = (float)stream.ReceiveNext();
        }
    }
}