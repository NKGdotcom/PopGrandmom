using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] private Animator textAnimator;
    private float enthusiasmWaitTime;

    [SerializeField] private GameObject comboTextPrefab;
    [SerializeField] private Canvas uiCanvas; 

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
        enthusiasmTMP.text = $"{EnthusiasmMoney}yen";
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
    /// 熱狂度が上がる
    /// </summary>
    public void UpEnthsiasm()
    {
        if (!startEnthusiasm) startEnthusiasm = true;
        if (isFinish) return;
        textAnimator.SetTrigger("HitWall");
        isComboTime = true;
        comboTime = defaultComboTime;
        enthusiasmWaitTime = defaultEnthusiasmWaitTime;
        ShowComboText();
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
    /// <summary>
    /// コンボテキストを表示
    /// </summary>
    private void ShowComboText()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        // 少しランダムにずらす（例：X方向±30、Y方向+30〜+60）
        float offsetX = Random.Range(-30f, 30f);
        float offsetY = Random.Range(30f, 60f);
        screenPos += new Vector3(offsetX, offsetY, 0);

        // コンボテキスト生成
        GameObject comboObj = Instantiate(comboTextPrefab, uiCanvas.transform);
        comboObj.GetComponent<RectTransform>().position = screenPos;

        // テキストをセット
        TextMeshProUGUI text = comboObj.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
        {
            text.text = $"{comboNum + 1} Combo!!";
        }

        Destroy(comboObj, 1f);
    }
}
