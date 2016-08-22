using UnityEngine;
using System;
using System.Collections;


public class Utility : MonoBehaviour
{
   public static void ExecuteAfter(Action action, ref float nextExec, float cooldown)
    {
        if(Time.time > nextExec)
        {
            nextExec = Time.time + cooldown;

            action();
        }
    }
}
