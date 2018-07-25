using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;

public class Trial : MonoBehaviour
{
    private bool start;
    public float trials;
    public static long totalTime = 0;
    public Animator anim;
    public Animator car;
    public static Stack<int> Pattern = new Stack<int>();
    public Stack<float> times = new Stack<float>();
    public Stopwatch timer;
    public static int correct = 0;
    public static long totalTrials;
    public int answer;
    public bool wait = false;
    private void Start()
    {
        start = false;
        for (int i = 0; i < 20; i++)
        {
            Pattern.Push(1);
            Pattern.Push(3);
            Pattern.Push(4);
            Pattern.Push(2);
        }
        trials = Pattern.Count;
        totalTrials = Pattern.Count;
    }
    void Update()
    {
        if (wait)
        {
            if (Input.GetKeyDown("d") || Input.GetKeyDown("f") || Input.GetKeyDown("j") || Input.GetKeyDown("k"))
            {
                timer.Stop();
                wait = false;

                if (Pattern.Count != 0)
                {
                    if (Input.GetKeyDown("d") && (answer != 1))
                    {
                        car.SetInteger("Key", 1);
                        StartCoroutine(wrong());
                    }
                    else if (Input.GetKeyDown("f") && (answer != 2))
                    {
                        car.SetInteger("Key", 2);
                        StartCoroutine(wrong());
                    }
                    else if (Input.GetKeyDown("j") && (answer != 3))
                    {
                        car.SetInteger("Key", 3);
                        StartCoroutine(wrong());
                    }
                    else if (Input.GetKeyDown("k") && (answer != 4))
                    {
                        car.SetInteger("Key", 4);
                        StartCoroutine(wrong());
                    }
                    else if (Input.GetKeyDown("d") && (answer == 1))
                    {
                        car.SetInteger("Key", 1);
                        correct = correct + 1;
                        StartCoroutine(StandBy());
                    }
                    else if (Input.GetKeyDown("f") && (answer == 2))
                    {
                        car.SetInteger("Key", 2);
                        correct = correct + 1;
                        StartCoroutine(StandBy());
                    }
                    else if (Input.GetKeyDown("j") && (answer == 3))
                    {
                        car.SetInteger("Key", 3);
                        correct = correct + 1;
                        StartCoroutine(StandBy());
                    }
                    else if (Input.GetKeyDown("k") && (answer == 4))
                    {
                        car.SetInteger("Key", 4);
                        correct = correct + 1;
                        StartCoroutine(StandBy());
                    }
                }

                else if (Pattern.Count == 0)
                {
                    if (Input.GetKeyDown("d") && (answer != 1))
                    {
                        car.SetInteger("Key", 1);
                        StartCoroutine(wrongend());
                    }
                    else if (Input.GetKeyDown("f") && (answer != 2))
                    {
                        car.SetInteger("Key", 2);
                        StartCoroutine(wrongend());
                    }
                    else if (Input.GetKeyDown("j") && (answer != 3))
                    {
                        car.SetInteger("Key", 3);
                        StartCoroutine(wrongend());
                    }
                    else if (Input.GetKeyDown("k") && (answer != 4))
                    {
                        car.SetInteger("Key", 4);
                        StartCoroutine(wrongend());
                    }
                    else if (Input.GetKeyDown("d") && (answer == 1))
                    {
                        car.SetInteger("Key", 1);
                        StartCoroutine(end());
                    }
                    else if (Input.GetKeyDown("f") && (answer == 2))
                    {
                        car.SetInteger("Key", 2);
                        StartCoroutine(end());
                    }
                    else if (Input.GetKeyDown("j") && (answer == 3))
                    {
                        car.SetInteger("Key", 3);
                        StartCoroutine(end());
                    }
                    else if (Input.GetKeyDown("k") && (answer == 4))
                    {
                        car.SetInteger("Key", 4);
                        StartCoroutine(end());
                    }
                }

            }
        }
        else if ((!start) && (Input.anyKey))
        {
            start = true;
            StartCoroutine(starting());
        }
    }




    IEnumerator StandBy()
    {
        yield return new WaitForSecondsRealtime(0.25f);
        anim.SetBool("press", true);
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        totalTime = totalTime + timer.ElapsedMilliseconds;
        timer = new Stopwatch();
        yield return new WaitForSecondsRealtime(2f);
        anim.SetBool("press", false);
        anim.SetInteger("Key", Pattern.Pop());
        answer = anim.GetInteger("Key");
        yield return new WaitForSecondsRealtime(.1f);
        wait = true;
        timer.Start();

    }
    IEnumerator wrong()
    {
        yield return new WaitForSecondsRealtime(0.25f);
        car.SetInteger("Key", answer);
        anim.SetBool("press", true);
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        totalTime = totalTime + timer.ElapsedMilliseconds;
        timer = new Stopwatch();
        yield return new WaitForSecondsRealtime(2f);
        anim.SetBool("press", false);
        anim.SetInteger("Key", Pattern.Pop());
        answer = anim.GetInteger("Key");
        yield return new WaitForSecondsRealtime(.1f);
        wait = true;
        timer.Start();
    }
    IEnumerator starting()
    {
        timer = new Stopwatch();
        yield return new WaitForSecondsRealtime(1);
        anim.SetInteger("Key", Pattern.Pop());
        answer = anim.GetInteger("Key");
        yield return new WaitForSecondsRealtime(.1f);
        wait = true;
        timer.Start();
    }
    IEnumerator end()
    {
        totalTime = totalTime + timer.ElapsedMilliseconds;
        correct = correct + 1;
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        UnityEngine.Debug.Log((correct / trials) * 100 + "% correct");
        UnityEngine.Debug.Log((totalTime / totalTrials) + " = Average response time");
        yield return new WaitForSecondsRealtime(0.25f);
        anim.SetBool("press", true);
        yield return new WaitForSecondsRealtime(1);
        UnityEngine.Debug.Log("YOU WIN");
        DestroyObject(this.gameObject);
    }
    IEnumerator wrongend()
    {
        yield return new WaitForSecondsRealtime(.25f);
        car.SetInteger("Key", answer);
        totalTime = totalTime + timer.ElapsedMilliseconds;
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        UnityEngine.Debug.Log((correct / trials) * 100 + "% correct");
        UnityEngine.Debug.Log((totalTime / totalTrials) + " = Average response time");
        yield return new WaitForSecondsRealtime(0.25f);
        anim.SetBool("press", true);
        yield return new WaitForSecondsRealtime(1);
        UnityEngine.Debug.Log("YOU WIN");
        DestroyObject(this.gameObject);
    }
}

