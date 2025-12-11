using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class UIManage : MonoBehaviour
{
    [Header("Gameplay Panels")]
    public List<GameObject> allPanels;

    private Stack<GameObject> panelHistory = new Stack<GameObject>();

    void Start()
    {
        // Gameplay scene starts with NO panel active
        foreach (var p in allPanels)
            p.SetActive(false);
    }

    public void ShowPanel(GameObject panel)
    {
        foreach (var p in allPanels)
            p.SetActive(false);

        AnimatePanel(panel);
        panelHistory.Push(panel);
    }

    private void AnimatePanel(GameObject panel)
    {
        CanvasGroup cg = panel.GetComponent<CanvasGroup>();

        if (cg == null)
            cg = panel.AddComponent<CanvasGroup>();

        panel.SetActive(true);

        panel.transform.localScale = Vector3.one * 0.4f;
        cg.alpha = 0f;

        panel.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack);
        cg.DOFade(1f, 0.25f);
    }

    public void CloseCurrectPanel()
    {
        if (panelHistory.Count == 0)
            return;

        GameObject closing = panelHistory.Pop();

        closing.transform.DOScale(Vector3.zero, 0.2f)
            .SetEase(Ease.InBack)
            .OnComplete(() => closing.SetActive(false));

        if (panelHistory.Count > 0)
        {
            GameObject previous = panelHistory.Peek();
            AnimatePanel(previous);
        }
        else
        {
            // No panel → return to gameplay
            // Example: GameManager.instance.ResumeGame();
        }
    }

    public void GoBack()
    {
        if (panelHistory.Count <= 1)
        {
            CloseCurrectPanel();
            return;
        }

        GameObject current = panelHistory.Pop();
        GameObject previous = panelHistory.Peek();

        CanvasGroup cg = current.GetComponent<CanvasGroup>();
        if (cg == null) cg = current.AddComponent<CanvasGroup>();

        current.transform.DOScale(0.8f, 0.2f).SetEase(Ease.InBack);
        cg.DOFade(0f, 0.2f).OnComplete(() =>
        {
            current.SetActive(false);
            AnimatePanel(previous);
        });
    }
}
