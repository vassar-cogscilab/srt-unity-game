using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using UnityEngine.SceneManagement;

public class Trial6 : MonoBehaviour
{
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
    //private Text Multiplier;
    private int score;
   // private int Streak;
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
    public int grammars;




    private void Awake()
    {
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
        //Multiplier = GameObject.Find("Multiplier").GetComponent<Text>();
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
        laneButtons = new GameObject[lanes];
        currentSequence = 1;

        sequenceLength = 30;
        cfig.Pattern = new int[sequenceLength];
        Grammar(grammars);
        for (int j = 0; j < cfig.Pattern.Length; j++)
        {
            Pattern.Push(cfig.Pattern[j]);
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
            shift[i].transform.position = new Vector3((x1 * i) - (camWidth / 2f) + (x1 / 2), -3, -23);
            sRender = shift[i].GetComponent<SpriteRenderer>();
            sRender.sprite = keySprites[k];
            laneButton.GetComponent<LaneButton>().laneNumber = i;
            laneButtons[i] = Sprite.Instantiate(laneButton, ButtonStorage) as GameObject;
            laneButtons[i].transform.position = new Vector3((x1 * i) - (camWidth / 2f)+(x1/2), -3, -10);
            laneButtons[i].transform.localScale = new Vector3(10f / lanes, 10f / lanes, 1);
            k++;



        }
        trials = Pattern.Count;
        totalTrials = Pattern.Count;
        answer = Pattern.Pop();
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
        SpeedUp.Play();
        totalTime = totalTime + timer.ElapsedMilliseconds;
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        inputPressed.Push(keyPressed);
        correctInput.Push(keyPressed);
        wasInputCorrect.Push(true);
        block.Push(level);
        responseTimes.Push(timer.ElapsedMilliseconds);
        sequenceInput.Push(currentSequence);
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
        SpeedUp.Play();
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        trials += 1;
        inputPressed.Push(keyPressed);
        correctInput.Push(answer);
        wasInputCorrect.Push(false);
        block.Push(level);
        responseTimes.Push(timer.ElapsedMilliseconds);
        sequenceInput.Push(currentSequence);
        loading = false;
    }

    void endCorrectAnswer()
    {
        time.Stop();
        levelText.text = "Level " + level + " Complete!";
        block.Push(level);
        level++;
        running = false;
        SpeedUp.Play();
        totalTime = totalTime + timer.ElapsedMilliseconds;
        UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        //percentCorrect.text = ((totalTrials / trials) * 100 + "% correct");
        responseTime.text = "You completed the level in " + time.ElapsedMilliseconds / 1000f + " seconds" ;
        if ((time.ElapsedMilliseconds < bestScore) || (bestScore == 0))
        {
            bestScore = time.ElapsedMilliseconds;
            bestScoreText.text = "Best Score: " + bestScore / 1000f;
        }
        UnityEngine.Debug.Log(responseTime.text);
        //UnityEngine.Debug.Log(percentCorrect.text);
        inputPressed.Push(keyPressed);
        correctInput.Push(keyPressed);
        wasInputCorrect.Push(true);
        responseTimes.Push(timer.ElapsedMilliseconds);
        sequenceInput.Push(currentSequence);
        currentSequence++;
        if (currentSequence > sequenceLength)
        {
            currentSequence = 1;
        }
        StartCoroutine(Upload());
        if (level > cfig.Repetitions)
        {
            levelText.text = "All Levels Completed";
        }
        if (((totalTrials / trials) * 100) > 90)
        {
            GameObject.Find("Progress").GetComponent<Progress>().level2 = true;
        }
    }

    void beginLevel()
    {
        StartCoroutine(begining());
    }

    void StartLevel()
    {
        if (level <= cfig.Repetitions)
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
        for (int i = 0; i < lanes; i++)
        {
            sRender = shift[i].GetComponent<SpriteRenderer>();
            sRender.sprite = sprites[0];
        }
        startingAnimation.SetInteger("seconds", 0);
        sRender = carz[answer].GetComponent<SpriteRenderer>();
        running = true;
        loading = true;
        time.Start();
    }
    
