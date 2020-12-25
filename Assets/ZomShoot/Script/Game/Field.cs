using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SubStage
{
    public int SubStageId = 0;
    public int NextSubStageId = 0;
    public List<GameObject> ColliderList = null;
    public List<Enemy> EnemyList = null;
}

public class FieldStateData
{
    public int SubStageId = 0;
    public List<SubStage> SubStageList = null;
}

public class Field : MonoBehaviour
{
    [SerializeField]
    private CameraTrack cameraTrack = null;

    [SerializeField]
    private List<SubStage> subStageList = null;

    public void ChangeStartSubStage()
    {

    }

    public void ChangeFinishSubState()
    {

    }

    public CameraTrack GetCameraTrack()
    {
        return cameraTrack;
    }
}
