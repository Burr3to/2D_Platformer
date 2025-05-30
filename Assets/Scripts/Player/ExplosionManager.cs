using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ExplosionManager : MonoBehaviour
{
    public PlayerAttack PlayerAttack;
    public Slider timerSlider;
    [SerializeField] TextMeshProUGUI timertext;
    [SerializeField] Image SliderFill;


    Color GreenTran, GreenOpaq;

    private void Update()
    {
        
        GreenOpaq = Color.green;
        GreenOpaq.a = 1f;

        GreenTran = Color.green;
        GreenTran.a = 0.01f;

        if (PlayerAttack.Explodebool)
        {
            timertext.gameObject.SetActive(true);
            SliderFill.color = Color.Lerp(GreenTran, GreenOpaq, timerSlider.value / 2);

            timertext.text = timerSlider.value.ToString();
            timerSlider.maxValue = PlayerAttack.InstantiateExplosionTime;
            timerSlider.value = (float)Math.Round(PlayerAttack.chargeTimeExplosion, 2);


            if (timerSlider.value == timerSlider.maxValue)
            {
                var tempColor = Color.green;
                tempColor.a = 1f;
                SliderFill.color = tempColor;
            }

        }

        if (!PlayerAttack.Explodebool)
        {
            timerSlider.value = 0;
            timertext.gameObject.SetActive(false);

        }
        
    }
}
