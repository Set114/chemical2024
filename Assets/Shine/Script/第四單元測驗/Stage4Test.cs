using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Stage4Test : MonoBehaviour
{
    [Header("記錄五題答對答錯")]
    public bool[] Ans5State;
    [Header("記錄五題選擇的答案")]
    public string[] Ans5;
    [Header("五題題目")]
    public GameObject[] Iteam5;
    [Header("五題結果")]
    public GameObject[] FinalIteam5;
    //--第一題
    [Header("第一題三個選項")]
    public GameObject[] Iteam1Ans;

    //--第二題
    [Header("第二題三個選項")]
    public Image[] Iteam2Ans;

    //--第三題
    [Header("第三題2個選項")]
    public Image[] Iteam3Ans;

    //--第四題
    [Header("第四題2個選項")]
    public Image[] Iteam4Ans;

    [Header("第五題3個選項")]
    public Image[] Iteam5Ans;

    public Sprite SelectSprite;

    public int Score;
    public TMP_Text ScoreText;
    public TMP_Text[] Iteams;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CheckIteam1() {
        for (int i = 0; i < Iteam1Ans.Length; i++) {
            if (Iteam1Ans[i].active) {
                if (Iteam1Ans[i].transform.parent.gameObject.name == "Hay") {
                    Ans5State[0] = true;
                }
                Ans5[0] = Iteam1Ans[i].transform.parent.gameObject.name;
            }
        }
        if (Ans5State[0])
        {
            FinalIteam5[0].transform.GetChild(3).gameObject.SetActive(true);
        }
        else {
            FinalIteam5[0].transform.GetChild(4).gameObject.SetActive(true);
        }
        Iteam5[0].SetActive(false);
        FinalIteam5[0].SetActive(true);
    }
    public void CheckIteam2()
    {
        for (int i = 0; i < Iteam2Ans.Length; i++)
        {
            if (Iteam2Ans[2].sprite == SelectSprite) {
                Ans5State[1] = true;
            }
            if (Iteam2Ans[i].sprite == SelectSprite)
            {

                Ans5[1] = Iteam2Ans[i].transform.GetChild(0).GetComponent<TMP_Text>().text;
            }
        }
        if (Ans5State[1])
        {
            FinalIteam5[1].transform.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            FinalIteam5[1].transform.GetChild(4).gameObject.SetActive(true);
        }
        Iteam5[1].SetActive(false);
        FinalIteam5[1].SetActive(true);
    }
    public void CheckIteam3()
    {
        for (int i = 0; i < Iteam3Ans.Length; i++)
        {
            if (Iteam3Ans[0].sprite == SelectSprite)
            {
                Ans5State[2] = true;
            }
            if (Iteam3Ans[i].sprite == SelectSprite)
            {

                Ans5[2] = Iteam3Ans[i].transform.GetChild(0).GetComponent<TMP_Text>().text;
            }
        }
        if (Ans5State[2])
        {
            FinalIteam5[2].transform.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            FinalIteam5[2].transform.GetChild(4).gameObject.SetActive(true);
        }
        Iteam5[2].SetActive(false);
        FinalIteam5[2].SetActive(true);
    }
    public void CheckIteam4()
    {
        for (int i = 0; i < Iteam4Ans.Length; i++)
        {
            if (Iteam4Ans[1].sprite == SelectSprite)
            {
                Ans5State[3] = true;
            }
            if (Iteam4Ans[i].sprite == SelectSprite)
            {

                Ans5[3] = Iteam4Ans[i].transform.GetChild(0).GetComponent<TMP_Text>().text;
            }
        }
        if (Ans5State[3])
        {
            FinalIteam5[3].transform.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            FinalIteam5[3].transform.GetChild(4).gameObject.SetActive(true);
        }
        Iteam5[3].SetActive(false);
        FinalIteam5[3].SetActive(true);
    }

    public void CheckIteam5()
    {
        for (int i = 0; i < Iteam5Ans.Length; i++)
        {
            if (Iteam5Ans[0].sprite == SelectSprite)
            {
                Ans5State[4] = true;
            }
            if (Iteam5Ans[i].sprite == SelectSprite)
            {

                Ans5[4] = Iteam5Ans[i].transform.GetChild(0).GetComponent<TMP_Text>().text;
            }
        }
        if (Ans5State[4])
        {
            FinalIteam5[4].transform.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            FinalIteam5[4].transform.GetChild(4).gameObject.SetActive(true);
        }
        Iteam5[4].SetActive(false);
        FinalIteam5[4].SetActive(true);
    }
    public void Final() {
        for (int i = 0; i < Ans5State.Length; i++) {
            if (Ans5State[i])
            {
                Score += 20;
                Iteams[i].color = Color.black;
            }
            else {
                Iteams[i].color = Color.red;
            }
            if (Ans5[0]!=null)
            {
                switch (Ans5[0]) {
                    case "Leaves":
                        Iteams[0].text =  "1.樹葉";

                        break;

                    case "Hay":
                        Iteams[0].text =  "1.乾草堆";

                        break;

                    case "Branches":
                        Iteams[0].text = "1.樹枝";

                        break;
                }
            }
            if (i>0)
            {
                Iteams[i].text = (i+1) + "." + Ans5[i];
            }
        }
        ScoreText.text = Score + "分";
        if (Score < 60)
        {
            ScoreText.color = Color.red;
        }
        else {
            ScoreText.color = Color.black;
        }

    }
}
