using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[PrefabPath("UI/GameScene")]
public class GameScene : SceneBase
{
    [SerializeField]
    private GameSceneView view = null;

    public override void OnInitializeScene(ISceneInitData initData)
    {

    }

    public override void OnDestroyScene()
    {

    }
}
