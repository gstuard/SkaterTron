using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinScreen : MonoBehaviour
{
    public TMP_Text win_text;
    // Update is called once per frame
    public void NameWinner(string winner_name)
    {
        win_text.text = winner_name + " Wins";
    }
}
