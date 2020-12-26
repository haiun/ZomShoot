using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GameSceneView
{
    public List<GameObject> NormalViewObject = null;
    public List<GameObject> ZoomViewObject = null;

    public void ApplyGameSceneState(GameSceneState state)
    {
        NormalViewObject.ForEach(o => o.SetActive(!state.HeliPlayerData.Zoom));
        ZoomViewObject.ForEach(o => o.SetActive(state.HeliPlayerData.Zoom));

    }
}

