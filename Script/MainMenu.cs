using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// reference: https://youtu.be/zc8ac_qUXQY?si=agBGmtx-BAxKd1zy
public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        Debug.Log("Thanks for Playing!");
        Application.Quit();
    } 
}
