using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class PlayerListing : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private RawImage _icon;

    [SerializeField]
    private Text _text;

    public Player Player { get; private set; }
    public bool Ready = false;


    public void SetPlayerInfo(Player player)
    {
        Player = player;
        //int result = (int)player.CustomProperties["RandomNumber"];
        SetPlayerProps(player);
        
    }

    public void SetIcon(Texture t)
    {
        _icon.texture = t;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        if(targetPlayer != null && targetPlayer == Player)
        {
            SetPlayerProps(targetPlayer);
        }
    }

    private void SetPlayerProps(Player player)
    {
        _text.text = player.NickName;
        if ((string)player.CustomProperties["IconURL"] != "")
        {
            GetComponent<ImageLoader>().setTextureFromURL((string)player.CustomProperties["IconURL"]);
        }
    }
}
