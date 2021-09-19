using UnityEngine.XR.ARFoundation;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using System;
using UnityEngine.UI;








class BodyPartMovement : MonoBehaviour
{
    public List<bool> CheckLast = new List<bool>();
    public List<bool> CheckLastX = new List<bool>();
    public List<bool> CheckLastZ = new List<bool>();


    public bool[] HighLowBool = new bool[3] { true, true, true }; // True if Highest 
    public float[] Highest = new float[3] { -100, -100, -100 }; // 0 is X, 1 is Y & Position 2 is Z
    public List<float> HighestListX = new List<float>();
    public List<float> HighestListY = new List<float>();
    public List<float> HighestListZ = new List<float>();

    public float[] Lowest = new float[3] { 100, 100, 100 };
    public List<float> LowestListX = new List<float>();
    public List<float> LowestListY = new List<float>();
    public List<float> LowestListZ = new List<float>();

    public int CountiesX = 0;
    public int CountiesY = 0;
    public int CountiesZ = 0;


    public bool CountHigh = true;
    public bool CountLow = true;

    public bool CountHighX = true;
    public bool CountLowX = true;

    public bool CountHighZ = true;
    public bool CountLowZ = true;

    public int OverAllCount = 0;

    public List<float> CheckyIfHigher = new List<float>(); // Checks if prev is higher
    public List<float> CheckyIfLower = new List<float>(); // Checks if prev is higher

    public List<float> CheckyIfHigherX = new List<float>(); // Checks if prev is higher
    public List<float> CheckyIfLowerX = new List<float>(); // Checks if prev is higher 

    public float FindNegativePercent(int Percent, float x)
    {
        float PercentReturn = 0f;

        if (x < 0 && Percent > 100)
        {
            PercentReturn = (((Percent - 100) - 100) * x) / 100;
        }
        else if (x < 0 && Percent < 100)
        {
            PercentReturn = (((100 - Percent) + 100) * x) / 100;
        }
        else
        {
            PercentReturn = (Percent * x) / 100;
        }
        return PercentReturn;


    }
    // Check if the movement of body was reaching the average 
    // Check if the movement of body was reaching the average
    int Passed = 0;
    public bool CheckAverage(List<float> Lists, List<float> LowLists, float CurrentValue, float PrevLow)
    { // If MoreLess is true it means 



        float Average = 0f;

        int i = (40 * (Math.Min(Lists.Count, LowLists.Count)) / 100);
        int DivideBy = 0;
        while (i < Math.Min(Lists.Count, LowLists.Count))
        {


            if (Math.Abs((Lists[i]) - (LowLists[i])) < 2f)
            {
                Average += Math.Abs((Lists[i]) - (LowLists[i]));
                DivideBy++;
            }










            i++;
        }



        Average = (Average / (DivideBy));

        Debug.Log("Startingsie New LOW last average-->" + LowLists[LowLists.Count - 1] + " With High Average " + Lists[Lists.Count - 1]);

        Debug.Log("Startingsie Average: " + (Average) + " Valuee " + (Math.Abs(((CurrentValue - PrevLow)))) + "With current value " + CurrentValue + "Prev " + PrevLow + "Lists Stages" + Lists.Count + "LowLists Stages" + LowLists.Count);

        if (Math.Abs(((CurrentValue - PrevLow))) >= (90 * (Average)) / 100 && Math.Abs(((CurrentValue - PrevLow))) <= (110 * (Average) / 100))
        {
            Debug.Log("Startingsie Passed " + Passed);
            Passed++;
            return true;
        }

        float TempAverage = Math.Abs(FindPercentAverage((LowLists[LowLists.Count - 1]), Lists[Lists.Count - 1]));
        float JustIncaseItIncreased = 0;


        Debug.Log("Startingsie Temp Average" + TempAverage + "PrevCurrent Avereage" + FindPercentAverage(CurrentValue, PrevLow));

        if (FindPercentAverage(CurrentValue, PrevLow) >= (90 * (TempAverage)) / 100 && FindPercentAverage(CurrentValue, PrevLow) <= (110 * (TempAverage) / 100))
        {
            Debug.Log("Startingsie Passed by temp " + Passed + " This " + TempAverage);
            Passed++;
            return true;
        }

        Debug.Log("Startingsie Failed " + Passed);

        return false;
    }

