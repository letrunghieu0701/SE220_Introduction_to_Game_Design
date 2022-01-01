using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideIntro : MonoBehaviour
{
    Text text;
    public static bool hideText = false;
    void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        if(hideText == true) {
            gameObject.SetActive(false);
        }
    }
}
