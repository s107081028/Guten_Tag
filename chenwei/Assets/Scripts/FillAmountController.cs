using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillAmountController : MonoBehaviour
{
    // GAMEOBJECT
    private Image skill1CoolDown;
    private Image skill2CoolDown;
    private Image skill3CoolDown;
    private Image skill4CoolDown;
    private Image sprintPower;
    private PlayerMovement playerMovement;

    void Start()
    {
        skill1CoolDown = GameObject.Find("Skill1CoolDown").GetComponent<Image>();
        skill2CoolDown = GameObject.Find("Skill2CoolDown").GetComponent<Image>();
        skill3CoolDown = GameObject.Find("Skill3CoolDown").GetComponent<Image>();
        skill4CoolDown = GameObject.Find("Skill4CoolDown").GetComponent<Image>();
        sprintPower = GameObject.Find("SprintPower").GetComponent<Image>();
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    void Update()
    {
        skill1CoolDown.fillAmount = (1.0f) * playerMovement.skill1Cooldown / playerMovement.skill1Speed;
        skill2CoolDown.fillAmount = (1.0f) * playerMovement.skill2Cooldown / playerMovement.skill2Speed;
        skill3CoolDown.fillAmount = (1.0f) * playerMovement.skill3Cooldown / playerMovement.skill3Speed;
        skill4CoolDown.fillAmount = (1.0f) * playerMovement.skill4Cooldown / playerMovement.skill4Speed;
        sprintPower.fillAmount = (1.0f) * playerMovement.sprintPower / playerMovement.sprintMaxPower;

    }
}
