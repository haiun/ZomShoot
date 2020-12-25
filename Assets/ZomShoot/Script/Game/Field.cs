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

public class Field : MonoBehaviour
{
    [SerializeField]
    private Animator camAnim = null;

    [SerializeField]
    private List<SubStage> subStageList = null;
}
