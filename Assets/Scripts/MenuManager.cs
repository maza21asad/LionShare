using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [Header("Menu Panels")]
    public GameObject mainMenuPanel;
    public List<GameObject> allPanel;

    private Stack<GameObject> panelHistory = new Stack<GameObject>();

    private void Awake()
    {
        if (instance == null) { instance = this; }

        ShowPanel(mainMenuPanel);
    }

    public void ShowPanel(GameObject panelToShow)
    {
        foreach (GameObject panel in allPanel)
            panel.SetActive(false);

        if (panelToShow != null)
        {
            AnimatePanelFade(panelToShow);
            panelHistory.Push(panelToShow);
        }
    }

    public void AnimatePanelFade(GameObject panel)
    {
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = panel.AddComponent<CanvasGroup>();

        panel.transform.localScale = Vector3.one * 0.4f;
        canvasGroup.alpha = 1f;
        panel.SetActive(true);

        // Animate both scale and fade
        panel.transform.DOScale(1f, .4f).SetEase(Ease.OutBack);
        canvasGroup.DOFade(1f, 0.3f);
    }

    public void CloseCurrectPanel()
    {
        if(panelHistory.Count > 0)
        {
            GameObject closePanel = panelHistory.Pop();

            // Animate closing before deactivating
            closePanel.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack).OnComplete(() => closePanel.SetActive(false));

            if(panelHistory.Count > 0)
            {
                GameObject previosPanel = panelHistory.Peek();
                AnimatePanelFade(previosPanel);
            }
            else
            {
                AnimatePanelFade(mainMenuPanel);
                panelHistory.Push(mainMenuPanel);
            }
        }
    }

    public void GoBack()
    {
        if(panelHistory.Count > 1)
        {
            GameObject currect = panelHistory.Pop();
            GameObject previous = panelHistory.Peek();

            CanvasGroup currentGroup = currect.GetComponent<CanvasGroup>();
            if(currentGroup == null) 
                currentGroup = currect.AddComponent<CanvasGroup>();

            // Animate currect panel closing
            currect.transform.DOScale(0.8f, 0.25f).SetEase(Ease.InBack);

            currentGroup.DOFade(0f, 0.25f).OnComplete(() =>
            {
                currect.SetActive(false);

                // Animate the previus panel back in
                AnimatePanelFade(previous);
            });
        }
    }

    public void LoadScene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }
}


