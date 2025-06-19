using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public float maxLeft = -250f;
    public float maxRight = 250f;
    public float moveSpeed = 250f;
    public float changeFrequency = 0.01f;
    public float targetPosition;
    public bool movingRight = true;

    internal void SetDifficulty(FishData fishBitting)
    {
        switch (fishBitting.fishDifficulty)
        {
            case 1:
                moveSpeed = 200;
                return;
            case 2:
                moveSpeed = 300;
                return;
            case 3:
                moveSpeed = 350;
                return;
            case 0:
                Debug.LogError("Difficulty not found for this ");
                moveSpeed = 250;
                return;

        }
    }

    private void Start()
    {
        targetPosition = Random.Range(maxLeft,maxRight);
    }
    private void Update()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(targetPosition, transform.localPosition.z), moveSpeed * Time.deltaTime);
        if (Mathf.Approximately(transform.localPosition.x, targetPosition))
        {
            targetPosition=Random.Range(maxLeft,maxRight);
        }
        if (Random.value<changeFrequency)
        {
            movingRight = !movingRight;
            targetPosition=movingRight?maxRight:maxLeft;
        }
    }
}
