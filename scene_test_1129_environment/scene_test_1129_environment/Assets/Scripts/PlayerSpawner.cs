using Photon.Pun;
using UnityEngine;

namespace PhotonTutorial
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab = null;
        [SerializeField] private GameObject playerCamera = null;
        [SerializeField] private GameObject Conveyor1 = null;
        [SerializeField] private GameObject Conveyor2 = null;
        [SerializeField] private GameObject Teleport1 = null;
        [SerializeField] private GameObject Teleport2 = null;

        private void Start()
        {
            var player = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(Random.Range(-20f, 10f), 1f, Random.Range(5f, 10f)), Quaternion.identity);
            //playerCamera = GameObject.Find("Main Camera");
            //playerCamera.GetComponent<Transform>().position = new Vector3(player.transform.position.x, 6.5f, player.transform.position.z) 
            //- player.transform.forward*10f;
            //playerCamera.GetComponent<Transform>().LookAt(player.transform);
            var conveyor1 = PhotonNetwork.Instantiate(Conveyor1.name, new Vector3(Conveyor1.GetComponent<ConveyorController>().x1, Conveyor1.GetComponent<ConveyorController>().y1, Conveyor1.GetComponent<ConveyorController>().z1), Quaternion.identity);
            var conveyor2 = PhotonNetwork.Instantiate(Conveyor2.name, new Vector3(Conveyor2.GetComponent<ConveyorController>().x1, Conveyor2.GetComponent<ConveyorController>().y1, Conveyor2.GetComponent<ConveyorController>().z1), Quaternion.identity);
            var teleport1 = PhotonNetwork.Instantiate(Teleport1.name, new Vector3(15.99f, 0.25f, 41f), Quaternion.identity);
            var teleport2 = PhotonNetwork.Instantiate(Teleport2.name, new Vector3(-22.26f, 0.05f,-16.4f), Quaternion.identity);
        }
    }
}