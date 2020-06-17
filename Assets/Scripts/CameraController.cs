using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private void Start()
    {
       /* bool deviceIsIphoneX = UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhoneX;
        if (deviceIsIphoneX)
        {
            camera.orthographicSize = 3.24f;
        }
        */
    }

    [SerializeField] Camera camera;

    [SerializeField] Vector3 CamGameplayPos;
    [SerializeField] Vector3 CamGameplayRot;
    [SerializeField] float CamGameplaySize;


    public Transform target;

	public float smoothSpeed = 0.125f;
	public Vector3 offset;

    public Transform currentStair;
	void FixedUpdate ()
	{
		Vector3 desiredPosition = target.position + offset;
		Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
		transform.position = smoothedPosition;


        //transform.LookAt(target);
    }

    private void Update()
    {
        currentStair = GameManager.self.sceneManager.stairControllers[GameManager.self.playerController.StepsCounter+10].transform;

        
        Debug.Log(currentStair.transform.localEulerAngles.y);
    }


    public void Init()
    {
       

        camera.backgroundColor = GameManager.self.colorManager.CameraColors[ColorManager.ColorIndex];

        //camera.transform.position = GameManager.self.playerController.transform.position + GameManager.self.playerController.transform.forward * 5 + new Vector3(0,1,0);

        //camera.transform.LookAt(GameManager.self.playerController.transform);

        //camera.orthographicSize = 0.88f;


        //CameraFollow();
    }

    public void StartCamera()
    {
        //RePositionCamera();
    }

    void RePositionCamera()
    {
        if (RePositionCameraRoutineC != null) StopCoroutine(RePositionCameraRoutineC);

        RePositionCameraRoutineC = StartCoroutine(RePositionCameraRoutine());
    }


    Coroutine RePositionCameraRoutineC;
    IEnumerator RePositionCameraRoutine()
    {
        float duration = 2f;

        float t = 0;
        float startT = Time.time;

        while(t<1)
        {
            t = (Time.time - startT) / duration;

            camera.transform.localPosition = Vector3.Lerp(camera.transform.localPosition, CamGameplayPos, t);

            camera.transform.localEulerAngles = new Vector3(
                Mathf.LerpAngle(camera.transform.localEulerAngles.x,CamGameplayRot.x,t),
                Mathf.LerpAngle(camera.transform.localEulerAngles.y, CamGameplayRot.y, t),
                Mathf.LerpAngle(camera.transform.localEulerAngles.z, CamGameplayRot.z, t));

            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, CamGameplaySize, t);

            yield return new WaitForEndOfFrame();
        }



    }


    void CameraFollow()
    {
        if (CameraFollowRoutineC != null) StopCoroutine(CameraFollowRoutineC);

        CameraFollowRoutineC = StartCoroutine(CameraFollowRoutine());
    }

    public void StopCameraFollow()
    {
        if (CameraFollowRoutineC != null) StopCoroutine(CameraFollowRoutineC);
    }

    public void UpdateCameraColor(int stairIndex)
    {
        //camera.backgroundColor = GameManager.self.colorManager.GetCameraGradient(stairIndex);
    }

    Coroutine CameraFollowRoutineC;
    IEnumerator CameraFollowRoutine()
    {

        while(true)
        {
            transform.position = GameManager.self.playerController.transform.position;

            yield return new WaitForFixedUpdate();
        }

    }


    
}
