using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float DelayToCheckHold;

    bool ControlEnabled = true;

    public int StepsCounter = 0;

    Vector3 playerPosOffset = new Vector3(0, 0.2f, 0);


    [SerializeField] float movementAcceleration;
    [SerializeField] Vector2 maximumMinumumSpeed;

    [SerializeField] CharController charController;

    public void InitPlayer()
    {
        ControlEnabled = true;
        StepsCounter = 0;
        
        transform.position = GetStairPosition(0);
        GameManager.self.cameraController.UpdateCameraColor(0);

        transform.localEulerAngles = GameManager.self.sceneManager.stairControllers[StepsCounter].transform.localEulerAngles;

        charController.SetMaterial(GameManager.self.colorManager.CharacterMaterials[ColorManager.ColorIndex]);
        charController.Standing();
    }


    void StartMovement()
    {

        if (MovementRoutineC != null) StopCoroutine(MovementRoutineC);

        MovementRoutineC = StartCoroutine(MovementRoutine());


    }


    Coroutine MovementRoutineC;
    IEnumerator MovementRoutine()
    {
        Vector3 startPos;
        Vector3 endPos;

        float t;
        float startT;
        float currentMovementSpeed = maximumMinumumSpeed.y;

        charController.Moving();

        bool continueMovement = true;
        ControlEnabled = false;
        yield return new WaitForSeconds(0.08f);


        GameManager.self.soundManager.fixPitch();

        GameManager.self.scoreManager.ResetRawCounter();

        while (continueMovement)
        {

#if UNITY_IOS
        if (GameManager.TapticEnabled)
            {
                TapticEngine.TriggerLight();
            }
#endif



            ControlEnabled = false;

            StopFollowStair();

            if (!CheckIfGameOverMove())
            {

                startPos = transform.position;
                StepsCounter += 1;
                endPos = CalculateNewPos(startPos);

                StairController enteredStairController = GameManager.self.sceneManager.StairEntered(StepsCounter);
                GameManager.self.sceneManager.StairPassed(StepsCounter-1);
                t = 0;
                startT = Time.time;

                while (t < 1)
                {
                    yield return new WaitForFixedUpdate();

                    t = (Time.time - startT) / currentMovementSpeed;

                    transform.position = Vector3.Lerp(startPos, endPos, t);

                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(enteredStairController.transform.localEulerAngles ),t);

                }
                transform.position = Vector3.Lerp(startPos, endPos, 1);



                currentMovementSpeed -= movementAcceleration;
                if (currentMovementSpeed < maximumMinumumSpeed.x)
                {
                    currentMovementSpeed = maximumMinumumSpeed.x;
                }

                StepCompleted(enteredStairController);

                ControlEnabled = true;

                continueMovement = isPointerDown;

            }
            else
            {
                break;
            }



        }

    }

    bool CheckIfGameOverMove()
    {
        StairController currentStairController = GameManager.self.sceneManager.stairControllers[ StepsCounter];

        int nextIndex =  StepsCounter + 1;

        if (nextIndex< 0)
        {
            return true;
        }


        StairController nextStairController = GameManager.self.sceneManager.stairControllers[nextIndex];


        Vector3 diff = nextStairController.transform.position - currentStairController.transform.position;

        Vector3 projected = Vector3.Project(diff, currentStairController.transform.right);


        if(projected.magnitude > GameManager.self.sceneManager.stairWidth*0.66f)
        {
            GameOverMove();
            return true;
        }
        else
        {
            return false;
        }
        
    }

    [SerializeField] LayerMask layerMask;
    RaycastHit hit;

    void StepCompleted(StairController enteredStairController)
    {
      
        if (enteredStairController.stairParameters.isActive)
        {
            FollowStair(enteredStairController);
        }

        if(enteredStairController.isLastStair)
        {
            GameManager.self.LevelPassed();
        }

        GameManager.self.levelManager.StairCompleted();
        GameManager.self.soundManager.StairSound();
        GameManager.self.scoreManager.AddScore(GameManager.self.sceneManager.StairEntered( StepsCounter ));
    }

    void GameOverMove()
    {
        ControlEnabled = false;
        GameManager.self.StopGame();

        GameManager.self.cameraController.StopCameraFollow();

        if (MovementRoutineC != null) StopCoroutine(MovementRoutineC);

        if (GameOverRoutineC != null) StopCoroutine(GameOverRoutineC);

        GameOverRoutineC = StartCoroutine(GameOverRoutine());

    }

    [SerializeField] AnimationCurve GameOverYAnim;

    Coroutine GameOverRoutineC;
    IEnumerator GameOverRoutine()
    {

        Vector3 startPos = transform.position;

        Vector3 endPos = startPos + (transform.forward*0.5f) + (transform.up * -5);

        float  t = 0;
        float startT = Time.time;

        float duration = 1;

        while (t < 1)
        {
            yield return new WaitForFixedUpdate();

            t = (Time.time - startT) / duration;

            Vector3 newPos = Vector3.Lerp(startPos, endPos, t);
            newPos.y = startPos.y + GameOverYAnim.Evaluate(t);

            transform.position = newPos;

        }


        GameManager.self.GameOver();

        yield return null;
    }

    void StopFollowStair()
    {
        if (StairFollowRoutineC != null) StopCoroutine(StairFollowRoutineC);
    }

    void FollowStair(StairController stairController)
    {
        if (StairFollowRoutineC != null) StopCoroutine(StairFollowRoutineC);


        StairFollowRoutineC = StartCoroutine(StairFollowRoutine(stairController));
    }

    Coroutine StairFollowRoutineC;
    IEnumerator StairFollowRoutine(StairController stairController)
    {
        while(true)
        {
            transform.position = stairController.transform.position + playerPosOffset;
            yield return new WaitForFixedUpdate();
        }
    }


    bool isPointerDown = false;

    public void PointerDown()
    {
        if (!ControlEnabled) return;

        if(GameManager.GameStatus == 0)
        {
            GameManager.self.StartGame();
        }

        StartMovement();
        isPointerDown = true;
    }

    public void PointerUp()
    {
        isPointerDown = false;

        charController.Standing();
        GameManager.self.soundManager.StopSound();
    }

    Vector3 CalculateNewPos(Vector3 startPos)
    {
        Vector3 endStairPos = GameManager.self.sceneManager.stairControllers[StepsCounter].transform.position + playerPosOffset;
        
        return endStairPos;
    }

    
    Vector3 GetStairPosition(int stairIndex)
    {
        return GameManager.self.sceneManager.GetStairPosition(stairIndex) + playerPosOffset;
    }

}
