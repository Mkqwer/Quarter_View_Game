using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
   public Transform Target;
   public Vector3 Offset;

    void Update()
    {
        transform.position = Target.position + Offset;
    }
}
