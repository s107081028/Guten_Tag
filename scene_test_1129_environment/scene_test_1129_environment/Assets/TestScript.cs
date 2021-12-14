using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{

    // For invisible
    [SerializeField]
    public SkinnedMeshRenderer[] renderers;
    // Start is called before the first frame update
    void Start()
    {

    foreach (SkinnedMeshRenderer renderer in renderers)
    {
        renderer.enabled = false;
    }
}

    // Update is called once per frame
    void Update()
    {
        
    }
}
