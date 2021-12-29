using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickedUP : MonoBehaviour
{
    public int Item;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision CollisionObject)
    {
        if(CollisionObject.gameObject.tag == "Player")
        {
            Debug.Log("i eat coin!!!!!");
            int i = UnityEngine.Random.Range(0, 3);
            CollisionObject.gameObject.GetComponent<SkillController>().PickUpCoin(i);
            Destroy(gameObject);
        }
    }
}
