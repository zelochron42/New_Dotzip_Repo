using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisruptionScreen : MonoBehaviour
{
    Image img;
    [SerializeField] float blindTime = 1f;
    void Start()
    {
        img = GetComponent<Image>();
    }
    public void Disrupt() {
        StartCoroutine(DisruptionRoutine());
    }
    IEnumerator DisruptionRoutine() {
        img.enabled = true;
        yield return new WaitForSeconds(blindTime);
        img.enabled = false;
        yield break;
    }
}
