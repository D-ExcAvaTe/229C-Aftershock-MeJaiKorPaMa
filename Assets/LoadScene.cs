using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    private void Start()
    {
        
        Cursor.lockState = CursorLockMode.None; 
    }

    public void LoadScenes()
    {
        SceneManager.LoadScene(1);
    }
}
