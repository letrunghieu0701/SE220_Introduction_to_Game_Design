using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    Text text;
    public static int gemAmount;
    void Start()
    {
        text = GetComponent<Text>();
        gemAmount = LevelManager.instance.gemAmount;
    }

    void Update()
    {
        text.text = gemAmount.ToString();
    }
}
