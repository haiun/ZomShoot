using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(StartUp());
    }

    IEnumerator StartUp()
    {
        while (UICamera.currentCamera == null)
            yield return new WaitForEndOfFrame();

        SceneManager.Inst.Initailze<TitleScene>(null);
        yield break;
    }
}
