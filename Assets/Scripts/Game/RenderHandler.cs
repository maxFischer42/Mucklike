using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderHandler : MonoBehaviour
{
    private void Start()
    {
        GetComponentInChildren<Renderer>().enabled = false;
        GetComponentInChildren<Rigidbody>().isKinematic = false;
    }
}
