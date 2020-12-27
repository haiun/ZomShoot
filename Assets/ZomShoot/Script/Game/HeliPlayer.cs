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

    [SerializeField]
    private Transform muzzle = null;

    [SerializeField]
    private GameObject BulletPrefab = null;

    private HeliPlayerData heliPlayerData = null;

    private bool muzzleTracking = true;

    public void ApplyHeliPlayerData(HeliPlayerData data)
    {
        heliPlayerData = data;

        bool zoom = data == null ? false : data.Zoom;
        normalCamera.enabled = !zoom;
        zoomCamera.enabled = zoom;

        bool aim = data == null ? false : data.Target != null;
        humanAni.SetFloat("WeaponType_int", (zoom && aim) ? 5 : 0);
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

        if (muzzleTracking)
            muzzle.LookAt(target);
    }

    public void Shoot()
    {
        StartCoroutine(Fire());
    }

    IEnumerator Fire()
    {
        if (humanAni == null)
        {
            var go = GameObject.Instantiate(BulletPrefab) as GameObject;
            go.transform.position = muzzle.position;
            go.transform.LookAt(heliPlayerData.Target);
            yield break;
        }

        if (!muzzleTracking)
            yield break;

        muzzleTracking = false;

        humanAni.SetBool("Shoot_b", true);
        GameInstance.Inst?.PlaySfx(SfxEnum.Shoot);

        yield return new WaitForSeconds(0.16f);

        {
            var go = GameObject.Instantiate(BulletPrefab) as GameObject;
            go.transform.position = muzzle.position;
            go.transform.LookAt(heliPlayerData.Target);
        }

        yield return new WaitForSeconds(0.5f);
        GameInstance.Inst?.PlaySfx(SfxEnum.Reload);
        yield return new WaitForSeconds(0.5f);

        humanAni.SetBool("Shoot_b", false);

        float fixAimFinish = Time.time + 1f;
        float leftFixAimTime = fixAimFinish - Time.time;
        while (leftFixAimTime >= 0)
        {
            var ratio = Mathf.Clamp01(leftFixAimTime);
            var currentRot = muzzle.rotation;
            muzzle.LookAt(heliPlayerData.Target);
            var targetRot = muzzle.rotation;

            muzzle.rotation = Quaternion.Lerp(targetRot, currentRot, ratio);
            yield return new WaitForEndOfFrame();
            leftFixAimTime = fixAimFinish - Time.time;
        }
        muzzle.LookAt(heliPlayerData.Target);

        GameInstance.Inst?.PlaySfx(SfxEnum.Mag);

        muzzleTracking = true;
    }
}