    public float FindPercentAverage(float a, float b)
    {
        float Percent = 0;
        if (a > 0 && b > 0 || a < 0 && b < 0)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);

            if (a > b)
            {
                Percent = b / a;
            }
            else
            {
                Percent = a / b;
            }
        }
        else
        {
            if (a < 0)
            {
                a = Math.Abs(a);
                b = b + a;
            }
            else
            {
                b = Math.Abs(b);
                a = b + a;
            }

            if (a < b)
            {
                Percent = a / b;
            }
            else
            {
                Percent = b / a;
            }
        }

        return Percent;
    }

    public float AddAll(List<float> Highest, List<float> Lowest)
    {

        float Final = 0;
        int i = 0;
        while (i < Math.Min(Highest.Count, Lowest.Count))
        {


            Final += Math.Abs((Highest[i] * 100) - (Lowest[i] * 100));





            i++;
        }



        return Final;
    }


}


class CoordinateBody
{

    public Dictionary<string, int> Counties = new Dictionary<string, int>();


    public int FindLeast()
    {

        int Least = 0;
        bool first = true;

        foreach (var kvp in Counties)
        {
            if (first)
            {
                Least = kvp.Value;
                first = false;
            }
            if (kvp.Value < Least)
            {
                Least = kvp.Value;

            }
            // Debug.Log("Startingsie KeyValue Print-> " + kvp.Key + "-" + kvp.Value);

        }

        return Least;

    }

}


namespace UnityEngine.XR.ARFoundation.Samples
{




