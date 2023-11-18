using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// reference: https://youtu.be/zc8ac_qUXQY?si=agBGmtx-BAxKd1zy
public class MainMenu : MonoBehaviour
{
    [SerializeField] string levelScene;
    public void StartGame()
    {
        SceneManager.LoadScene(levelScene);
    }

    public void ExitGame()
    {
        Debug.Log("Thanks for Playing!");
        Application.Quit();
    } 
}
