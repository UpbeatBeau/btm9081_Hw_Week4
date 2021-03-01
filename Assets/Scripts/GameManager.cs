using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //static variable means the value is the same for all the objects of this class type and the class itself
    public static GameManager instance; //this static var will hold the Singleton

    private int score = 0;

    private bool isGame = true;
    public float gameTime = 10;
    public Text timerText;
    private float timer = 0;

    const string DIR_LOGS = "/Logs";
    const string FILE_SCORES = DIR_LOGS + "/highScore.txt";
    const string FILE_ALL_SCORES = DIR_LOGS + "/allScores.csv";
    string FILE_PATH_HIGH_SCORE;
    string FILE_PATH_ALL_SCORES;

    public int Score
    {
        get { return score; }
        set
        {
            score = value;

            //Debug.Log("Someone set the Score!");
            /*if (score > highScore)
            {
                highScores = score;
            }

            string fileContents = "";
            if (File.Exists(FILE_PATH_ALL_SCORES))
            {
                fileContents = File.ReadAllText(FILE_PATH_ALL_SCORES);
            }

            fileContents += score + ",";
            File.WriteAllText(FILE_PATH_ALL_SCORES, fileContents);*/

        }
    }

    private List<int> highScores;
    
    //const string PREF_KEY_HIGH_SCORE = "HighScoreKey";
    //int highScore = -1;

    /*public int HighScore
    {
        get
        {
            if (highScore < 0)
            {
                //highScore = PlayerPrefs.GetInt(PREF_KEY_HIGH_SCORE, 3);
                if (File.Exists(FILE_PATH_HIGH_SCORES))
                {
                    string fileContents = File.ReadAllText(FILE_PATH_HIGH_SCORES);
                    highScore = Int32.Parse(fileContents);
                }
                else
                {
                    highScore = 3;
                }
            }

            return highScore;
        }
        set
        {
            highScore = value;
            Debug.Log("New High Score!!!");
            Debug.Log("File Path: " + FILE_PATH_HIGH_SCORES);
            //PlayerPrefs.SetInt(PREF_KEY_HIGH_SCORE, highScore);

            if (!File.Exists(FILE_PATH_HIGH_SCORES))
            {
                Directory.CreateDirectory(Application.dataPath + DIR_LOGS);
                //File.Create(FILE_PATH_HIGH_SCORES);
            }

            File.WriteAllText(FILE_PATH_HIGH_SCORES, highScore + ",");
        }
    }*/

    int targetScore = 3;

    int currentLevel = 0;

    void Awake()
    {
        if (instance == null) //instance hasn't been set yet
        {
            DontDestroyOnLoad(gameObject);  //Dont Destroy this object when you load a new scene
            instance = this;  //set instance to this object
        }
        else  //if the instance is already set to an object
        {
            Destroy(gameObject); //destroy this new object, so there is only ever one
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        FILE_PATH_HIGH_SCORE = Application.dataPath + FILE_SCORES;
        FILE_PATH_ALL_SCORES  = Application.dataPath + FILE_ALL_SCORES;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (!isGame)
        {
            string highScoreString = "High Scores\n\n";

            for (var i = 0; i < highScores.Count; i++)
            {
                highScoreString += highScores[i] + "\n";
            }

            timerText.text = highScoreString;
        }
        else
        {
            timerText.text = "Time: " + (int) (gameTime - timer) + " Level:" + currentLevel +
                             "\nScore: " + score + " Target: " + targetScore;
            //"\nHigh Score: " + HighScore;
        }
        
        if (gameTime < timer && isGame) //Time is up
        {
            SceneManager.LoadScene(4);
            isGame = false;
            UpdateHighScores();
        }
      
        
        if (score == targetScore)  //if the current score == the targetScore
        {
            currentLevel++; //increase the level number
            SceneManager.LoadScene(currentLevel); //go to the next level
            targetScore += targetScore + targetScore/2; //update target score
        }
    }
    
    void UpdateHighScores()
    {
        if (highScores == null) //if we don't have the high scores yet
        {
            highScores = new List<int>();

            string fileContents = File.ReadAllText(FILE_PATH_HIGH_SCORE);

            string[] fileScores = fileContents.Split(',');
            
            print(fileScores.Length);

            for (var i = 0; i < fileScores.Length - 1; i++)
            {
                highScores.Add(Int32.Parse(fileScores[i]));
            }
        }

        for (var i = 0; i < highScores.Count; i++)
        {
            if (score > highScores[i])
            {
                highScores.Insert(i, score);
                break;
            }
        }

        string saveHighScoreString = "";

        for (var i = 0; i < highScores.Count; i++)
        {
            saveHighScoreString += highScores[i] + ",";
        }

        File.WriteAllText(FILE_PATH_HIGH_SCORE, saveHighScoreString);
    }
}
