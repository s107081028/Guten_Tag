using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillAmountController : MonoBehaviour
{
    // GAMEOBJECT
    // private Image skill1CoolDown;
    // private Image skill2CoolDown;
    // private Image skill3CoolDown;
    // private Image skill4CoolDown;
    // private Image sprintPower;
    // private SkillController skillController;

    // public GameObject skill1CoolDown;
    // public GameObject skill2CoolDown;
    // public GameObject skill3CoolDown;
    // public GameObject skill4CoolDown;
    // public GameObject sprintPower;
    
    private Image skill1CoolDownImage;
    private Image skill2CoolDownImage;
    private Image skill3CoolDownImage;
    private Image skill4CoolDownImage;
    private Image sprintPowerImage;
    // public GameObject player;
    private SkillController skillController;

    void Start()
    {
        skill1CoolDownImage = GameObject.Find("SkillAndSprintCanvas/SkillCoolDown/Skill1/Skill1CoolDown").GetComponent<Image>();
        skill2CoolDownImage = GameObject.Find("SkillAndSprintCanvas/SkillCoolDown/Skill2/Skill2CoolDown").GetComponent<Image>();
        skill3CoolDownImage = GameObject.Find("SkillAndSprintCanvas/SkillCoolDown/Skill3/Skill3CoolDown").GetComponent<Image>();
        skill4CoolDownImage = GameObject.Find("SkillAndSprintCanvas/SkillCoolDown/Skill4/Skill4CoolDown").GetComponent<Image>();
        sprintPowerImage = GameObject.Find("SkillAndSprintCanvas/SprintBar/SprintPower").GetComponent<Image>();
        skillController = gameObject.GetComponent<SkillController>();
    }

    void Update()
    {
        skill1CoolDownImage.fillAmount = (1.0f) * skillController.skill1Cooldown / skillController.skill1Speed;
        skill2CoolDownImage.fillAmount = (1.0f) * skillController.skill2Cooldown / skillController.skill2Speed;
        skill3CoolDownImage.fillAmount = (1.0f) * skillController.skill3Cooldown / skillController.skill3Speed;
        skill4CoolDownImage.fillAmount = (1.0f) * skillController.skill4Cooldown / skillController.skill4Speed;
        sprintPowerImage.fillAmount = (1.0f) * skillController.sprintPower / skillController.sprintMaxPower;

    }
}
