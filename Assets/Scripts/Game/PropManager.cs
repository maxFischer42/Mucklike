using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropManager : MonoBehaviourPunCallbacks
{

    private const byte
        SEND_DROP_AMOUNT_EVENT = 2;

    public Dictionary<string, int> propDictionary = new Dictionary<string, int>();
    public List<GameObject> objectList = new List<GameObject>();
    public List<int> healthList = new List<int>();
    public int size = 0;

    public void AddItem(GameObject prop)
    {
        propDictionary.Add(prop.name, size);
        size++;
        objectList.Add(prop);
        healthList.Add(prop.GetComponentInChildren<HealthScript>().maxHp);
        base.photonView.RPC("RPC_AddItem", RpcTarget.Others, prop.name, prop.GetComponentInChildren<HealthScript>().maxHp);
    }

    [PunRPC]
    public void RPC_AddItem(string name, int hp)
    {
        propDictionary.Add(name, size);
        size++;
        GameObject temp = GameObject.Find(name);
        objectList.Add(temp);
        healthList.Add(hp);
    }



    public void ChangeHealth(string objectName, int newHealth)
    {
        int index = propDictionary[objectName];
        healthList[index] = newHealth;
        base.photonView.RPC("RPC_ChangeHealth", RpcTarget.Others, objectName, newHealth);
    }

    [PunRPC]
    public void RPC_ChangeHealth(string objectName, int newHealth)
    {
        int index = propDictionary[objectName];
        healthList[index] = newHealth;
    }

    public void RemoveObject(string objectName)
    {
        //StartCoroutine(delayRemoveObject(objectName));

        base.photonView.RPC("RPC_RemoveObject", RpcTarget.Others, objectName);
        int index = propDictionary[objectName];
        GameObject _obj = GameObject.Find(objectName);
        GameObject.Find("ItemActivatorObject").GetComponent<ItemActivator>().RemoveFromList(_obj);
        objectList.RemoveAt(index);
        propDictionary.Remove(objectName);
        healthList.RemoveAt(index);
        Destroy(objectList[index].gameObject);
        size--;
    }

    [PunRPC]
    public void RPC_RemoveObject(string objectName)
    {
        //StartCoroutine(delayRemoveObject(objectName));
        int index = propDictionary[objectName];
        GameObject _obj = GameObject.Find(objectName);
        GameObject.Find("ItemActivatorObject").GetComponent<ItemActivator>().RemoveFromList(_obj);
        objectList.RemoveAt(index);
        propDictionary.Remove(objectName);
        healthList.RemoveAt(index);
        Destroy(objectList[index].gameObject);
        size--;
    }

    /*IEnumerator delayRemoveObject(string objectName)
    {
        int index = propDictionary[objectName];
        GameObject _obj = GameObject.Find(objectName);
        GameObject.Find("ItemActivatorObject").GetComponent<ItemActivator>().RemoveFromList(_obj);
        yield return new WaitForSeconds(0.5f);
        objectList.RemoveAt(index);
        propDictionary.Remove(objectName);
        healthList.RemoveAt(index);
        Destroy(objectList[index].gameObject);
        size--;
        
    }*/

    public void SpawnParticleEffect(string objectName, int particleCount)
    {
        base.photonView.RPC("RPC_SpawnParticleEffect", RpcTarget.Others, objectName, particleCount);
        int index = propDictionary[objectName];
        objectList[index].GetComponentInChildren<HealthScript>().SpawnParticles(particleCount, 3f);
    }

    [PunRPC]
    public void RPC_SpawnParticleEffect(string objectName, int particleCount)
    {
        int index = propDictionary[objectName];
        objectList[index].GetComponentInChildren<HealthScript>().isDisabled = true;
        objectList[index].GetComponentInChildren<HealthScript>().SpawnParticles(particleCount, 3f);
    }

    public void SpawnItemDrops(string _drop, int _amount, Vector3 _spawnPos)
    {
        GameObject _gameObject = (GameObject)PhotonNetwork.InstantiateRoomObject(_drop, _spawnPos, Quaternion.identity);
        _gameObject.GetComponent<PickUpItem>().amount = _amount;
        object[] data = new object[] { _amount, _gameObject.name };
        PhotonNetwork.RaiseEvent(SEND_DROP_AMOUNT_EVENT, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);
    }

    private void NetworkingClient_EventReceived(EventData obj)
    {
        if (obj.Code == SEND_DROP_AMOUNT_EVENT)
        {
            object[] datas = (object[])obj.CustomData;
            int data = (int)datas[0];
            GameObject.Find((string)datas[1]).GetComponent<PickUpItem>().amount = data;
        }
    }

}
