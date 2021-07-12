using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class CreateRoom : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text _roomName;

    private RoomsCanvases _roomsCanvases;

    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomsCanvases = canvases;
    }

    public void OnClick_CreateRoom()
    {
        // dont create a room if not connected
        if (!PhotonNetwork.IsConnected) return;

        //JoinOrCreateRoom
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4; //change to get from UI
        PhotonNetwork.JoinOrCreateRoom(_roomName.text, options, TypedLobby.Default);

    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created room succesfully.", this);
        _roomsCanvases.CurrentRoomCanvas.Show();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room creation failed:" + message, this);
    }
}
