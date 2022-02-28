using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlatform : MonoBehaviour
{
    public bool coll;
    public PlatformEffector2D effector;

    public void Update()
    {
        if(coll && Input.GetKey(KeyCode.S))
        {
            effector.surfaceArc = 0f;
            StartCoroutine(Wait());
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        coll = true;
    }

    void OnCollisionExit2D(Collision2D other) 
    {
        coll = false;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.3f);
        effector.surfaceArc = 180f;
    }
}
