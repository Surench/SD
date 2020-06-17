using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class StairsManager : MonoBehaviour
{
    [SerializeField] SplineComputer splineComputer;

    [SerializeField] GameObject stairPrefab;

    void Start()
    {
        GenerateStair();
    }
    

    void GenerateStair()
    {
        float stairSize = stairPrefab.transform.localScale.z * stairPrefab.GetComponent<BoxCollider>().size.z;

        float pathLenght = splineComputer.CalculateLength();

        float P = 0;

        SplineResult stairResult = splineComputer.Evaluate(0);

        GameObject obj = Instantiate(stairPrefab);

        obj.transform.position = stairResult.position;
        obj.transform.rotation = stairResult.rotation;
        obj.transform.localEulerAngles = new Vector3(0, obj.transform.localEulerAngles.y, obj.transform.localEulerAngles.z);


        Vector3 currentPos = obj.transform.position;

        while (P<1)
        {
            P += 0.001f;

            stairResult = splineComputer.Evaluate(P);
            
            float distance = Vector3.Distance(stairResult.position, currentPos);

            if (distance >= stairSize)
            {
                obj = Instantiate(stairPrefab);

                obj.transform.position = stairResult.position;
                obj.transform.rotation = stairResult.rotation;


                obj.transform.localEulerAngles = new Vector3(0, obj.transform.localEulerAngles.y, obj.transform.localEulerAngles.z);

                currentPos = obj.transform.position;
                
            }
        }    
    }

    /*
    void oldMethod()
    {

        for (int i = 0; i < 100; i++)
        {
            SplineResult stairResult = splineComputer.Evaluate(stairByPercent * i);


            GameObject newOBJ = Instantiate(stairPrefab);


            newOBJ.transform.position = stairResult.position;
            newOBJ.transform.rotation = stairResult.rotation;

        }
    }*/

}
