using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBttn : MonoBehaviour
{

    public void showOtherButtons()
    {
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
        string file = "/storage/emulated/0/DCIM/VideoRecorders/20181215_174343.mp4";
        Handheld.PlayFullScreenMovie(file, Color.black, FullScreenMovieControlMode.Full);
    }
    
}
