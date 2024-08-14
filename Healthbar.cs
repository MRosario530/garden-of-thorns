using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public int health;
    public Slider slider1;
    public Image fill1;
    public Slider slider2;
    public Image fill2;
    public Gradient gradient;
    public Color damageColor;
    public Color healColor;
    float healthBarResetTime = 1f;

    public void SetMaxHealth(int maxHealth)
    {
        slider1.maxValue = slider2.maxValue = maxHealth;
        slider1.value = maxHealth;
        slider2.value = 0;
        fill1.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int newHealth)
    {
        int prevHealth = health;

        if (newHealth < prevHealth)
        {
            slider2.value = prevHealth;
            fill2.color = damageColor;
            Invoke(nameof(ResetHealthChange), healthBarResetTime);
            slider1.value = newHealth;
            fill1.color = gradient.Evaluate(slider1.normalizedValue);
        }
        else
        {
            slider2.value = newHealth;
            fill2.color = healColor;
            Invoke(nameof(ResetHealthChange), healthBarResetTime);
        }

        health = newHealth;
    }

    void ResetHealthChange()
    {
        slider1.value = health;
        fill1.color = gradient.Evaluate(slider1.normalizedValue);
        slider2.value = 0;
    }
}
