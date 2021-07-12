using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemActivator : MonoBehaviour
{

    // --------------------------------------------------
    // Variables:

    [SerializeField]
    private int distanceFromPlayer;

    private GameObject player;

    private List<ActivatorItem> activatorItems;

    public List<ActivatorItem> addList;

    // --------------------------------------------------

    void Start()
    {
        player = GameObject.Find("Player");
        activatorItems = new List<ActivatorItem>();
        addList = new List<ActivatorItem>();

        AddToList();
    }

    public void RemoveFromList(GameObject g)
    {
        ActivatorItem a = new ActivatorItem();
        a.item = g;

        activatorItems.Remove(a);
        addList.Remove(a);
    }

    void AddToList()
    {
        if (addList.Count > 0)
        {
            foreach (ActivatorItem item in addList)
            {
                if (item.item != null)
                {
                    activatorItems.Add(item);
                }
            }

            addList.Clear();
        }

        StartCoroutine("CheckActivation");
    }

    IEnumerator CheckActivation()
    {
        List<ActivatorItem> removeList = new List<ActivatorItem>();

        if (activatorItems.Count > 0)
        {
            foreach (ActivatorItem item in activatorItems)
            {
                if (item.item == null) continue;
                if (Vector3.Distance(player.transform.position, item.item.transform.position) > distanceFromPlayer)
                {
                    if (item.item == null)
                    {
                        removeList.Add(item);
                    }
                    else
                    {
                        item.item.SetActive(false);
                    }
                }
                else
                {
                    if (item.item == null)
                    {
                        removeList.Add(item);
                    }
                    else
                    {
                        item.item.SetActive(true);
                    }
                }
            }
        }

        if (removeList.Count > 0)
        {
            foreach (ActivatorItem item in removeList)
            {
                activatorItems.Remove(item);
            }
        }

        yield return new WaitForEndOfFrame();

        AddToList();
    }
}

public class ActivatorItem
{
    public GameObject item;
}