using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

// Voice

 
 

namespace UnityEngine.XR.ARFoundation.Samples
{
    public class HumanBodyTracker : MonoBehaviour
    {
        private float nextActionTime = 35;
        private float StartTime = 2;
        public float period = 35;
        bool DontAdd = true;

        public Text Texty;
        private void Update()
        {
            boneController.Textie = Texty;
            
                
                boneController.Called = true;
                 
              

            
            boneController.rawImageColor = rawImageColor;
            boneController.Work = true;
          
            


            skeletonPrefab.gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
        [SerializeField]
        [Tooltip("The Skeleton prefab to be controlled.")]
        GameObject m_SkeletonPrefab;
        public Image rawImageColor;

        [SerializeField]
        [Tooltip("The ARHumanBodyManager which will produce body tracking events.")]
        ARHumanBodyManager m_HumanBodyManager;

        /// <summary>
        /// Get/Set the <c>ARHumanBodyManager</c>.
        /// </summary>
        public ARHumanBodyManager humanBodyManager
        {
            get { return m_HumanBodyManager; }
            set { m_HumanBodyManager = value; }
        }

        // Chicago could be the best city in the world!
        /// <summary>
        /// Get/Set the skeleton prefab.
        /// </summary>
        public GameObject skeletonPrefab
        {
            get { return m_SkeletonPrefab; }
            set { m_SkeletonPrefab = value; }
        }

        Dictionary<TrackableId, BoneController> m_SkeletonTracker = new Dictionary<TrackableId, BoneController>();
        public Button StopStart;
        public void StartStopFunc() {

            boneController.start = !boneController.start;
            Debug.Log("Startingsie Button Clicked");
        }
        void OnEnable()
        {
            
            m_HumanBodyManager.humanBodiesChanged += OnHumanBodiesChanged;
            skeletonPrefab.GetComponent<Renderer>().material.color = Color.red;

            foreach (Renderer r in skeletonPrefab.GetComponentsInChildren<Renderer>())
            {
                r.material.color = Color.red;
            }
        }

        void OnDisable()
        {
            if (m_HumanBodyManager != null)
                m_HumanBodyManager.humanBodiesChanged -= OnHumanBodiesChanged;
        }

        public void Count () {
            boneController.GiveDetails();
        }
        BoneController boneController;
        bool added = false;
        void OnHumanBodiesChanged(ARHumanBodiesChangedEventArgs eventArgs)
        {
           

            foreach (var humanBody in eventArgs.added)
            {
                if (!m_SkeletonTracker.TryGetValue(humanBody.trackableId, out boneController))
                {
                    
                    var newSkeletonGO = Instantiate(m_SkeletonPrefab, humanBody.transform);
                    boneController = newSkeletonGO.GetComponent<BoneController>();
                    m_SkeletonTracker.Add(humanBody.trackableId, boneController);
                }

                boneController.InitializeSkeletonJoints();
                boneController.ApplyBodyPose(humanBody, Texty);
                added = true;


            }

            foreach (var humanBody in eventArgs.updated)
            {
                if (m_SkeletonTracker.TryGetValue(humanBody.trackableId, out boneController))
                {
                    boneController.ApplyBodyPose(humanBody , Texty);
                    
                }
            }

            foreach (var humanBody in eventArgs.removed)
            {
                
                if (m_SkeletonTracker.TryGetValue(humanBody.trackableId, out boneController))
                {
                    Destroy(boneController.gameObject);
                    m_SkeletonTracker.Remove(humanBody.trackableId);
                }
            }
        }
    }
}