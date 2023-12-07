using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Realtime;
using Photon.Pun;

//Reference to this script: https://www.youtube.com/watch?v=PkNRPOrtyls 
public class Powerup_ALT : MonoBehaviour
{
    public PowerupEffect powerupEffect;
    PhotonView pv;
    private void Awake() {
        pv = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        ApplyEffect(go);
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        GameObject go = collision.gameObject;
        ApplyEffect(go);
    }

    void ApplyEffect(GameObject go) {
        if (!go.CompareTag("Player"))
            return;
        PhotonView ppv = go.GetComponent<PhotonView>();
        if (!ppv)
            return;
        Player plr = ppv.Owner;
        if (PhotonNetwork.LocalPlayer != plr)
            return;
        powerupEffect.Apply(go);
        pv.RPC("NetworkRemove", pv.Owner);
    }
}
