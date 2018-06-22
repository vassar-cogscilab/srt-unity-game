using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel2 : MonoBehaviour
{
    private void Start()
    {


        if (GameObject.Find("Progress").GetComponent<Progress>().level2)
        {
            GameObject.Find("Level2Button").SetActive(true);
        }
        else
        {
            GameObject.Find("Level2Button").SetActive(false);
        }

    }

    public void LoadByIndex(int sceneIndex)
    {
            SceneManager.LoadScene(sceneIndex);
    }

}
