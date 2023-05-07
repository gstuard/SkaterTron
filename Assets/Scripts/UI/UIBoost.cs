using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBoost : MonoBehaviour
{
    public RectTransform boosterSlider;

    public void SetBoost(int maxBoost, int boost)
    {
        UpdateSlider((float)boost / (float)maxBoost);
    }

    void UpdateSlider(float percentBoost)
    {
        if (boosterSlider == null)
            return;
        Vector3 scale = boosterSlider.transform.localScale;
        scale.x = percentBoost;
        boosterSlider.transform.localScale = scale;
    }
}