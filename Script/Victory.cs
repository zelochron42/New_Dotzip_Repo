using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour
{
        [SerializeField] string levelScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("You Win");
              SceneManager.LoadScene(levelScene);

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
