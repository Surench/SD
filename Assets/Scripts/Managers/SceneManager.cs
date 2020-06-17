using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class SceneManager : MonoBehaviour
{
    [SerializeField] public GameObject StairPrefab;


    [SerializeField] int[] CurrentLevelPatterns;

    public int StairsAmount;

    public float stairWidth;

    public List<StairController> stairControllers = new List<StairController>();

    [SerializeField] SplineComputer splineComputer;

    [SerializeField] Transform FinishContainer;
    
    public void InitScene()
    {
        currentPatternIndex = 0;
        currentPatternSpawnedAmount = 0;

        stairControllers.Clear();


        float stairSize = StairPrefab.transform.localScale.z * StairPrefab.GetComponent<BoxCollider>().size.z -0.1f;
        stairWidth = stairSize;
        float pathLenght = splineComputer.CalculateLength();

        float P = 0;

        SplineResult stairResult = splineComputer.Evaluate(0);

        int i = 0;

        GameObject obj = Instantiate(StairPrefab);

        obj.transform.position = stairResult.position ;
        obj.transform.rotation = stairResult.rotation;
        obj.transform.localEulerAngles = new Vector3(0, obj.transform.localEulerAngles.y, obj.transform.localEulerAngles.z);

        StairController stairController = obj.GetComponent<StairController>();

        stairController.InitStair(i, GameManager.self.colorManager.GetStairColor(i));

        stairControllers.Add(stairController);

        Vector3 currentPos = obj.transform.position;


        while (P < 1)
        {
            P += 0.001f;

            stairResult = splineComputer.Evaluate(P);

            float distance = Vector3.Distance(stairResult.position, currentPos);

            if (distance >= stairSize)
            {
                obj = Instantiate(StairPrefab);

                obj.transform.position = stairResult.position;
                obj.transform.rotation = stairResult.rotation;


                obj.transform.localEulerAngles = new Vector3(0, obj.transform.localEulerAngles.y, obj.transform.localEulerAngles.z);

                currentPos = obj.transform.position;


                stairController = obj.GetComponent<StairController>();
                i += 1;
                stairController.InitStair(i, GameManager.self.colorManager.GetStairColor(i));

                stairControllers.Add(stairController);

            }
        }

        int lastIndex = stairControllers.Count - 1;
        FinishContainer.position = stairControllers[lastIndex].transform.position;
        stairControllers[lastIndex].isLastStair = true;

        StairsAmount = stairControllers.Count;

        for (int j = 0; j < stairControllers.Count; j++)
        {
            stairControllers[j].SetParameters(StairPatterns(j));
            stairControllers[j].DoMovement();
        }
    }
   
    public Vector3 GetStairPosition(int stairIndex)
    {
        return stairControllers[stairIndex].transform.position;
    }



    public StairController StairEntered(int index)
    {
        stairControllers[index].StairEntered();

        return stairControllers[index];
    }

    public void StairPassed(int index)
    {
        stairControllers[index].StairPassed();
    }


    StairParameters GetParametersByType(int type,float offsetTpos=0)
    {

        StairParameters stairParameters = new StairParameters();
        if (type == 0)
        {
            stairParameters.isActive = false;
        }
        else  if(type ==1)
        {
            stairParameters.isActive = true;
            stairParameters.isIndependent = true;
            stairParameters.parentIndex = 0;
            stairParameters.offsetTPos = offsetTpos;
            stairParameters.movementDistance = 0.5f;
            stairParameters.movementSpeed = 3;
            stairParameters.dirOut = true;
            stairParameters.isSmooth = false;
            stairParameters.endDelay = 0.2f;
            stairParameters.startDelay = 0.5f;
            stairParameters.bothDirectionMovement = false;
            stairParameters.onlyOutIn = true;
        }
        else if (type == 2)
        {
            stairParameters.isActive = true;
            stairParameters.isIndependent = true;
            stairParameters.parentIndex = 0;
            stairParameters.offsetTPos = offsetTpos;
            stairParameters.movementDistance = 0.5f;
            stairParameters.movementSpeed = 3;
            stairParameters.dirOut = true;
            stairParameters.isSmooth = false;
            stairParameters.endDelay = 0.2f;
            stairParameters.startDelay = 0.5f;
            stairParameters.bothDirectionMovement = false;
            stairParameters.onlyOutIn = false;
        }
        else if (type == 3)
        {
            stairParameters.isActive = true;
            stairParameters.isIndependent = true;
            stairParameters.parentIndex = 0;
            stairParameters.offsetTPos = offsetTpos;
            stairParameters.movementDistance = 0.4f;
            stairParameters.movementSpeed = 1;
            stairParameters.dirOut = false;
            stairParameters.isSmooth = true;
            stairParameters.endDelay = 0.2f;
            stairParameters.startDelay = 0.2f;
            stairParameters.bothDirectionMovement = true;
        }
        else if (type == 4)
        {
            stairParameters.hasObstacle = true;
            stairParameters.isActive = false;
            stairParameters.isIndependent = true;
            stairParameters.parentIndex = 0;
            stairParameters.offsetTPos = offsetTpos;
            stairParameters.movementDistance = 75;
            stairParameters.movementSpeed = 2;
            stairParameters.dirOut = false;
            stairParameters.isSmooth = true;
            stairParameters.endDelay = 0f;
            stairParameters.startDelay = 0f;
            stairParameters.bothDirectionMovement = true;
        }


        return stairParameters;
    }



    int currentPatternIndex = 0;
    int currentPatternSpawnedAmount = 0;

    StairParameters StairPatterns(int index)
    {

        if (index < 10)
        {
            return GetParametersByType(0);
        }

        if(index > StairsAmount-10)
        {
            return GetParametersByType(0);
        }

        currentPatternSpawnedAmount += 1;

        int currentPattern = CurrentLevelPatterns[currentPatternIndex];

        if (currentPattern == 0)
        {

            if (currentPatternSpawnedAmount <= 10)
            {
                return GetParametersByType(0);
            }
            else
            {
                return NewPattern(index);
            }


        }
        else if (currentPattern == 1)
        {

            if (currentPatternSpawnedAmount == 1)
            {
                return GetParametersByType(1);
            }
            else
            {
                return NewPattern(index);
            }

        }
        else if (currentPattern == 2)
        {
            if (currentPatternSpawnedAmount < 3)
            {
                return GetParametersByType(2);
            }
            else
            {
                return NewPattern(index);
            }

        }
        else if (currentPattern == 3)
        {
            if (currentPatternSpawnedAmount < 3)
            {
                return GetParametersByType(3);
            }
            else
            {
                return NewPattern(index);
            }

        }
        else if (currentPattern == 4)
        {
            if (currentPatternSpawnedAmount < 2)
            {
                return GetParametersByType(4);
            }
            else
            {
                return NewPattern(index);
            }

        }
        else if (currentPattern == 5)
        {
            if (currentPatternSpawnedAmount <= 1)
            {
                return GetParametersByType(2);
            }
            else if (currentPatternSpawnedAmount <= 4)
            {
                return GetParametersByType(0);
            }
            else if (currentPatternSpawnedAmount <= 6)
            {
                return GetParametersByType(4, 0.4f);
            }
           
            else
            {
                return NewPattern(index);
            }

        }
        else if (currentPattern == 6)
        {
            if (currentPatternSpawnedAmount <= 1)
            {
                return GetParametersByType(0);
            }
            else if (currentPatternSpawnedAmount < 3)
            {
                return GetParametersByType(4,0.4f);
            }
            if (currentPatternSpawnedAmount < 7)
            {
                return GetParametersByType(0);
            }
            else if (currentPatternSpawnedAmount < 9)
            {
                return GetParametersByType(4, 0.4f);
            }
            if (currentPatternSpawnedAmount < 14)
            {
                return GetParametersByType(0);
            }
            else if (currentPatternSpawnedAmount <= 16)
            {
                return GetParametersByType(1);
            }

            else
            {
                return NewPattern(index);
            }

        }
        else// if (currentPattern == 7)
        {
            if (currentPatternSpawnedAmount <= 1)
            {
                return GetParametersByType(1);
            }
            else if (currentPatternSpawnedAmount < 3)
            {
                return GetParametersByType(1);
            }
            if (currentPatternSpawnedAmount < 8)
            {
                return GetParametersByType(0);
            }
            else if (currentPatternSpawnedAmount < 10)
            {
                return GetParametersByType(4, 0.4f);
            }
            if (currentPatternSpawnedAmount < 16)
            {
                return GetParametersByType(0);
            }
            else if (currentPatternSpawnedAmount < 17)
            {
                return GetParametersByType(2);
            }

            else
            {
                return NewPattern(index);
            }

        }
    }

    StairParameters NewPattern(int index)
    {

        currentPatternIndex += 1;

        if (currentPatternIndex == CurrentLevelPatterns.Length)
        {
            currentPatternIndex = 0;
        }



        currentPatternSpawnedAmount = 0;

        return StairPatterns(index);
    }

}
