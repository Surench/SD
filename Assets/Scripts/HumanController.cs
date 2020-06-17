using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour
{
    Vector3 HipsStartPos;
    private void Start()
    {
        HipsStartPos = transform.position;
    }

    [SerializeField] Animator animator;

    [SerializeField] Transform HipsTransform;


    [SerializeField] Vector2 StairSize;

    [SerializeField] float DelayToCheckHold;
    [SerializeField] float DelayToCheckSlowRun;

    int StepsCounter = 0;


    void OneStepMovement()
    {
        if (StepC != null) StopCoroutine(StepC);
        if (SlowRunC != null) StopCoroutine(SlowRunC);
        StepC =  StartCoroutine(Step());
    }

    Coroutine StepC;
    IEnumerator Step()
    {

        StepsCounter += 1;
        transform.position = HipsStartPos + (new Vector3(0, StairSize.y, StairSize.x) * (StepsCounter));

        animator.Play("OneStep", -1, 0);

        yield return new WaitForSeconds(DelayToCheckHold);

        StepCompleted();
    }

    void StepCompleted()
    {
        if(isPointerDown)
        {
            SlowRunMovement();
        }
    }

    void SlowRunMovement()
    {
        if (StepC != null) StopCoroutine(StepC);
        if (SlowRunC != null) StopCoroutine(SlowRunC);

        SlowRunC = StartCoroutine(SlowRun());

    }


    Coroutine SlowRunC;
    IEnumerator SlowRun()
    {

        StepsCounter += 2;
        transform.position = HipsStartPos + (new Vector3(0, StairSize.y, StairSize.x) * (StepsCounter));

        animator.Play("SlowRun", -1, 0);

        yield return new WaitForSeconds(DelayToCheckSlowRun);

        SlowRunStepCompleted();
    }

    void SlowRunStepCompleted()
    {
        if (isPointerDown)
        {
            SlowRunMovement();
        }
        else
        {
            StepsCounter += 1;
            animator.Play("StopSlowRun", -1, 0);
        }
    }


    bool isPointerDown = false;

    public void PointerDown()
    {
        OneStepMovement();
        isPointerDown = true;
    }

    public void PointerUp()
    {
        isPointerDown = false;
    }

}
