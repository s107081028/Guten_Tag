using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SprintBar : MonoBehaviour
{
    [SerializeField] Slider sprintBar;
    private SkillController skillController;
    // Start is called before the first frame update
    void Start()
    {
        skillController = GetComponent<SkillController>();
    }

    // Update is called once per frame
    void Update()
    {
        sprintBar.value = (1.0f) * skillController.sprintPower / skillController.sprintMaxPower;
    }
}