    public class BoneController : MonoBehaviour
    {
        // 3D joint skeleton
        enum JointIndices
        {
            Invalid = -1,
            Root = 0, // parent: <none> [-1]
            Hips = 1, // parent: Root [0]
            LeftUpLeg = 2, // parent: Hips [1]
            LeftLeg = 3, // parent: LeftUpLeg [2]
            LeftFoot = 4, // parent: LeftLeg [3]
            LeftToes = 5, // parent: LeftFoot [4]
            LeftToesEnd = 6, // parent: LeftToes [5]
            RightUpLeg = 7, // parent: Hips [1]
            RightLeg = 8, // parent: RightUpLeg [7]
            RightFoot = 9, // parent: RightLeg [8]
            RightToes = 10, // parent: RightFoot [9]
            RightToesEnd = 11, // parent: RightToes [10]
            Spine1 = 12, // parent: Hips [1]
            Spine2 = 13, // parent: Spine1 [12]
            Spine3 = 14, // parent: Spine2 [13]
            Spine4 = 15, // parent: Spine3 [14]
            Spine5 = 16, // parent: Spine4 [15]
            Spine6 = 17, // parent: Spine5 [16]
            Spine7 = 18, // parent: Spine6 [17]
            LeftShoulder1 = 19, // parent: Spine7 [18]
            LeftArm = 20, // parent: LeftShoulder1 [19]
            LeftForearm = 21, // parent: LeftArm [20]
            LeftHand = 22, // parent: LeftForearm [21]
            LeftHandIndexStart = 23, // parent: LeftHand [22]
            LeftHandIndex1 = 24, // parent: LeftHandIndexStart [23]
            LeftHandIndex2 = 25, // parent: LeftHandIndex1 [24]
            LeftHandIndex3 = 26, // parent: LeftHandIndex2 [25]
            LeftHandIndexEnd = 27, // parent: LeftHandIndex3 [26]
            LeftHandMidStart = 28, // parent: LeftHand [22]
            LeftHandMid1 = 29, // parent: LeftHandMidStart [28]
            LeftHandMid2 = 30, // parent: LeftHandMid1 [29]
            LeftHandMid3 = 31, // parent: LeftHandMid2 [30]
            LeftHandMidEnd = 32, // parent: LeftHandMid3 [31]
            LeftHandPinkyStart = 33, // parent: LeftHand [22]
            LeftHandPinky1 = 34, // parent: LeftHandPinkyStart [33]
            LeftHandPinky2 = 35, // parent: LeftHandPinky1 [34]
            LeftHandPinky3 = 36, // parent: LeftHandPinky2 [35]
            LeftHandPinkyEnd = 37, // parent: LeftHandPinky3 [36]
            LeftHandRingStart = 38, // parent: LeftHand [22]
            LeftHandRing1 = 39, // parent: LeftHandRingStart [38]
            LeftHandRing2 = 40, // parent: LeftHandRing1 [39]
            LeftHandRing3 = 41, // parent: LeftHandRing2 [40]
            LeftHandRingEnd = 42, // parent: LeftHandRing3 [41]
            LeftHandThumbStart = 43, // parent: LeftHand [22]
            LeftHandThumb1 = 44, // parent: LeftHandThumbStart [43]
            LeftHandThumb2 = 45, // parent: LeftHandThumb1 [44]
            LeftHandThumbEnd = 46, // parent: LeftHandThumb2 [45]
            Neck1 = 47, // parent: Spine7 [18]
            Neck2 = 48, // parent: Neck1 [47]
            Neck3 = 49, // parent: Neck2 [48]
            Neck4 = 50, // parent: Neck3 [49]
            Head = 51, // parent: Neck4 [50]
            Jaw = 52, // parent: Head [51]
            Chin = 53, // parent: Jaw [52]
            LeftEye = 54, // parent: Head [51]
            LeftEyeLowerLid = 55, // parent: LeftEye [54]
            LeftEyeUpperLid = 56, // parent: LeftEye [54]
            LeftEyeball = 57, // parent: LeftEye [54]
            Nose = 58, // parent: Head [51]
            RightEye = 59, // parent: Head [51]
            RightEyeLowerLid = 60, // parent: RightEye [59]
            RightEyeUpperLid = 61, // parent: RightEye [59]
            RightEyeball = 62, // parent: RightEye [59]
            RightShoulder1 = 63, // parent: Spine7 [18]
            RightArm = 64, // parent: RightShoulder1 [63]
            RightForearm = 65, // parent: RightArm [64]
            RightHand = 66, // parent: RightForearm [65]
            RightHandIndexStart = 67, // parent: RightHand [66]
            RightHandIndex1 = 68, // parent: RightHandIndexStart [67]
            RightHandIndex2 = 69, // parent: RightHandIndex1 [68]
            RightHandIndex3 = 70, // parent: RightHandIndex2 [69]
            RightHandIndexEnd = 71, // parent: RightHandIndex3 [70]
            RightHandMidStart = 72, // parent: RightHand [66]
            RightHandMid1 = 73, // parent: RightHandMidStart [72]
            RightHandMid2 = 74, // parent: RightHandMid1 [73]
            RightHandMid3 = 75, // parent: RightHandMid2 [74]
            RightHandMidEnd = 76, // parent: RightHandMid3 [75]
            RightHandPinkyStart = 77, // parent: RightHand [66]
            RightHandPinky1 = 78, // parent: RightHandPinkyStart [77]
            RightHandPinky2 = 79, // parent: RightHandPinky1 [78]
            RightHandPinky3 = 80, // parent: RightHandPinky2 [79]
            RightHandPinkyEnd = 81, // parent: RightHandPinky3 [80]
            RightHandRingStart = 82, // parent: RightHand [66]
            RightHandRing1 = 83, // parent: RightHandRingStart [82]
            RightHandRing2 = 84, // parent: RightHandRing1 [83]
            RightHandRing3 = 85, // parent: RightHandRing2 [84]
            RightHandRingEnd = 86, // parent: RightHandRing3 [85]
            RightHandThumbStart = 87, // parent: RightHand [66]
            RightHandThumb1 = 88, // parent: RightHandThumbStart [87]
            RightHandThumb2 = 89, // parent: RightHandThumb1 [88]
            RightHandThumbEnd = 90, // parent: RightHandThumb2 [89]
        }

