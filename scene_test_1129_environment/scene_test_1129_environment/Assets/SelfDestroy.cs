using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{

    public float DestroyAfterStartTime = 2f; 
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(destroy), DestroyAfterStartTime);
    }

    void destroy() {
        Destroy(gameObject);
    }

}
