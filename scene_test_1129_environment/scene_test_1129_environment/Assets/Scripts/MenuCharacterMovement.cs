using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCharacterMovement : MonoBehaviour
{
    public float speed = 15f;
    public Transform startPos;
    public Transform endPos;
    private int dir = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        transform.Translate(speed * dir * transform.right*-1 * Time.deltaTime);
        if (Vector3.Distance(transform.position, startPos.position) <= 1f)
        {
            if(dir != 1)
                turnBody();
            dir = 1;
            
            
        }
        else if (Vector3.Distance(transform.position, endPos.position) <= 1f)
        {
            if (dir != -1)
                turnBody();
            dir = -1;
        }
    }

    void turnBody() {
        transform.Rotate(0, 180, 0);
    }
}
