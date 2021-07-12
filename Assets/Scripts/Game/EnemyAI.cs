using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float modifier = 1f;

    // Update is called once per frame
    void Update()
    {
        //if ((player.position - transform.position).magnitude > 50)
        //{
        //    transform.LookAt(player);
        //    Vector3 movement = transform.forward * Time.deltaTime * modifier;
        //    GetComponent<NavMeshAgent>().Move(movement);
        //}
        GetComponent<NavMeshAgent>().destination = player.position;
    }
}
