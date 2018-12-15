using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatIntro : MonoBehaviour
{
    // Start is called before the first frame update
    public void Repeat()
    {
        string file = "/storage/emulated/0/DCIM/VideoRecorders/20181215_174343.mp4";
        Handheld.PlayFullScreenMovie(file, Color.black, FullScreenMovieControlMode.Full);
    }
}
