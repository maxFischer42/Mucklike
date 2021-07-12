using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomCustomPropertyGenerator : MonoBehaviour
{

    private ExitGames.Client.Photon.Hashtable _myCustomProperties = new ExitGames.Client.Photon.Hashtable();

    [SerializeField]
    private Text _text;

    public void OnClick_Button()
    {
        SetCustomNumber();
    }

    private void SetCustomNumber()
    {
        System.Random rnd = new System.Random();
        int result = rnd.Next(0, 99);

        _text.text = result.ToString();

        _myCustomProperties["RandomNumber"] = result;
        PhotonNetwork.LocalPlayer.CustomProperties = _myCustomProperties;
    }
}
