using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TeleportController : MonoBehaviourPun
{
    public GameObject AnotherTeleport;
    public float lastEnterTime;
    public float coolTime;
    public Dictionary<string , float> dict = new Dictionary<string,  float>();

    public GameObject teleportEffect;
    public ParticleSystem teleportParticalEffect;
    // Start is called before the first frame update
    void Start()
    {
        lastEnterTime = Time.time;
        coolTime = 5.0f;
        if (TryGetComponent(out ParticleSystem ps)) {
            teleportParticalEffect = ps;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider Collision)
    {   
        if(Collision.gameObject.tag == "Player")
        {
            if (!dict.ContainsKey(Collision.gameObject.GetComponent<PhotonView>().Owner.UserId))
            {
                dict.Add(Collision.gameObject.GetComponent<PhotonView>().Owner.UserId, Time.time);
                AnotherTeleport.GetComponent<TeleportController>().dict.Add(Collision.gameObject.GetComponent<PhotonView>().Owner.UserId, Time.time);
                Collision.transform.position = AnotherTeleport.transform.position;
                hideTeleportEffect();
                Invoke(nameof(showTeleportEffect), coolTime);
            }
            else if (Time.time - dict[Collision.gameObject.GetComponent<PhotonView>().Owner.UserId] >= coolTime)
            {
                dict[Collision.gameObject.GetComponent<PhotonView>().Owner.UserId] = Time.time;
                AnotherTeleport.GetComponent<TeleportController>().dict[Collision.gameObject.GetComponent<PhotonView>().Owner.UserId] = Time.time;
                Collision.transform.position = AnotherTeleport.transform.position;
                hideTeleportEffect();
                Invoke(nameof(showTeleportEffect), coolTime);
            }
            /*
            if (!dict.ContainsKey(Collision.gameObject.name)) {
                dict.Add(Collision.gameObject.name, Time.time);
                AnotherTeleport.GetComponent<TeleportController>().dict.Add(Collision.gameObject.name, Time.time);
                Collision.transform.position = AnotherTeleport.transform.position;
            }
            else if (Time.time - dict[Collision.gameObject.name] >= coolTime)
            {
                dict[Collision.gameObject.name] = Time.time;
                AnotherTeleport.GetComponent<TeleportController>().dict[Collision.gameObject.name] = Time.time;
                Collision.transform.position = AnotherTeleport.transform.position;
            }      */
        }
    }

    void showTeleportEffect() {
        if (teleportEffect && AnotherTeleport.GetComponent<TeleportController>().teleportEffect) {
            teleportEffect.SetActive(true);
            AnotherTeleport.GetComponent<TeleportController>().teleportEffect.SetActive(true);
        }

        if (teleportParticalEffect && AnotherTeleport.GetComponent<TeleportController>().teleportParticalEffect)
        {
            teleportParticalEffect.Play();
            AnotherTeleport.GetComponent<TeleportController>().teleportParticalEffect.Play();
        }
        
    }

    void hideTeleportEffect() {
        if (teleportEffect && AnotherTeleport.GetComponent<TeleportController>().teleportEffect){
            teleportEffect.SetActive(false);
            AnotherTeleport.GetComponent<TeleportController>().teleportEffect.SetActive(false);
        }

        if (teleportParticalEffect && AnotherTeleport.GetComponent<TeleportController>().teleportParticalEffect)
        {
            teleportParticalEffect.Stop();
            AnotherTeleport.GetComponent<TeleportController>().teleportParticalEffect.Stop();
        }
    }

}
