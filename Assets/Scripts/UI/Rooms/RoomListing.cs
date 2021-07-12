using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class RoomListing : MonoBehaviour
{
    [SerializeField]
    private Text _text;

    public RoomInfo RoomInfo { get; private set; }
    public string _name;

    public void SetRoomInfo(RoomInfo roominfo)
    {
        RoomInfo = roominfo;
        _name = RoomInfo.Name;
        _text.text = roominfo.MaxPlayers + ", " + roominfo.Name;
    }

    public void OnClick_Button()
    {
        PhotonNetwork.JoinRoom(RoomInfo.Name);
    }
}
