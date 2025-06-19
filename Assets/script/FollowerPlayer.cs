using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerPlayer : MonoBehaviour
{
    public Transform player;
    public Vector3 offest;
    private void LateUpdate()
    {
        if (player != null) 
        {
            Vector3 targetPosition = player.position+offest;
            transform.position = targetPosition;
        }
    }
}
