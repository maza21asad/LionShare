using UnityEngine;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;

    [Header("Players In Order")]
    public List<PlayerController> players;   // exactly 4 players

    private int turnIndex = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartTurn();
    }

    public PlayerController CurrentPlayer => players[turnIndex];

    public void StartTurn()
    {
        // Skip eliminated players
        while (CurrentPlayer.isEliminated)
        {
            NextTurnIndex();
        }

        // Tell RoundManager whose turn this is
        RoundManager.instance.StartRoundFor(CurrentPlayer);
    }

    public void EndTurn()
    {
        NextTurnIndex();
        StartTurn();
    }

    private void NextTurnIndex()
    {
        turnIndex++;
        if (turnIndex >= players.Count)
            turnIndex = 0;
    }

    public bool AreAllPlayersDoneExceptOne()
    {
        int active = 0;

        foreach (var p in players)
        {
            if (!p.isEliminated)
                active++;
        }

        return active <= 1;
    }

    public PlayerController GetWinner()
    {
        foreach (var p in players)
        {
            if (!p.isEliminated)
                return p;
        }

        return null;
    }
}
