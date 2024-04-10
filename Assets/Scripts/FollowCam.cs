using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [SerializeField] private Transform trsPlayer;
    [SerializeField] private Transform trsFollowCam;
    [SerializeField,Range(0f, 100f)] private float mouseSensitivity;

    Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        lookAround();
    }


    private void lookAround()
    {
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector3 camAngle = trsFollowCam.rotation.eulerAngles;
        float x = camAngle.x - mouseInput.y;

        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 300f, 361f);
        }

        trsFollowCam.rotation = Quaternion.Euler(x, camAngle.y + mouseInput.x, camAngle.z);


        Vector3 lookForward = new Vector3(trsFollowCam.forward.x, 0f, trsFollowCam.forward.z).normalized;
        Debug.DrawRay(trsFollowCam.transform.position, lookForward, Color.red);

        //trsPlayer.forward = lookForward;
    }
}
