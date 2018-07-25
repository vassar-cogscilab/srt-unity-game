using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using UnityEngine.SceneManagement;


public class Tutorial : MonoBehaviour
{
    private bool waiting = false;
    private bool loading = false;
    private bool running = false;
    private GameObject obstacles;
    private GameObject lines;
    private Transform startPoint;
    private Transform restartPoint;
    private Transform midPoint;
    private Transform endPoint;
    private Transform lineStart;
    private Transform lineEnd;
    private float shiftSpeed;
    private Button startButton;
    private GameObject car;
    private Animator startingAnimation;
    private static Stack<int> Pattern = new Stack<int>();
    private float trials;
    private static long totalTrials;
    private int answer;
    private float road;
    private int maxSpeed;
    private bool restarting = false;
    private float carSpeed;
    private float obstacleSpeed;
    private float originalSpeed;
    private int lanes;
    private Camera cam;
    private GameObject yellowLines;
    private GameObject seperators;
    private GameObject whiteLine;
    private GameObject obstacle1;
    private GameObject[] carz;
    private GameObject[] shift;
    private GameObject[] laneButtons;
    private Transform pos;
    private Transform obs;
    private Transform shifts;
    private Transform ButtonStorage;
    public Sprite[] sprites = new Sprite[2];
    public Sprite[] keySprites = new Sprite[8];
    private GameObject Locations;
    private GameObject laneButton;
    private SpriteRenderer sRender;
    private string[] keys;
    private string[] posKeys;
    private int j;
    private Text scoreBox;
    private Text remaining;

    private int score;


    private int sequenceLength;
    private int currentSequence;
    public config cfig;
    private Text levelText;
    private Stopwatch time;

    //Tutorial specific
    private GameObject controlBorder;
    private GameObject countdownArrow;
    private GameObject playerBorder;
    private GameObject obstacleBorder;
    private GameObject timerArrow;
    private GameObject Tutorial1;
    private GameObject Tutorial2;
    private GameObject Tutorial3;
    private GameObject Tutorial4;
    private GameObject Tutorial5;
    private Button Continue1;
    private Button Continue2;
    private Button Continue3;
    private Button Continue;
    private bool inputKey;




