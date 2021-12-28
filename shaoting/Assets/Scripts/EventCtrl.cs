using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCtrl : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    public GameObject canva_win;

    public GameObject canva_lose;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Onclick(){
        player.transform.position = new Vector3(0.94f, 0.4f, -9.462654f);
        enemy.transform.position = new Vector3(0f, 0.4f, 0f);

        canva_win.SetActive(false);
        canva_lose.SetActive(false);

        player.GetComponent<PlayerController>().SetEnd();
    }
}
