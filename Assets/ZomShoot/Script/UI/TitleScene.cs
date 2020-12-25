using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneInitData : ISceneInitData
{
    public int StageId = 0;
}

[PrefabPath("UI/TitleScene")]
public class TitleScene : SceneBase
{
    [SerializeField]
    private TitleSceneView view = null;

    public override void OnInitializeScene(ISceneInitData initData)
    {

    }

    public override void OnDestroyScene()
    {

    }

    public void OnClickGameStart()
    {
        SceneManager.Inst.SwitchScene<GameScene>(null);
    }
}
