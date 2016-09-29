using UnityEngine;
using UnityEngine.UI;

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class ScoreSaver : MonoBehaviour
{
    public GameObject IScoreSaver;
    public InputField InputName;
    public Text[] Scores;

    const int SCORES_ALLOWED = 5;
    const char SEPERATOR = ':';
    const string FILE_NAME = "Scores.txt";
    const string PATH = "./Scores/" + FILE_NAME;

    private StreamWriter writer;
    private StreamReader reader;

    private Movement player;
    private MainMenu mainMenu;
    private HUD hud;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        mainMenu = gameObject.GetComponent<MainMenu>();
        hud = gameObject.GetComponent<HUD>();
        mainMenu.Restarted += MainMenu_Restarted;
    }

    private void MainMenu_Restarted(object sender, EventArgs e)
    {
        string name = InputName.text;
        int score = int.Parse(hud.Score.text.Split(SEPERATOR)[1]);
        if (SaveScore(name, score))
        {
            Debug.Log("Saved successfully");
            return;
        }
        else
        {
            ReplaceSmallestScore(ReadScores(), name, score, SaveScore);
        }
    }

    void Update()
    {

    }

    #region Could be moved to H.U.D
    /// <summary>
    /// Needs fixing in the inspector
    /// </summary>
    /// <param name="activate"></param>
    public void ToggleScoreSaver(bool activate)
    {
        IScoreSaver.SetActive(activate);
    }

    /// <summary>
    /// Ready
    /// </summary>
    /// <param name="scores"></param>
    public void DisplayScores(Dictionary<string, int> scores)
    {
        var query = scores.OrderByDescending(scr => scr.Value);

        int index = 0;
        foreach (var score in query)
        {
            Scores[index].text = score.Key + ": " + score.Value;
            index++;
        }
    }
    #endregion

    /// <summary>
    /// Ready
    /// </summary>
    /// <returns></returns>
    private bool CanSave()
    {
        using (reader = new StreamReader(PATH))
        {
            string line = default(string);
            int scoresFound = 0;
            while ((line = reader.ReadLine()) != null) scoresFound++;
            if (scoresFound >= SCORES_ALLOWED) return false; else return true;
        }
    }

    /// <summary>
    /// Ready
    /// </summary>
    /// <param name="scores"></param>
    /// <param name="fn"></param>
    private void ReplaceSmallestScore(Dictionary<string, int> scores, string name, int score, Func<string, int, bool> saveFunc)
    {
        File.Delete(PATH);
        string minScoreKey = MinScore(scores);

        scores.Remove(minScoreKey);
        scores[name] = score;

        foreach (var sc in scores)
            saveFunc(sc.Key, sc.Value);
    }

    /// <summary>
    /// Ready
    /// </summary>
    /// <param name="scores"></param>
    /// <returns></returns>
    private string MinScore(Dictionary<string, int> scores)
    {
        var smallest = scores.First();
        foreach (var score in scores)
            if (smallest.Value > score.Value)
                smallest = score;

        return smallest.Key;
    }

    /// <summary>
    /// Ready
    /// </summary>
    /// <param name="filename"></param>
    private void TryCreateFile(string filename)
    {
        if (!File.Exists(filename))
            File.Create(filename).Close();
    }

    private string IsNameEmpty(string playerName)
    {
        if (playerName == "") return "unknown";
        else return playerName;
    }

    /// <summary>
    /// Ready
    /// </summary>
    /// <param name="playerName"></param>
    /// <param name="score"></param>
    /// <returns></returns>
    public bool SaveScore(string playerName, int score)
    {
        TryCreateFile(PATH);
        playerName = IsNameEmpty(playerName);
        bool canSave = CanSave();
        if (canSave)
            using (writer = new StreamWriter(PATH, true))
                writer.WriteLine(playerName + SEPERATOR + score);

        return canSave;
    }

    /// <summary>
    /// Ready
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, int> ReadScores()
    {
        Dictionary<string, int> scores = new Dictionary<string, int>();
        if (File.Exists(PATH))
        {
            using (reader = new StreamReader(PATH))
            {
                string line = default(string);
                while ((line = reader.ReadLine()) != null)
                {
                    var score = line.Split(SEPERATOR);
                    scores[score[0]] = int.Parse(score[1]);
                }
            }
         
        }
        return scores;
    }
}
