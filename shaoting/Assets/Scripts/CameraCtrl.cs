using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraCtrl : MonoBehaviour//Pun
{
    float mouse_sensitivity = 50f;
    float xRotation = 0f;

    public GameObject player = null;
    //public GameObject player;
    //public GameObject lighter;
    //public GameObject particals;

    private Vector3 cameraOffeset;
    public float SmoothFactor = 0.5f;
    public bool LookAtPlayer = false;
    public bool RotateAroundPlayer = true;
    public float RotateSpeed = 5.0f;

    //bool lighton;

    // Start is called before the first frame update
    void Start()
    {
        //lighton = false;
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    public void setCameraOffset() {
        cameraOffeset = transform.position - player.transform.position;
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            //Debug.Log("I found player");

            if (RotateAroundPlayer)
            {
                Quaternion camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotateSpeed, Vector3.up);
                cameraOffeset = camTurnAngle * cameraOffeset;
            }
            Vector3 newPos = player.transform.position + cameraOffeset;
            transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);

            if (LookAtPlayer || RotateAroundPlayer)
            {
                transform.LookAt(player.transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {   



        /*
        //if (photonView.IsMine){
            float mouse_x = Input.GetAxis("Mouse X") * mouse_sensitivity * Time.deltaTime * 2;
            float mouse_y = Input.GetAxis("Mouse Y") * mouse_sensitivity * Time.deltaTime;

            xRotation -= mouse_y;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            
            transform.Rotate(new Vector3(0f, 1f, 0f) * mouse_x);
        //}
        */

        // if(Input.GetMouseButtonDown(0)){
        //     lighter.GetComponent<lightCtrl>().lightup();
        //     particals.SetActive(true);
        //     lighton = true;
        // }
        // if(Input.GetMouseButtonUp(0)){
        //     lighter.GetComponent<lightCtrl>().lightdown();
        //     particals.SetActive(false);
        //     lighton = false;
        // }
    }

    void FixedUpdate(){
        // RaycastHit hit;

        // if(lighton){
        //     if (Physics.Raycast(transform.position, transform.forward, out hit)) {
        //         //print("Raycast hit : " + hit.collider.gameObject.name);
        //         if(hit.collider.gameObject.name == "BattleSpider02"){
        //             Destroy(hit.collider.gameObject);
        //         }
        //     }
        // }
    }
}
