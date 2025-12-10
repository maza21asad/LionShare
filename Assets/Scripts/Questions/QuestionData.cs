using UnityEngine;

[System.Serializable]
public class QuestionData
{
    [TextArea(2, 4)]
    public string questionText;

    public string[] options;   // A, B, C, D
    public int correctIndex;   // correct option index

    //public Sprite questionImage;   // optional (you can ignore if not needed)
}
