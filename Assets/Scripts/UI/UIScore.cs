using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIScore : MonoBehaviour
{
    public TMP_Text winsText;
    public void UpdateWins (int wins)
    {
        winsText.text = wins.ToString();
    }
}
