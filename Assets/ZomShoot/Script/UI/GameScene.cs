using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneInitData : ISceneInitData
{
    public GameInstance GameInstance = null;
    public int FieldId = 0;
}

[PrefabPath("UI/GameScene")]
public class GameScene : SceneBase
{
    [SerializeField]
    private GameSceneView view = null;

    private Field field = null;

    private CameraTrack cameraTrack = null;
    private HeliPlayer heliPlayer = null;
    private SubStage subStage = null;

    #region SceneBase
    public override void OnInitializeScene(ISceneInitData initData)
    {
        {
            //initData.FieldId
            var prefab = Resources.Load("Field/1");
            var go = GameObject.Instantiate(prefab) as GameObject;
            field = go.GetComponent<Field>();

            cameraTrack = field.GetCameraTrack();
            heliPlayer = GenericPrefab.Instantiate<HeliPlayer>(cameraTrack.GetCameraJoint());
        }
    }

    public override void OnDestroyScene()
    {
        cameraTrack = null;
        heliPlayer = null;
        subStage = null;

        Destroy(field.gameObject);
    }
    #endregion

    //Select Enemy
    //Zoom In
    //Zoom Out
    //Fire
    //Kill Enemy
}
