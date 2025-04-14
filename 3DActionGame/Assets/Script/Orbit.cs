using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform target;
    public float OrbitSpeed;
    Vector3 OffSet;

    void Start()
    {
        OffSet = transform.position - target.position;
    }

    void Update()
    {
        transform.position = target.position + OffSet;
        transform.RotateAround(target.position,
                               Vector3.up,
                               OrbitSpeed * Time.deltaTime);
        OffSet = transform.position = target.position;
    }


}
