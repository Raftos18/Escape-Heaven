using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;


public class MainMenu : MonoBehaviour
{
    public AudioSource Audio;
    public GameObject[] MainMenuElements;
    public event EventHandler Restarted;                // Event called when game is restarted

    private Movement player;
    private Death death;
    private Hunter hunter;
    private ScoreSaver scoreSaver;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        death = player.GetComponent<Death>();
        hunter = GameObject.FindGameObjectWithTag("Hunter").GetComponent<Hunter>();
        scoreSaver = gameObject.GetComponent<ScoreSaver>();
    }

    void Update()
    {
        if (player.isDead)
        {
            ToogleMainMenuElements(false);
            ShowFinalScore();

        }

        if(death.CanRestart)
            ToogleRestart(true);
        else
            ToogleRestart(false);
    }

    public void StartGame()
    {
        if (!player.StartGame)
        {
            player.StartGame = true;
            player.Reset();
            Vector3 resetPos = new Vector3(0, 0.5f, -15);
            hunter.Reset(resetPos);

            ToogleMainMenuElements(!player.StartGame);
        }
    }

    bool isPlaying = true;
    public void ToggleAudio()
    {
        if (isPlaying)
        {
            isPlaying = false;
            Audio.enabled = false;
        }
        else
        {
            isPlaying = true;
            Audio.enabled = true;
        }
    }

    

    protected virtual void OnRestarted(EventArgs e)
    {
        var handler = Restarted;
        if(handler != null)
            handler(this, e);
    }

    private void ToogleMainMenuElements(bool activate)
    {
        for (int i = 0; i < 3; i++)
        {
            MainMenuElements[i].SetActive(activate);
        }
    }

    private void ToogleRestart(bool activate)
    {
        MainMenuElements[2].SetActive(activate);
        MainMenuElements[3].SetActive(activate);
        MainMenuElements[4].SetActive(activate);

        MainMenuElements[5].SetActive(activate);

        scoreSaver.DisplayScores(scoreSaver.ReadScores());
    }

    public static bool ShowScore = false;
    public void ShowFinalScore()
    {
        if (ShowScore)
        {
            ShowScore = false;
            PointsCalculator pc = GameObject.FindGameObjectWithTag("PointsManager").GetComponent<PointsCalculator>();
            int score = ((int)pc.CalculateScore());
            MainMenuElements[4].GetComponent<Text>().text = "Score : " + score.ToString();
        }
    }

    public void RestartGame()
    {
        OnRestarted(EventArgs.Empty);
        var death = player.gameObject.GetComponent<Death>();
        if (death.CanRestart)
        {
            player.Reset();
            death.CanRestart = false;
            hunter.Reset(new Vector3(0, 0.5f, -15));
            ToogleRestart(false);
        }
    }

    public void ExitGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
