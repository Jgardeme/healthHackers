namespace Common
{
    using UnityEngine;
    using HuaweiARUnitySDK;
    using System.Collections.Generic;
    using HuaweiARInternal;
    using System.Text;
    using System.IO;

    public class FaceVisualizer : MonoBehaviour
    {
        private ARFace m_face;
        StringBuilder sb = new StringBuilder();
        StringBuilder sb2 = new StringBuilder();

        public void Initialize(ARFace face)
        {
            Debug.Log("Beginning");
            m_face = face;
            Update();
        }

        public void Update()
        {
            sb2.Remove(0, sb2.Length);
            sb2.Append("RIGHT");    
            sb.Remove(0, sb.Length);
            sb.Append("MY TEST");
            if (null == m_face)
            {
                sb.Append("Not detect face");
                return;
            }
            else if(m_face.GetTrackingState() == ARTrackable.TrackingState.PAUSED)
            {
                //disable face renderer
                return;
            }
            else if (m_face.GetTrackingState() == ARTrackable.TrackingState.STOPPED)
            {
                Destroy(gameObject);
                return;
            }
            //update according to Face parameter
            sb.Remove(0, sb.Length);
            sb2.Remove(0, sb2.Length);

            sb.Append("Face Pose: ");
            sb.Append(m_face.GetPose().ToString());
            sb.Append("\n");

            var blendShapes = m_face.GetBlendShapeWithBlendName();
            foreach(var bs in blendShapes)
            {
                var key = bs.Key;
                if (key.Contains("Right"))
                {
                    sb2.Append(key);
                    sb2.Append(": ");
                    sb2.Append(bs.Value);
                    sb2.Append("\n");
                }
                else
                {
                    sb.Append(key);
                    sb.Append(": ");
                    sb.Append(bs.Value);
                    sb.Append("\n");
                }

                /*var text = bs.Value + ";";
                File.AppendAllText(Application.persistentDataPath + @"/file.txt", text);
                */

                if (key.Contains("Animoji_Eye_Blink_Left"))
                {
                    var text = bs.Value + ";";
                    File.AppendAllText(Application.persistentDataPath + @"/left.txt", text);
                }
                if (key.Contains("Animoji_Eye_Wide_Left"))
                {
                    var text = bs.Value + ";";
                    File.AppendAllText(Application.persistentDataPath + @"/left.txt", text);
                }
                if (key.Contains("Animoji_Eye_Blink_Right"))
                {
                    var text = bs.Value + ";";
                    File.AppendAllText(Application.persistentDataPath + @"/left.txt", text);
                }
                if (key.Contains("Animoji_Eye_Wide_Right"))
                {
                    var text = bs.Value + ";";
                    File.AppendAllText(Application.persistentDataPath + @"/right.txt", text);
                }
            }
            //File.AppendAllText(Application.persistentDataPath + @"/file.txt", "/n");


            var faceGeometry = m_face.GetFraceGeometry();
            sb.Append("Face Geometry triangle count: ");
            sb.Append(faceGeometry.TriangleCount);
            sb.Append("\n");
        }

        public void OnGUI()
        {
            GUIStyle bb = new GUIStyle();
            bb.normal.background = null;
            bb.normal.textColor = new Color(1, 0, 0);
            bb.fontSize = 25;
            GUI.Label(new Rect(0, 100, 200, 200), sb.ToString(), bb);
            GUIStyle bb2 = new GUIStyle();
            bb2.normal.background = null;
            bb2.normal.textColor = new Color(0, 1, 0);
            bb2.fontSize = 25;
            GUI.Label(new Rect(600, 100, 200, 200), sb2.ToString(), bb2);
        }
    }
}
