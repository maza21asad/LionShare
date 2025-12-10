using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public List<GameObject> allPanel;
    private Stack<GameObject> panelHistory = new Stack<GameObject>();


}
