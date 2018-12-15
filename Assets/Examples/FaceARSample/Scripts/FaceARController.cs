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

    public class FaceARController : MonoBehaviour
    {
        [Tooltip("face prefabs")]
        public GameObject facePrefabs;

        private List<ARFace> m_newFaces = new List<ARFace>();
        public RecordManager recordManager;

        void Update()
        {
            _DrawFace();
        }

        private void _DrawFace()
        {
            m_newFaces.Clear();
            ARFrame.GetTrackables<ARFace>(m_newFaces, ARTrackableQueryFilter.NEW);
            for(int i=0;i< m_newFaces.Count; i++)
            {
                GameObject faceObject = Instantiate(facePrefabs, Vector3.zero, Quaternion.identity, transform);
                faceObject.GetComponent<FaceVisualizer>().Initialize(m_newFaces[i]);
            }
        }

        public void startVid()
        {
            recordManager.StartRecord();
        }

        public void stopVid()
        {
            recordManager.StopRecord();
        }
    }
}
