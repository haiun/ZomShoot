using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneInitData : ISceneInitData
{
    public GameInstance GameInstance = null;
}

[PrefabPath("UI/TitleScene")]
public class TitleScene : SceneBase
{
    [SerializeField]
    private TitleSceneView view = null;

    private TitleSceneInitData initData = null;

    public override void OnInitializeScene(ISceneInitData initData)
    {
        this.initData = initData as TitleSceneInitData;

        GameInstance.Inst.PlayBGM(BgmEnum.Title, true);
        GameInstance.Inst.PlayBGM(BgmEnum.Game, false);
        GameInstance.Inst.PlayBGM(BgmEnum.GameAmbient, false);
    }

    public override void OnDestroyScene()
    {

    }

    public void OnClickGameStart()
    {
        SceneManager.Inst.SwitchScene<GameScene>(new GameSceneInitData()
        {
            GameInstance = initData.GameInstance
        });
    }
}
