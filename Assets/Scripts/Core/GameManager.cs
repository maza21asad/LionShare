using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<PlayerController> players;
    private int currentPlayerIndex = 0;

    private void Awake()
    {
        instance = this;
    }

    public PlayerController CurrentPlayer => players[currentPlayerIndex];

    public void NextTurn()
    {
        currentPlayerIndex++;

        if (currentPlayerIndex >= players.Count)
            currentPlayerIndex = 0;

        RoundManager.instance.StartRoundFor(CurrentPlayer);
    }

    public void OnPlayerAnswered(PlayerController player, bool isCorrect)
    {
        if (isCorrect)
        {
            RoundManager.instance.CorrectAnswer(player);
        }
        else
        {
            RoundManager.instance.WrongAnswer(player);
        }

        NextTurn();
    }
}
