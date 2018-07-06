using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class Trial5 : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void HelloString(string str);

    private bool waiting = false;
    private bool loading = false;
    private bool running = false;
    private float appearance;
    private GameObject obstacles;
    private GameObject lines;
    private Transform startPoint;
    private Transform restartPoint;
    private Transform midPoint;
    private Transform endPoint;
    private Transform lineStart;
    private Transform lineEnd;
    private Transform slowPoint;
    private float shiftSpeed;
    private Button startButton;
    private Button nextLevel;
    private Text responseTime;
    private Text percentCorrect;
    private GameObject endPanel;
    private GameObject car;
    private int keyPressed;
    private Animator startingAnimation;
    private static Stack<int> Pattern = new Stack<int>();
    private float trials;
    private static long totalTrials;
    private int answer;
    private Stopwatch timer;
    private static long totalTime;
    private float road;
    private int maxSpeed;
    private int midSpeed;
    private int minSpeed;
    private float speedChange;
    private bool restarting = false;
    private float carSpeed;
    private float obstacleSpeed;
    private float originalSpeed;
    private AudioSource brake;
    private AudioSource SpeedUp;
    private int lanes;
    private Camera cam;
    private GameObject yellowLines;
    private GameObject seperators;
    private GameObject whiteLine;
    private GameObject obstacle1;
    private GameObject[] carz;
    private GameObject[] shift;
    private GameObject[] endShift;
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
    private int scoreMultiplier;
    private int points;
    private Stack<int> inputPressed;
    private Stack<int> correctInput;
    private Stack<bool> wasInputCorrect;
    private Stack<float> responseTimes;
    private Stack<int> sequenceInput;
    private Stack<int> block;
    private Stack<int> inputPressed1;
    private Stack<int> correctInput1;
    private Stack<bool> wasInputCorrect1;
    private Stack<float> responseTimes1;
    private Stack<int> sequenceInput1;
    private Stack<int> block1;
    private int sequenceLength;
    private int currentSequence;
    public config cfig;
    private int level;
    private Text levelText;
    private string json;
    private Stopwatch time;
    private Text bestScoreText;
    private long bestScore;
    private int trial;
    private bool end;
    private Quaternion right;
    private Quaternion left;
    private Quaternion center;




    private void Awake()
    {
        right = Quaternion.Euler(0, 0, -20);
        left = Quaternion.Euler(0, 0, 20);
        center = Quaternion.Euler(0, 0, 0);
        end = true;
        trial = 1;
        bestScore = 0;
        inputPressed = new Stack<int>();
        correctInput = new Stack<int>();
        wasInputCorrect = new Stack<bool>();
        responseTimes = new Stack<float>();
        sequenceInput = new Stack<int>();
        block = new Stack<int>();
        inputPressed1 = new Stack<int>();
        correctInput1 = new Stack<int>();
        wasInputCorrect1 = new Stack<bool>();
        responseTimes1 = new Stack<float>();
        sequenceInput1 = new Stack<int>();
        block1 = new Stack<int>();
        level = 1;
        lanes = cfig.Lanes;
        appearance = 3.8f;
        obstacles = GameObject.Find("Obstacles");
        lines = GameObject.Find("Road Lines");
        startPoint = GameObject.Find("startPoint").transform;
        restartPoint = GameObject.Find("restartPoint").transform;
        midPoint = GameObject.Find("midPoint").transform; ;
        endPoint = GameObject.Find("endPoint").transform; ;
        lineStart = GameObject.Find("linesStart").transform; ;
        lineEnd = GameObject.Find("linesEnd").transform; ;
        slowPoint = GameObject.Find("slowPoint").transform;
        startButton = GameObject.Find("startButton").GetComponent<Button>();
        nextLevel = GameObject.Find("Next Level").GetComponent<Button>();
        responseTime = GameObject.Find("Response Time").GetComponent<Text>();
        //percentCorrect = GameObject.Find("Percent Correct").GetComponent<Text>();
        levelText = GameObject.Find("levelnumber").GetComponent<Text>();
        bestScoreText = GameObject.Find("Best Score").GetComponent<Text>();
        endPanel = GameObject.Find("EndScreen");
        endPanel.SetActive(false);
        car = GameObject.Find("Car");
        startingAnimation = GameObject.Find("Countdown").GetComponent<Animator>();
        brake = GameObject.Find("car brake").GetComponent<AudioSource>();
        SpeedUp = GameObject.Find("speed up").GetComponent<AudioSource>();
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
        remaining = GameObject.Find("Remaining").GetComponent<Text>();
        points = 10;
        score = 0;
        scoreMultiplier = 1;
        scoreBox.text = ("Time: " + score);
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
        endShift = new GameObject[lanes];
        laneButtons = new GameObject[lanes];
        currentSequence = 1;

        sequenceLength = cfig.Pattern.Length;
        if (cfig.Random == true)
        {
            for (int i = 0; i<cfig.Repetitions; i++)
            {
                Pattern.Push(UnityEngine.Random.Range(0, lanes));
            }
        }
        else
        {   for (int i = 0; i < cfig.Repetitions; i++)
            {
                for (int j = 0; j < cfig.Pattern.Length; j++)
                {
                    Pattern.Push(cfig.Pattern[j]);
                }
            }
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
            carz[i] = Sprite.Instantiate(obstacle1, obs) as GameObject;
            carz[i].transform.position = new Vector3((x1 * i) - (camWidth / 2f) + (x1 / 2), obs.position.y, -11);
            carz[i].transform.localScale = new Vector3(5f / lanes, 5f / lanes, 1);
            shift[i] = Sprite.Instantiate(Locations, shifts) as GameObject;
            shift[i].transform.position = new Vector3((x1 * i) - (camWidth / 2f) + (x1 / 2), -3, -10);
            sRender = shift[i].GetComponent<SpriteRenderer>();
            sRender.sprite = keySprites[k];
            endShift[i] = Sprite.Instantiate(Locations, shifts) as GameObject;
            endShift[i].transform.position = new Vector3((x1 * i) - (camWidth / 2f) + (x1 / 2), 5, -10);
            laneButton.GetComponent<LaneButton>().laneNumber = i;
            laneButtons[i] = Sprite.Instantiate(laneButton, ButtonStorage) as GameObject;
            laneButtons[i].transform.position = new Vector3((x1 * i) - (camWidth / 2f)+(x1/2), -3, -10);
            laneButtons[i].transform.localScale = new Vector3(10f / lanes, 10f / lanes, 1);
            k++;



        }
        trials = Pattern.Count;
        totalTrials = Pattern.Count;
        answer = Pattern.Pop();
        remaining.text = "Remaining: " + Pattern.Count;
        timer = new Stopwatch();
        sRender = carz[answer].GetComponent<SpriteRenderer>();
        sRender.sprite = sprites[0];
        startButton.onClick.AddListener(beginLevel);
        nextLevel.onClick.AddListener(StartLevel);
        carSpeed = maxSpeed;
        originalSpeed = 10;
        car.transform.position = (shift[keyPressed].transform.position);
        car.transform.localScale = new Vector3(5f/ lanes, 5f / lanes, 1);
        time = new Stopwatch();
    }

    // Update is called once per frame
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

        if (Pattern.Count > 0)
        {
            if ((waiting) && (!loading) && running)
            {
                step = obstacleSpeed * Time.deltaTime * 3f;
                lineStep = carSpeed * Time.deltaTime * 3f;
                car.transform.position = Vector3.MoveTowards(car.transform.position, shift[answer].transform.position, shifting);
                obstacles.transform.position = Vector2.MoveTowards(obstacles.transform.position, endPoint.position, step);
                if(car.transform.position.x > shift[answer].transform.position.x)
                {
                    car.transform.rotation = Quaternion.Slerp(car.transform.rotation, left, 1 );
                }
                else if (car.transform.position.x < shift[answer].transform.position.x)
                {
                    car.transform.rotation = Quaternion.Slerp(car.transform.rotation, right, 1);
                }
                else
                {
                    car.transform.rotation = Quaternion.Slerp(car.transform.rotation, center, 1);
                }
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
                    remaining.text = "Remaining: " + Pattern.Count;
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
                    for (int i = 0; i < lanes; i++)
                    {
                        if (((answer == i) && (Input.GetKeyDown(posKeys[i]))) || (answer == i) && cfig.lanePressed[i])
                        {
                            timer.Stop();
                            keyPressed = answer;
                            correctAnswer();
                        }
                        else if (Input.GetKeyDown(posKeys[i]) || cfig.lanePressed[i])
                        {
                            keyPressed = i;
                            wrongAnswer();
                        }
                        else if ((obstacles.transform.position.y <= midPoint.position.y) && (!restarting))
                        {
                            restarting = true;
                            keyPressed = -1;
                            wrongAnswer();
                            SpeedUp.Stop();
                            brake.Play();
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
                    if (car.transform.position.x > shift[keyPressed].transform.position.x)
                    {
                        car.transform.rotation = Quaternion.Slerp(car.transform.rotation, left, 1);
                    }
                    else if (car.transform.position.x < shift[keyPressed].transform.position.x)
                    {
                        car.transform.rotation = Quaternion.Slerp(car.transform.rotation, right, 1);
                    }
                    else
                    {
                        car.transform.rotation = Quaternion.Slerp(car.transform.rotation, center, 1);
                    }
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
                        if (((answer == i) && (Input.GetKeyDown(posKeys[i]))) || (answer == i) && cfig.lanePressed[i])
                        {
                            timer.Stop();
                            keyPressed = answer;
                            endCorrectAnswer();
                        }
                        else if (Input.GetKeyDown(posKeys[i]) || cfig.lanePressed[i])
                        {
                            keyPressed = i;
                            wrongAnswer();
                        }
                        else if ((obstacles.transform.position.y <= midPoint.position.y) && (!restarting))
                        {
                            restarting = true;
                            keyPressed = -1;
                            wrongAnswer();
                            SpeedUp.Stop();
                            brake.Play();
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
                    if (car.transform.position.x > shift[keyPressed].transform.position.x)
                    {
                        car.transform.rotation = Quaternion.Slerp(car.transform.rotation, left, 1);
                    }
                    else if (car.transform.position.x < shift[keyPressed].transform.position.x)
                    {
                        car.transform.rotation = Quaternion.Slerp(car.transform.rotation, right, 1);
                    }
                    else
                    {
                        car.transform.rotation = Quaternion.Slerp(car.transform.rotation, center, 1);
                    }
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
                if (end)
                {
                    car.transform.position = Vector3.MoveTowards(car.transform.position, shift[answer].transform.position, shifting);
                    if (car.transform.position.x > shift[answer].transform.position.x)
                    {
                        car.transform.rotation = Quaternion.Slerp(car.transform.rotation, left, 1);
                    }
                    else if (car.transform.position.x < shift[answer].transform.position.x)
                    {
                        car.transform.rotation = Quaternion.Slerp(car.transform.rotation, right, 1);
                    }
                    else
                    {
                        car.transform.rotation = Quaternion.Slerp(car.transform.rotation, center, 1);
                    }
                    obstacles.transform.position = Vector2.MoveTowards(obstacles.transform.position, endPoint.position, step);
                }
                else
                {
                    endPanel.SetActive(true);
                    car.transform.position = Vector3.MoveTowards(car.transform.position, endShift[answer].transform.position, shifting);
                }
                if (obstacles.transform.position.y == endPoint.position.y)
                {
                    end = false;
                }
                
            }

            
        }
    }

    void correctAnswer()
    {
        TrialData storage = new TrialData();
        SpeedUp.Play();
        totalTime = totalTime + timer.ElapsedMilliseconds;
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        storage.inputPressed = keyPressed;
        storage.correctInput = keyPressed;
        storage.wasInputCorrect = true;
        storage.responseTimes = timer.ElapsedMilliseconds;
        storage.block = level;
        storage.sequenceIndex = currentSequence;
        storage.trialIndex = trial;
        //HelloString(JsonUtility.ToJson(storage));
        trial = trial + 1;
        currentSequence++;
        if (currentSequence > sequenceLength)
        {
            currentSequence = 1;
        }
        waiting = true;
        loading = false;
        timer = new Stopwatch();
    }
    void wrongAnswer()
    {
        TrialData storage = new TrialData();
        SpeedUp.Play();
        totalTime = totalTime + timer.ElapsedMilliseconds;
        storage.inputPressed = keyPressed;
        storage.correctInput = answer;
        storage.wasInputCorrect = false;
        storage.responseTimes = timer.ElapsedMilliseconds;
        storage.block = level;
        storage.sequenceIndex = currentSequence;
        storage.trialIndex = trial;
        trial = trial + 1;
        trials += 1;
        //HelloString(JsonUtility.ToJson(storage));
        loading = false;
    }

    void endCorrectAnswer()
    {
        time.Stop();
        TrialData storage = new TrialData();
        levelText.text = "Attempt " + level + " Complete!";
        storage.inputPressed = keyPressed;
        storage.correctInput = answer;
        storage.wasInputCorrect = true;
        storage.responseTimes = timer.ElapsedMilliseconds;
        storage.block = level;
        storage.sequenceIndex = currentSequence;
        currentSequence = 1;
        storage.trialIndex = trial;
        trial = 1;
        trials += 1;
        //HelloString(JsonUtility.ToJson(storage));
        level++;
        running = false;
        SpeedUp.Play();
        totalTime = totalTime + timer.ElapsedMilliseconds;
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        responseTime.text = "You completed the attempt in " + time.ElapsedMilliseconds / 1000f + " seconds";
        if ((time.ElapsedMilliseconds < bestScore) || (bestScore == 0))
        {
            bestScore = time.ElapsedMilliseconds;
            bestScoreText.text = "Best Score: " + bestScore / 1000f;
        }
        UnityEngine.Debug.Log(responseTime.text);
        if (level > cfig.Attempts)
        {
            levelText.text = "All Attempts Completed";
        }
    }

    void beginLevel()
    {
        StartCoroutine(begining());
    }

    void StartLevel()
    {
        if (level <= cfig.Attempts)
        {
            StartCoroutine(startNextLevel());
        }
        else
        {
            SceneManager.LoadScene(0);
        }
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
        time.Start();
    }
    
    IEnumerator startNextLevel()
    {
        end = true;
        car.transform.position = (shift[0].transform.position);
        time = new Stopwatch();
        scoreBox.text = ("Time: " + 0);
        endPanel.SetActive(false);
        if (cfig.Random == true)
        {
            for (int i = 0; i < cfig.Repetitions; i++)
            {
                Pattern.Push(UnityEngine.Random.Range(0, lanes));
            }
        }
        else
        {
            for (int i = 0; i < cfig.Repetitions; i++)
            {
                for (int j = 0; j < cfig.Pattern.Length; j++)
                {
                    Pattern.Push(cfig.Pattern[j]);
                }
            }
        }

        trials = Pattern.Count;
        totalTrials = Pattern.Count;
        answer = Pattern.Pop();
        remaining.text = "Remaining: " + Pattern.Count;
        timer = new Stopwatch();
        sRender.sprite = sprites[1];
        sRender = carz[answer].GetComponent<SpriteRenderer>();
        sRender.sprite = sprites[0];
        obstacles.transform.position = startPoint.position;
        startingAnimation.SetInteger("seconds", 3);
        yield return new WaitForSecondsRealtime(1);
        startingAnimation.SetInteger("seconds", 2);
        yield return new WaitForSecondsRealtime(1);
        startingAnimation.SetInteger("seconds", 1);
        yield return new WaitForSecondsRealtime(1);
        startingAnimation.SetInteger("seconds", 0);
        running = true;
        loading = true;
        waiting = false;
        time.Start();
    }

}
//move trial data class into its own script
[Serializable]
public class TrialData
{
    public int trialIndex;
    public int sequenceIndex;
    public int inputPressed;
    public int block;
    public int correctInput;
    public bool wasInputCorrect;
    public float responseTimes;
}



