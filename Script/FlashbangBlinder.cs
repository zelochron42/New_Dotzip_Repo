using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FlashbangBlinder : MonoBehaviour
{
    [SerializeField] float blindDelay = 2f;
    DisruptionScreen ds;
    void Start()
    {
        ds = FindObjectOfType<DisruptionScreen>();
        if (ds)
            StartCoroutine("BlindRoutine");
    }

    void Update()
    {
        
    }

    IEnumerator BlindRoutine() {
        yield return new WaitForSeconds(blindDelay);
        Vector2 camPos = Camera.main.WorldToViewportPoint(transform.position);
        bool canSee = camPos.x > 0f && camPos.x < 1f && camPos.y > 0f && camPos.y < 1f;
        if (ds && canSee)
            ds.Disrupt();
        yield return null;
        PhotonNetwork.Destroy(gameObject);
        yield break;
    }
}
