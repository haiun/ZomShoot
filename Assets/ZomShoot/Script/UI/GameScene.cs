﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[PrefabPath("UI/GameScene")]
public class GameScene : SceneBase
{
    [SerializeField]
    private GameSceneView view = null;

    private GameSceneInitData initData = null;

    private Field field = null;

    private CameraTrack cameraTrack = null;
    private HeliPlayer heliPlayer = null;
    private List<SubStage> subStageList = null;

    private GameSceneState sameSceneState = null;

    #region SceneBase
    public override void OnInitializeScene(ISceneInitData _initData)
    {
        initData = (GameSceneInitData)_initData;

        GameInstance.Inst.PlayBGM(BgmEnum.Title, false);
        GameInstance.Inst.PlayBGM(BgmEnum.Game, true);
        GameInstance.Inst.PlayBGM(BgmEnum.GameAmbient, true);

        field = GenericPrefab.InstantiatePathFormat<Field>(initData.FieldId.ToString());
        cameraTrack = field.GetCameraTrack();
        heliPlayer = GenericPrefab.Instantiate<HeliPlayer>(cameraTrack.GetCameraJoint());
        subStageList = field.GetSubStageList();

        //GameSceneState
        {
            int enemyCount = 0;
            var fistStage = subStageList.FirstOrDefault();
            Transform firstTarget = null;
            if (fistStage != null)
            {
                var firstEnemy = fistStage.EnemyList.FirstOrDefault();
                enemyCount = fistStage.EnemyList.Count;
                if (firstEnemy != null)
                {
                    firstTarget = firstEnemy.transform;
                }
            }

            sameSceneState = new GameSceneState()
            {
                CurrentSubStage = fistStage,
                HeliPlayerData = new HeliPlayerData()
                {
                    Zoom = false,
                    ZoomStartTime = Time.time,
                    Target = firstTarget
                },
                MaxBullet = initData.MaxBullet,
                CurrentBullet = initData.MaxBullet,
                LeftEnemyCount = enemyCount,
                CurrentEnemyIndex = 0
            };
            sameSceneState.InvalidTarget();
            heliPlayer.ApplyHeliPlayerData(sameSceneState.HeliPlayerData);
        }

        //init enemy
        {
            var enemyInitData = new EnemyInitData()
            {
                OnKillEnemy = OnKillEnemy,
                OnRemoveEnemy = OnRemoveEnemy
            };

            foreach (var subStage in subStageList)
            {
                foreach (var enemy in subStage.EnemyList)
                {
                    enemy.Initialize(enemyInitData);
                }
            }
        }

        //Init CameraTrack
        cameraTrack.PlayNext(1);
    }

    public override void OnDestroyScene()
    {
        sameSceneState = null;

        subStageList = null;
        heliPlayer = null;
        cameraTrack = null;

        Destroy(field.gameObject);
    }
    #endregion

    private IEnumerator ZoomProcess(Action onFinish)
    {
        heliPlayer.ApplyHeliPlayerData(sameSceneState.HeliPlayerData);
        view.ApplyGameSceneState(sameSceneState);

        onFinish();
        yield break;
    }

    public void OnClickSelectEnemy()
    {
        sameSceneState.SetNextEnemy();

        heliPlayer.ApplyHeliPlayerData(sameSceneState.HeliPlayerData);
    }

    public void OnClickZoomIn()
    {
        sameSceneState.SetZoom(true);
        SceneTransition.StartTransition(ZoomProcess, () => { });
    }

    public void OnClickZoomOut()
    {
        sameSceneState.SetZoom(false);
        sameSceneState.InvalidTarget();
        heliPlayer.ApplyHeliPlayerData(sameSceneState.HeliPlayerData);
        view.ApplyGameSceneState(sameSceneState);

        //SceneTransition.StartTransition(ZoomProcess, () => { });
    }

    public void OnClickShoot()
    {
        heliPlayer.Shoot();
    }

    public void OnKillEnemy(Enemy enemy)
    {
        sameSceneState.OnRemoveEnemy(enemy);
        sameSceneState.SetZoom(false);
        heliPlayer.ApplyHeliPlayerData(sameSceneState.HeliPlayerData);
        view.ApplyGameSceneState(sameSceneState);
    }

    public void OnRemoveEnemy(Enemy enemy)
    {

    }
}