        public bool Called = false;
        public Image rawImageColor;
        const int k_NumSkeletonJoints = 91;
        private CoordinateBody BodyCoordinator = new CoordinateBody();
        [SerializeField]
        [Tooltip("The root bone of the skeleton.")]
        Transform m_SkeletonRoot;

        /// <summary>
        /// Get/Set the root bone of the skeleton.
        /// </summary>
        public Transform skeletonRoot
        {
            get
            {
                return m_SkeletonRoot;
            }
            set
            {
                m_SkeletonRoot = value;
            }
        }

        Transform[] m_BoneMapping = new Transform[k_NumSkeletonJoints];

        public void InitializeSkeletonJoints()
        {
            // Walk through all the child joints in the skeleton and
            // store the skeleton joints at the corresponding index in the m_BoneMapping array.
            // This assumes that the bones in the skeleton are named as per the
            // JointIndices enum above.
            Queue<Transform> nodes = new Queue<Transform>();
            nodes.Enqueue(m_SkeletonRoot);
            while (nodes.Count > 0)
            {
                Transform next = nodes.Dequeue();
                for (int i = 0; i < next.childCount; ++i)
                {
                    nodes.Enqueue(next.GetChild(i));
                }
                ProcessJoint(next);
            }
        }



        // Algo
        private void Start()
        {
            Debug.Log("Startingsie 10e WEeee2 5425 eDiddd Kinda Hear youuu and you loud an clear with start keys");
            //CheckLast.Add(true);
            //CheckLastX.Add(true);
            //CheckLastZ.Add(true);
            BodyParts.Add(new BodyPartMovement());


            BodyParts.Add(new BodyPartMovement());
            BodyParts.Add(new BodyPartMovement());
            BodyParts.Add(new BodyPartMovement());

            BodyParts[0].CheckyIfHigherX.Add(100);
            BodyParts[0].CheckyIfHigher.Add(100);

            BodyParts[3].CheckyIfHigherX.Add(100);
            BodyParts[3].CheckyIfHigher.Add(100);
            BodyParts[0].CheckyIfHigherX.Add(100);
            BodyParts[0].CheckyIfHigher.Add(100);

            BodyParts[3].CheckyIfHigherX.Add(100);
            BodyParts[3].CheckyIfHigher.Add(100);

            BodyParts[0].CheckyIfLower.Add(-100);
            BodyParts[3].CheckyIfLower.Add(-100);

            BodyParts[0].CheckyIfLowerX.Add(-100);
            BodyParts[3].CheckyIfLowerX.Add(-100);
            BodyParts[0].CheckyIfLowerX.Add(-100);
            BodyParts[3].CheckyIfLowerX.Add(-100);


            int i = 0;
            while (i < BodyParts.Count)
            {

                BodyParts[i].CheckLast.Add(true);
                BodyParts[i].CheckLastX.Add(true);
                BodyParts[i].CheckLastZ.Add(true);
                i++;
            }
            time = Time.time + 4;
        }




