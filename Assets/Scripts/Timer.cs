using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timer;
    public Text timer2;
    public float time;
    public float msec;
    public float sec;
    public float min;
    public float cleartime;

    void Start()
    {
        StartCoroutine("StopWatch");
    }

    IEnumerator StopWatch()
    {
        while(true)
        {
            time += Time.deltaTime;
            msec = (int)((time-(int)time) * 100);
            sec = (int)(time % 60);
            min =(int)(time/60 % 60);
            
            timer.text = string.Format("{0:00}:{1:00}:{2:00}",min,sec,msec);
            timer2.text = string.Format("{0:00}:{1:00}:{2:00}",min,sec,msec);
            yield return null;
        }
    }
}
