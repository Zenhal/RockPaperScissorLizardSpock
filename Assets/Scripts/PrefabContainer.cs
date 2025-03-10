using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Prefab
{
    public string name;
    public GameObject prefab;
}

[CreateAssetMenu(fileName = "GameRules", menuName = "ScriptableObject/PrefabContainer")]
public class PrefabContainer : ScriptableObject
{
    public List<Prefab> prefabs;
}
