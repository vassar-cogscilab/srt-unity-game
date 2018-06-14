using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;

public class NoAnimationsTrial : MonoBehaviour {
    public bool waiting = false;
    public bool loading = false;
    public bool started = false;
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
    public static long totalTrials;
    public int answer;
    public int correct = 0;
    public Stopwatch timer;

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
        answer = Pattern.Pop();
        trials = Pattern.Count;
        totalTrials = Pattern.Count;
        timer = new Stopwatch();
        anim.SetInteger("Key", answer);
    }
    // Update is called once per frame
    void Update () {
        float step = speed * Time.deltaTime;
        if (Pattern.Count != 0)
        {
            if (!waiting && !loading && started)
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
            else if ((waiting) && (!loading) && started)
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
            else if ((!waiting) && (loading) && started)
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
            else if (!started)
            {
                obstacles.transform.position = Vector2.MoveTowards(obstacles.transform.position, midPoint.position, step);
                if (obstacles.transform.position.y == midPoint.position.y)
                {
                    started = true;
                }
                else if ((obstacles.transform.position.y <= 4) && timer.ElapsedMilliseconds == 0)
                    {
                        timer.Start();
                    }
            }
        }
        else
        {

        }

	}
    void correctAnswer()
    {
        correct += 1;
        waiting = true;
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        timer = new Stopwatch();
    }
    void wrongAnswer()
    {
        waiting = true;
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        StartCoroutine(wait());

    }
    void earlyCorrectAnswer()
    {
        correct += 1;
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        waiting = true;
        loading = false;
        timer = new Stopwatch();
    }
    void earlyWrongAnswer()
    {
        waiting = true;
        loading = false;
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        StartCoroutine(wait());

    }
    IEnumerator wait()
    {
        yield return new WaitForSecondsRealtime(.25f);
        car.SetInteger("Key", answer);
        timer = new Stopwatch();
    }
}
