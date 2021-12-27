using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScreenPoint : MonoBehaviour
{
    public Transform target;
    private Camera camera;
    public Transform player;
    private Image img;
    public float imgWidth = 100;
    // Start is called before the first frame update
    void Start()
    {
        
        camera = Camera.main;
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null || player == null)
            return;
        
        Vector3 pos = camera.WorldToScreenPoint(target.position);
        Debug.Log(pos);

        if (pos.z > 0 && pos.x < Screen.width && pos.x > 0 && pos.y < Screen.height && pos.y > 0) {
            img.enabled = false;
            return;

        }

        img.enabled = true;
        if (pos.z < 0)
            pos.x = pos.x > Screen.width/2 ? Screen.width - imgWidth : 0;
        else    
            pos.x = Mathf.Clamp(pos.x, 0, Screen.width- imgWidth);
    
        pos.y = Mathf.Clamp(pos.y, 0, Screen.height- imgWidth);
        img.rectTransform.position = pos;
    }


}
