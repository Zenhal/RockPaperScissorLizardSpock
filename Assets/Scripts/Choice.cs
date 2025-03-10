using System.Collections.Generic;
using UnityEngine;

public class Choice : MonoBehaviour
{

    public void OnButtonPressed(string choice)
    {
        Debug.Log(choice + " selected");
        EventManager.TriggerEvent("PlayerSelect", new Dictionary<string, object>() {{ "choice", choice }});
    }
    
}
