using UnityEngine;

[System.Serializable]
public class Row
{
    public RowHelper[] rowHelpers = new RowHelper[6];
    public bool MovingRow = false;
    public Vector2 Pos1, Pos2;
}
[System.Serializable]
public class RowHelper
{
    public int stack;
    public TargetColor color;
}