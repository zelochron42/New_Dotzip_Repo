using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameTag : MonoBehaviourPun
{
    [SerializeField] private TMPro.TextMeshProUGUI nameText;
    private void Start()
    {
        if (photonView.IsMine) {return;}
        SetName();
    }
private void SetName()
{
    nameText.text = photonView.Owner.NickName;
}
    // Update is called once per frame
    void Update()
    {
        
    }
}
