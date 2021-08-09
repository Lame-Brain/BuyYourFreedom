using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Script : MonoBehaviour
{
    public bool followPlayer;
    public float cameraMoveSpeed;

    Vector3 _targetPos;

    void Update()
    {
        if (followPlayer && GameObject.FindGameObjectWithTag("Player") != null) 
        { 
            _targetPos.x = GameObject.FindGameObjectWithTag("Player").transform.position.x; 
            _targetPos.y = GameObject.FindGameObjectWithTag("Player").transform.position.y;
            _targetPos.z = -10;
        }
        else
        {
            _targetPos.x = 0f;
            _targetPos.y = 0f;
            _targetPos.z = -10;
        }

        transform.position = Vector3.Lerp(transform.position, _targetPos, cameraMoveSpeed * Time.deltaTime);
    }
}
