using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_2_1 : MonoBehaviour
{
    [Header("木屑")]
    [Tooltip("用於縮放的parent")]
    [SerializeField] Transform woodPowder;
    [SerializeField] MeshRenderer meshRenderer_woodPowder;
    private Color color_woodPowder;
    [Tooltip("木屑燃燒效果")]
    [SerializeField] private ParticleSystem fire_woodPowder;
    [Tooltip("磅秤文字")]
    [SerializeField] Text weightText_woodPowder;
    [SerializeField] Text weightText_woodPowderDisplay;

    [Tooltip("目前木屑重量")]
    [SerializeField] private float initialWeight_woodPowder = 20f;
    [Tooltip("燃燒後木屑重量")]
    [SerializeField] private float finalWeight_woodPowder = 10f;
    float currentWeight_woodPowder;
    //private float weightChangeSpeed_woodPowder;          //  木屑改變重量速度(沒用到的數值，功能與燃燒時間重複)
    [Tooltip("木屑需要持續加熱的時間")]
    [SerializeField] private float heatingTime_woodPowder = 3f;
    [Tooltip("木屑持續燃燒時間")]
    [SerializeField] private float burningTime_woodPowder = 10f;
    [Tooltip("木屑冷卻時間")]
    [SerializeField] private float coolingTime_woodPowder = 1f;
    [Tooltip("木屑燃燒後的尺寸")]
    [SerializeField] private float finalRelativeScale = 0.5f;
    private Vector3 initialScale_woodPowder;              //  初始木屑尺寸
    private Vector3 finalScale_woodPowder;                //  最後的木屑尺寸
    //private float scalingSpeed_woodPowder;              //  木屑縮小速度(沒用到的數值，功能與燃燒時間重複)
    [Tooltip("木屑變色速度")]
    [SerializeField] private float colorChangeSpeed_woodPowder;

    private float timer_woodPowder = 0.0f;              //  計時器
    private int status_woodPowder = 0;                  //  木屑狀態

    [Header("鋼絲絨")]
    [SerializeField] MeshRenderer[] steelWools;         //  鋼絲絨
    private Color[] steelWoolColors;
    [Tooltip("鋼絲絨燃燒效果")]
    [SerializeField] private ParticleSystem fire_steelWool;
    [Tooltip("磅秤文字")]
    [SerializeField] Text weightText_steelWool;
    [SerializeField] Text weightText_steelWoolDisplay;

    [Tooltip("目前鋼絲絨重量")]
    [SerializeField] private float initialWeight_steelWool = 20f;
    [Tooltip("燃燒後鋼絲絨重量")]
    [SerializeField] private float finalWeight_steelWool = 30f;
    float currentWeight_steelWool;
    //private float weightChangeSpeed_steelWool = 1f;     //  鋼絲絨改變重量速度
    [Tooltip("鋼絲絨需要持續加熱的時間")]
    [SerializeField] private float heatingTime_steelWool = 5f;
    [Tooltip("鋼絲絨持續燃燒時間")]
    [SerializeField] private float burningTime_steelWool = 10f;
    [Tooltip("鋼絲絨冷卻時間")]
    [SerializeField] private float coolingTime_steelWool = 3f;
    [Tooltip("鋼絲絨變色速度")]
    [SerializeField] private float colorChangeSpeed_steelWool;

    private float timer_steelWool = 0.0f;                //  計時器
    private int status_steelWool = 0;                    //  鋼絲絨狀態

    private bool firstTimeWarning = true;               //  第一次抓取危險物品的通知

    private LevelObjManager levelObjManager;
    private AudioManager audioManager;          //音樂管理
    private HintManager hintManager;            //管理提示板

    // Start is called before the first frame update
    void OnEnable()
    {
        levelObjManager = FindObjectOfType<LevelObjManager>();
        audioManager = FindObjectOfType<AudioManager>();
        hintManager = FindObjectOfType<HintManager>();

        hintManager.gameObject.SetActive(true);
        hintManager.SwitchStep("T2_1_1");

        fire_woodPowder.Stop();
        
        currentWeight_woodPowder = initialWeight_woodPowder;
        // 計算改變重量速度 這個沒用到...
        //weightChangeSpeed_woodPowder = (initialweight_woodPowder - finalWeight_woodPowder) / burningTime_woodPowder;
        // 初始化比例
        initialScale_woodPowder = Vector3.one * woodPowder.localScale.x;
        finalScale_woodPowder = new Vector3( initialScale_woodPowder.x * (finalRelativeScale + ( 1.0f - finalRelativeScale ) * 0.8f), initialScale_woodPowder.y * finalRelativeScale, initialScale_woodPowder.z * (finalRelativeScale + (1.0f - finalRelativeScale) * 0.8f));
        //scalingSpeed_woodPowder 完全沒使用到...
        //scalingSpeed_woodPowder = (initialScale_woodPowder - finalScale_woodPowder) / burningTime_woodPowder;
        color_woodPowder = meshRenderer_woodPowder.material.color;

        fire_steelWool.Stop();

        currentWeight_steelWool = initialWeight_steelWool;
        // 計算改變重量速度 這個沒用到...
        //weightChangeSpeed_steelWool = (finalWeight_steelWool - weight_steelWool) / burningTime_steelWool;
        steelWoolColors = new Color[steelWools.Length];
        for (int i = 0; i < steelWools.Length; i++)
        {
            steelWoolColors.SetValue(steelWools[i].material.color, i);
            //  隱藏其他鋼絲絨
            if (i > 0)
            {
                Color invisible = steelWoolColors[i];
                invisible.a = 0f;
                steelWools[i].material.color = invisible;
            }
        }
        weightText_woodPowder.text = currentWeight_woodPowder.ToString("F2") + "g";
        weightText_woodPowderDisplay.text = currentWeight_woodPowder.ToString("F2") + "g";
        weightText_steelWool.text = currentWeight_steelWool.ToString("F2") + "g";
        weightText_steelWoolDisplay.text = currentWeight_steelWool.ToString("F2") + "g";
    }

    void Update()
    {
        switch (status_woodPowder)
        {
            case 0:     //  待加熱
                if (timer_woodPowder >= heatingTime_woodPowder)
                {
                    timer_woodPowder = burningTime_woodPowder;
                    fire_woodPowder.Play();
                    status_woodPowder++;
                }
                break;
            case 1:     //  燃燒中
                timer_woodPowder -= Time.deltaTime;

                //計算比例 0% > 100%
                float processPercent = 1.0f - timer_woodPowder/burningTime_woodPowder;
                //scale_woodPowder -= scalingSpeed_woodPowder * Time.deltaTime;
                Vector3 currentScale;
                currentScale = Vector3.Lerp(initialScale_woodPowder, finalScale_woodPowder, processPercent);
                woodPowder.localScale = currentScale;

                //重量減輕
                //currentWeight_woodPowder -= weightChangeSpeed_woodPowder * Time.deltaTime;
                currentWeight_woodPowder = (initialWeight_woodPowder - finalWeight_woodPowder) * (1.0f - processPercent) + finalWeight_woodPowder;
                //處理變色
                color_woodPowder.a = timer_woodPowder / burningTime_woodPowder;
                // 更新物件的顏色
                meshRenderer_woodPowder.material.color = color_woodPowder;

                if (timer_woodPowder <= 0f)
                {
                    timer_woodPowder = coolingTime_woodPowder;
                    fire_woodPowder.Stop();
                    status_woodPowder++;
                }
                break;
            case 2:     //  冷卻中
                timer_woodPowder -= Time.deltaTime;
                if (timer_woodPowder <= 0f)
                {
                    if (status_steelWool == 3)
                        EndTheTutorial();
                    status_woodPowder++;
                }
                break;
        }

        switch (status_steelWool)
        {
            case 0:     //  待加熱

                //處理變色
                steelWoolColors[1].a = timer_steelWool / heatingTime_steelWool;
                if (timer_steelWool >= heatingTime_steelWool)
                {
                    timer_steelWool = burningTime_steelWool;
                    //fire_steelWool.Play();    //鋼絲絨燃燒沒有煙
                    steelWoolColors[1].a = 1f;
                    steelWoolColors[2].a = 1f;
                    steelWools[2].material.color = steelWoolColors[2];
                    status_steelWool++;
                }
                steelWools[1].material.color = steelWoolColors[1];

                break;
            case 1:     //  燃燒中
                timer_steelWool -= Time.deltaTime;

                //計算比例 0% > 100%
                float processPercent = 1.0f - timer_steelWool / burningTime_steelWool;
                //重量減輕
                currentWeight_steelWool = (initialWeight_steelWool - finalWeight_steelWool) * (1.0f - processPercent) + finalWeight_steelWool;
                //處理變色
                steelWoolColors[1].a = timer_steelWool / burningTime_steelWool;

                if (timer_steelWool <= 0f)
                {
                    timer_steelWool = coolingTime_steelWool;
                    fire_steelWool.Stop();
                    steelWoolColors[1].a = 0f;
                    status_steelWool++;
                }
                steelWools[1].material.color = steelWoolColors[1];
                break;
            case 2:     //  冷卻中
                timer_steelWool -= Time.deltaTime;
                if (timer_steelWool <= 0f)
                {
                    if (status_woodPowder == 3)
                        EndTheTutorial();
                    status_steelWool++;
                }
                break;
        }

        weightText_woodPowder.text = currentWeight_woodPowder.ToString("F2") + "g";
        weightText_woodPowderDisplay.text = currentWeight_woodPowder.ToString("F2") + "g";
        weightText_steelWool.text = currentWeight_steelWool.ToString("F2") + "g";
        weightText_steelWoolDisplay.text = currentWeight_steelWool.ToString("F2") + "g";
    }

    public void ReactionStay(GameObject obj)
    {
        if (obj.name == "WoodPowders" && status_woodPowder == 0)
        {
            //  開始加熱
            timer_woodPowder += Time.deltaTime;
        }
        else if (obj.name == "SteelWool" && status_steelWool == 0)
        {
            //  開始加熱
            timer_steelWool += Time.deltaTime;
        }
    }

    public void OnBlowtorchGrabbed()
    {
        if (firstTimeWarning)
        {
            audioManager.PlayVoice("W_Blowtorch");
            firstTimeWarning = false;
        }
    }

    void StartHeating(bool on)
    {

    }

    private void EndTheTutorial()   //完成教學
    {
        hintManager.SwitchStep("T2_1_2");
        hintManager.ShowNextButton(this.gameObject);
    }

    void CloseHint()    //關閉提示視窗
    {
        levelObjManager.LevelClear(0);
    }
}
