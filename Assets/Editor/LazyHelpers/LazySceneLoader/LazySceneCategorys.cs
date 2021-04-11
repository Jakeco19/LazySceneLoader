using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazySceneCategorys : ScriptableObject
{
    public string categoryTitle = "New";
    public string categoryDescription = "Basic Description";
    public List<UnityEditor.SceneAsset> Scenes = new List<UnityEditor.SceneAsset>();
}
