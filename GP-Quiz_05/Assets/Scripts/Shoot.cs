using Photon.Pun;
using UnityEngine;

namespace PhotonTutorial.Weapons
{
    public class Shoot : MonoBehaviourPun
    {
        [SerializeField] private float projectileSpeed = 100f;
        [SerializeField] public GameObject projectile;
        [SerializeField] public GameObject spawnPoint;

        private void Update()
        {
            if (photonView.IsMine)
            {
                TakeInput();
            }
        }

        private void TakeInput()
        {
            if (!Input.GetMouseButtonDown(0)) { return; }

            FireProjectile();
        }

        private void FireProjectile()
        {
            var projectileInstance = PhotonNetwork.Instantiate(
                projectile.name,
                spawnPoint.transform.position,
                spawnPoint.transform.rotation
            );

            projectileInstance.GetComponent<Rigidbody>().velocity = projectileInstance.transform.forward * projectileSpeed;
        }
    }
}