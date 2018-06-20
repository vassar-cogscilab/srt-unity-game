using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;

public class Trial3 : MonoBehaviour
{
    public bool waiting = false;
    public bool loading = false;
    public bool running = false;
    private int g = 0;
    private int h = 1;
    private int f = 2;
    private int j = 3;
    private int d = 4;
    private int k = 5;
    private int s = 6;
    private int l = 7;
    public float appearance;
    public GameObject obstacles;
    public GameObject lines;
    public Transform startPoint;
    public Transform restartPoint;
    public Transform midPoint;
    public Transform endPoint;
    public Transform lineStart;
    public Transform lineEnd;
    public Transform slowPoint;
    public float shiftSpeed;
    public Button startButton;
    public Text responseTime;
    public Text percentCorrect;
    public GameObject endPanel;
    public GameObject car;
    public int keyPressed;
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
    public int maxSpeed;
    public int midSpeed;
    public int minSpeed;
    public float speedChange;
    public bool restarting = false;
    private float carSpeed;
    private float obstacleSpeed;
    private float originalSpeed;
    public AudioSource brake;
    private int lanes;
    public Camera cam;
    public GameObject yellowLines;
    public GameObject seperators;
    public GameObject whiteLine;
    public GameObject obstacle1;
    public GameObject[] carz;
    public GameObject[] shift;
    public Transform pos;
    public Transform obs;
    public Transform shifts;
    public Sprite[] sprites = new Sprite[2];
    public GameObject Locations;
    private SpriteRenderer sRender;
    public string[] keys;
    public string[] posKeys;

