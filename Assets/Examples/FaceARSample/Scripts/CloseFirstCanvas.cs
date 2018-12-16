using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseFirstCanvas : MonoBehaviour
{

    public void showRepeat()
    {
        GameObject.Find("RepeatIntro").GetComponent<Canvas>().enabled = true;
    }
}
