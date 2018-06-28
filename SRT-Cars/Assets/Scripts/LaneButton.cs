using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneButton : MonoBehaviour {
    public config cfig;
    public int laneNumber;
    
    private void OnMouseDown()
    {
        cfig.lanePressed[laneNumber] = true;
        StartCoroutine(makeFalse());
    }
    IEnumerator makeFalse()
    {
        yield return new WaitForSecondsRealtime(.25f);
        cfig.lanePressed[laneNumber] = false;
    }
}
