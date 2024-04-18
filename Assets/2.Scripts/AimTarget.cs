using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimTarget : MonoBehaviour
{
    Camera Maincam;
    [SerializeField] Transform cameraTrs;

    void Start()
    {
        Maincam = Camera.main;
    }

    void Update()
    {
        Vector2 mouseinput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        float x = cameraTrs.forward.x - mouseinput.y;
        gameObject.transform.position = new Vector3(cameraTrs.forward.x, cameraTrs.forward.y, cameraTrs.forward.z).normalized;

        Debug.DrawRay(gameObject.transform.position, 
            new Vector3(cameraTrs.forward.x, cameraTrs.forward.y, cameraTrs.forward.z), Color.red);
    }
}
