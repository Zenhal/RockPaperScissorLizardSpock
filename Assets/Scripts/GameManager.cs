using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameRules gameRules;
    [SerializeField] private PrefabContainer prefabContainer;
    [SerializeField] private HorizontalLayoutGroup layoutGroup;
    [SerializeField] private Transform playerChoiceContainer;
    [SerializeField] private Transform botChoiceContainer;

    private Dictionary<string, GameObject> choiceDictionary;
    private List<string> choiceList;

    private string playerChoice;
    private string botChoice;

    private int playerScore = 0;
    private bool isWin = false;

    private void Start()
    {
        Init();
        InitTimer();
        UpdateHighScore();
        InitialiseChoices();
    }

    private void OnEnable()
    {
        EventManager.StartListening("PlayerSelect", PlayerSelect);
        EventManager.StartListening("TimesUp", OnTimesUp);
    }

    private void OnDisable()
    {
        EventManager.StopListening("PlayerSelect", PlayerSelect);
        EventManager.StopListening("TimesUp", OnTimesUp);
    }

    private void Init()
    {
        choiceList = new List<string>();
        choiceDictionary = new Dictionary<string, GameObject>();
        if (!isWin)
        {
            playerScore = 0;
            UpdateScore();
        }
        playerChoice = string.Empty;
        botChoice = string.Empty;
        isWin = false;
    }

    private void InitTimer()
    {
        EventManager.TriggerEvent("StartTimer", null);
    }

    private void InitialiseChoices()
    {
        var prefabList = prefabContainer.prefabs;
        for (int i = 0; i < prefabList.Count; i++)
        {
            var choiceName = prefabList[i].name;
            var gameObj = Instantiate(prefabList[i].prefab, layoutGroup.transform);
            choiceDictionary.Add(choiceName, gameObj);
            choiceList.Add(choiceName);
        }
    }
    
    private void PlayerSelect(Dictionary<string, object> message)
    {
        var choice = message["choice"].ToString();
        
        if (!string.IsNullOrEmpty(playerChoice)) return; // Prevent multiple inputs

        EventManager.TriggerEvent("StopTimer", null);
        playerChoice = choice;
        botChoice = choiceList[Random.Range(0, choiceList.Count)];
        Debug.Log("Bot Choice : " + botChoice);
        
        SpawnChoice(playerChoice, playerChoiceContainer); //spawn player choice
        SpawnChoice(botChoice, botChoiceContainer); //spawn bot choice
        
        //timer.StopTimer();
        CheckWinner(playerChoice, botChoice);
    }

    private void SpawnChoice(string choice, Transform parentTransform)
    {
        var gameObj = choiceDictionary[choice];
        var gO = Instantiate(gameObj, parentTransform);
        gO.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        gO.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
    }

    private void CheckWinner(string playerChoice, string botChoice)
    {
        if (playerChoice.Equals(botChoice)) //Tie Condition
        {
            var message = $"Both chose {playerChoice}. It's a tie!";
            DisplayMessage(message);
            Invoke(nameof(Restart), 2f);
        }
        else if (gameRules.DoesWin(playerChoice, botChoice)) //Win Codition
        {
            isWin = true;
            var message = $"You chose {playerChoice}, bot chose {botChoice}. You Win!";
            DisplayMessage(message);
            playerScore++;
            UpdateScore();
            Invoke(nameof(Restart), 2f);
        }
        else //Lose Condition
        {
            var message = $"You chose {playerChoice}, bot chose {botChoice}. You Lose!";
            DisplayMessage(message);
            Invoke(nameof(Restart), 2f);
        }
    }

    private void DisplayMessage(string message)
    {
        EventManager.TriggerEvent("DisplayMessage", new Dictionary<string, object>() {{ "message", message }});
    }

    private void UpdateScore()
    {
        EventManager.TriggerEvent("UpdateCurrentScore", new Dictionary<string, object>() {{ "score", playerScore}});
        CheckAndUpdateHighScore();
    }

    private void CheckAndUpdateHighScore()
    {
        var highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (playerScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", playerScore);
            UpdateHighScore();
        }
    }

    private void UpdateHighScore()
    {
        var highScore = PlayerPrefs.GetInt("HighScore");
        EventManager.TriggerEvent("UpdateHighScore", new Dictionary<string, object>() {{ "score", highScore}});
    }

    private void Restart()
    {
        Debug.Log("Restart");
        ClearObjects(layoutGroup.transform);
        ClearObjects(playerChoiceContainer);
        ClearObjects(botChoiceContainer);
        
        EventManager.TriggerEvent("HideMessage", null);
        
        Init();
        InitTimer();
        InitialiseChoices();
    }

    private void ClearObjects(Transform parentTransform)
    {
        foreach (Transform child in parentTransform)
        {
            Destroy(child.gameObject);
        }
    }

    private void OnTimesUp(Dictionary<string, object> message)
    {
        var text = "Times Up! You Lose!";
        DisplayMessage(text);
        Invoke(nameof(Restart), 2f);
    }
}
