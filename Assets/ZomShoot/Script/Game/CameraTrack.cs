using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrack : MonoBehaviour
{
    [SerializeField]
    private Animator ani = null;

    [SerializeField]
    private Transform cameraJnt = null;

    public Transform GetCameraJoint()
    {
        return cameraJnt;
    }

    public void PlayNext(int subStageId)
    {
        ani.SetInteger("SubStageId", subStageId);
    }

    public void OnPlayTrack()
    {

    }

    public void OnStopTrack()
    {

    }
}
