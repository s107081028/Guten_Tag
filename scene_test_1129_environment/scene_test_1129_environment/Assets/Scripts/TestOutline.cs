using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOutline : MonoBehaviour
{
    /*Adding delay of setup  outline*/
    private Outline outline;
    public float delay;
    // Start is called before the first frame update
    void Start()
    {
        //Invoke(nameof(turnOnOutline), delay);
    }

    public void turnOnOutline(Color color) {
        Outline outline = gameObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineHidden;
        outline.OutlineColor = color;
    }

}
