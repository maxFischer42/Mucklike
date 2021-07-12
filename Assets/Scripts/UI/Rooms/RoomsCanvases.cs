using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsCanvases : MonoBehaviour
{
    [SerializeField]
    private CreateOrJoinRoomCanvas _createOrJoinRoomCanvas;
    public CreateOrJoinRoomCanvas CreateOrJoinRoomCanvas { get { return _createOrJoinRoomCanvas; } }

    [SerializeField]
    private CurrentRoomCanvas _currentRoomCanvas;
    public CurrentRoomCanvas CurrentRoomCanvas { get { return _currentRoomCanvas; } }

    [SerializeField]
    private CustomizePlayerCanvas _customizePlayerCanvas;
    public CustomizePlayerCanvas CustomizePlayerCanvas { get { return _customizePlayerCanvas; } }

    private void Awake()
    {
        FirstInitialize();
    }

    private void FirstInitialize()
    {
        CreateOrJoinRoomCanvas.FirstInitialize(this);
        CurrentRoomCanvas.FirstInitialize(this);
        CustomizePlayerCanvas.FirstInitialize(this);
    }
}
