
using System.Collections;

using UnityEngine;

using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun.Demo.Asteroids;
using Photon.Pun;

public class PhotonPlayer : MonoBehaviour
{
    public float RotationSpeed = 90.0f;
    public float MovementSpeed = 2.0f;
    public float MaxSpeed = 0.2f;

    private PhotonView photonView;

#pragma warning disable 0109
    private new Rigidbody rigidbody;
    private new Collider collider;
    private new Renderer renderer;
#pragma warning restore 0109

    public GameObject[] gameObjectsToDisable;
    public Behaviour[] behaviorsToDisable;

    private bool controllable = true;
    public GameObject PlayerObject;

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        // e.g. store this gameobject as this player's charater in Player.TagObject
        info.Sender.TagObject = PlayerObject;
        transform.position = GameObject.Find("SpawnPoint").transform.position;
    }

    #region UNITY

    public void Awake()
    {
        photonView = GetComponent<PhotonView>();

        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        renderer = GetComponent<Renderer>();
    }

    public void Start()
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.color = AsteroidsGame.GetColor(photonView.Owner.GetPlayerNumber());
        }

        // Disable Player-Only Objects
        if(!photonView.AmOwner || !controllable)
        {
            DisableBehaviors();
            DisableGameObjects();
        }
    }

    void DisableBehaviors()
    {
        foreach(Behaviour b in behaviorsToDisable)
        {
            b.enabled = false;
        }
    }

    void DisableGameObjects()
    {
        foreach (GameObject g in gameObjectsToDisable)
        {
            g.SetActive(false);
        }
    }

    public void Update()
    {
        if (!photonView.AmOwner || !controllable)
        {
            return;
        }

        // we don't want the master client to apply input to remote ships while the remote player is inactive
        if (this.photonView.CreatorActorNr != PhotonNetwork.LocalPlayer.ActorNumber)
        {
            return;
        }

       // rotation = Input.GetAxis("Horizontal");
      //  acceleration = Input.GetAxis("Vertical");

      //  if (Input.GetButton("Jump") && shootingTimer <= 0.0)
      //  {
     //       shootingTimer = 0.2f;
     //
//photonView.RPC("Fire", RpcTarget.AllViaServer, rigidbody.position, rigidbody.rotation);
    //    }

      //  if (shootingTimer > 0.0f)
      //  {
     //       shootingTimer -= Time.deltaTime;
     //   }
    }

    public void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (!controllable)
        {
            return;
        }

       // Quaternion rot = rigidbody.rotation * Quaternion.Euler(0, rotation * RotationSpeed * Time.fixedDeltaTime, 0);
      //  rigidbody.MoveRotation(rot);

      //  Vector3 force = (rot * Vector3.forward) * acceleration * 1000.0f * MovementSpeed * Time.fixedDeltaTime;
      //  rigidbody.AddForce(force);

        if (rigidbody.velocity.magnitude > (MaxSpeed * 1000.0f))
        {
            rigidbody.velocity = rigidbody.velocity.normalized * MaxSpeed * 1000.0f;
        }

        CheckExitScreen();
    }

    #endregion

    #region COROUTINES

    private IEnumerator WaitForRespawn()
    {
        yield return new WaitForSeconds(AsteroidsGame.PLAYER_RESPAWN_TIME);

        photonView.RPC("RespawnSpaceship", RpcTarget.AllViaServer);
    }

    #endregion

    #region PUN CALLBACKS

    [PunRPC]
    public void DestroySpaceship()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        collider.enabled = false;
        renderer.enabled = false;

        controllable = false;

        if (photonView.IsMine)
        {
            object lives;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(AsteroidsGame.PLAYER_LIVES, out lives))
            {
                PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { AsteroidsGame.PLAYER_LIVES, ((int)lives <= 1) ? 0 : ((int)lives - 1) } });

                if (((int)lives) > 1)
                {
                    StartCoroutine("WaitForRespawn");
                }
            }
        }
    }

    //[PunRPC]
    //public void Fire(Vector3 position, Quaternion rotation, PhotonMessageInfo info)
    //{
        //float lag = (float)(PhotonNetwork.Time - info.SentServerTime);
        //GameObject bullet;

        /** Use this if you want to fire one bullet at a time **/
        //bullet = Instantiate(BulletPrefab, position, Quaternion.identity) as GameObject;
        //bullet.GetComponent<Bullet>().InitializeBullet(photonView.Owner, (rotation * Vector3.forward), Mathf.Abs(lag));


        /** Use this if you want to fire two bullets at once **/
        //Vector3 baseX = rotation * Vector3.right;
        //Vector3 baseZ = rotation * Vector3.forward;

        //Vector3 offsetLeft = -1.5f * baseX - 0.5f * baseZ;
        //Vector3 offsetRight = 1.5f * baseX - 0.5f * baseZ;

        //bullet = Instantiate(BulletPrefab, rigidbody.position + offsetLeft, Quaternion.identity) as GameObject;
        //bullet.GetComponent<Bullet>().InitializeBullet(photonView.Owner, baseZ, Mathf.Abs(lag));
        //bullet = Instantiate(BulletPrefab, rigidbody.position + offsetRight, Quaternion.identity) as GameObject;
        //bullet.GetComponent<Bullet>().InitializeBullet(photonView.Owner, baseZ, Mathf.Abs(lag));
    //}

    [PunRPC]
    public void RespawnSpaceship()
    {
        collider.enabled = true;
        renderer.enabled = true;

        controllable = true;

    }

    #endregion

    private void CheckExitScreen()
    {
        if (Camera.main == null)
        {
            return;
        }

        if (Mathf.Abs(rigidbody.position.x) > (Camera.main.orthographicSize * Camera.main.aspect))
        {
            rigidbody.position = new Vector3(-Mathf.Sign(rigidbody.position.x) * Camera.main.orthographicSize * Camera.main.aspect, 0, rigidbody.position.z);
            rigidbody.position -= rigidbody.position.normalized * 0.1f; // offset a little bit to avoid looping back & forth between the 2 edges 
        }

        if (Mathf.Abs(rigidbody.position.z) > Camera.main.orthographicSize)
        {
            rigidbody.position = new Vector3(rigidbody.position.x, rigidbody.position.y, -Mathf.Sign(rigidbody.position.z) * Camera.main.orthographicSize);
            rigidbody.position -= rigidbody.position.normalized * 0.1f; // offset a little bit to avoid looping back & forth between the 2 edges 
        }
    }
}