    private void Awake()
    {
        keys = new string[] { "g", "h", "f", "j", "d", "k", "s", "l" };
        lanes = 6;
        posKeys = new string[lanes];
        float camHeight = cam.orthographicSize * 1.9f;
        float camWidth = camHeight * cam.aspect;
        float x1 = camWidth/(lanes);
        appearance = 3.8f;
        maxSpeed = 16;
        midSpeed = 11;
        minSpeed = 2;
        speedChange = .5f;
        shiftSpeed = 12;
        obstacles.transform.position = startPoint.position;
        carz = new GameObject[lanes];
        shift = new GameObject[lanes];
        for (int i = 0; i < 5; i++)
        {
            Pattern.Push(d);
            Pattern.Push(j);
            Pattern.Push(k);
            Pattern.Push(f);
        }
        for(int i = 0; i<lanes; i++)
        {
            posKeys[i] = keys[i];
        }
            for (float i = 0 - ((lanes) / 2f); i <= ((lanes) / 2f); i++)
            {
                if (i == (0 - (lanes) / 2f))
                {
                    Sprite.Instantiate(yellowLines, pos).transform.position = new Vector3(x1 * i, 0, 0);
                }
                else if (i == ((lanes) / 2f))
                {
                    Sprite.Instantiate(whiteLine, pos).transform.position = new Vector3(x1 * i, 0, 0);
                }
                else
                {
                    Sprite.Instantiate(seperators, pos).transform.position = new Vector3(x1 * i, 0, 0);
                }
            }
            for(int i = 0; i< lanes; i++)
        {
            carz[i] = Sprite.Instantiate(obstacle1,obs) as GameObject;
            carz[i].transform.position = new Vector3(obs.position.x + (x1*i)+(x1/1.5f), obs.position.y, -10);
            carz[i].transform.localScale = new Vector3(20/lanes,20/lanes,1);
            shift[i] = Sprite.Instantiate(Locations,shifts) as GameObject;
            shift[i].transform.position = new Vector3(shifts.position.x + +(x1 * i) + (x1 / 1.5f), -3,-10);


        }
        trials = Pattern.Count;
        totalTrials = Pattern.Count;
        answer = Pattern.Pop();
        timer = new Stopwatch();
        sRender = carz[answer].GetComponent<SpriteRenderer>();
        sRender.sprite = sprites[0];
        startButton.onClick.AddListener(beginLevel);
        carSpeed = maxSpeed;
        originalSpeed = 10;
    }
    // Update is called once per frame
    void Update()
    {
        obstacleSpeed = carSpeed - originalSpeed;
        float shifting = shiftSpeed * Time.deltaTime;
        float step = obstacleSpeed * Time.deltaTime;
        float lineStep = carSpeed * Time.deltaTime;

        lines.transform.position = Vector2.MoveTowards(lines.transform.position, lineEnd.position, lineStep);
        if (lines.transform.position.y == lineEnd.position.y)
        {
            lines.transform.position = lineStart.position;
        }

        if (Pattern.Count != 0)
        {
            if ((waiting) && (!loading) && running)
            {
                car.transform.position = Vector3.MoveTowards(car.transform.position, shift[answer].transform.position, shifting);
                obstacles.transform.position = Vector2.MoveTowards(obstacles.transform.position, endPoint.position, step*1.5f);
                if (carSpeed < maxSpeed)
                {
                    carSpeed += speedChange;
                }
                if (obstacles.transform.position.y == endPoint.position.y)
                {
                    waiting = false;
                    loading = true;
                    sRender.sprite = sprites[1];
                    answer = Pattern.Pop();
                    sRender = carz[answer].GetComponent<SpriteRenderer>();
                    sRender.sprite = sprites[0];
                    obstacles.transform.position = startPoint.position;
                }
            }
            else if ((!waiting) && (loading) && running)
            {
                if (obstacles.transform.position.y >= slowPoint.position.y)
                {
                    if (carSpeed < maxSpeed)
                    {
                        carSpeed += speedChange;
                    }
                }
                else if (obstacles.transform.position.y <= slowPoint.position.y)
                {
                    if (carSpeed > midSpeed)
                    {
                        carSpeed -= speedChange;
                    }
                }


                obstacles.transform.position = Vector2.MoveTowards(obstacles.transform.position, endPoint.position, step);

                if ((obstacles.transform.position.y <= appearance) && timer.ElapsedMilliseconds == 0)
                {
                    timer.Start();
                }
                else if (obstacles.transform.position.y <= appearance)
                {
                    for (int i = 0; i<lanes; i++)
                    {
                        if((answer==i) && (Input.GetKeyDown(posKeys[i])))
                        {
                            timer.Stop();
                            keyPressed = answer;
                            correctAnswer();
                        }
                        else if (Input.GetKeyDown(posKeys[i]))
                        {
                            keyPressed = i;
                            wrongAnswer();
                        }
                        else if (obstacles.transform.position.y <= midPoint.position.y)
                        {
                            wrongAnswer();
                            brake.Play();
                            restarting = true;
                        }

                    }
                    if (((answer == d) && Input.GetKeyDown("d")) || ((answer == f) && Input.GetKeyDown("f")) || ((answer == j) && Input.GetKeyDown("j")) || ((answer == k) && Input.GetKeyDown("k")) || ((answer == g) && Input.GetKeyDown("g")) || ((answer == h) && Input.GetKeyDown("h")) || ((answer == j) && Input.GetKeyDown("j")) || ((answer == k) && Input.GetKeyDown("k")))
                    {
                        timer.Stop();
                        keyPressed = answer;
                        correctAnswer();
                    }

                    else if (Input.GetKeyDown("d"))
                    {
                        keyPressed = d;
                        wrongAnswer();
                    }
                    else if (Input.GetKeyDown("f"))
                    {
                        keyPressed = f;
                        wrongAnswer();
                    }
                    else if (Input.GetKeyDown("j"))
                    {
                        keyPressed = j;
                        wrongAnswer();
                    }
                    else if (Input.GetKeyDown("k"))
                    {
                        keyPressed = k;
                        wrongAnswer();
                    }
                    else if (obstacles.transform.position.y <= midPoint.position.y)
                    {
                        wrongAnswer();
                        brake.Play();
                        restarting = true;
                    }
                }

            }
            else if ((!waiting) && (!loading) && running)
            {
                car.transform.position = Vector3.MoveTowards(car.transform.position, shift[keyPressed].transform.position, shifting);
                obstacles.transform.position = Vector2.MoveTowards(obstacles.transform.position, endPoint.position, step*1.5f);
                if (!restarting)
                {
                    if (carSpeed < maxSpeed)
                    {
                        carSpeed += speedChange;
                    }
                    if (obstacles.transform.position.y <= midPoint.position.y)
                    {
                        brake.Play();
                        restarting = true;
                    }
                }
                else
                {
                    if (carSpeed > minSpeed)
                    {
                        carSpeed -= speedChange;
                    }
                    else if (obstacles.transform.position.y >= restartPoint.position.y)
                    {
                        loading = true;
                        restarting = false;
                    }
                }
            }
        }
        else
        {
            if ((waiting) && (!loading) && running)
            {
                if (road < maxSpeed)
                {
                    road += speedChange;
                    shiftSpeed += speedChange;
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
                if (obstacles.transform.position.y > slowPoint.position.y)
                {
                    if (carSpeed < maxSpeed)
                    {
                        carSpeed += speedChange;
                    }
                }
                else if (obstacles.transform.position.y <= slowPoint.position.y)
                {
                    if (carSpeed > midSpeed)
                    {
                        carSpeed -= speedChange;
                    }
                }
                obstacles.transform.position = Vector2.MoveTowards(obstacles.transform.position, endPoint.position, step);
                if ((obstacles.transform.position.y <= appearance) && timer.ElapsedMilliseconds == 0)
                {
                    timer.Start();
                }
                if (obstacles.transform.position.y <= appearance)
                {
                    if (((answer == d) && Input.GetKeyDown("d")) || ((answer == f) && Input.GetKeyDown("f")) || ((answer == j) && Input.GetKeyDown("j")) || ((answer == k) && Input.GetKeyDown("k")))
                    {
                        timer.Stop();
                        keyPressed = answer;
                        endCorrectAnswer();
                    }

                    else if (Input.GetKeyDown("d"))
                    {
                        keyPressed = d;
                        wrongAnswer();
                    }
                    else if (Input.GetKeyDown("f"))
                    {
                        keyPressed = f;
                        wrongAnswer();
                    }
                    else if (Input.GetKeyDown("j"))
                    {
                        keyPressed = j;
                        wrongAnswer();
                    }
                    else if (Input.GetKeyDown("k"))
                    {
                        keyPressed =  k;
                        wrongAnswer();
                    }
                    else if (obstacles.transform.position.y <= midPoint.position.y)
                    {
                        wrongAnswer();
                        brake.Play();
                        restarting = true;
                    }
                }

            }
            else if ((!waiting) && (!loading) && running)
            {
                obstacles.transform.position = Vector2.MoveTowards(obstacles.transform.position, endPoint.position, step * 1.5f);
                if (!restarting)
                {
                    car.transform.position = Vector3.MoveTowards(car.transform.position, shift[keyPressed].transform.position, shifting);
                    if (carSpeed < maxSpeed)
                    {
                        carSpeed += speedChange;
                    }
                    if (obstacles.transform.position.y <= midPoint.position.y)
                    {
                        brake.Play();
                        restarting = true;
                    }
                }
                else
                {
                    if (carSpeed > minSpeed)
                    {
                        carSpeed -= speedChange;
                    }
                    else if (obstacles.transform.position.y >= restartPoint.position.y)
                    {
                        loading = true;
                        restarting = false;
                    }
                }
            }
        
            else if (!running)
            {
                //road.SetBool("Speeding", true);
                if (road < maxSpeed)
                {
                    road += speedChange;
                    shiftSpeed += speedChange;
                }
                car.transform.position = Vector3.MoveTowards(car.transform.position, shift[answer].transform.position, shifting);
                obstacles.transform.position = Vector2.MoveTowards(obstacles.transform.position, endPoint.position, step*1.5f);
                if(obstacles.transform.position.y == endPoint.position.y)
                {
                    endPanel.SetActive(true);
                }

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
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        trials += 1;
        loading = false;
    }

    void endCorrectAnswer()
    {
        running = false;
        totalTime = totalTime + timer.ElapsedMilliseconds;
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        responseTime.text = ((totalTrials / trials) * 100 + "% correct");
        percentCorrect.text = ((totalTime / totalTrials) + " = Average response time");
        UnityEngine.Debug.Log(responseTime.text);
        UnityEngine.Debug.Log(percentCorrect.text);
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
}