    IEnumerator startNextLevel()
    {
        time = new Stopwatch();
        scoreBox.text = ("Time: " + 0);
        endPanel.SetActive(false);
        Grammar(grammars);
        for (int j = 0; j < cfig.Pattern.Length; j++)
        {
                Pattern.Push(cfig.Pattern[j]);
        }

        trials = Pattern.Count;
        totalTrials = Pattern.Count;
        answer = Pattern.Pop();
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
    IEnumerator Upload()
    {
        json = "]";
        WWWForm form = new WWWForm();
        string[] myData = new string[inputPressed.Count];

        int i = inputPressed.Count-1;
        int k = 10;
        while(inputPressed.Count != 0)
        {
            TrialData storage = new TrialData();
            storage.trialIndex = i+1;
            storage.sequenceIndex = sequenceInput.Pop();
            storage.inputPressed = inputPressed.Pop();
            storage.block = block.Pop();
            storage.correctInput = correctInput.Pop() ;
            storage.wasInputCorrect = wasInputCorrect.Pop();
            storage.responseTimes = responseTimes.Pop();
            inputPressed1.Push(storage.inputPressed);
            correctInput1.Push(storage.correctInput);
            wasInputCorrect1.Push(storage.wasInputCorrect);
            responseTimes1.Push(storage.responseTimes);
            sequenceInput1.Push(storage.sequenceIndex);
            block1.Push(storage.block);
            if (json == "]")
            {
                json = JsonUtility.ToJson(storage) + "\n" + json;
            }
            else
            {
                json = JsonUtility.ToJson(storage) + ",\n" + json;
            }
            i--;
            k--;
            if (k == 0)
            {
                k = 10;
            }
        }
        json = "[" + json;
        while (inputPressed1.Count != 0)
        {
            sequenceInput.Push(sequenceInput1.Pop());
            inputPressed.Push(inputPressed1.Pop());
            block.Push(block1.Pop());
            correctInput.Push(correctInput1.Pop());
            wasInputCorrect.Push(wasInputCorrect1.Pop());
            responseTimes.Push(responseTimes1.Pop());

        }


        form.AddField("myData", json);
        string url = "http://jdeleeuw.vassarcis.net/SRT-Game/data.php";
        using (var www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                UnityEngine.Debug.Log(www.error);
            }
            else
            {
                UnityEngine.Debug.Log("Upload complete!");
                UnityEngine.Debug.Log(www.downloadHandler.text);
            }
        }
    }
    void grammar1()
    {
       int startingNumber =  UnityEngine.Random.Range(0, lanes - 1);
        cfig.Pattern[lanes - 1] = startingNumber;
        int switcher = 0;
        for(int i = cfig.Pattern.Length -1; i>=0; i--)
        {
            if(startingNumber == 0)
            {
                startingNumber = 1;
            }
            else if (startingNumber == 1)
            {
                startingNumber = 2;
            }
            else if (startingNumber == 2)
            {
                startingNumber = 3;
            }
            else if (startingNumber == 3)
            {
                startingNumber = UnityEngine.Random.Range(4,6);
            }
            else if (startingNumber == 4)
            {
                switcher = UnityEngine.Random.Range(0, 2);
                if (switcher == 0)
                {
                    startingNumber = 5;
                }
                else
                {
                    startingNumber = 0;
                }
            }
            else if (startingNumber == 5)
            {
                startingNumber = UnityEngine.Random.Range(0, 2);
            }
            cfig.Pattern[i] = startingNumber;

        }
    }
    void grammar2()
    {
        int startingNumber = UnityEngine.Random.Range(0, lanes - 1);
        cfig.Pattern[lanes - 1] = startingNumber;
        int switcher = 0;
        for (int i = cfig.Pattern.Length - 1; i >= 0; i--)
        {
            if (startingNumber == 0)
            {
                startingNumber = UnityEngine.Random.Range(1, 3);
            }
            else if (startingNumber == 1)
            {
                startingNumber = UnityEngine.Random.Range(2, 4);
            }
            else if (startingNumber == 2)
            {
                startingNumber = UnityEngine.Random.Range(3, 5);
            }
            else if (startingNumber == 3)
            {
                startingNumber = UnityEngine.Random.Range(4, 6);
            }
            else if (startingNumber == 4)
            {
                switcher = UnityEngine.Random.Range(0, 2);
                if (switcher == 0)
                {
                    startingNumber = 5;
                }
                else
                {
                    startingNumber = 0;
                }
            }
            else if (startingNumber == 5)
            {
                startingNumber = UnityEngine.Random.Range(0, 2);
            }
            cfig.Pattern[i] = startingNumber;

        }
    }
    void grammar3()
    {
        int startingNumber = UnityEngine.Random.Range(0, lanes - 1);
        cfig.Pattern[lanes - 1] = startingNumber;
        int switcher = 0;
        for (int i = cfig.Pattern.Length - 1; i >= 0; i--)
        {
            if (startingNumber == 0)
            {
                startingNumber = UnityEngine.Random.Range(1, 4);
            }
            else if (startingNumber == 1)
            {
                startingNumber = UnityEngine.Random.Range(2, 5);
            }
            else if (startingNumber == 2)
            {
                startingNumber = UnityEngine.Random.Range(3, 6);
            }
            else if (startingNumber == 3)
            {
                switcher = UnityEngine.Random.Range(0, 3);
                if (switcher == 0)
                {
                    startingNumber = 4;
                }
                else if (switcher == 1)
                {
                    startingNumber = 5;
                }
                else
                {
                    startingNumber = 0;
                }
            }
            else if (startingNumber == 4)
            {
                switcher = UnityEngine.Random.Range(0, 3);
                if (switcher == 0)
                {
                    startingNumber = 5;
                }
                else if(switcher==1)
                {
                    startingNumber = 0;
                }
                else
                {
                    startingNumber = 1;
                }
            }
            else if (startingNumber == 5)
            {
                startingNumber = UnityEngine.Random.Range(0, 3);
            }
            cfig.Pattern[i] = startingNumber;

        }
    }

    void Grammar(int grammar)
    {
        if (grammar == 1)
        {
            grammar1();   
        }
        else if (grammar == 2)
        {
            grammar2();
        }
        else if (grammar == 3)
        {
            grammar3();
        }
    }
}





