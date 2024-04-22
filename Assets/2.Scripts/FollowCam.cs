using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [SerializeField] Transform playerTrs;
    [SerializeField] Transform camTrs;


    [SerializeField] float distance;
    [SerializeField] float height;
    [SerializeField] float camOffset;

    private void Awake()
    {
        camTrs = GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        lookAround();
    }

    private void lookAround()
    {
        camTrs.position = playerTrs.position + (-playerTrs.forward * distance) + (Vector3.up * height);

        camTrs.LookAt(playerTrs.position + (playerTrs.up * camOffset));
    }
}
