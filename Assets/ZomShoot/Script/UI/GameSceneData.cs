using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneInitData : ISceneInitData
{
    public GameInstance GameInstance = null;
    public int FieldId = 1;
    public int MaxBullet = 5;
}

public class GameSceneState
{
    public SubStage CurrentSubStage = null;
    public HeliPlayerData HeliPlayerData = null;

    public int MaxBullet = 5;
    public int CurrentBullet = 5;

    public int LeftEnemyCount = 0;
    public int CurrentEnemyIndex = 0;

    public void InvalidTarget()
    {
        if (LeftEnemyCount <= CurrentEnemyIndex)
            CurrentEnemyIndex = 0;

        if (CurrentEnemyIndex < CurrentSubStage.EnemyList.Count)
        {
            HeliPlayerData.Target = CurrentSubStage.EnemyList[CurrentEnemyIndex].TargetJoint;
        }
        else
        {
            HeliPlayerData.Target = null;
        }
    }

    public void OnRemoveEnemy(Enemy enemy)
    {
        var enemyList = CurrentSubStage.EnemyList;
        enemyList.Remove(enemy);

        LeftEnemyCount = enemyList.Count;
        InvalidTarget();
    }

    public void SetNextEnemy()
    {
        CurrentEnemyIndex++;
        InvalidTarget();
    }

    public void SetZoom(bool zoom)
    {
        HeliPlayerData.Zoom = zoom;
        HeliPlayerData.ZoomStartTime = Time.time;
    }

    public void OnShoot()
    {

    }
}
