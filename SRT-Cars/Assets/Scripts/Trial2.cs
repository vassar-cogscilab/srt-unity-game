using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;

public class Trial2 : MonoBehaviour
{
    public bool waiting = false;
    public bool loading = false;
    public bool running = false;
    private int d = 1;
    private int f = 2;
    private int j = 3;
    private int k = 4;
    private float appearance = 3.5f;
    public GameObject obstacles;
    public GameObject lines;
    public Transform endPoint;
    public Transform startPoint;
    public Transform midPoint;
    public Transform lineStart;
    public Transform lineEnd;
    private float speed = 4;
    public Button startButton;
    public Text responseTime;
    public Text percentCorrect;
    public GameObject endPanel;
    public Animator car;
    public Animator anim;
    public Animator startingAnimation;
    public static Stack<int> Pattern = new Stack<int>();
    public float trials;
    public static long totalTrials = 0;
    public int answer;
    public int correct = 0;
    public Stopwatch timer;
    public static long totalTime = 0;
    public float road = 10;
    private int maxSpeed = 10;
    private int minSpeed = 6;
    private float speedChange = .05f;

    void Start()
    {

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
        startButton.onClick.AddListener(beginLevel);
    }
    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        float lineStep = road * Time.deltaTime;

        lines.transform.position = Vector2.MoveTowards(lines.transform.position, lineEnd.position, lineStep);
        if(lines.transform.position.y == lineEnd.position.y)
        {
            lines.transform.position = lineStart.position;
        }
        if (Pattern.Count != 0)
        {
            if ((waiting) && (!loading) && running)
            {
                //road.SetBool("Speeding", true);
                if (road < maxSpeed)
                {
                    road += speedChange;
                    speed += speedChange;
                }
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
                //if(obstacles.transform.position.y == midPoint.position.y)
                //{
                // road.SetBool("Speeding", false);
                if (road > minSpeed)
                {
                    road -= speedChange;
                    speed -= speedChange;
                }
                // }
                if (obstacles.transform.position.y <= appearance)
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
        }
        else
        {
            if (!waiting && !loading && running)
            {
                // road.SetBool("Speeding", false);
                if (road > minSpeed)
                {
                    road -= speedChange;
                    speed -= speedChange;
                }
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
                //road.SetBool("Speeding", true);
                if (road < maxSpeed)
                {
                    road += speedChange;
                    speed += speedChange;
                }
                obstacles.transform.position = Vector2.MoveTowards(obstacles.transform.position, endPoint.position, step);
                if (obstacles.transform.position.y == endPoint.position.y)
                {
                    waiting = false;
                }
            }
            else if ((!waiting) && (loading) && running)
            {
                //road.SetBool("Speeding", true);
                if (road > minSpeed)
                {
                    road -= speedChange;
                    speed -= speedChange;
                }
                obstacles.transform.position = Vector2.MoveTowards(obstacles.transform.position, midPoint.position, step);
                if ((obstacles.transform.position.y <= appearance) && timer.ElapsedMilliseconds == 0)
                {
                    timer.Start();
                }
                if (obstacles.transform.position.y <= appearance)
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
                //road.SetBool("Speeding", true);
                if (road < maxSpeed)
                {
                    road += speedChange;
                    speed += speedChange;
                }
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
        running = false;
        correct += 1;
        totalTime = totalTime + timer.ElapsedMilliseconds;
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        responseTime.text = ((correct / trials) * 100 + "% correct");
        percentCorrect.text = ((totalTime / totalTrials) + " = Average response time");
        StartCoroutine(ending());
    }
    void endWrongAnswer()
    {
        running = false;
        totalTime = totalTime + timer.ElapsedMilliseconds;
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        StartCoroutine(wait());
        responseTime.text = ((correct / trials) * 100 + "% correct");
        percentCorrect.text = ((totalTime / totalTrials) + " = Average response time");
        StartCoroutine(ending());

    }
    void endEarlyCorrectAnswer()
    {
        running = false;
        correct += 1;
        totalTime = totalTime + timer.ElapsedMilliseconds;
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        responseTime.text = ((correct / trials) * 100 + "% correct");
        percentCorrect.text = ((totalTime / totalTrials) + " = Average response time");
        StartCoroutine(ending());
    }
    void endEarlyWrongAnswer()
    {
        running = false;
        totalTime = totalTime + timer.ElapsedMilliseconds;
        StartCoroutine(wait());
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        responseTime.text = ((correct / trials) * 100 + "% correct");
        percentCorrect.text = ((totalTime / totalTrials) + " = Average response time");
        StartCoroutine(ending());

    }
    IEnumerator wait()
    {
        yield return new WaitForSecondsRealtime(.25f);
        car.SetInteger("Key", answer);
        timer = new Stopwatch();
    }
    void beginLevel()
    {
        StartCoroutine(begining());
    }
    IEnumerator begining()
    {
        startingAnimation.SetInteger("seconds", 3);
        yield return new WaitForSecondsRealtime(1);
        startingAnimation.SetInteger("seconds", 2);
        yield return new WaitForSecondsRealtime(1);
        startingAnimation.SetInteger("seconds", 1);
        yield return new WaitForSecondsRealtime(1);
        startingAnimation.SetInteger("seconds", 0);
        running = true;
        loading = true;
    }
    IEnumerator ending()
    {
        yield return new WaitForSecondsRealtime(1);
        endPanel.SetActive(true);
        UnityEngine.Debug.Log(responseTime.text);
        UnityEngine.Debug.Log(percentCorrect.text);
    }
}

