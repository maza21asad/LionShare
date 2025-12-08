using UnityEditor.Rendering.Universal.ShaderGUI;
using UnityEngine;

public class LionController : MonoBehaviour
{
    public int lionStep = 0; // 0 means inside cage
    public bool isCageOpen = false;

    public LionUI ui;

    // Called when ANY player gives a wrong answer AND the player is step >= 2
    public void HandleWrongAnswer()
    {
        if (!isCageOpen)
        {
            // FIRST wrong answer → Open cage
            isCageOpen = true;
            ui.OpenCage();
        }
        else
        {
            // SECOND+ wrong answers → Move lion forward
            lionStep++;
            ui.MoveToStep(lionStep);
        }
    }

    // Check if lion catches a specific player
    public bool CheckPlayerCaught(PlayerController player)
    {
        if (player.isEliminated || player.isFinished || player.isCashedOut)
            return false;

        if (lionStep == player.step)
        {
            player.isEliminated = true;
            return true;
        }

        return false;
    }

    // Called when round restarts
    public void ResetLion()
    {
        lionStep = 0;
        isCageOpen = false;
        ui.ResetPosition();
    }
}
