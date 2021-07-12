using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotate : MonoBehaviour
{

    private void Start()
    {
        float x = Random.Range(-180f,180f);
        float y = Random.Range(-180f, 180f);
        float z = Random.Range(-180f, 180f);
        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(x, y, z));
    }

}