        public void ApplyBodyPose(ARHumanBody body, Text texite)
        {

            var joints = body.joints;
            if (!joints.IsCreated)
                return;

            for (int i = 0; i < k_NumSkeletonJoints; ++i)
            {
                XRHumanBodyJoint joint = joints[i];
                var bone = m_BoneMapping[i];
                if (bone != null)
                {


                    if (i == 22) // Left Hand s
                    {


                        Algo(bone.transform.position.x, bone.transform.position.y, bone.transform.position.z, BodyParts[0], "22");
                        var Least = BodyCoordinator.FindLeast();
                        if (BodyParts[0].LowestListY.Count >= Least + 2 && BodyParts[0].LowestListY.Count > FreeFirst)
                        {

                            Debug.Log("Startingsie Least Was " + Least + " Low Count " + BodyParts[0].LowestListY.Count);
                            var Difference = BodyParts[0].LowestListY.Count - (Least + 2);
                            int K = 0;
                            rawImageColor.GetComponent<Image>().color = Color.red;
                            while (K < Difference)
                            {
                                BodyParts[0].LowestListY.Remove(0);
                                BodyParts[0].HighestListY.Remove(0);
                                K++;
                            }
                        }

                    }
                    else if (i == 4)
                    { // Left Foot    
                        //Algo(bone.transform.position.x, bone.transform.position.y, bone.transform.position.z, BodyParts[1], "4", BodyCoordinator.FindLeast(), 4);
                    }
                    else if (i == 9) // Right Foot
                    {
                        //                        Algo(bone.transform.position.x, bone.transform.position.y, bone.transform.position.z, BodyParts[2], "9", BodyCoordinator.FindLeast(), 4);
                    }
                    else if (i == 66) // Right Hand
                    {

                        Algo(bone.transform.position.x, bone.transform.position.y, bone.transform.position.z, BodyParts[3] , "66");
                        var Least = BodyCoordinator.FindLeast();
                        if (BodyParts[3].LowestListY.Count >= Least + 2 && BodyParts[0].LowestListX.Count > FreeFirst)

                        {
                            Debug.Log("Startingsie Least Was " + Least + " Low Count X " + BodyParts[0].LowestListX.Count);
                            var Difference = BodyParts[3].LowestListY.Count - (Least + 2);
                            int K = 0;
                            while (K < Difference)
                            {
                                BodyParts[3].LowestListY.Remove(0);
                                BodyParts[3].HighestListY.Remove(0);
                                K++;
                            }
                        }
                        //texite.text = "" + BodyCoordinator.FindLeast();

                    }


                    bone.transform.localPosition = joint.localPose.position;
                    bone.transform.localRotation = joint.localPose.rotation;


                    //texite.text = "" + Counties;

                    texite.text = "" + BodyCoordinator.FindLeast();




                }
            }
        }

        void ProcessJoint(Transform joint)
        {
            int index = GetJointIndex(joint.name);
            if (index >= 0 && index < k_NumSkeletonJoints)
            {
                m_BoneMapping[index] = joint;
            }
            else
            {
                //Debug.LogWarning($"{joint.name} was not found.");
            }
        }

        // Returns the integer value corresponding to the JointIndices enum value
        // passed in as a string.
        int GetJointIndex(string jointName)
        {
            JointIndices val;
            if (Enum.TryParse(jointName, out val))
            {
                return (int)val;
            }
            return -1;
        }

        //bool[] HighLowBool = new bool[3] { true, true, true }; // True if Highest 
        //float[] Highest = new float[3] { 0,0, 0}; // 0 is X, 1 is Y & Position 2 is Z
        //List<float> HighestListX = new List<float>();
        //List<float> HighestListY = new List<float>();
        //List<float> HighestListZ = new List<float>();

        //float[] Lowest = new float[3] { 0, 0, 0 };
        //List<float> LowestListX = new List<float>();
        //List<float> LowestListY = new List<float>();
        //List<float> LowestListZ = new List<float>();



        public void GiveDetails()
        {
            //float FinalX = GiveFinalAnswer(HighestListX);
            //float FinalY = GiveFinalAnswer(HighestListY);
            //float FinalZ = GiveFinalAnswer(HighestListZ);
 

        }

        bool TrueIt = true;
        public bool Work = true;
        int FreeFirst = 7;
        //List<bool> CheckLast = new List<bool>();
        //List<bool> CheckLastX = new List<bool>();
        //List<bool> CheckLastZ = new List<bool>();

