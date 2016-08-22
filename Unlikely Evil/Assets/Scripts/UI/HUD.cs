using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour
{
    public Text Score;
    public Text Speed;
    public Text Distance;

    private Movement player;
    private Hunter hunter;

    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        hunter = GameObject.FindGameObjectWithTag("Hunter").GetComponent<Hunter>();
    }
	
	void Update ()
    {
        if (!player.isDead && player.StartGame)
        {
            ToggleHud(true);

            Score.text = "Score : " + ((int)GameObject.FindGameObjectWithTag("PointsManager").GetComponent<PointsCalculator>().CalculateScore()).ToString();
            Speed.text = "Speed : " + player.Speed.ToString();


            float distance = player.transform.position.z - hunter.transform.position.z;
            if (distance > 0)
                Distance.text = "Distance : " + ((int)distance).ToString();
            else
                Distance.text = "Distance : Purified";
        }
        else if(player.isDead)
        {
            ToggleHud(false);
        }
    }

    public void ToggleHud(bool activate)
    {
        Score.gameObject.SetActive(activate);
        Speed.gameObject.SetActive(activate);
        Distance.gameObject.SetActive(activate);   
    }
}
