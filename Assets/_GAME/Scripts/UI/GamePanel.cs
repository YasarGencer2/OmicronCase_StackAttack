using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    [SerializeField] Slider xp;
    void Awake()
    {
        xp.maxValue = 100;
        xp.value = 0;
    }   
    void OnEnable()
    {
        GameEventSystem.Instance.OnXPChange += ChangeXPSlider;
    }
    void OnDisable()
    {
        GameEventSystem.Instance.OnXPChange -= ChangeXPSlider;
    }
    private void ChangeXPSlider(int arg0, int arg1)
    {
        xp.maxValue = arg1;
        xp.value = arg0;
    }
}
