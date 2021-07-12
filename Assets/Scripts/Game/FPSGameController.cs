using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSGameController : MonoBehaviour
{

    public enum HitboxTypes { axe};
    public GameObject axeHitbox;

    public void ReceiveMessage(string msg)
    {
        switch(msg)
        {
            // Spawn Axe Hitbox
            case "axe":
                HandleHitbox(HitboxTypes.axe);
                break;
        }
    }

    public void HandleHitbox(HitboxTypes type)
    {
        GameObject gObj = (GameObject)Instantiate(axeHitbox);
        gObj.transform.position = transform.position;
        gObj.transform.rotation = transform.rotation;
        float lifetime = 0.1f;
        Destroy(gObj, lifetime);
    }

}
