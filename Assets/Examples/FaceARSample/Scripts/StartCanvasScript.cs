using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCanvasScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;
    }

  
}
