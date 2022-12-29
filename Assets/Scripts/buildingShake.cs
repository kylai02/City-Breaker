using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildingShake : MonoBehaviour
{

    public float shakeAmount = 0.1f;
    bool is_shake;
    Vector3 first_pos;
    
    private void Start()
    {
        first_pos = this.transform.localPosition;
    }

    public void startShake()
    {
        is_shake = true;

    }
   
    public void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            startShake();
        }
        if (Input.GetKeyDown(KeyCode.S)) 
        {
            endShake();
        }

        if (!is_shake) return;
        Vector3 pos = first_pos + Random.insideUnitSphere * shakeAmount;
        pos.y = transform.localPosition.y;
        transform.localPosition = pos;
    }

    public void endShake()
    {
        is_shake = false;
        first_pos.y = transform.localPosition.y;
        transform.localPosition = first_pos;
    }

    
}
