using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageLoader : MonoBehaviour
{
    //public string url = "https://64.media.tumblr.com/6778a41330b8e7cf7478e347c3dd80a3/tumblr_inline_peioaoyVUy1s4qpfd_100.jpg";

    public PlayerListing behavior;

    // This will run independantly
    public IEnumerator LoadFromLikeCoroutine(string url)
    {
        Debug.Log("Loading image...");
        WWW wwwLoader = new WWW(url); // Create WWW pointer
        yield return wwwLoader; //Begin loading the URL
        Debug.Log("Loaded image!");
        behavior.SetIcon(wwwLoader.texture);
    }

    public void setTextureFromURL(string url)
    {
        StartCoroutine(LoadFromLikeCoroutine(url));
        
    }

}
