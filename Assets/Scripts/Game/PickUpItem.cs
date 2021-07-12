using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PickUpItem : MonoBehaviour
{
    public ItemScriptableObject item;
    public int amount;

    private void OnTriggerEnter(Collider other)
    {
        print("pickup");
        if(other.gameObject.tag == "Pickup")
        {
            // Add item to player inventory
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
