using Photon.Pun;
using UnityEngine;

namespace PhotonTutorial
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab = null;
        [SerializeField] private GameObject playerCamera = null;

        private void Start()
        {
            var player = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(Random.Range(-20f, 10f), 1f, Random.Range(5f, 10f)), Quaternion.identity);
            //playerCamera = GameObject.Find("Main Camera");
            //playerCamera.GetComponent<Transform>().position = new Vector3(player.transform.position.x, 6.5f, player.transform.position.z) 
            //- player.transform.forward*10f;
            //playerCamera.GetComponent<Transform>().LookAt(player.transform);
        }
    }
}