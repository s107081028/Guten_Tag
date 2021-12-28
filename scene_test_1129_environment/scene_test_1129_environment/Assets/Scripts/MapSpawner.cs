using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace PhotonTutorial
{
    public class MapSpawner : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private GameObject Conveyor1 = null;
        [SerializeField] private GameObject Conveyor2 = null;
        [SerializeField] private GameObject Teleport1 = null;
        [SerializeField] private GameObject Teleport2 = null;

        void Start()
        {
            var conveyor1 = PhotonNetwork.Instantiate(Conveyor1.name, new Vector3(Conveyor1.GetComponent<ConveyorController>().x1, Conveyor1.GetComponent<ConveyorController>().y1, Conveyor1.GetComponent<ConveyorController>().z1), Quaternion.identity);
            var conveyor2 = PhotonNetwork.Instantiate(Conveyor2.name, new Vector3(Conveyor2.GetComponent<ConveyorController>().x1, Conveyor2.GetComponent<ConveyorController>().y1, Conveyor2.GetComponent<ConveyorController>().z1), Quaternion.identity);
            var teleport1 = PhotonNetwork.Instantiate(Teleport1.name, new Vector3(15.99f, 0.25f, 41f), Quaternion.identity);
            var teleport2 = PhotonNetwork.Instantiate(Teleport2.name, new Vector3(-22.26f, 0.05f,-16.4f), Quaternion.identity);
        }
    }
}