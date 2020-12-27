﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

[Serializable]
public class GameSceneView
{
    public List<GameObject> NormalViewObject = null;
    public List<GameObject> ZoomViewObject = null;

    public UILabel Time = null;
    public UILabel LeftTargetCount = null;

    private StringBuilder timeStringBuilder = new StringBuilder();

    public void ApplyGameSceneState(GameSceneState state)
    {
        NormalViewObject.ForEach(o => o.SetActive(!state.HeliPlayerData.Zoom));
        ZoomViewObject.ForEach(o => o.SetActive(state.HeliPlayerData.Zoom));

        LeftTargetCount.text = state.LeftEnemyCount.ToString();
    }

    public void ApplyTime(float seconds)
    {
        int sec = (int)seconds;
        int millisec = (int)((seconds - sec) * 100);

        int ret;
        int div = Math.DivRem(sec, 60, out ret);

        timeStringBuilder.Length = 0;
        timeStringBuilder.AppendFormat("{0:00}:{1:00}:{2:00}", div, ret, millisec);
        Time.text = timeStringBuilder.ToString();
    }
}

