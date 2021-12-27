using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkinChanger : MonoBehaviour
{
    /*
    public List<GameObject> mainBodySelect;
    public List<GameObject> bodyPartSelect;
    public List<GameObject> eyeSelcet;
    public List<GameObject> gloveSelcet;
    public List<GameObject> headPartSelect;
    public List<GameObject> mouthNoseSelect;
    public List<GameObject> tailSelect;
    */

    public GameObject defaultCharacter;
    

    public List<string> partNames = new List<string>(){"Bodies", "Bodyparts" ,"Eyes","Gloves", "Headparts","MouthandNoses","Tails" };

    public bool changeSkinOfThis = false;
    public List<int> changeSkinList = new List<int>();


    void Start()
    {
        if (changeSkinOfThis)
            setAllSkinPartByIndex(gameObject, changeSkinList);
        //setAllSkinPartByIndex(defaultCharacter,new List<int> { 2, 2, 2, 3, 3, 4, 2 });
        
    }



    public void setAllSkinPartByIndex( GameObject character, List<int> showSkinPartIndexList) {

        /*Disable all first*/
        foreach (string parentName in partNames) {
            Transform targetGroup = character.transform.Find(parentName);

            if (targetGroup == null) Debug.Log(parentName + "group i null");
            foreach (Transform obj in targetGroup)
            {
                obj.gameObject.SetActive(false);
            }

        }



        for (int i = 0; i < 7; i++) {
            GameObject findSkinPart = character.transform.Find(partNames[i]).transform.GetChild(showSkinPartIndexList[i]).gameObject;
            if (findSkinPart != null) findSkinPart.SetActive(true);
        }

    }

    /*
    public void setAllSkinPart(GameObject character) {

        //body
        setSkinPartOn(character, "Bodies", mainBodySelect.ConvertAll<string>(mainBodySelect => mainBodySelect.name));

        //bodyParts
        setSkinPartOn(character, "Bodyparts", bodyPartSelect.ConvertAll<string>(bodyPartSelect => bodyPartSelect.name));

        //eyes
        setSkinPartOn(character, "Eyes", eyeSelcet.ConvertAll<string>(eyeSelcet => eyeSelcet.name));

        setSkinPartOn(character, "Gloves", gloveSelcet.ConvertAll<string>(gloveSelcet => gloveSelcet.name));

        setSkinPartOn(character, "Headparts", headPartSelect.ConvertAll<string>(headPartSelect => headPartSelect.name));

        setSkinPartOn(character, "MouthandNoses", mouthNoseSelect.ConvertAll<string>(mouthNoseSelect => mouthNoseSelect.name));

        setSkinPartOn(character, "Tails", tailSelect.ConvertAll<string>(tailSelect => tailSelect.name));
    }

    */

    void setSkinPartOn(GameObject character, string findGroupName,List<string> findTargetNameList) {

       



 
        Transform targetGroup = character.transform.Find(findGroupName);

        if (targetGroup == null) Debug.Log(findGroupName+"group si null");
        foreach (Transform obj in targetGroup)
        {
            obj.gameObject.SetActive(false);
            foreach(string findTargetName in findTargetNameList)
                if (obj.gameObject.name == findTargetName) obj.gameObject.SetActive(true);
        }

        return;
    }
}
