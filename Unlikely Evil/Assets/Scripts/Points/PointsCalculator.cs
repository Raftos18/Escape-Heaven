using UnityEngine;
using System.Collections;

public class PointsCalculator : MonoBehaviour
{
    private int count;
    private int sum;
    private Movement player;

	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
	}
	
    public float CalculateScore()
    {
        return (CalculateAverageSpeed(player.Speed) * CalculateDistanceTraveled()) / 2;
    }


    float CalculateAverageSpeed(float speed)
    {
        count++;
        sum += (int)speed;
        return sum / count;
    }

    float CalculateDistanceTraveled()
    {
        CorridorGenerator rg = player.GetComponent<CorridorGenerator>();
        int timesReseted = rg.TimesReseted;
        int resetPoint = rg.ResetPoint;

        return (resetPoint * timesReseted) + player.transform.position.z;
    }
}
