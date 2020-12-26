using System.Collections;
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

    private GameSceneState gameSceneState = null;

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

            gameSceneState = new GameSceneState()
            {
                TargetEnemyList = fistStage.EnemyList.ToList(),
                HeliPlayerData = new HeliPlayerData()
                {
                    Zoom = false,
                    ZoomStartTime = Time.time,
                    Target = firstTarget
                },
                SubStageId = fistStage.SubStageId,
                NextSubStageId = fistStage.NextSubStageId,
                MaxBullet = initData.MaxBullet,
                CurrentBullet = initData.MaxBullet,
                LeftEnemyCount = enemyCount,
                CurrentEnemyIndex = 0
            };
            fistStage.SetColliderActive(true);

            gameSceneState.InvalidTarget();
            heliPlayer.ApplyHeliPlayerData(gameSceneState.HeliPlayerData);
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
        cameraTrack.SetSubStage(1);
    }

    public override void OnDestroyScene()
    {
        gameSceneState = null;

        subStageList = null;
        heliPlayer = null;
        cameraTrack = null;

        Destroy(field.gameObject);
    }
    #endregion

    private IEnumerator ZoomProcess(Action onFinish)
    {
        heliPlayer.ApplyHeliPlayerData(gameSceneState.HeliPlayerData);
        view.ApplyGameSceneState(gameSceneState);

        onFinish();
        yield break;
    }

    public void OnClickSelectEnemy()
    {
        gameSceneState.SetNextEnemy();

        heliPlayer.ApplyHeliPlayerData(gameSceneState.HeliPlayerData);
    }

    public void OnClickZoomIn()
    {
        gameSceneState.SetZoom(true);

        SceneTransition.StartTransition(ZoomProcess, () => { });
    }

    public void OnClickZoomOut()
    {
        gameSceneState.SetZoom(false);
        gameSceneState.InvalidTarget();
        heliPlayer.ApplyHeliPlayerData(gameSceneState.HeliPlayerData);
        view.ApplyGameSceneState(gameSceneState);

        //SceneTransition.StartTransition(ZoomProcess, () => { });
    }

    public void OnClickShoot()
    {
        heliPlayer.Shoot();
    }

    public void OnKillEnemy(Enemy enemy)
    {
        gameSceneState.OnRemoveEnemy(enemy);
        gameSceneState.SetZoom(false);
        heliPlayer.ApplyHeliPlayerData(gameSceneState.HeliPlayerData);
        view.ApplyGameSceneState(gameSceneState);

        if (gameSceneState.TargetEnemyList.Count == 0)
        {
            int nextSubStageId = gameSceneState.NextSubStageId;
            cameraTrack.SetSubStage(nextSubStageId);

            int enemyCount = 0;
            var nextStage = subStageList.FirstOrDefault(s => s.SubStageId == nextSubStageId);
            Transform firstTarget = null;
            if (nextStage != null)
            {
                var firstEnemy = nextStage.EnemyList.FirstOrDefault();
                enemyCount = nextStage.EnemyList.Count;
                if (firstEnemy != null)
                {
                    firstTarget = firstEnemy.transform;
                    //
                }

                if (gameSceneState.SubStageId == nextStage.NextSubStageId)
                {
                    SceneManager.Inst.SwitchScene<TitleScene>(new TitleSceneInitData()
                    {
                        GameInstance = initData.GameInstance
                    });
                    return;
                }
            }

            gameSceneState = new GameSceneState()
            {
                TargetEnemyList = nextStage.EnemyList.ToList(),
                HeliPlayerData = new HeliPlayerData()
                {
                    Zoom = false,
                    ZoomStartTime = Time.time,
                    Target = firstTarget
                },
                SubStageId = nextStage.SubStageId,
                NextSubStageId = nextStage.NextSubStageId,
                MaxBullet = initData.MaxBullet,
                CurrentBullet = initData.MaxBullet,
                LeftEnemyCount = enemyCount,
                CurrentEnemyIndex = 0
            };
            nextStage.SetColliderActive(true);

            gameSceneState.InvalidTarget();
            heliPlayer.ApplyHeliPlayerData(gameSceneState.HeliPlayerData);
        }
    }

    public void OnRemoveEnemy(Enemy enemy)
    {

    }
}
