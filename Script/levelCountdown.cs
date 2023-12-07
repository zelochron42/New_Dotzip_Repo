using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

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
        addtime();
    }

    public void addtime()
    {
        //add a point every second as the scene is loaded
        timeCount += 1 * Time.deltaTime;
        timeCountText.text = "Current Time: " + timeCount.ToString("0.0");
    }
}