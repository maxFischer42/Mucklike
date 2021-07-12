using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolAnimationHandler : MonoBehaviour
{
    public FPSGameController fps;
    public void Message(string msg)
    {
        fps.ReceiveMessage(msg);
    }

    public Animator anim;

    public float cooldown;
    float currentCooldown;
    bool isCooldown;
    bool isUsing = false;

    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            if (isUsing) return;
            isUsing = true;
            anim.SetBool("useTool", true);
        }
        if (Input.GetButtonUp("Fire1"))
        {
            isUsing = false;
            anim.SetBool("useTool", false);
        }
    }

    void HandleCooldown()
    {
        if(isCooldown)
        {
            currentCooldown += Time.deltaTime;
            if(currentCooldown >= cooldown)
            {
                isCooldown = false;
                currentCooldown = 0f;
            }
        }
    }
}
