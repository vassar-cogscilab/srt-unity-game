using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;

public class NoAnimationsTrial : MonoBehaviour {
    public bool waiting = false;
    public bool loading = false;
    public bool running = false;
    private int d = 1;
    private int f = 2;
    private int j = 3;
    private int k = 4;
    private int appearance = 4;
    public GameObject obstacles;
    public Transform endPoint;
    public Transform startPoint;
    public Transform midPoint;
    public float speed = 5;

    public Animator car;
    public Animator anim;
    public static Stack<int> Pattern = new Stack<int>();
    public float trials;
    public static long totalTrials = 0;
    public int answer;
    public int correct = 0;
    public Stopwatch timer;
    public static long totalTime = 0;

    void Start () {
		
	}
    private void Awake()
    {
        obstacles.transform.position = startPoint.position;
        for (int i = 0; i < 5; i++)
        {
            Pattern.Push(d);
            Pattern.Push(j);
            Pattern.Push(k);
            Pattern.Push(f);
        }
        trials = Pattern.Count;
        totalTrials = Pattern.Count;
        answer = Pattern.Pop();
        timer = new Stopwatch();
        anim.SetInteger("Key", answer);
    }
    // Update is called once per frame
    void Update () {
        float step = speed * Time.deltaTime;
        if (Pattern.Count != 0)
        {
            if (!waiting && !loading && running)
            {
                if (((answer == d) && Input.GetKeyDown("d")) || ((answer == f) && Input.GetKeyDown("f")) || ((answer == j) && Input.GetKeyDown("j")) || ((answer == k) && Input.GetKeyDown("k")))
                {
                    timer.Stop();
                    car.SetInteger("Key", answer);
                    correctAnswer();
                }
                else if (Input.GetKeyDown("d"))
                {
                    timer.Stop();
                    car.SetInteger("Key", d);
                    wrongAnswer();
                }
                else if (Input.GetKeyDown("f"))
                {
                    timer.Stop();
                    car.SetInteger("Key", f);
                    wrongAnswer();
                }
                else if (Input.GetKeyDown("j"))
                {
                    timer.Stop();
                    car.SetInteger("Key", j);
                    wrongAnswer();
                }
                else if (Input.GetKeyDown("k"))
                {
                    timer.Stop();
                    car.SetInteger("Key", k);
                    wrongAnswer();
                }
            }
            else if ((waiting) && (!loading) && running)
            {
                obstacles.transform.position = Vector2.MoveTowards(obstacles.transform.position, endPoint.position, step);
                if (obstacles.transform.position.y == endPoint.position.y)
                {
                    waiting = false;
                    loading = true;
                    answer = Pattern.Pop();
                    anim.SetInteger("Key", answer);
                    obstacles.transform.position = startPoint.position;
                }
            }
            else if ((!waiting) && (loading) && running)
            {
                obstacles.transform.position = Vector2.MoveTowards(obstacles.transform.position, midPoint.position, step);
                if ((obstacles.transform.position.y <= appearance) && timer.ElapsedMilliseconds == 0)
                {
                    timer.Start();
                }
                if (transform.position.y <= appearance)
                {
                    if (((answer == d) && Input.GetKeyDown("d")) || ((answer == f) && Input.GetKeyDown("f")) || ((answer == j) && Input.GetKeyDown("j")) || ((answer == k) && Input.GetKeyDown("k")))
                    {
                        timer.Stop();
                        car.SetInteger("Key", answer);
                        earlyCorrectAnswer();
                    }
                
                    else if (Input.GetKeyDown("d"))
                    {
                        timer.Stop();
                        car.SetInteger("Key", d);
                        earlyWrongAnswer();
                    }
                    else if (Input.GetKeyDown("f"))
                    {
                        timer.Stop();
                        car.SetInteger("Key", f);
                        earlyWrongAnswer();
                    }
                    else if (Input.GetKeyDown("j"))
                    {
                        timer.Stop();
                        car.SetInteger("Key", j);
                        earlyWrongAnswer();
                    }
                    else if (Input.GetKeyDown("k"))
                    {
                        timer.Stop();
                        car.SetInteger("Key", k);
                        earlyWrongAnswer();
                    }
                }
            }
            else if (!running)
            {
                running = true;
                loading = true;
            }
        }
        else
        {
            if (!waiting && !loading && running)
            {
                if (((answer == d) && Input.GetKeyDown("d")) || ((answer == f) && Input.GetKeyDown("f")) || ((answer == j) && Input.GetKeyDown("j")) || ((answer == k) && Input.GetKeyDown("k")))
                {
                    timer.Stop();
                    car.SetInteger("Key", answer);
                    endCorrectAnswer();
                }
                else if (Input.GetKeyDown("d"))
                {
                    timer.Stop();
                    car.SetInteger("Key", d);
                    endWrongAnswer();
                }
                else if (Input.GetKeyDown("f"))
                {
                    timer.Stop();
                    car.SetInteger("Key", f);
                    endWrongAnswer();
                }
                else if (Input.GetKeyDown("j"))
                {
                    timer.Stop();
                    car.SetInteger("Key", j);
                    endWrongAnswer();
                }
                else if (Input.GetKeyDown("k"))
                {
                    timer.Stop();
                    car.SetInteger("Key", k);
                    endWrongAnswer();
                }
            }
            else if ((waiting) && (!loading) && running)
            {
                obstacles.transform.position = Vector2.MoveTowards(obstacles.transform.position, endPoint.position, step);
                if (obstacles.transform.position.y == endPoint.position.y)
                {
                    waiting = false;
                }
            }
            else if ((!waiting) && (loading) && running)
            {
                obstacles.transform.position = Vector2.MoveTowards(obstacles.transform.position, midPoint.position, step);
                if ((obstacles.transform.position.y <= appearance) && timer.ElapsedMilliseconds == 0)
                {
                    timer.Start();
                }
                if (transform.position.y <= appearance)
                {
                    if (((answer == d) && Input.GetKeyDown("d")) || ((answer == f) && Input.GetKeyDown("f")) || ((answer == j) && Input.GetKeyDown("j")) || ((answer == k) && Input.GetKeyDown("k")))
                    {
                        timer.Stop();
                        car.SetInteger("Key", answer);
                        endEarlyCorrectAnswer();
                    }

                    else if (Input.GetKeyDown("d"))
                    {
                        timer.Stop();
                        car.SetInteger("Key", d);
                        endEarlyWrongAnswer();
                    }
                    else if (Input.GetKeyDown("f"))
                    {
                        timer.Stop();
                        car.SetInteger("Key", f);
                        endEarlyWrongAnswer();
                    }
                    else if (Input.GetKeyDown("j"))
                    {
                        timer.Stop();
                        car.SetInteger("Key", j);
                        endEarlyWrongAnswer();
                    }
                    else if (Input.GetKeyDown("k"))
                    {
                        timer.Stop();
                        car.SetInteger("Key", k);
                        endEarlyWrongAnswer();
                    }
                }
            }
            else if (!running)
            {
                obstacles.transform.position = Vector2.MoveTowards(obstacles.transform.position, endPoint.position, step);

            }
        }

	}
    void correctAnswer()
    {
        correct += 1;
        totalTime = totalTime + timer.ElapsedMilliseconds;
        waiting = true;
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        timer = new Stopwatch();
    }
    void wrongAnswer()
    {
        waiting = true;
        totalTime = totalTime + timer.ElapsedMilliseconds;
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        StartCoroutine(wait());

    }
    void earlyCorrectAnswer()
    {
        correct += 1;
        totalTime = totalTime + timer.ElapsedMilliseconds;
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        waiting = true;
        loading = false;
        timer = new Stopwatch();
    }
    void earlyWrongAnswer()
    {
        waiting = true;
        loading = false;
        totalTime = totalTime + timer.ElapsedMilliseconds;
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        StartCoroutine(wait());

    }
    void endCorrectAnswer()
    {
        correct += 1;
        waiting = true;
        totalTime = totalTime + timer.ElapsedMilliseconds;
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        UnityEngine.Debug.Log((correct / trials) * 100 + "% correct");
        UnityEngine.Debug.Log((totalTime / totalTrials) + " = Average response time");
        running = false;
    }
    void endWrongAnswer()
    {
        waiting = true;
        totalTime = totalTime + timer.ElapsedMilliseconds;
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        StartCoroutine(wait());
        UnityEngine.Debug.Log((correct / trials) * 100 + "% correct");
        UnityEngine.Debug.Log((totalTime / totalTrials) + " = Average response time");
        running = false;

    }
    void endEarlyCorrectAnswer()
    {
        correct += 1;
        totalTime = totalTime + timer.ElapsedMilliseconds;
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        UnityEngine.Debug.Log((correct / trials) * 100 + "% correct");
        UnityEngine.Debug.Log((totalTime / totalTrials) + " = Average response time");
        waiting = true;
        loading = false;
        running = false;
    }
    void endEarlyWrongAnswer()
    {
        waiting = true;
        loading = false;
        totalTime = totalTime + timer.ElapsedMilliseconds;
        StartCoroutine(wait());
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        UnityEngine.Debug.Log((correct / trials) * 100 + "% correct");
        UnityEngine.Debug.Log((totalTime / totalTrials) + " = Average response time");
        running = false;

    }
    IEnumerator wait()
    {
        yield return new WaitForSecondsRealtime(.25f);
        car.SetInteger("Key", answer);
        timer = new Stopwatch();
    }
}
