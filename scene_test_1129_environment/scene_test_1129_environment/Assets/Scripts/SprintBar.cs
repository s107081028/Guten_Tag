using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SprintBar : MonoBehaviourPun
{
    [SerializeField] Slider sprintBar;
    private SkillController skillController;
    // Start is called before the first frame update

    private bool isTimeToSync = true;
    public float syncTimeDiff = 0.3f;

    private float currentSprintValue = 1f;
    void Start()
    {
        skillController = GetComponent<SkillController>();
    }

    // Update is called once per frame
    void Update()
    {

        
        sprintBar.value = Mathf.Lerp(sprintBar.value, currentSprintValue, Time.deltaTime); ;


        if (!photonView.IsMine)
            return;

        currentSprintValue = (1.0f) * skillController.sprintPower / skillController.sprintMaxPower;
       


        if (isTimeToSync) {
            isTimeToSync = false;
            CallOtherUpdateSprintBarValueRPC();
            Invoke(nameof(resetIsTimeToSync),syncTimeDiff);
        }
    }


    void resetIsTimeToSync() {
        isTimeToSync = true;
    }

    void CallOtherUpdateSprintBarValueRPC() {
        photonView.RPC(nameof(updateSprintValue), RpcTarget.Others,sprintBar.value);
    }

    [PunRPC]
    void updateSprintValue(float val) {
        currentSprintValue = val;
    }
}