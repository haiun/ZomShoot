using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInitData
{
    public Action<Enemy> OnKillEnemy = null;
}

public class Enemy : MonoBehaviour
{
    private Animator ani = null;
    private Collider targetCollider = null;
    private Rigidbody[] rigidbodyList = null;

    private EnemyInitData initData = null;

    public void Initialize(EnemyInitData initData)
    {
        this.initData = initData;

        ani = GetComponent<Animator>();
        targetCollider = GetComponent<Collider>();
        rigidbodyList = GetComponentsInChildren<Rigidbody>();
        ToggleRagdoll(true);
    }

    private void ToggleRagdoll(bool isAnimating)
    {
        ani.enabled = isAnimating;

        targetCollider.enabled = isAnimating;

        foreach (var rigidbody in rigidbodyList)
        {
            rigidbody.isKinematic = isAnimating;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            //kill precess
            ToggleRagdoll(false);

            initData.OnKillEnemy?.Invoke(this);
        }
    }
}
