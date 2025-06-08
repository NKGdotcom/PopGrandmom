using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameResult : MonoBehaviour
{
    public static GameResult Instance { get; private set; }

    [SerializeField] private GameObject retry;
    [SerializeField] private Animator resultAnimator;

    [SerializeField] private float buttonDisplayTime = 4 / 3;
    [SerializeField] private float changeColorTime = 0.3f;

    private Color redColor = new Color(1, 0, 0);
    private Color whiteColor = new Color(1, 1, 1);

    private bool isPlayMusic;
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
        SoundManager.Instance.PlayBGM(BGMSource.gamescene);
        SetEventTrigger(retry);
        ChangeTextColor(retry);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// テキストの色を変える準備
    /// </summary>
    /// <param name="_textObj"></param>
    private void ChangeTextColor(GameObject _textObj)
    {
        _textObj.AddComponent<EventTrigger>();
        TextMeshProUGUI tmp = _textObj.GetComponent<TextMeshProUGUI>();

        if (tmp == null)
        {
            Debug.LogWarning("TextMeshProUGUI が見つかりません");
            return;
        }

        EventTrigger _eventTrigger = _textObj.GetComponent<EventTrigger>();

        void AddTrigger(EventTriggerType type, Color color)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = type;
            entry.callback.AddListener((x) => HandlePointerColorChange(_textObj, color));
            _eventTrigger.triggers.Add(entry);
        }

        AddTrigger(EventTriggerType.PointerEnter, redColor);
        AddTrigger(EventTriggerType.PointerExit, whiteColor);
    }
    /// <summary>
    /// 色を変える
    /// </summary>
    /// <param name="_textObj"></param>
    /// <param name="targetColor"></param>
    private void HandlePointerColorChange(GameObject _textObj, Color targetColor)
    {
        TextMeshProUGUI tmp = _textObj.GetComponent<TextMeshProUGUI>();
        if (tmp != null)
        {
            ChangeColor(tmp, targetColor).Forget();
        }
    }
    private async UniTaskVoid ChangeColor(TextMeshProUGUI text, Color targetColor)
    {
        float duration = changeColorTime; // 色変化の所要時間
        float time = 0f;
        Color startColor = text.color;

        while (time < duration)
        {
            time += Time.deltaTime;
            text.color = Color.Lerp(startColor, targetColor, time / duration);
            await UniTask.Yield();
        }
        text.color = targetColor;
    }
    public async UniTaskVoid Result()
    {
        resultAnimator.SetTrigger("Finish");
        await UniTask.Delay(TimeSpan.FromSeconds(buttonDisplayTime));
        retry.SetActive(true);
        if (isPlayMusic) return;
        isPlayMusic = true;
        SoundManager.Instance.PlaySE(SESource.result);
    }
    private void SetEventTrigger(GameObject _textObj)
    {
        _textObj.AddComponent<EventTrigger>();
        EventTrigger _eventTrigger = _textObj.GetComponent<EventTrigger>();
        EventTrigger.Entry _entry = new EventTrigger.Entry();
        _entry.eventID = EventTriggerType.PointerClick;
        _entry.callback.AddListener((x) => PlayScene());
        _eventTrigger.triggers.Add(_entry);
    }
    private void PlayScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
