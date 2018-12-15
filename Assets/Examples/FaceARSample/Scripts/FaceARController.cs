namespace FaceAR
{
    using UnityEngine;
    using System.Collections.Generic;
    using HuaweiARUnitySDK;
    using System.Collections;
    using System;
    using HuaweiARInternal;
    using Common;
    using Recorder;
    using UnityEditor;
    using UnityEngine.Video;
    using UnityEngine.UI;
    using System.Net;
    using System.IO;
    using UnityEngine.Networking;

    public class FaceARController : MonoBehaviour
    {
        [Tooltip("face prefabs")]
        public GameObject facePrefabs;

        private List<ARFace> m_newFaces = new List<ARFace>();
        public RecordManager recordManager;
        private bool recording = false;
        public string fileName = "";

        private void _DrawFace()
        {
            m_newFaces.Clear();
            ARFrame.GetTrackables<ARFace>(m_newFaces, ARTrackableQueryFilter.NEW);
            for(int i=0;i< m_newFaces.Count; i++)
            {
                GameObject faceObject = Instantiate(facePrefabs, Vector3.zero, Quaternion.identity, transform);
                faceObject.GetComponent<FaceVisualizer>().Initialize(m_newFaces[i], fileName, recording);
            }
        }

        private void RequestToServer()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("http://api.openweathermap.org/data/2.5/weather?id={0}&APPID={1}", 11, "API-key"));
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string jsonResponse = reader.ReadToEnd();
            Debug.Log(jsonResponse);
        }

        public void startVid()
        {
            //RequestToServer();
            recordManager.StartRecord();
            recording = true;
            System.DateTime now = System.DateTime.Now;
            fileName = "Video_" + now.Year + "_" + now.Month + "_" + now.Day + "_" + now.Hour + "_" + now.Minute + "_" + now.Second;
            _DrawFace();
        }

        public void stopVid()
        {
            string path = "/storage/emulated/0/DCIM/VideoRecorders/";
            Debug.Log(fileName);
            recordManager.StopRecord(fileName);
            recording = false;
            //ARFrame.StopTracking();
            //faceObject.SetActive(false);
            StartCoroutine(waiter(path+fileName+".mp4"));
            //_DrawFace();
        }

        private void ShowTheVideo(string file)
        {
            Debug.Log("START");
            Handheld.PlayFullScreenMovie(file, Color.black, FullScreenMovieControlMode.Full);
            /*GameObject camera = GameObject.Find("PreviewCamera");

            //GameObject videoCamera = GameObject.Find("VideoCamera");

            //Destroy(GameObject.Find("HuaweiAR Device"));//.SetActive(false);


            //imagePlane = GameObject.Find("ImagePlane");

            var videoPlayer = gameObject.AddComponent<UnityEngine.Video.VideoPlayer>();
            videoPlayer.playOnAwake = false;
            videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;
            //videoPlayer.targetCameraAlpha = 0.5F;
            videoPlayer.url = file;
            //videoPlayer.playbackSpeed = videoPlayer.playbackSpeed / 2.0F;
            //videoPlayer.frame = 100;
            videoPlayer.isLooping = true;


            videoPlayer.loopPointReached += EndReached;
            videoPlayer.Prepare();

            while (!videoPlayer.isPrepared)
            {
                Debug.Log("Preparing video!");
                //yield return new WaitForSeconds(1);
            }

            Debug.Log("DONE");
            imagePlane.texture = videoPlayer.texture;

            videoPlayer.Play();

            while (videoPlayer.isPlaying)
            {
                Debug.Log("Playing video at " + Mathf.FloorToInt((float)videoPlayer.time));
                yield return null;
            }

            Debug.Log("DONE PLAYING");
            */

            //camera.SetActive(false);
            //videoCamera.SetActive(true);


            /*MonoBehaviour[] components = facePrefabs.GetComponents<MonoBehaviour>();
            foreach(MonoBehaviour c in components)
            {
                c.enabled = false;
                Debug.Log(c.isActiveAndEnabled);
            }*/

            //Destroy(camera);
            //GameObject videoCamera = GameObject.Find("")


            //var videoPlayer = videoCamera.AddComponent<UnityEngine.Video.VideoPlayer>();
            /* var videoPlayer = camera.AddComponent<UnityEngine.Video.VideoPlayer>();
            videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;
            //videoPlayer.targetCameraAlpha = 0.5F;
            videoPlayer.url = file;
            //videoPlayer.playbackSpeed = videoPlayer.playbackSpeed / 2.0F;
            //videoPlayer.frame = 100;
            videoPlayer.isLooping = true;
            videoPlayer.loopPointReached += EndReached;
            videoPlayer.Play(); */
        }

        /*void EndReached(UnityEngine.Video.VideoPlayer vp)
        {
            Debug.Log("sdfsdfsdf");
            //Destroy(vp);
            //vp.Stop();
        }*/

        IEnumerator setRequest(string videoPath)
        {
            Debug.Log("START SENDING");
            (byte[] videoData, string videoName) = readFile(videoPath);
            (byte[] rightData, string rightName) = readFile(Application.persistentDataPath + "/" + fileName + "_right.txt");
            (byte[] leftData, string leftName) = readFile(Application.persistentDataPath + "/" + fileName + "_left.txt");
            WWWForm form = new WWWForm();

            Debug.Log("videoData = " + videoData.Length);
            Debug.Log("videoName = " + videoName);
            Debug.Log("rightData = " + rightData.Length);
            Debug.Log("rightName = " + rightName);
            Debug.Log("leftData = " + leftData.Length);
            Debug.Log("leftName = " + leftName);
            Debug.Log("form = " + form);

            form.AddBinaryData("video", videoData, videoName);
            form.AddBinaryData("right_eye", rightData, rightName);
            form.AddBinaryData("left_eye", leftData, leftName);

            using (UnityWebRequest www = UnityWebRequest.Post("http://10.100.39.4:5000/uploader", form))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log("ERROR");
                    Debug.Log(www.error);
                    Debug.Log(www);
                }
                else
                {
                    Debug.Log("Form upload complete!");
                    Debug.Log(www.downloadHandler.text);
                    byte[] results = www.downloadHandler.data;
                    Debug.Log(results.Length);
                    Debug.Log(results);
                    string path = "/storage/emulated/0/DCIM/VideoRecorders/";
                    File.WriteAllBytes(path + fileName + "_output.mp4", results);
                    while (!System.IO.File.Exists(path + fileName + "_output.mp4"))
                    {
                        yield return new WaitForSeconds(1);
                    }
                    ShowTheVideo(path + fileName + "_output.mp4");
                    /*foreach (KeyValuePair<string, string> entry in www.GetResponseHeaders())
                    {
                        Debug.Log(entry.Value + "=" + entry.Key);
                    }*/
                }
            }
        }

        IEnumerator waiter(string file)
        {
            while (!System.IO.File.Exists(file))
            {
                yield return new WaitForSeconds(1);
            }

            StartCoroutine(setRequest(file));
            ShowTheVideo(file);
        }

        (byte[], string) readFile(string file_path)
        {
            byte[] data = System.IO.File.ReadAllBytes(file_path);
            string Name = Path.GetFileName(file_path);
            return (data, Name);
        }
    }
}