    private void Awake()
    {
        //
        inputKey = false;
        controlBorder = GameObject.Find("control border");
        countdownArrow = GameObject.Find("countdown arrow");
        playerBorder = GameObject.Find("player border");
        obstacleBorder = GameObject.Find("Obstacle Border");
        timerArrow = GameObject.Find("timer arrow");
        Tutorial1 = GameObject.Find("Tutorial 1");
        Tutorial2 = GameObject.Find("Tutorial 2");
        Tutorial3 = GameObject.Find("Tutorial 3");
        Tutorial4 = GameObject.Find("Tutorial 4");
        Tutorial5 = GameObject.Find("Tutorial 5");
        Continue = GameObject.Find("continue").GetComponent<Button>();
        Continue1 = GameObject.Find("continue1").GetComponent<Button>();
        Continue3 = GameObject.Find("continue3").GetComponent<Button>();
        remaining = GameObject.Find("Remaining").GetComponent<Text>();
        remaining.text = ("Remaining: " + 1);
        controlBorder.SetActive(false);
        countdownArrow.SetActive(false);
        obstacleBorder.SetActive(false);
        timerArrow.SetActive(false);
        Tutorial1.SetActive(false);
        Tutorial2.SetActive(false);
        Tutorial3.SetActive(false);
        Tutorial4.SetActive(false);
        Tutorial5.SetActive(false);
        Continue.onClick.AddListener(nextStep);
        Continue1.onClick.AddListener(stepTwo);
        Continue3.onClick.AddListener(stepFour);
        //
        lanes = cfig.Lanes;
       
        obstacles = GameObject.Find("Obstacles");
        lines = GameObject.Find("Road Lines");
        startPoint = GameObject.Find("startPoint").transform;
        restartPoint = GameObject.Find("restartPoint").transform;
        midPoint = GameObject.Find("midPoint").transform; ;
        endPoint = GameObject.Find("endPoint").transform; ;
        lineStart = GameObject.Find("linesStart").transform; ;
        lineEnd = GameObject.Find("linesEnd").transform; ;
       
        startButton = GameObject.Find("startButton").GetComponent<Button>();
        car = GameObject.Find("Car");
        startingAnimation = GameObject.Find("Countdown").GetComponent<Animator>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        yellowLines = GameObject.Find("Yellow Lines");
        seperators = GameObject.Find("Seperator");
        whiteLine = GameObject.Find("White Line");
        obstacle1 = GameObject.Find("obstacle1");
        pos = GameObject.Find("Road Lines").transform;
        obs = GameObject.Find("Obstacles").transform;
        shifts = GameObject.Find("Locations").transform;
        ButtonStorage = GameObject.Find("LaneButtonStorage").transform;
        Locations = GameObject.Find("LocationsChild");
        laneButton = GameObject.Find("Lane Button");
        scoreBox = GameObject.Find("Score").GetComponent<Text>();
        score = 0;
        scoreBox.text = ("Time: " + score);
        keys = new string[] { "s", "d", "f", "g", "h", "j", "k", "l" };
        posKeys = new string[lanes];
        float camHeight = cam.orthographicSize * 1.9f;
        float camWidth = camHeight * cam.aspect;
        float x1 = camWidth / (lanes);
        maxSpeed = 16;
        shiftSpeed = 40;
        obstacles.transform.position = startPoint.position;
        carz = new GameObject[lanes];
        shift = new GameObject[lanes];
        laneButtons = new GameObject[lanes];
        Pattern.Push(0);
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
        for (int i = 0; i < lanes; i++)
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
        for (int i = 0; i < lanes; i++)
        {
            carz[i] = Sprite.Instantiate(obstacle1, obs) as GameObject;
            carz[i].transform.position = new Vector3((x1 * i) - (camWidth / 2f) + (x1 / 2), obs.position.y, -11);
            carz[i].transform.localScale = new Vector3(2.5f / lanes, 2.5f / lanes, 1);
            shift[i] = Sprite.Instantiate(Locations, shifts) as GameObject;
            shift[i].transform.position = new Vector3((x1 * i) - (camWidth / 2f) + (x1 / 2), -3, -23);
            sRender = shift[i].GetComponent<SpriteRenderer>();
            sRender.sprite = keySprites[k];
            laneButton.GetComponent<LaneButton>().laneNumber = i;
            laneButtons[i] = Sprite.Instantiate(laneButton, ButtonStorage) as GameObject;
            laneButtons[i].transform.position = new Vector3((x1 * i) - (camWidth / 2f) + (x1 / 2), -3, -10);
            laneButtons[i].transform.localScale = new Vector3(10f / lanes, 10f / lanes, 1);
            k++;

        }
        car.transform.position = shift[2].transform.position;
        playerBorder.transform.position = shift[0].transform.position;
        playerBorder.SetActive(false);
        answer = Pattern.Pop();
        sRender = carz[answer].GetComponent<SpriteRenderer>();
        sRender.sprite = sprites[0];
        startButton.onClick.AddListener(beginLevel);
        carSpeed = maxSpeed;
        originalSpeed = 10;
        car.transform.localScale = new Vector3(2.5f / lanes, 2.5f / lanes, 1);
        time = new Stopwatch();
    }
    void Update()
    {
        if (time.ElapsedMilliseconds > 0)
        {
            scoreBox.text = ("Time: " + (time.ElapsedMilliseconds / 1000f));
        }
        obstacleSpeed = carSpeed - originalSpeed;
        float shifting = shiftSpeed * Time.deltaTime;
        float step = obstacleSpeed * Time.deltaTime;
        float lineStep = carSpeed * Time.deltaTime;

        lines.transform.position = Vector2.MoveTowards(lines.transform.position, lineEnd.position, lineStep);
        if (lines.transform.position.y == lineEnd.position.y)
        {
            lines.transform.position = lineStart.position;
        }
        if(loading && running && !restarting)
        {
            obstacles.transform.position = Vector2.MoveTowards(obstacles.transform.position, restartPoint.position, step);
            if(obstacles.transform.position.y == restartPoint.position.y)
            {
                loading = false;
                time.Stop();
                Tutorial2.SetActive(true);
                obstacleBorder.SetActive(true);
            }
        }
        else if(waiting && running && !restarting)
        {
            Tutorial3.SetActive(true);
            playerBorder.SetActive(true);
            if (Input.GetKeyDown("f"))
            {
                inputKey = true;
            }
            if (inputKey == true)
            {
                car.transform.position = Vector3.MoveTowards(car.transform.position, shift[0].transform.position, shifting);
                obstacles.transform.position = Vector2.MoveTowards(obstacles.transform.position, endPoint.position, step);
            }
            if(obstacles.transform.position.y == endPoint.position.y)
            {
                inputKey = false;
                running = false;
                time.Stop();
                Tutorial3.SetActive(false);
                playerBorder.SetActive(false);
                stepThree();
            }
        }
        else if (waiting && loading && !restarting)
        {
            obstacles.transform.position = Vector2.MoveTowards(obstacles.transform.position, endPoint.position, step);
            if (obstacles.transform.position.y == endPoint.position.y)
            {
                waiting = false;
                restarting = true;
                remaining.text = "Remaining: 0";
                obstacles.transform.position = startPoint.position;
            }
        }
        else if(loading && restarting)
        {   if (!running)
            {

                if (inputKey)
                {
                    Tutorial4.SetActive(false);
                    obstacles.transform.position = Vector2.MoveTowards(obstacles.transform.position, midPoint.position, step);
                    car.transform.position = Vector3.MoveTowards(car.transform.position, shift[3].transform.position, shifting);
                }
                else
                {
                    Tutorial4.SetActive(true);
                    obstacles.transform.position = Vector2.MoveTowards(obstacles.transform.position, restartPoint.position, step);
                }
                if (obstacles.transform.position.y == midPoint.position.y)
                {
                    inputKey = false;
                    running = true;
                }
            }
            else
            {
                obstacles.transform.position = Vector2.MoveTowards(obstacles.transform.position, restartPoint.position, step);
                if(obstacles.transform.position.y == restartPoint.position.y)
                {
                    loading = false;
                    restarting = false;
                    running = false;
                    time.Stop();
                    Tutorial5.SetActive(true);
                }
            }
        }

    }
        void beginLevel()
    {
        startingAnimation.SetInteger("seconds", 3);
        countdownArrow.SetActive(true);
        Tutorial1.SetActive(true);
    }
    void nextStep()
    {
        countdownArrow.SetActive(false);
        Tutorial1.SetActive(false);
        StartCoroutine(begining());
    }
    IEnumerator begining()
    {
        yield return new WaitForSecondsRealtime(1);
        startingAnimation.SetInteger("seconds", 2);
        yield return new WaitForSecondsRealtime(1);
        startingAnimation.SetInteger("seconds", 1);
        yield return new WaitForSecondsRealtime(1);
        startingAnimation.SetInteger("seconds", 0);
        running = true;
        loading = true;
        time.Start();
    }
    void stepTwo()
    {
        time.Start();
        waiting = true;
    }

    void stepThree()
    {
        time.Start();
        loading = true;
    }
    void stepFour()
    {
        inputKey = true;
    }



}

