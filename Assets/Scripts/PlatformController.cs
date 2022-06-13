using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public Rigidbody platformRB;
    public Transform[] platformPositions;
    public float platformSpeed;

    private int actualPosition = 0;
    private int nextPosition = 1;

    public bool moveTotheNext = true;
    public float waitTime;

    private void Update()
    {
        MovePlatform();    
    }

    private void MovePlatform()
    {
        if (moveTotheNext)
        {
            StopCoroutine(WhaitForMove(0));
            platformRB.MovePosition(Vector3.MoveTowards(platformRB.position, platformPositions[nextPosition].position, platformSpeed * Time.deltaTime));
        }

        if (Vector3.Distance(platformRB.position, platformPositions[nextPosition].position) <= 0)
        {
            StartCoroutine(WhaitForMove(waitTime));
            actualPosition = nextPosition;
            nextPosition++;
            if(nextPosition > platformPositions.Length - 1)
            {
                nextPosition = 0;
            }
        }
    }

    IEnumerator WhaitForMove(float time)
    {
        moveTotheNext = false;
        yield return new WaitForSeconds(time);
        moveTotheNext = true;
    }
}
