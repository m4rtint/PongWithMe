using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class DebugToolBehaviour : MonoBehaviour
{
        
    public Vector3 Force = Vector3.zero;

    private void Awake()
    {
        GetComponent<Rigidbody2D>().AddForce(Force);
    }


    [Button]
    private void ResetToMiddle()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        transform.position = Vector3.zero;
    }

    [Button]
    public void AddForce()
    {
        GetComponent<Rigidbody2D>().AddForce(Force);
    }
}
