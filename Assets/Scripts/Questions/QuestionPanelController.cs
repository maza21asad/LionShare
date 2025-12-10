using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class QuestionPanelController : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text questionText;
    public TMP_Text[] optionTexts;      // size = 4
    public Button[] optionButtons;      // size = 4

    public TMP_Text playerNameText;
    public Image playerAvatar;

    public TMP_Text timerText;

    [Header("Timer Settings")]
    public float questionTime = 10f;
    private float timer;

    [HideInInspector]
    public QuestionData currentQuestion;

    private bool answeringAllowed = false;

    // -----------------------------------------------------------------------

    public void Show(QuestionData q, PlayerController currentPlayer)
    {
        currentQuestion = q;

        // Set question text
        questionText.text = q.questionText;

        // Set options
        for (int i = 0; i < optionTexts.Length; i++)
        {
            optionTexts[i].text = q.options[i];
            optionButtons[i].interactable = true;
        }

        // Set player UI
        playerNameText.text = currentPlayer.playerName;
        if (currentPlayer.avatar != null)
            playerAvatar.sprite = currentPlayer.avatar;

        // Show panel
        gameObject.SetActive(true);

        // Start timer
        timer = questionTime;
        answeringAllowed = true;
        StartCoroutine(TimerRoutine());
    }

    // -----------------------------------------------------------------------

    IEnumerator TimerRoutine()
    {
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.Ceil(timer).ToString();
            yield return null;
        }

        // Time out => auto Wrong Answer
        SubmitAnswer(-1);
    }

    // -----------------------------------------------------------------------

    public void OnOptionClicked(int index)
    {
        SubmitAnswer(index);
    }

    // -----------------------------------------------------------------------

    private void SubmitAnswer(int optionIndex)
    {
        if (!answeringAllowed) return;

        answeringAllowed = false;

        // Disable buttons so user can’t click again
        foreach (var b in optionButtons)
            b.interactable = false;

        bool isCorrect = (optionIndex == currentQuestion.correctIndex);

        // Send result to RoundManager
        RoundManager.instance.CheckAnswer(isCorrect);

        // Hide panel
        gameObject.SetActive(false);
    }
}
