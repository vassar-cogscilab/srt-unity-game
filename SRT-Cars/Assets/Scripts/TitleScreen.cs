using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour {
    public GUIStyle guiStyle;
    public int level;
    private void Awake()
    {
        
        guiStyle.fontSize = 20;
        
    }
    private void OnGUI()
    {
        new Rect(Screen.width / 2, Screen.height / 2, Screen.width / 4, Screen.height / 4);

        if (GUI.Button(new Rect(10, 10, 100, 50), "Play", guiStyle))
        {
            SceneManager.LoadScene(level);
        }
    }
}
