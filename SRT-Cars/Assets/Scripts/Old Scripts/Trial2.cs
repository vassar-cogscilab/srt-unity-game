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
    public float appearance = 3.8f;
    public GameObject obstacles;
    public GameObject lines;
    public Transform startPoint;
    public Transform restartPoint;
    public Transform midPoint;
    public Transform endPoint;
    public Transform lineStart;
    public Transform lineEnd;
    public float speed;
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
    public int wrong = 0;
    public Stopwatch timer;
    public static long totalTime = 0;
    public float road;
    public float wrongSpeed;
    public int maxSpeed = 14;
    public int minSpeed = 6;
    public float speedChange = .25f;
    public int brakeSpeed;
    public bool restarting = false;

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
        road = maxSpeed;
        speed = maxSpeed - minSpeed+ 1;
        wrongSpeed = 0;
        brakeSpeed = 2;
}
    // Update is called once per frame
    void Update()
    {
        float wrongStep = wrongSpeed * Time.deltaTime;
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
                if (restarting)
                {
                    if(road < maxSpeed)
                    {
                        road += speedChange;
                        wrongSpeed += speedChange;
                    }

                    obstacles.transform.position = Vector2.MoveTowards(obstacles.transform.position, restartPoint.position, wrongStep);
                    if (obstacles.transform.position.y == restartPoint.position.y)
                    {
                        restarting = false;
                    }
                }
                else
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
                            correctAnswer();
                        }

                        else if (Input.GetKeyDown("d"))
                        {
                            car.SetInteger("Key", d);
                            wrongAnswer();
                        }
                        else if (Input.GetKeyDown("f"))
                        {
                            car.SetInteger("Key", f);
                            wrongAnswer();
                        }
                        else if (Input.GetKeyDown("j"))
                        {
                            car.SetInteger("Key", j);
                            wrongAnswer();
                        }
                        else if (Input.GetKeyDown("k"))
                        {
                            car.SetInteger("Key", k);
                            wrongAnswer();
                        }
                        else if (obstacles.transform.position.y == midPoint.position.y)
                        {
                            wrongAnswer();
                            restarting = true;
                        }
                    }
                }
            }
            else if ((!waiting) && (!loading) && running)
            {
                if (!restarting)
                {
                    if (road < maxSpeed)
                    {
                        road += speedChange;
                        speed += speedChange;
                    }
                    obstacles.transform.position = Vector2.MoveTowards(obstacles.transform.position, midPoint.position, step);
                    if (obstacles.transform.position.y == midPoint.position.y)
                    {
                        restarting = true;
                        wrongSpeed = -1;
                    }
                }
                else if (restarting)
                {
                    if (obstacles.transform.position.y < restartPoint.position.y)
                    {
                        if (road > brakeSpeed)
                        {
                            road -= speedChange;
                            wrongSpeed -= speedChange;
                        }
                        obstacles.transform.position = Vector2.MoveTowards(obstacles.transform.position, endPoint.position, wrongStep);
                    }
                    else if (obstacles.transform.position.y >= restartPoint.position.y)
                    {
                        loading = true;
                        wrongSpeed = 0;
                    }
                }
            }
        }
        else
        {
            if (road < maxSpeed)
            {
                road += speedChange;
                speed += speedChange;
            }
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
                }
            }
            else if ((!waiting) && (loading) && running)
            {
                if (!restarting)
                {
                    obstacles.transform.position = Vector2.MoveTowards(obstacles.transform.position, midPoint.position, step);
                }
                if (restarting)
                {
                    obstacles.transform.position = Vector2.MoveTowards(obstacles.transform.position, restartPoint.position, wrongStep);
                    if (obstacles.transform.position.y == restartPoint.position.y)
                    {
                        road = minSpeed;
                        restarting = false;
                    }
                }
                //road.SetBool("Speeding", true);
                if (road > minSpeed)
                {
                    road -= speedChange;
                    speed -= speedChange;
                }
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
                        endCorrectAnswer();
                    }

                    else if (Input.GetKeyDown("d"))
                    {
                        car.SetInteger("Key", d);
                        endWrongAnswer();
                    }
                    else if (Input.GetKeyDown("f"))
                    {
                        car.SetInteger("Key", f);
                        endWrongAnswer();
                    }
                    else if (Input.GetKeyDown("j"))
                    {
                        car.SetInteger("Key", j);
                        endWrongAnswer();
                    }
                    else if (Input.GetKeyDown("k"))
                    {
                        car.SetInteger("Key", k);
                        endWrongAnswer();
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
        totalTime = totalTime + timer.ElapsedMilliseconds;
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        waiting = true;
        loading = false;
        timer = new Stopwatch();
    }
    void wrongAnswer()
    {
        trials += 1;
        loading = false;
        wrongSpeed = 0;
        

    }
    
    void endCorrectAnswer()
    {
        running = false;
        totalTime = totalTime + timer.ElapsedMilliseconds;
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        responseTime.text = ((totalTrials/trials) * 100 + "% correct");
        percentCorrect.text = ((totalTime / totalTrials) + " = Average response time");
        StartCoroutine(ending());
    }
    void endWrongAnswer()
    {
        running = false;
        trials += 1; 
        totalTime = totalTime + timer.ElapsedMilliseconds;
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        responseTime.text = (((totalTrials) / trials) * 100 + "% correct");
        percentCorrect.text = ((totalTime / totalTrials) + " = Average response time");
        StartCoroutine(ending());

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

