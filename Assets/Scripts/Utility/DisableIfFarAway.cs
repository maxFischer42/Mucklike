using UnityEngine;
using System.Collections;

public class DisableIfFarAway : MonoBehaviour
{

    // --------------------------------------------------
    // Variables:

    private GameObject itemActivatorObject;
    private ItemActivator activationScript;
    public string ObjectName = "ItemActivatorObject";

    // --------------------------------------------------

    void Start()
    {
        itemActivatorObject = GameObject.Find(ObjectName);
        activationScript = itemActivatorObject.GetComponent<ItemActivator>();

        StartCoroutine("AddToList");
    }

    IEnumerator AddToList()
    {
        yield return new WaitForEndOfFrame();

        activationScript.addList.Add(new ActivatorItem { item = this.gameObject });
        this.gameObject.SetActive(false);
    }
}