using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour
{
    Text text;
    public static int lifesCount;
    void Start()
    {
        text = GetComponent<Text>();
        lifesCount = LevelManager.instance.lifeCount;
    }

    void Update()
    {
        text.text = lifesCount.ToString();
    }
}
