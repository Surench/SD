using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] Vector2 StairDimension;


    void Start()
    {
        StartCoroutine(OneStep());
    }

    [SerializeField] Transform HipsTransform;
    [SerializeField] Transform RightFootHips;
    [SerializeField] Transform RightFootLeg;
    [SerializeField] Transform LeftFootHips;
    [SerializeField] Transform LeftFootLeg;

    [SerializeField] AnimationCurve FootHipsRotation;

    IEnumerator OneStep()
    {

        float duration = 1;
        float t = 0;
        float starTime = Time.time;


        Vector3 RightFootHipsStartRot = RightFootHips.transform.localEulerAngles;


        while (t<1)
        {

            t = (Time.time - starTime) / duration;

            

           // RightFootHips.transform.localEulerAngles = new Vector3(RightFootHipsStartRot.x + , RightFootHipsStartRot.y, RightFootHipsStartRot.z);

            yield return new WaitForEndOfFrame();

        }

    }

}
