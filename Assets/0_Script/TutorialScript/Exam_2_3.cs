﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Exam_2_3 : MonoBehaviour
{
    [Header("實驗道具")]
    [Tooltip("試管")]
    [SerializeField] private GameObject testTube;
    [Tooltip("試管內粉末")]
    [SerializeField] private LiquidController testTubePowder;
    [Tooltip("試管內粉末顏色")]
    private Color testTubePowderColor;
    [Tooltip("試管內粉末反應後顏色")]
    [SerializeField] private Color testTubePowderColor_final;
    [Tooltip("試管架位置")]
    [SerializeField] private Transform testTubePoint;
    [Tooltip("盤子內的碳粉")]
    [SerializeField] private GameObject tonerPowder;
    [Tooltip("盤子內的氧化銅粉")]
    [SerializeField] private GameObject copperOxidePowder;
    [Tooltip("碳粉效果")]
    [SerializeField] private GameObject particleSystem_toner;
    [Tooltip("氧化銅粉效果")]
    [SerializeField] private GameObject particleSystem_copperOxide;
    [Tooltip("酒精燈蓋子")]
    [SerializeField] private Animator cap;
    [Tooltip("酒精燈火焰")]
    [SerializeField] private GameObject fire;


    [Header("質量設定")]
    [Tooltip("磅秤文字")]
    [SerializeField] Text weightText;
    [Tooltip("磅秤目前數值")]
    [SerializeField] private float scaleVale = 0f;
    [Tooltip("碳粉重量")]
    [SerializeField] private float weight_toner = 50f;
    [Tooltip("氧化銅粉重量")]
    [SerializeField] private float weight_copperOxide = 50f;
    [Tooltip("反應後的重量")]
    [SerializeField] private float weight_finsh = 80f;

    [Header("UI")]
    [Tooltip("質量文字")]
    [SerializeField] private Text[] massTexts;

    [Tooltip("反應時間")]
    [SerializeField] private float reactionTime = 5f;
    [Tooltip("答題延遲")]
    [SerializeField] private float answerDelay = 3f;
    private float timer = 0f;
    private int Status = 0;

    private LevelObjManager levelObjManager;
    private HintManager hintManager;            //管理提示板
    private QuestionManager questionManager; //管理題目介面
    private AudioManager audioManager;          //音樂管理

    // Start is called before the first frame update
    void Start()
    {
        levelObjManager = FindObjectOfType<LevelObjManager>();
        hintManager = FindObjectOfType<HintManager>();
        questionManager = FindObjectOfType<QuestionManager>();
        audioManager = FindObjectOfType<AudioManager>();

        hintManager.gameObject.SetActive(true);
        hintManager.SwitchStep("E2_3_1");
        weightText.text = "0g";
        testTubePowderColor = testTubePowder.liquidColor;
    }

    private void Update()
    {
        switch (Status)
        {
            case 0://待粉末裝滿
                break;
            case 1://待點燃酒精燈
                break;
            case 2://待試管加熱完成
                break;
            case 3://待試管放置於架上
                break;
            case 4://試管放置於架上
                timer += Time.deltaTime;
                if (timer >= answerDelay)
                {
                    levelObjManager.loading_sign.SetActive(false);
                    questionManager.ShowExam(2, gameObject);
                    timer = 0f;
                    Status++;
                }
                break;
            case 5://回答第一題
                break;
            case 6://答完第一題
                timer += Time.deltaTime;
                if (timer >= answerDelay)
                {
                    levelObjManager.loading_sign.SetActive(false);
                    questionManager.ShowExam(3, gameObject);
                    timer = 0f;
                    Status++;
                }
                break;
            case 7://回答第二題
                break;
            case 8://答完第二題
                timer += Time.deltaTime;
                if (timer >= answerDelay)
                {
                    levelObjManager.loading_sign.SetActive(false);
                    questionManager.ShowExam(4, gameObject);
                    timer = 0f;
                    Status++;
                }
                break;
            case 9://回答第三題
                break;
            case 10://答完第三題
                timer += Time.deltaTime;
                if (timer >= answerDelay)
                {
                    levelObjManager.loading_sign.SetActive(false);
                    levelObjManager.LevelClear(2);
                    timer = 0f;
                    Status++;
                }
                break;
        }
    }

    void CloseHint()    //關閉提示視窗
    {

    }

    public void ReactionStay(GameObject sender)
    {
        {
            switch (Status)
            {
                case 2://試管加熱
                    if (sender.name == "TestTube_Mixed_2-6_Clamp")
                    {
                        timer += Time.deltaTime;
                        //計算比例 0% > 100%
                        float processPercent = timer / reactionTime;

                        // 使用 Lerp 混合顏色
                        testTubePowderColor = Color.Lerp(testTubePowderColor, testTubePowderColor_final, processPercent);
                        // 更新物件的顏色
                        testTubePowder.liquidColor = testTubePowderColor;

                        if (timer >= reactionTime)
                        {
                            testTube.GetComponent<CollisionDetection>().targetName = "TestTube_point";
                            hintManager.SwitchStep("E2_3_5");
                            timer = 0f;
                            Status++;
                        }
                    }
                    break;
                case 3://加熱完的試管放置於架上
                    if (sender.name == "TestTube_Mixed_2-6")
                    {
                        testTube.tag = "Untagged";
                        testTube.transform.SetParent(testTubePoint);
                        testTube.transform.localPosition = new Vector3(0f, 0f, 0f);
                        testTube.transform.localRotation = Quaternion.identity;

                        //記錄反應後重量
                        scaleVale = weight_finsh;
                        weightText.text = scaleVale.ToString("0") + "g";
                        massTexts[1].text = scaleVale.ToString("0") + "g";
                        levelObjManager.loading_sign.SetActive(true);
                        Status++;
                    }
                    break;
            }
        }
    }

    //  液體裝滿時通知
    public void LiquidFull(GameObject obj)
    {
        switch (obj.name)
        {
            case "TestTube_Empty_2-6":
                if (Status == 0)
                {
                    //將試管倒入篩選改為氧化銅粉
                    testTube.GetComponent<LiquidController>().injectFilter = "CopperOxide_2-6";
                    testTube.name = "TestTube_Toner_2-6";
                    tonerPowder.SetActive(false);
                    particleSystem_toner.SetActive(false);
                    scaleVale = weight_toner;
                    weightText.text = scaleVale.ToString("0") + "g";
                }
                break;
            case "TestTube_Toner_2-6":
                if (Status == 0)
                {
                    testTube.GetComponent<CollisionDetection>().targetName = "Alcohol Lamp Flame";
                    testTube.name = "TestTube_Mixed_2-6";
                    testTube.tag = "TweezersClamp";
                    copperOxidePowder.SetActive(false);
                    particleSystem_copperOxide.SetActive(false);
                    scaleVale = weight_toner + weight_copperOxide;
                    weightText.text = scaleVale.ToString("0") + "g";
                    //記錄反應前重量
                    massTexts[0].text = scaleVale.ToString("0") + "g";
                    hintManager.SwitchStep("E2_3_2");
                    Status++;
                }
                break;
        }
    }

    public void OnAlcoholLampTouched()
    {
        if (Status == 1)
        {
            audioManager.PlayVoice("W_AlcoholLamp");
            cap.SetBool("cover", true);
            fire.SetActive(true);

            hintManager.SwitchStep("E2_3_4");
            Status++;
        }
    }

    //答題完畢
    public void FinishExam()
    {
        levelObjManager.loading_sign.SetActive(true);
        Status++;
    }
}
