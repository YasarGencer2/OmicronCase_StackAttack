using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelEndPanel : MonoBehaviour
{
    public static bool OPEN = false;


    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Transform panel;
    [SerializeField] List<Button> close;
    [SerializeField] Transform winText, loseText;
    [SerializeField] TextMeshProUGUI levelText;

    void Awake()
    {
        foreach (var item in close)
        {
            item.onClick.AddListener(Close);
        }
        Hide(0);
    }
    void OnEnable()
    {
        GameEventSystem.Instance.OnLevelCompleted += Win;
        GameEventSystem.Instance.OnLevelFailed += Lose;
    }
    void OnDisable()
    {
        GameEventSystem.Instance.OnLevelCompleted -= Win;
        GameEventSystem.Instance.OnLevelFailed -= Lose;
    }

    private void Lose()
    {
        loseText.gameObject.SetActive(true);
        winText.gameObject.SetActive(false);
        Show();
    }

    private void Win()
    {
        loseText.gameObject.SetActive(false);
        winText.gameObject.SetActive(true);
        Show();
        LevelManager.CurrentLevel += 1;
    }
    void Show()
    {
        OPEN = true;
        canvasGroup.DOFade(1, 0.25f);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        panel.localScale = Vector3.zero;
        panel.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);

        var level = LevelManager.VisualLevel;
        levelText.text = "LEVEL " + level;
    }
    void Hide(float time = 0.25f)
    {
        OPEN = false;
        canvasGroup.DOFade(0, time).OnComplete(() =>
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        });
        panel.DOScale(Vector3.zero, time).SetEase(Ease.InBack);
    }
    void Close()
    {
        LevelManager.Instance.LoadCurrentLevel();
        Hide();
    }
}
