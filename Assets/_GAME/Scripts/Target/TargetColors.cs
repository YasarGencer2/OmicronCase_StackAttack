using System;
using System.Collections.Generic;
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
    public static TargetColor GetRandomColor()
    {
        Array colors = Enum.GetValues(typeof(TargetColor));
        System.Random random = new System.Random();
        List<TargetColor> validColors = new List<TargetColor>();

        foreach (TargetColor c in colors)
        {
            int val = Convert.ToInt32(c);
            if (val >= 1 && val <= 99)
                validColors.Add(c);
        }

        return validColors[random.Next(validColors.Count)];
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


    BossGreen = 101,
}
