using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterManager : MonoBehaviour
{
    public GameObject targetPlayer;
    public RectTransform boosterSlider;

    // Update is called once per frame
    public void UpdateSlider(int maxBoost, int boost)
    {
        if (boosterSlider == null)
        {
            return;
        }

        Vector3 scale = boosterSlider.transform.localScale;
        scale.x = (float)boost / (float)maxBoost;
        boosterSlider.transform.localScale = scale;
    }
}
