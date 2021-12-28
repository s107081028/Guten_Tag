using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletColor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var otherRenderer = gameObject.GetComponent<Renderer>();

        //Call SetColor using the shader property name "_Color" and setting the color to red
        otherRenderer.material.SetColor("_Color", Color.blue);
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 5);
    }

    private void OnCollisionEnter(Collision CollisionObject)
    {
        if(CollisionObject.collider.gameObject.name == "Player(Clone)")
        {
            var otherRenderer = gameObject.GetComponent<Renderer>();

            //Call SetColor using the shader property name "_Color" and setting the color to red
            otherRenderer.material.SetColor("_Color", Color.red);
        }
        else
        {
            var otherRenderer = gameObject.GetComponent<Renderer>();

            //Call SetColor using the shader property name "_Color" and setting the color to red
            //otherRenderer.material.SetColor("_Color", Color.yellow);
        }
    }
}
