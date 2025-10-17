using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelList", menuName = "LevelList")]
public class LevelList : ScriptableObject
{
    public List<Level> Levels;
}
