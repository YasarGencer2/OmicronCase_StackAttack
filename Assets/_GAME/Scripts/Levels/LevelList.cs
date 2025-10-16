using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelList", menuName = "Scriptable Objects/LevelList")]
public class LevelList : ScriptableObject
{
    public List<Level> Levels;
}
