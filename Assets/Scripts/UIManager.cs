using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private GameObject rulesPopup;
    [SerializeField] private GameObject textBoxObject;
    [SerializeField] private TextMeshProUGUI textBoxText;

    [SerializeField] private Slider timerSlider;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameRules gameRules;

    private float timeLeft;
    private bool stopTimer;
    private void Start()
    {
        InitTimer(null);
    }

    private void InitTimer(Dictionary<string, object> message)
    {
        stopTimer = false;
        var maxTimer = gameRules.maxTime;
        timerSlider.maxValue = maxTimer;
        timerSlider.value = maxTimer;
        timerText.text = $"{maxTimer:0}s";;
        timeLeft = maxTimer;
    }

    private void OnEnable()
    {
        EventManager.StartListening("UpdateCurrentScore", UpdateCurrentScore);
        EventManager.StartListening("UpdateHighScore", UpdateHighScore);
        EventManager.StartListening("DisplayMessage", DisplayMessage);
        EventManager.StartListening("HideMessage", HideMessage);
        EventManager.StartListening("StartTimer", InitTimer);
        EventManager.StartListening("StopTimer", StopTimer);
    }

    private void OnDisable()
    {
        EventManager.StopListening("UpdateCurrentScore", UpdateCurrentScore);
        EventManager.StopListening("UpdateHighScore", UpdateHighScore);
        EventManager.StopListening("DisplayMessage", DisplayMessage);
        EventManager.StopListening("HideMessage", HideMessage);
        EventManager.StopListening("StartTimer", InitTimer);
        EventManager.StopListening("StopTimer", StopTimer);
        
    }

    private void Update()
    {
        CalculateTimer();
    }

    private void CalculateTimer()
    {
        if (stopTimer != false) return;
        
        
        timeLeft -= Time.deltaTime; // Decrease timer each frame
        if (timerText != null)
        {
            timerSlider.value = timeLeft;
            timerText.text = Mathf.Max(0, timeLeft).ToString("F2") + "s"; // Display remaining time
        }

        if (timeLeft <= 0)
        {
            stopTimer = true;
            timeLeft = 0;
            EventManager.TriggerEvent(("TimesUp"), null);
        }
    }

    private void StopTimer(Dictionary<string, object> message)
    {
        stopTimer = true;
    }
    
    private void UpdateCurrentScore(Dictionary<string, object> message)
    {
        var score = message["score"].ToString();
        currentScoreText.text = score;
    }

    private void UpdateHighScore(Dictionary<string, object> message)
    {
        var score = message["score"].ToString();
        highScoreText.text = score;
    }

    private void DisplayMessage(Dictionary<string, object> message)
    {
        textBoxObject.SetActive(true);
        var text = message["message"].ToString();
        textBoxText.text = text;
    }

    private void HideMessage(Dictionary<string, object> message)
    {
        textBoxText.text = string.Empty;
        textBoxObject.SetActive(false);
    }

    public void ShowRulesPopup()
    {
        rulesPopup.SetActive(true);
    }

    public void ClosePopup()
    {
        rulesPopup.SetActive(false);
    }
}
