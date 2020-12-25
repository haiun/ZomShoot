using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliPlayerData
{
    public bool Zoom = false;
    public float ZoomStartTime = 0f;
    public Transform Target = null;
}

[PrefabPath("Game/HeliPlayer")]
public class HeliPlayer : MonoBehaviour 
{
    [SerializeField]
    private Camera normalCamera = null;

    [SerializeField]
    private Camera zoomCamera = null;

    [SerializeField]
    private Animator humanAni = null;

    private HeliPlayerData heliPlayerData = null;

    public void ApplyHeliPlayerData(HeliPlayerData data)
    {
        heliPlayerData = data;

        bool zoom = data == null ? false : data.Zoom;
        normalCamera.enabled = !zoom;
        zoomCamera.enabled = zoom;

        bool aim = data == null ? false : data.Target != null;
        humanAni.SetFloat("WeaponType_int", aim ? 5 : 0);
    }

    public void LateUpdate()
    {
        if (heliPlayerData == null)
        {
            return;
        }

        var target = heliPlayerData.Target;
        if (target == null)
        {
            return;
        }

        var localPosition = humanAni.transform.worldToLocalMatrix.MultiplyPoint3x4(target.position);
        var distanceVector = localPosition;

        {
            var ignoreYForward = Vector3.forward;
            var ignoreYVector = distanceVector;
            ignoreYForward.y = ignoreYVector.y = 0;

            ignoreYForward = Vector3.Normalize(ignoreYForward);
            ignoreYVector = Vector3.Normalize(ignoreYVector);

            var cross = Vector3.Cross(ignoreYForward, ignoreYVector);
            var sin = cross.magnitude;
            var degree = Mathf.Asin(sin) * (cross.y >= 0 ? 1 : -1);

            humanAni.SetFloat("Body_Horizontal_f", degree);
        }

        {
            var ignoreXForward = Vector3.forward;
            var ignoreXVector = distanceVector;
            ignoreXForward.x = ignoreXVector.x = 0;

            ignoreXForward = Vector3.Normalize(ignoreXForward);
            ignoreXVector = Vector3.Normalize(ignoreXVector);

            var cross = Vector3.Cross(ignoreXForward, ignoreXVector);
            var sin = cross.magnitude;
            var degree = Mathf.Asin(sin) * (cross.x >= 0 ? -1 : 1);

            humanAni.SetFloat("Body_Vertical_f", degree);
        }
    }
}
