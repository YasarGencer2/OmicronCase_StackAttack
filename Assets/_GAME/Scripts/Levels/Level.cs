using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level", order = 1)]
public class Level : ScriptableObject
{
    public Boss Boss;
    public GameObject Custom;
    public Row[] Rows;
}