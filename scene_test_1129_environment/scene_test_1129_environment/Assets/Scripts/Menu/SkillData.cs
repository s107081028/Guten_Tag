using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData : MonoBehaviour
{

    

    [SerializeField]
    public List<string> skillTextList;

    [SerializeField]
    public List<Sprite> spriteList;


    public List<List<int>> skinList = new List<List<int>>();



    private void Start()
    {
        setSkinList();
    }

    public string getSkillText(int idx) {
        
        if (idx > skillTextList.Count - 1 || idx < 0) return "";
        return skillTextList[idx];
    }

    public Sprite getSkillSpite(int idx)
    {

        if (idx > spriteList.Count - 1 || idx < 0) return null;
        return spriteList[idx];
    }

    public List<int> getSkinSeq(int idx)
    {

        if (idx > spriteList.Count - 1 || idx < 0) return null;
        return skinList[idx];
    }



    public void setSkinList() {
        skinList.Add(new List<int>() { 2, 2, 2, 2, 2, 0, 2 });
        skinList.Add(new List<int>() { 5, 1, 1, 3, 1, 1, 1 });
    }

    public int getSkillCount() {
        //gameObject.AddComponent<Skill_HIDE>();
        return spriteList.Count;
    }


}
