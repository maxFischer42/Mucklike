using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizeMenu : MonoBehaviour
{
    [SerializeField]
    private CustomizePlayerCanvas _customizePlayerCanvas;

    public void OnClick_Customize()
    {
        _customizePlayerCanvas.Show();
    }

}
