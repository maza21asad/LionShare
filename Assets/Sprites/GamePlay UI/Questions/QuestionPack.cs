using UnityEngine;

[CreateAssetMenu(fileName = "QuestionPack", menuName = "Scriptable Objects/QuestionPack")]
public class QuestionPack : ScriptableObject
{
    public QuestionData[] questions;
}
