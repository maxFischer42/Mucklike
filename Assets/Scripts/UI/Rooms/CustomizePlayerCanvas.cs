using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CustomizePlayerCanvas : MonoBehaviour
{
    private ExitGames.Client.Photon.Hashtable _myCustomProperties = new ExitGames.Client.Photon.Hashtable();
    public string iconUrl;

    private RoomsCanvases _roomsCanvases;

    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomsCanvases = canvases;
    }

    private void Awake()
    {
        IconOnAwake();
    }

    #region IconHandler

    [SerializeField]
    private InputField imageInputField;
    public Texture2D defaultIcon;
    public RawImage IconPreviewImage;
    private string url = "";

    void IconOnAwake()
    {
        var tmp = PlayerPrefs.GetString("IconURL");
        if(tmp != "")
        {
            url = tmp;
            StartCoroutine(LoadFromLikeCoroutine());
        }
    }

    public void OnClick_Apply()
    {
        iconUrl = url;
    }

    public void OnClick_UploadImage()
    {
        url = imageInputField.text;
        StartCoroutine(LoadFromLikeCoroutine());
    }

    public void OnClick_Clear()
    {
        url = "";
        imageInputField.text = "";
        IconPreviewImage.texture = defaultIcon;
    }

    public IEnumerator LoadFromLikeCoroutine()
    {
        Debug.Log("Loading image : \"" + url + "\"");
        WWW wwwLoader = new WWW(url); // Create WWW pointer
        yield return wwwLoader; //Begin loading the URL
        IconPreviewImage.texture = wwwLoader.texture;
    }

    #endregion

    #region SaveAndExitHandling

    public void OnClick_Save()
    {
        HandleSave();
    }

    public void OnClick_Exit()
    {
        HandleExit();
    }

    private void HandleSave()
    {
        // Save Icon if exists
        _myCustomProperties["IconURL"] = iconUrl;

        PlayerPrefs.SetString("IconURL", iconUrl);

        PhotonNetwork.LocalPlayer.CustomProperties = _myCustomProperties;
        HandleExit();
    }

    private void HandleExit()
    {
        Hide();
    }

    #endregion

    #region Lifecycle
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    #endregion

}
