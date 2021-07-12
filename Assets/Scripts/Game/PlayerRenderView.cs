using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRenderView : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("PropLayer") && GetComponentInChildren<Renderer>())
        {
            GetComponentInChildren<Renderer>().enabled = true;
            GetComponentInChildren<Rigidbody>().isKinematic = true;
        } 
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PropLayer") && GetComponentInChildren<Renderer>())
        {
            GetComponentInChildren<Renderer>().enabled = false;
            GetComponentInChildren<Rigidbody>().isKinematic = false;
        }
    }
}
