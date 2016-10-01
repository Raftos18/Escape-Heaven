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

    /// <summary>
    /// Calculate the average speed.
    /// </summary>
    /// <param name="speed"></param>
    /// <returns></returns>
    float CalculateAverageSpeed(float speed)
    {
        count++;
        sum += (int)speed;
        return sum / count;
    }

    /// <summary>
    /// Calculates the distance traveled
    /// </summary>
    /// <returns></returns>
    float CalculateDistanceTraveled()
    {
        CorridorGenerator rg = player.GetComponent<CorridorGenerator>();
        int timesReseted = rg.TimesReseted;
        int resetPoint = rg.ResetPoint;

        // Take into account the times the level has reseted.
        return (resetPoint * timesReseted) + player.transform.position.z;
    }
}
