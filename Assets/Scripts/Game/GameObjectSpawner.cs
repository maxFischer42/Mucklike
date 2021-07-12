using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.MapGenerator.Generators;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun;
using ExitGames.Client.Photon;

public class GameObjectSpawner : MonoBehaviourPunCallbacks
{
    private const byte
        SEND_SEED_EVENT = 1;

    private int width;
    private int height;
    private int depth;

    private HeightsGenerator gen;

    public GameObject[] Trees;
    public GameObject[] Containers;
    public GameObject[] Chests;
    public GameObject[] Props;
    public GameObject[] BigProps;

    Transform propParent;

    public float objectSpawnMin = 0.2f;

    public int treeSpawnTries = 75;  
    public float treeSpawnMin = 0.35f;

    public int containerSpawnTries = 2500;

    public int propSpawnTries = 500;

    public int bigPropSpawnTries = 600;

    public PropManager treeManager, containerManager, propManager, bigPropManager;

    public Dictionary<int, GameObject> propDictionary = new Dictionary<int, GameObject>();
    public Dictionary<GameObject, int> idDictionary = new Dictionary<GameObject, int>();

    private void Start()
    {
        CreateDictionaries();
        gen = GetComponent<HeightsGenerator>();
        width = gen.Width;
        height = gen.Height;
        depth = gen.Depth;
        if (PhotonNetwork.IsMasterClient)
        {
            float seed = Random.Range(0f, 9999f);
            object[] seedData = new object[] { seed };
            PhotonNetwork.RaiseEvent(SEND_SEED_EVENT, seedData, RaiseEventOptions.Default, SendOptions.SendUnreliable);
            gen.Offset = seed;
            gen.Generate();

            GenerateTrees();
            GenerateContainers();
            GenerateProps();
            GenerateBigProps();
        }

  }

    void CreateDictionaries()
    {
        int counter = 0;
        //Start with trees
        foreach(GameObject a in Trees)
        {
            propDictionary.Add(counter, a);
            idDictionary.Add(a, counter);
            counter++;
        }
        //then containers
        foreach (GameObject a in Containers)
        {
            propDictionary.Add(counter, a);
            idDictionary.Add(a, counter);
            counter++;
        }
        //then props
        foreach (GameObject a in Props)
        {
            propDictionary.Add(counter, a);
            idDictionary.Add(a, counter);
            counter++;
        }
        //then big props
        foreach (GameObject a in BigProps)
        {
            propDictionary.Add(counter, a);
            idDictionary.Add(a, counter);
            counter++;
        }
    }

    private void NetworkingClient_EventReceived(EventData obj)
    {
        if (obj.Code == SEND_SEED_EVENT)
        {
            object[] datas = (object[])obj.CustomData;
            float seed = (float)datas[0];
            gen.Offset = seed;
            gen.Generate();
        }
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    void GenerateTrees()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                PlaceTree((float)x, (float)y);
            }
        }
    }
    GameObject PickTree()
    {
        float r = Random.Range(0, 100);
        r = r / 100;
        GameObject obj = Trees[0];
        if (r > 0.5f)
        {
            if (r <= 0.5f)
            {
                obj = Trees[1];
            }
            else if (r <= 0.7f)
            {
                obj = Trees[2];
            }
            else if (r <= 0.9f)
            {
                obj = Trees[3];
            }
        }
        return obj;
    }

    void PlaceTree(float x, float y)
    {
        if(Roll(treeSpawnTries))
        {
            float height = Terrain.activeTerrain.SampleHeight(new Vector3(x, 0, y));
            if(height > treeSpawnMin)
            {
                GameObject objToSpawn = PickTree();
                PlaceObject(objToSpawn, x, y, height, treeManager);
            }
        }
    }

    public int propID = 0;

    void PlaceObject(GameObject obj, float x, float y, float height, PropManager manager)
    {
        Vector3 spawnPt = new Vector3(x, height, y);
        GameObject _gameObject = (GameObject)Instantiate(obj, spawnPt, Quaternion.identity);
        _gameObject.name = _gameObject.name + propID;
        _gameObject.GetComponentInChildren<HealthScript>().SetName(_gameObject.name);
        propID++;
        manager.AddItem(_gameObject);
        base.photonView.RPC("RPC_PlaceObject", RpcTarget.Others, idDictionary[obj], x, y, height);
    }

    [PunRPC]
    void RPC_PlaceObject(int objID, float x, float y, float height)
    {
        Vector3 spawnPt = new Vector3(x, height, y);
        GameObject _gameObject = (GameObject)Instantiate(propDictionary[objID], spawnPt, Quaternion.identity);
        _gameObject.name = _gameObject.name + propID;
        _gameObject.GetComponentInChildren<HealthScript>().SetName(_gameObject.name);
        propID++;
    }

    bool Roll(int chance)
    {
        int rollnum = Random.Range(1, chance + 1);
        if (rollnum == chance) return true;
        return false;
    }

    void GenerateContainers()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                PlaceContainer((float)x, (float)y);
            }
        }
    }

    void PlaceContainer(float x, float y)
    {
        if (Roll(containerSpawnTries))
        {
            float height = Terrain.activeTerrain.SampleHeight(new Vector3(x, 0, y));
            if (height > objectSpawnMin)
            {
                GameObject objToSpawn = PickContainer();
                PlaceObject(objToSpawn, x, y, height, containerManager);
            }
        }
    }

    GameObject PickContainer()
    {
        float r = Random.Range(0, 100);
        r = r / 100;
        GameObject obj = Containers[0];
        if (r > 0.65f)
        {
            obj = Containers[1];
        }
        return obj;
    }

    void GenerateProps()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                PlaceProps((float)x, (float)y);
            }
        }
    }

    void PlaceProps(float x, float y)
    {
        if (Roll(propSpawnTries))
        {
            float height = Terrain.activeTerrain.SampleHeight(new Vector3(x, 0, y));
            if (height > objectSpawnMin)
            {
                GameObject objToSpawn = PickProp();
                PlaceObject(objToSpawn, x, y, height, propManager);
            }
        }
    }

    GameObject PickProp()
    {
        int s = Props.Length;
        int r = Random.Range(0, s);
        GameObject p = Props[r];
        return p;
    }

    void GenerateBigProps()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                PlaceBigProps((float)x, (float)y);
            }
        }
    }

    void PlaceBigProps(float x, float y)
    {
        if (Roll(bigPropSpawnTries))
        {
            float height = Terrain.activeTerrain.SampleHeight(new Vector3(x, 0, y));
            if (height > objectSpawnMin)
            {
                GameObject objToSpawn = PickBigProp();
                PlaceObject(objToSpawn, x, y, height, bigPropManager);
            }
        }
    }

    GameObject PickBigProp()
    {
        int s = BigProps.Length;
        int r = Random.Range(0, s);
        GameObject p = BigProps[r];
        print("placed big prop");
        return p;
    }

}
