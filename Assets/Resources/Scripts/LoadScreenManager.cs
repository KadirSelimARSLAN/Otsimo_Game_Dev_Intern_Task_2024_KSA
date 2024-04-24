using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScreenManager : MonoBehaviour
{
    
    void Awake()
    {
        if (SaveManager.ControlLoadFile())
        {
            LoadCanvasScene();
        }
    }

    public void LoadCanvasScene()
    {
        SceneManager.LoadScene(1);
    }
   
}
