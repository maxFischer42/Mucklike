using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Item")]
public class ItemScriptableObject : ScriptableObject
{
    public GameObject prefab;
    public Sprite icon;
}
