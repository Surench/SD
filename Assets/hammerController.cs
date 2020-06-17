using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hammerController : MonoBehaviour
{

    public AnimationCurve curve;

    [SerializeField] GameObject parentObj;
    [SerializeField] float speed;
    [SerializeField] float ZRotation;

    public bool startFromRight; 

    private void Start()
    {
          initCurve();
          StartCoroutine(RotateHammer());       
    }

    void initCurve()
    {
        if (startFromRight) parentObj.transform.eulerAngles = new Vector3(0, 180, 0);
        
        curve = new AnimationCurve(new Keyframe(-ZRotation, -ZRotation), new Keyframe(ZRotation, ZRotation));

        curve.preWrapMode = WrapMode.PingPong;
        curve.postWrapMode = WrapMode.PingPong;
    }

    float Ypos;
    IEnumerator RotateHammer()
    {

        while (true)
        {

            Ypos = curve.Evaluate(Time.time * speed);

            transform.localEulerAngles = new Vector3(0,0, Ypos);

            yield return new WaitForEndOfFrame();
        }

    }


  

}
