using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairParameters
{
    public bool isActive = false;
    public bool isIndependent = true;
    public bool bothDirectionMovement = true;
    public bool onlyOutIn = false;
    public int parentIndex = 0;
    public float offsetTPos = 0;
    public float movementDistance = 0;
    public float movementSpeed = 0;
    public bool dirOut = true;
    public bool isSmooth = false;
    public float endDelay = 0;
    public float startDelay = 0;
    public bool hasObstacle = false;
}

public class StairController : MonoBehaviour
{
    public StairParameters stairParameters;

    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] MeshRenderer[] BrokenMeshRenderers;

    [SerializeField] GameObject hammerContainer;

    Vector3 defaultPosition;
    Vector3 defaultHammerRotation;

    public bool isLastStair = false;

    int stairIndex;
    public void InitStair(int i, Color color)
    {
        stairIndex = i;
        defaultPosition = transform.position;// GameManager.self.sceneManager.GetStairPosition(stairIndex);
        defaultHammerRotation = hammerContainer.transform.localEulerAngles;

        VisualContainer.SetActive(true);
        BreakVisualContainer.SetActive(false);

        foreach (MeshRenderer mesh in BrokenMeshRenderers)
        {
            mesh.material = GameManager.self.colorManager.BrokenStairs[ColorManager.ColorIndex];
        }

        meshRenderer.material.color = color;
        
    }

    public void SetParameters(StairParameters param)
    {
        stairParameters = param;

        if(stairParameters.isActive)
        {
            meshRenderer.material = GameManager.self.colorManager.MovingStairs[ColorManager.ColorIndex];
        }
    }


    [SerializeField] GameObject VisualContainer;
    [SerializeField] GameObject BreakVisualContainer;


    public void StairEntered()
    {
        if(!stairParameters.isActive)
        {
            meshRenderer.material = GameManager.self.colorManager.ActiveStairs[ColorManager.ColorIndex]; ;
        }
    }

    public void StairPassed()
    {
        VisualContainer.SetActive(false);
        BreakVisualContainer.SetActive(true);

        if (stairParameters.hasObstacle) hammerContainer.SetActive(false);

    }


    public void DoMovement()
    {
        if (!stairParameters.isActive && !stairParameters.hasObstacle) return;

        if (MovementRoutineC != null) StopCoroutine(MovementRoutineC);

        if (!stairParameters.hasObstacle)
        {
        
            if (stairParameters.isIndependent)
            {
                MovementRoutineC = StartCoroutine(MovementRoutine());
            }
            else
            {
                MovementRoutineC = StartCoroutine(MovementFollowRoutine());
            }
        }
        else
        {
            MovementRoutineC = StartCoroutine(ObstacleMovementRoutine());
        }
        

    }

    public float MovementT;

    Coroutine MovementRoutineC;
    IEnumerator MovementRoutine()
    {
        float t = 0;


        t = stairParameters.offsetTPos;
        transform.position = GetMovementFromT(t);

        while (true)
        {

            if (GameManager.GameStatus > 1)
            {
                break;
            }

            while(t>=-1 && t<=1)
            {
                t += (stairParameters.dirOut ? 1 : -1) * Time.fixedDeltaTime * stairParameters.movementSpeed;

                MovementT = t;
                if (stairParameters.isSmooth)
                {
                    MovementT = Mathf.SmoothStep(-1, 1, (t/2)+0.5f);
                }

                if(!stairParameters.bothDirectionMovement)
                {
                    MovementT = (t / 2) + (0.5f * (stairParameters.onlyOutIn ? 1:-1));
                }


                transform.position = GetMovementFromT(MovementT);

                if (GameManager.GameStatus > 1)
                {
                    break;
                }


                yield return new WaitForFixedUpdate();
            }

            stairParameters.dirOut = !stairParameters.dirOut;

            if (t > 1)
            {
                t = 1;
                yield return new WaitForSeconds(stairParameters.endDelay);
            }
            else if (t < -1)
            {
                t = -1;
                yield return new WaitForSeconds(stairParameters.startDelay);
            }




        }
    }

    Vector3 movementRightDir;
    Vector3 GetMovementFromT(float t)
    {
        if(stairParameters.isIndependent)
        {
            movementRightDir = transform.right;
        }
        else
        {
            movementRightDir = stairControllerToFollow.transform.right;
        }


        Vector3 newPos =  defaultPosition + (movementRightDir * stairParameters.movementDistance * t);

        return newPos;
    }

    Vector3 GetRotationFromT(float t)
    {
        Vector3 newRot = new Vector3(0,0,  defaultHammerRotation.z +  (stairParameters.movementDistance * t));

        return newRot;
    }


    StairController stairControllerToFollow;
    IEnumerator MovementFollowRoutine()
    {
        stairControllerToFollow = GameManager.self.sceneManager.stairControllers[stairParameters.parentIndex];

        stairParameters.movementDistance = stairControllerToFollow.stairParameters.movementDistance;

        while (true)
        {
            MovementT = stairControllerToFollow.MovementT;

            transform.position = GetMovementFromT(MovementT);

            yield return new WaitForFixedUpdate();
        }
    }
   
    IEnumerator ObstacleMovementRoutine()
    {
        hammerContainer.gameObject.SetActive(true);

        float t = 0;

        t = stairParameters.offsetTPos;
        hammerContainer.transform.localEulerAngles = GetRotationFromT(t);

        while (true)
        {

            if (GameManager.GameStatus > 1)
            {
                break;
            }

            while (t >= -1 && t <= 1)
            {
                t += (stairParameters.dirOut ? 1 : -1) * Time.fixedDeltaTime * stairParameters.movementSpeed;

                MovementT = t;
                if (stairParameters.isSmooth)
                {
                    MovementT = Mathf.SmoothStep(-1, 1, (t / 2) + 0.5f);
                }

                if (!stairParameters.bothDirectionMovement)
                {
                    MovementT = (t / 2) + (0.5f * (stairParameters.onlyOutIn ? 1 : -1));
                }


                hammerContainer.transform.localEulerAngles = GetRotationFromT(MovementT);

                if (GameManager.GameStatus > 1)
                {
                    break;
                }


                yield return new WaitForFixedUpdate();
            }

            stairParameters.dirOut = !stairParameters.dirOut;

            if (t > 1)
            {
                t = 1;
                yield return new WaitForSeconds(stairParameters.endDelay);
            }
            else if (t < -1)
            {
                t = -1;
                yield return new WaitForSeconds(stairParameters.startDelay);
            }




        }
    }

}
