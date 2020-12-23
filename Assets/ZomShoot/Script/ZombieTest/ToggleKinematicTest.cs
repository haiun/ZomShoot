using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleKinematicTest : MonoBehaviour
{
    public Rigidbody rigidbody = null;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            rigidbody.isKinematic = false;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            rigidbody.AddForce(transform.forward * 100f, ForceMode.Acceleration);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false);
    }
}
