using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGameObject : MonoBehaviour
{
    public GameObject targetObject;

    public void toggleGameObject() {
        targetObject.SetActive(!targetObject.activeSelf);
    }
}
