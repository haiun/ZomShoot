using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

[Serializable]
public class GameSceneView
{
    public List<GameObject> NormalViewObject = null;
    public List<GameObject> ZoomViewObject = null;

    public List<UIButton> ShootingDelayButton = null;

    public UILabel Time = null;
    public UILabel LeftTargetCount = null;

    private StringBuilder timeStringBuilder = new StringBuilder();

    public void ApplyGameSceneState(GameSceneState state)
    {
        var data = state.HeliPlayerData;
        bool zoom = data == null ? false : data.Zoom;
        NormalViewObject.ForEach(o => o.SetActive(!zoom));
        ZoomViewObject.ForEach(o => o.SetActive(zoom));


        LeftTargetCount.text = state.HeliPlayerData.Target == null ? "0" : state.LeftEnemyCount.ToString();
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

    public void ApplyShootingDelayButton(bool active)
    {
        ShootingDelayButton.ForEach(b => b.isEnabled = active);
    }
}