        List<BodyPartMovement> BodyParts = new List<BodyPartMovement>();
         
       
        private void Algo(float x, float y, float z, BodyPartMovement Body, string BodyPart)
        {
           

            if (!start)
            {
                return;

            }
            //if (y > 3f || x > 3f)
            //{
            //    Debug.Log("StartingsieReturneddd");
            //    Debug.Log("Startingsie X :" + x + "Y " + y + "Z " + z);
            //    return;
            //}
            //if (x < -3f || y < -3f)
            //{
            //    Debug.Log("Startingsie X :" + x + "Y " + y + "Z " + z);
            //    return;
            //}




            int ItemX = (int)(Body.AddAll(Body.LowestListX, Body.HighestListX) * 100);
            int ItemY = (int)(Body.AddAll(Body.LowestListY, Body.HighestListY) * 100);

            if (TrueIt)
            {

                if (y * 100 > 0)
                {
                    Body.HighLowBool[1] = false;
                    Body.CheckLast.Add(false);


                }
                if (x * 100 > 0)
                {
                    Body.HighLowBool[0] = false;
                    Body.CheckLastX.Add(false);
                }

                if (z * 100 > 0)
                {
                    Body.HighLowBool[2] = false;
                    Body.CheckLastZ.Add(false);
                }
                TrueIt = false;
            }
            // X-----XXXX-----X-----XXXX-----X-----XXXX-----X-----XXXX-----X-----XXXX-----X-----XXXX-----X-----XXXX-----

            if (((Body.Highest[0] * 1000)) <= x * 1000 ) // was 97 
            {

                Body.Highest[0] = x;
                Body.CheckyIfLowerX.Add(Body.Highest[0]);
                Body.HighLowBool[0] = true;
                Body.CheckLastX.Add(true);
                Body.CheckyIfHigherX.Add(x);


            }
            else if ((((Body.Lowest[0] * 1000))) >= x * 1000 ) /// was 103  // 
            {

                Body.Lowest[0] = x;
                Body.CheckyIfHigherX.Add(Body.Lowest[0]);
                Body.HighLowBool[0] = false;
                Body.CheckLastX.Add(false);
                rawImageColor.GetComponent<Image>().color = Color.green;
                Body.CheckyIfLowerX.Add(x);

            }


            if (Body.CheckLastX[Body.CheckLastX.Count - 1])
            {


                if (Body.HighLowBool[0])
                {



                    // 
                    if (Body.LowestListX.Count < FreeFirst)
                    {
                        Body.LowestListX.Add(Body.Lowest[0]);
                        float Temp = Body.Lowest[0];
                        Body.Lowest[0] = Body.Highest[0];
                        Body.Highest[0] = Temp;
                        Body.CountLowX = true;

                        rawImageColor.GetComponent<Image>().color = Color.green;

                    }
                    else if (Body.CheckAverage(Body.LowestListX, Body.HighestListX, Body.Lowest[0], Body.HighestListX[Body.HighestListX.Count - 1]))
                    {
                        if (Body.CountHighX && Body.CountHigh)
                        {

                            Body.LowestListX.Add(Body.Lowest[0]);


                            float Temp = Body.Lowest[0];
                            Body.Lowest[0] = Body.Highest[0];
                            Body.Highest[0] = Temp;
                            rawImageColor.GetComponent<Image>().color = Color.green;
                        }


                        Body.CountLowX = true;
                    }
                    else
                    {
                        Body.CountLowX = false;
                        rawImageColor.GetComponent<Image>().color = Color.red;

                    }

                }
                else
                {

                    if (Body.CountiesX < FreeFirst)
                    {

                        float Temp = Body.Highest[0];
                        Body.HighestListX.Add(Body.Highest[0]);
                        Body.Highest[0] = Body.Lowest[0];
                        Body.Lowest[0] = Temp;
                        Body.CountHighX = true;
                        Body.CheckLastX.Add(false);
                        rawImageColor.GetComponent<Image>().color = Color.green;

                    }
                    else if (Body.CheckAverage(Body.HighestListX, Body.LowestListX, Body.Highest[0], Body.LowestListX[Body.LowestListX.Count - 1]))
                    {
                        if (Body.CountLowX && Body.CountLow)
                        {

                            Body.HighestListX.Add(Body.Highest[0]);


                            float Temp = Body.Highest[0];
                            Body.Highest[0] = Body.Lowest[0];
                            Body.Lowest[0] = Temp;
                            rawImageColor.GetComponent<Image>().color = Color.green;
                        }

                        Body.CountHighX = true;
                    }
                    else
                    {
                        Body.CountHighX = false;
                        rawImageColor.GetComponent<Image>().color = Color.red;

                    } 

                }

            }

            // Y----------Y--------Y--------Y----------Y--------Y------Y----------Y--------Y---Y----------Y--------Y


            if ((((Body.Highest[1] * 1000))) <= y * 1000 || (y * 1000 > Body.FindNegativePercent(120, (Body.CheckyIfHigher[Body.CheckyIfHigher.Count - 1] * 1000)) && Body.CheckLast[Body.CheckLast.Count - 1] == false)) // was 97 
            {
                // 
                Debug.Log("Startingsie" + BodyPart + " Too Highss   " + Body.Highest[1] + " With Low " + Body.Lowest[1]);
                Body.Highest[1] = y;
                Body.CheckyIfLower.Add(Body.Highest[1]);

                Body.HighLowBool[1] = true;
                Body.CheckLast.Add(true);
                Body.CheckyIfHigher.Add(y);



            }
            else if ((((Body.Lowest[1] * 1000))) >= y * 1000 || (y * 1000 < Body.FindNegativePercent(80, (Body.CheckyIfLower[Body.CheckyIfLower.Count - 1] * 1000)) && Body.CheckLast[Body.CheckLast.Count - 1] == true)) /// was 103 
            {

                Debug.Log("Startingsie" + BodyPart + " TooLow " + BodyPart + "  " + Body.Lowest[1] + "With High " + Body.Highest[1]);
                Body.Lowest[1] = y;
                Body.CheckyIfHigher.Add(Body.Lowest[1]);

                Body.HighLowBool[1] = false;
                Body.CheckLast.Add(false);
                Body.CheckyIfLower.Add(y);

            }




            if (Body.CheckLast[Body.CheckLast.Count - 1] != Body.CheckLast[Body.CheckLast.Count - 2])
            {
                if (Body.HighLowBool[1])
                {


                    //
                    if (Body.LowestListY.Count < FreeFirst)
                    {
                        Body.LowestListY.Add(Body.Lowest[1]);

                        float Temp = Body.Lowest[1];
                        Body.Lowest[1] = Body.Highest[1];
                        Body.Highest[1] = Temp;
                        Body.CountLow = true;
                        rawImageColor.GetComponent<Image>().color = Color.green;

                    }
                    else if (Body.CheckAverage(Body.LowestListY, Body.HighestListY, Body.Lowest[1], Body.HighestListY[Body.HighestListY.Count - 1]))
                    {
                        if (Body.CountHigh && Body.CountHighX)
                        {

                            Body.LowestListY.Add(Body.Lowest[1]);
                            rawImageColor.GetComponent<Image>().color = Color.green;
                            float Temp = Body.Lowest[1];
                            Body.Lowest[1] = Body.Highest[1];
                            Body.Highest[1] = Temp;
                            
                        }

                        Body.CountLow = true;
                    }
                    else
                    {
                        Body.CountLow = false;
                        rawImageColor.GetComponent<Image>().color = Color.red;

                    }



                }
                else
                {
                    if (Body.HighestListY.Count < FreeFirst)
                    {

                        Body.HighestListY.Add(Body.Highest[1]);

                        float Temp = Body.Highest[1];
                        Body.Highest[1] = Body.Lowest[1];
                        Body.Lowest[1] = Temp;
                        Body.CountHigh = true;
                        rawImageColor.GetComponent<Image>().color = Color.green;

                    }
                    else if (Body.CheckAverage(Body.HighestListY, Body.LowestListY, Body.Highest[1], Body.LowestListY[Body.LowestListY.Count - 1]))
                    {
                        if (Body.CountLow & Body.CountLowX)
                        {

                            Body.HighestListY.Add(Body.Highest[1]);

                            rawImageColor.GetComponent<Image>().color = Color.green;
                            float Temp = Body.Highest[1];
                            Body.Highest[1] = Body.Lowest[1];
                            Body.Lowest[1] = Temp;
                        }
                        else
                        {
                            rawImageColor.GetComponent<Image>().color = Color.red;
                        }

                        Body.CountHigh = true;

                    }
                    else
                    {
                        rawImageColor.GetComponent<Image>().color = Color.red;
                        Body.CountHigh = false;

                    }





                }
            }

            // Z-----Z-----Z-----Z-----Z-----Z-----Z-----Z-----Z-----Z-----Z-----Z-----Z-----Z-----Z-----Z-----Z-----Z-----
            //if (((97 * (Body.Highest[2] * 100)) / 100) <= z * 100)
            //{

            //    Body.Highest[2] = z;
            //    Body.HighLowBool[2] = true;
            //    Body.CheckLastZ.Add(true);

            //}
            //else if (((103 * (Body.Lowest[2] * 100)) / 100) >= z * 100)
            //{

            //    Body.Lowest[2] = z;
            //    Body.HighLowBool[2] = false;
            //    Body.CheckLastZ.Add(false);

            //}

            //if (Body.CheckLastZ[Body.CheckLastZ.Count - 1] != Body.CheckLastZ[Body.CheckLastZ.Count - 2])
            //{
            //    if (Body.HighLowBool[2])
            //    {

            //        if (Body.CountiesZ < 5)
            //        {
            //            Body.LowestListZ.Add(Body.Lowest[2]);
            //            float Temp = Body.Lowest[2];
            //            Body.Lowest[2] = Body.Highest[2];
            //            Body.Highest[2] = Temp;
            //            Body.CountLowZ = true;
            //        }
            //        else if (Body.CheckAverage(Body.LowestListZ, Body.HighestListZ, z, Body.HighestListZ[Body.HighestListZ.Count - 1]))
            //        {

            //            if (Body.CountHighZ && Body.CountHighX && Body.CountHigh)
            //            {
            //                Body.LowestListZ.Add(Body.Lowest[2]);
            //                float Temp = Body.Lowest[2];
            //                Body.Lowest[2] = Body.Highest[2];
            //                Body.Highest[2] = Temp;
            //            }
            //            Body.CountLowZ = true;

            //        }
            //        else
            //        {
            //            Body.CountLowZ = false;
            //        }


            //    }
            //    else
            //    {

            //        //
            //        if (Body.CountiesZ < FreeFirst)
            //        {
            //            Body.HighestListZ.Add(Body.Highest[2]);

            //            float Temp = Body.Highest[2];
            //            Body.Highest[2] = Body.Lowest[2];
            //            Body.Lowest[2] = Temp;
            //            Body.CountHighZ = true;
            //        }
            //        else if (Body.CheckAverage(Body.HighestListZ, Body.LowestListZ, z, Body.LowestListZ[Body.LowestListZ.Count - 1]))
            //        {
            //            if (Body.CountLowZ && Body.CountLow && Body.CountLowZ)
            //            {
            //                Body.HighestListZ.Add(Body.Highest[2]);
            //                float Temp = Body.Highest[2];
            //                Body.Highest[2] = Body.Lowest[2];
            //                Body.Lowest[2] = Temp;
            //            }
            //            Body.CountHighZ = true;

            //        }
            //        else
            //        {

            //            Body.CountHighZ = false;

            //        }



            //    }
            //}



            Body.CountiesY = Body.LowestListY.Count;
            Body.CountiesX = Body.LowestListX.Count;
            Body.CountiesZ = Body.LowestListZ.Count;

            //Debug.Log("Startingsie XCounties: " + Body.CountiesX + " Y Counties " + Body.CountiesY);

            // int ItemZ = (int)(Body.AddAll(Body.LowestListZ , Body.HighestListZ) * 100);

            if (ItemX > ItemY)
            {
                Counties = Body.CountiesX;
                BodyCoordinator.Counties[BodyPart] = Body.CountiesX;
            }

            else
            {
                Counties = Body.CountiesY;
                BodyCoordinator.Counties[BodyPart] = Body.CountiesY;
            }


            // Counties = Body.CountiesY;



        }
        int Counties = 0;

        private float GiveFinalAnswer(List<float> A)
        {
            int size = A.Count;
            float Final = 0;
            int i = 0;
            while (i < size)
            {
                float Num = A[i];
                if (Num < 0)
                {
                    Num = Num * -1;
                }
                Final += Num;
                i++;
            }

            return Final;
        }

        public bool start = false;
        float time = 1f;
        private void Update()
        {

            if (Time.time >= time)
            {
                start = true;

            }
            else
            {
                Debug.Log("Starting");
                Textie.text = "Ready?";

            }





        }
        public Text Textie;
    }



}
