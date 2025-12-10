using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void StartRoundFor(PlayerController player)
    {
        // Show question UI for this player
        // After clicking answer -> GameManager.instance.OnPlayerAnswered(player, isCorrect)
    }

    public void CheckAnswer(bool isCorrect)
    {
        PlayerController p = GameManager.instance.CurrentPlayer;

        if (isCorrect)
        {
            CorrectAnswer(p);
        }
        else
        {
            WrongAnswer(p);
        }

        // Move to next turn
        GameManager.instance.NextTurn();
    }

    // NEW — Called by GameManager when answer is correct
    public void CorrectAnswer(PlayerController p)
    {
        if (p.isEliminated) return;

        p.points += 10;
        p.MoveForward(1);

        if (p.ReachedFinish())
        {
            PlayerReachedCage(p);
            return;
        }
    }

    // NEW — Called by GameManager when answer is wrong
    public void WrongAnswer(PlayerController p)
    {
        if (p.isEliminated) return;

        // Move lion closer to the player
        p.MoveLionCloser(1);

        // If lion catches player -> elimination handled inside PlayerController
    }

    // Player reaches the last step (finish)
    public void PlayerReachedCage(PlayerController p)
    {
        p.points += 50;
        // You can show UI here like "Completed!"
        GameManager.instance.NextTurn();
    }

    // Called when lion catches the player
    public void PlayerEliminated(PlayerController p)
    {
        p.points = 0;
        // Show UI: eliminated
        GameManager.instance.NextTurn();
    }
}
