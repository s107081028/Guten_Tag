using Photon.Pun;


using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillBoxChanger : MonoBehaviour
{

    /*For waiting room change skill box image and text*/
    public Image skillImage;
    public TextMeshProUGUI skillText;

    public int curSkillNum = 0;

    public SkillData skillData;

    public GameObject targetCharacter;
    public CharacterSkinChanger skinChanger;

    private MenuStart menuStart;
    private PhotonView photonView;

    void Start()
    {
        menuStart = GameObject.Find("RoomManager").GetComponent<MenuStart>();
        photonView = gameObject.GetComponent<PhotonView>();

        //setup defualt skill descriptiona and character skin
        skillImage.sprite = skillData.getSkillSpite(curSkillNum);
        skillText.text = skillData.getSkillText(curSkillNum);
        skinChanger.setAllSkinPartByIndex(targetCharacter, skillData.getSkinSeq(curSkillNum));

    }

    public void addCurSkillNum() {
        curSkillNum++;
        curSkillNum = (curSkillNum) % skillData.getSkillCount();
        updateSkillUI();
                
    }

    public void subCurSkillNum() {
        curSkillNum--;
        curSkillNum = (curSkillNum+ skillData.getSkillCount()) % skillData.getSkillCount();
        updateSkillUI();
    }

    public void setCurSkillNum(int num)
    {
        curSkillNum = num;

    }
    public void updateSkillUI() {

        updateSkin();
       
    }





    /*if skill realted to skin, example*/
    public void updateSkin() {
        Debug.Log("UPDATE SKIN");


        //Update player properity
        menuStart.UpdatePlayerCharacter(curSkillNum);


        //Notify other player 
        photonView.RPC("updateSkinRPC", RpcTarget.All, curSkillNum);

        //skinChanger.setAllSkinPartByIndex(targetCharacter, skillData.getSkinSeq(curSkillNum));

    }



    [PunRPC]
    void updateSkinRPC(int num) {
        Debug.Log("UPDATE SKIN RPC: "+num.ToString());
        skinChanger.setAllSkinPartByIndex(targetCharacter, skillData.getSkinSeq(num));
        skillImage.sprite = skillData.getSkillSpite(num);
        skillText.text = skillData.getSkillText(num);
    }

   
}
