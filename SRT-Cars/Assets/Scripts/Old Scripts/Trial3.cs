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
    public AudioSource SpeedUp;
    public int lanes;
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
    public Sprite[] keySprites = new Sprite[8];
    public GameObject Locations;
    private SpriteRenderer sRender;
    public string[] keys;
    public string[] posKeys;
    private int j;
    public Text scoreBox;
    public Text Multiplier;
    public int score;
    public int Streak;
    public int scoreMultiplier;
    public int points;


    private void Awake()
    {
        points = 10;
        score = 0;
        Streak = 0;
        scoreMultiplier = 1;
        scoreBox.text = ("Score: " + score);
        keys = new string[] { "s", "d", "f", "g", "h", "j", "k", "l" };
        posKeys = new string[lanes];
        float camHeight = cam.orthographicSize * 1.9f;
        float camWidth = camHeight * cam.aspect;
        float x1 = camWidth/(lanes);
        appearance = 3.8f;
        maxSpeed = 16;
        midSpeed = 11;
        minSpeed = 2;
        speedChange = 1f;
        shiftSpeed = 40;
        obstacles.transform.position = startPoint.position;
        carz = new GameObject[lanes];
        shift = new GameObject[lanes];
        for (int i = 0; i < 5; i++)
        {

            Pattern.Push(0);
            Pattern.Push(1);
            Pattern.Push(2);
            Pattern.Push(3);
            Pattern.Push(1);
            Pattern.Push(2);
            Pattern.Push(0);
            Pattern.Push(2);
            Pattern.Push(1);
            Pattern.Push(3);
        }
        if (lanes <= 2)
        {
            j = 3;
        }
        else if (lanes <= 4)
        {
            j = 2;
        }
        else if (lanes <= 6)
        {
            j = 1;
        }
        else
        {
            j = 0;
        }
        int k = j;
        for (int i = 0; i<lanes; i++)
        { 
            posKeys[i] = keys[k];
            k++;
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
        k = j;
            for(int i = 0; i< lanes; i++)
        {
            carz[i] = Sprite.Instantiate(obstacle1,obs) as GameObject;
            carz[i].transform.position = new Vector3(obs.position.x + (x1*i)+(x1/1.5f), obs.position.y, -11);
            carz[i].transform.localScale = new Vector3(5f/lanes,5f/lanes,1);
            shift[i] = Sprite.Instantiate(Locations,shifts) as GameObject;
            shift[i].transform.position = new Vector3(shifts.position.x + +(x1 * i) + (x1 / 1.5f), -3,-10);
            sRender = shift[i].GetComponent<SpriteRenderer>();
            sRender.sprite = keySprites[k];
            k++;


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
        car.transform.position = (shift[keyPressed].transform.position);
        car.transform.localScale = new Vector3(5f/ lanes, 5f / lanes, 1);
    }

    // Update is called once per frame
    void Update()
    {
        scoreBox.text = ("Score: " + score);
        Multiplier.text = "X" + scoreMultiplier;
        obstacleSpeed = carSpeed - originalSpeed;
        float shifting = shiftSpeed * Time.deltaTime;
        float step = obstacleSpeed * Time.deltaTime;
        float lineStep = carSpeed * Time.deltaTime;

        lines.transform.position = Vector2.MoveTowards(lines.transform.position, lineEnd.position, lineStep);
        if (lines.transform.position.y == lineEnd.position.y)
        {
            lines.transform.position = lineStart.position;
        }

        if (Pattern.Count > 0)
        {
            if ((waiting) && (!loading) && running)
            {
                step = obstacleSpeed * Time.deltaTime * 1.5f;
                lineStep = carSpeed * Time.deltaTime * 1.5f;
                car.transform.position = Vector3.MoveTowards(car.transform.position, shift[answer].transform.position, shifting);
                obstacles.transform.position = Vector2.MoveTowards(obstacles.transform.position, endPoint.position, step);
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
                            SpeedUp.Stop();
                            brake.Play();
                            restarting = true;
                        }

                    }

                }

            }
            else if ((!waiting) && (!loading) && running)
            {
                step = obstacleSpeed * Time.deltaTime * 1.5f;
                lineStep = carSpeed * Time.deltaTime * 1.5f;
                car.transform.position = Vector3.MoveTowards(car.transform.position, shift[keyPressed].transform.position, shifting);
                obstacles.transform.position = Vector2.MoveTowards(obstacles.transform.position, endPoint.position, step);
                if (!restarting)
                {
                    if (carSpeed < maxSpeed)
                    {
                        carSpeed += speedChange;
                    }
                    if (obstacles.transform.position.y <= midPoint.position.y)
                    {
                        SpeedUp.Stop();
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
                else if (obstacles.transform.position.y <= appearance)
                {
                    for (int i = 0; i < lanes; i++)
                    {
                        if ((answer == i) && (Input.GetKeyDown(posKeys[i])))
                        {
                            timer.Stop();
                            keyPressed = answer;
                            endCorrectAnswer();
                        }
                        else if (Input.GetKeyDown(posKeys[i]))
                        {
                            keyPressed = i;
                            wrongAnswer();
                        }
                        else if (obstacles.transform.position.y <= midPoint.position.y)
                        {
                            wrongAnswer();
                            SpeedUp.Stop();
                            brake.Play();
                            restarting = true;
                        }

                    }


                }
            }
            else if ((!waiting) && (!loading) && running)
            {
                step = obstacleSpeed * Time.deltaTime * 1.5f;
                lineStep = carSpeed * Time.deltaTime * 1.5f;
                obstacles.transform.position = Vector2.MoveTowards(obstacles.transform.position, endPoint.position, step);
                if (!restarting)
                {
                    car.transform.position = Vector3.MoveTowards(car.transform.position, shift[keyPressed].transform.position, shifting);
                    if (carSpeed < maxSpeed)
                    {
                        carSpeed += speedChange;
                    }
                    if (obstacles.transform.position.y <= midPoint.position.y)
                    {
                        SpeedUp.Stop();
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
                step = obstacleSpeed * Time.deltaTime * 1.5f;
                lineStep = carSpeed * Time.deltaTime * 1.5f;
                //road.SetBool("Speeding", true);
                if (carSpeed < maxSpeed)
                {
                    carSpeed += speedChange;
                }
                car.transform.position = Vector3.MoveTowards(car.transform.position, shift[answer].transform.position, shifting);
                obstacles.transform.position = Vector2.MoveTowards(obstacles.transform.position, endPoint.position, step);
                if (obstacles.transform.position.y == endPoint.position.y)
                {
                    endPanel.SetActive(true);
                }
            }

            
        }
    }
    
    void correctAnswer()
    {
        if (Streak < 5)
        {
            scoreMultiplier = 1;
        }
        else if (Streak < 10)
        {
            scoreMultiplier = 2;
        }
        else if (Streak < 15)
        {
            scoreMultiplier = 3;
        }
        else
        {
            scoreMultiplier = 4;
        }
        score = score + (points * scoreMultiplier);
        Streak ++;
        SpeedUp.Play();
        totalTime = totalTime + timer.ElapsedMilliseconds;
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        waiting = true;
        loading = false;
        timer = new Stopwatch();
    }
    void wrongAnswer()
    {
        SpeedUp.Play();
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        trials += 1;
        loading = false;
        scoreMultiplier = 1;
        Streak = 0;
    }

    void endCorrectAnswer()
    {
        running = false;
        SpeedUp.Play();
        totalTime = totalTime + timer.ElapsedMilliseconds;
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        percentCorrect.text = ((totalTrials / trials) * 100 + "% correct");
        responseTime.text = ((totalTime / totalTrials) + " = Average response time");
        UnityEngine.Debug.Log(responseTime.text);
        UnityEngine.Debug.Log(percentCorrect.text);
        if (((totalTrials / trials) * 100) > 90)
        {
            GameObject.Find("Progress").GetComponent<Progress>().level2 = true;
        }
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

