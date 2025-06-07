using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enthusiasm : MonoBehaviour
{
    public static Enthusiasm Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI enthusiasmTMP;

    [SerializeField] private List<GameObject> notEnthusiast;
    [SerializeField] private Slider enthusiasmSlider;
    [SerializeField] private float defaultAddEnthusiasm = 5;
    [SerializeField] private int borderEnthusiast = 10;
    [SerializeField] private float defaultAddMoney = 100;
    [SerializeField] private float defaultComboMoney = 0.05f;
    private int comboNum;
    private List<GameObject> enthusiast = new List<GameObject>();
    [SerializeField] private float addEnthusiastNum = 15;
    [SerializeField] private float multiplyEnthusiasm = 10;
    private float nowEnthusiasm;
    private float maxEnthusiasm;

    [SerializeField] private float defaultComboTime = 5;
    private float comboTime;
    [SerializeField] private float defaultEnthusiasmWaitTime = 10;
    private float enthusiasmWaitTime;

    private bool isComboTime = false;
    private bool startEnthusiasm = false;
    private bool isFinish = false;

    public float EnthusiasmMoney { get; private set; }
    public float EnthusiasmPower { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxEnthusiasm = notEnthusiast.Count * addEnthusiastNum;
        enthusiasmSlider.value = nowEnthusiasm / maxEnthusiasm;
        comboTime = defaultComboTime;
        enthusiasmWaitTime = defaultEnthusiasmWaitTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (nowEnthusiasm < 0)
        {
            if (isFinish) return;
            isFinish = false;
            nowEnthusiasm = 0;
            GameResult.Instance.Result().Forget();
        }
        if (isComboTime)
        {
            comboTime -= Time.deltaTime;
            if(comboTime < 0)
            {
                isComboTime = false;
                comboNum = 0;
                comboTime = defaultComboTime;         
            }
        }
        else if (startEnthusiasm && !isComboTime)
        {
            nowEnthusiasm -= Time.deltaTime*multiplyEnthusiasm;
            enthusiasmSlider.value = nowEnthusiasm / maxEnthusiasm;
            if(nowEnthusiasm % addEnthusiastNum <0.13f)
            {
                if(enthusiast.Count > 0)
                {
                    int _num = Random.Range(0, enthusiast.Count);
                    enthusiast[_num].SetActive(false);

                    notEnthusiast.Add(enthusiast[_num]);
                    enthusiast.Remove(enthusiast[_num]);
                }
            }
        }
        
    }
    /// <summary>
    /// îMã∂ìxÇ™è„Ç™ÇÈ
    /// </summary>
    public void UpEnthsiasm()
    {
        if (!startEnthusiasm) startEnthusiasm = true;

        isComboTime = true;
        comboTime = defaultComboTime;
        enthusiasmWaitTime = defaultEnthusiasmWaitTime;

        nowEnthusiasm += defaultAddEnthusiasm;
        if (nowEnthusiasm > maxEnthusiasm) nowEnthusiasm = maxEnthusiasm;
        enthusiasmSlider.value = nowEnthusiasm / maxEnthusiasm;

        if(nowEnthusiasm % addEnthusiastNum == 0)
        {
            if(notEnthusiast.Count > 0)
            {
                int _num = Random.Range(0, notEnthusiast.Count);
                notEnthusiast[_num].SetActive(true);

                enthusiast.Add(notEnthusiast[_num]);
                notEnthusiast.Remove(notEnthusiast[_num]);
            }
        }

        EnthusiasmMoney += (defaultAddMoney + defaultAddMoney * defaultComboMoney * comboNum);
        enthusiasmTMP.text = $"{EnthusiasmMoney}yen";
        comboNum++;
    }
}
