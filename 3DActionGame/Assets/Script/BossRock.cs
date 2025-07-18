using System.Collections;
using UnityEngine;

public class BossRock : Bullet
{

    Rigidbody rigid;
    float angularPower = 2;
    float scaleValue = 0.1f;
    bool IsShoot;

    void Awake()
    {
        Application.targetFrameRate = 60;
        rigid = GetComponent<Rigidbody>();
        StartCoroutine(GainPowerTimer());
        StartCoroutine(GainPower());
    }

    IEnumerator GainPowerTimer()
    {
        yield return new WaitForSeconds(2.2f);
        IsShoot = true;
    }

    IEnumerator GainPower()
    {
        while (!IsShoot)
        {
            angularPower += 0.02f;
            scaleValue += 0.005f;
            transform.localScale = Vector3.one * scaleValue;
            rigid.AddTorque(transform.right * angularPower, ForceMode.Acceleration);
            yield return null;
        }
    }

}
