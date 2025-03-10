using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameRules", menuName = "ScriptableObject/Game Rules")]
public class GameRules : ScriptableObject
{
    public int maxTime;
    public List<Rule> rules;
    
    public bool DoesWin(string playerChoice, string opponentChoice)
    {
        for (int i = 0; i < rules.Count; i++)
        {
            var rule = rules[i];
            if(rule.choice.Equals(playerChoice))
                return rule.beats.Contains(opponentChoice);
        }

        return false;
    }
}
