using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public static GameController control;

    public int score;
    private int multiplier = 10;

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 50), "Score: " + score);
    }
    private void Update()
    {
        score = Trial.correct * multiplier;

    }
}

