using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands.Gestures;

namespace UnityEngine.XR.Hands.Samples.Gestures.DebugTools
{
    public class CurlDetection : MonoBehaviour
    {
        static List<XRHandSubsystem> s_SubsystemsReuse = new List<XRHandSubsystem>();
        public Handedness trackedHand;
        public XRFingerShape[] fingerShapes;
        public List<float> tipCurls;
        // Start is called before the first frame update
        void Start()
        {
            if (trackedHand == Handedness.Invalid){
                Debug.LogWarning($"The Handedness property of { GetType() } is set to Invalid and will default to Right.", this);
                trackedHand = Handedness.Right;
            }
            fingerShapes = new XRFingerShape[(int)XRHandFingerID.Little - (int)XRHandFingerID.Thumb + 1];
        }

        // Update is called once per frame
        void Update()
        {
            var subsystem = TryGetSubsystem();
            if (subsystem == null){
                return;
            }
            var hand = trackedHand == Handedness.Left ? subsystem.leftHand : subsystem.rightHand;
            for (var fingerIndex = (int)XRHandFingerID.Thumb; fingerIndex <= (int)XRHandFingerID.Little; ++fingerIndex)
            {
                fingerShapes[fingerIndex] = hand.CalculateFingerShape(
                    (XRHandFingerID)fingerIndex, XRFingerShapeTypes.All);
                UpdateFinger(fingerIndex);
            }
            // fingerShapes[0].TryGetBaseCurl(out float baseCurl);
            // var shapes = fingerShapes[1];
            // if (shapes.TryGetTipCurl(out var tipCurl)){
            //     Debug.Log(tipCurl);
            // } else {
            //     Debug.Log("Finger not found");
            // }
            // Debug.Log(fingerShapes[0].TryGetBaseCurl(out float baseCurl));

        }

        void UpdateFinger(int fingerIndex)
        {
            var shapes = fingerShapes[fingerIndex];
            if (shapes.TryGetFullCurl(out var tipCurl)){
                tipCurls[fingerIndex] = tipCurl;
            }
                // if (shapes.TryGetBaseCurl(out var baseCurl))
                //     graph.SetFingerShape((int)XRFingerShapeType.BaseCurl, baseCurl);
                // else
                //     graph.HideFingerShape((int)XRFingerShapeType.BaseCurl);

                // if (shapes.TryGetTipCurl(out var tipCurl))
                //     graph.SetFingerShape((int)XRFingerShapeType.TipCurl, tipCurl);
                // else
                //     graph.HideFingerShape((int)XRFingerShapeType.TipCurl);

                // if (shapes.TryGetPinch(out var pinch))
                //     graph.SetFingerShape((int)XRFingerShapeType.Pinch, pinch);
                // else
                //     graph.HideFingerShape((int)XRFingerShapeType.Pinch);

                // if (shapes.TryGetSpread(out var spread))
                //     graph.SetFingerShape((int)XRFingerShapeType.Spread, spread);
                // else
                //     graph.HideFingerShape((int)XRFingerShapeType.Spread);
        }

        // Imported from OpenXR Library do not touch
        static XRHandSubsystem TryGetSubsystem()
        {
            SubsystemManager.GetSubsystems(s_SubsystemsReuse);
            return s_SubsystemsReuse.Count > 0 ? s_SubsystemsReuse[0] : null;
        }
    }
}