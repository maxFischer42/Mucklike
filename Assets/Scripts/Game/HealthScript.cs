using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HealthScript : MonoBehaviour
{
    public enum propType { Tree, Container, Chest, Prop, BigProp};
    public propType myPropType = 0; //default to tree
    public int maxHp;
    public GameObject hurtEffect;
    private PropManager myManager;
    public string myName;
    public bool isDisabled = false;
    public GameObject itemDrop;
    public Vector2Int dropRange = new Vector2Int(5, 10);
    private void Start()
    {
        switch(myPropType)
        {
            case propType.Tree:
                myManager = GameObject.Find("TreeManager").GetComponent<PropManager>();
                break;
            case propType.Container:
                myManager = GameObject.Find("ContainerManager").GetComponent<PropManager>();
                break;
            case propType.Chest:
                myManager = GameObject.Find("ChestManager").GetComponent<PropManager>();
                break;
            case propType.Prop:
                myManager = GameObject.Find("PropManager").GetComponent<PropManager>();
                break;
            case propType.BigProp:
                myManager = GameObject.Find("BigPropManager").GetComponent<PropManager>();
                break;
        }
    }

    public void SetName(string val)
    {
        myName = val;
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.tag);
        if (other.tag == "Weapon")
        {
            Damage(other.GetComponent<DamageHolder>().damage);
        }
    }

    void Damage(int dmg) 
    {
        maxHp -= dmg;
        myManager.SpawnParticleEffect(myName, 1);
        myManager.ChangeHealth(myName, maxHp);
        if (maxHp <= 0)
        {
            myManager.SpawnParticleEffect(myName, 2);
            //myManager.SpawnParticleEffect(myName, 3);
            //myManager.RemoveObject(myName);
            myManager.SpawnItemDrops(itemDrop.name, Random.Range(dropRange.x, dropRange.y), transform.position + Vector3.up);
            Destroy(gameObject, 0.5f);
        }

    }

    public void SpawnParticles(int _numOfParticles, float _lifetime)
    {
        Vector3 pos = transform.position;

        for (int c = 0; c < _numOfParticles; c++)
        {
            GameObject particle = (GameObject)Instantiate(hurtEffect, pos + hurtEffect.transform.position, Quaternion.identity);
            particle.transform.parent = null;
            Destroy(particle, _lifetime);
        }
    }

}
