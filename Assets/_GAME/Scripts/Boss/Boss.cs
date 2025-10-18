using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "New Boss", menuName = "Boss")]
public class Boss : ScriptableObject
{ 
    public string Name = "Boss";
    public GameObject Prefab;
    [Space(5)]
    public int MaxHealth = 100;
    public TargetColor Color;

    [Space(5)]
    public ShieldingType ShieldingType;
    public TargetColor ShieldColor;
    public float ShieldSpeed;
    public int ShieldingAmount;
    public bool ShiledRandomized;
    [SerializeField] int2 shiledingStackRange;
    public int ShieldingStack => UnityEngine.Random.Range(shiledingStackRange.x, shiledingStackRange.y + 1);
    
    [Space(5)]
    public float FireRate;
    [SerializeField] float2 rowSpeedRange;
    [SerializeField] int2 rowStackRange;
    [SerializeField] int2 rowAmountRange;
    public float RowMoveSpeed => UnityEngine.Random.Range(rowSpeedRange.x, rowSpeedRange.y);
    public int RowStack => UnityEngine.Random.Range(rowStackRange.x, rowStackRange.y + 1);
    public int RowAmount => UnityEngine.Random.Range(rowAmountRange.x, rowAmountRange.y + 1);

}

public enum ShieldingType{
    None = 0,
    RoundRotate = 10,
}
