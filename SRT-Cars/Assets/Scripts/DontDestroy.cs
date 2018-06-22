using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour {

	void Awake () {
        DontDestroyOnLoad(this.gameObject);
    }
    private void Update()
    {
        if (GameObject.FindGameObjectsWithTag("progress").Length > 1)
        {
            Destroy(GameObject.FindGameObjectsWithTag("progress")[1]);
        }
        if (GameObject.FindGameObjectsWithTag("music").Length > 1)
        {
            Destroy(GameObject.FindGameObjectsWithTag("music")[1]);
        }
    }

}
