using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public TextMeshProUGUI itemDescription;

    public void SetMessage(string message)
    {
        textMeshPro.text = message;
    }

    public void SetDescription(string description)
    {
        itemDescription.text = description;
    }
}
