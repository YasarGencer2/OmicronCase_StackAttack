using UnityEngine;

[CreateAssetMenu(fileName = "TargetColors", menuName = "TargetColors", order = 1)]
public class TargetColors : ScriptableObject
{
    public ColorData[] colors;

    public Color GetColor(TargetColor targetColor)
    {
        var colorData = System.Array.Find(colors, c => c.targetColor == targetColor);
        if (colorData.color != null)
            return colorData.color;
        return Color.white;
    }
}
[System.Serializable]
public struct ColorData
{
    public Color color;
    public TargetColor targetColor;
}
public enum TargetColor
{
    None = 0,
    Blue = 1,
    Purple = 2,
    Pink = 3,
    SkyBlue = 4,
    Lime = 5,
}
