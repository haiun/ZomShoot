using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance : MonoBehaviour
{
    [SerializeField]
    private AudioSource titleBGM = null;
    [SerializeField]
    private AudioSource gameBGM = null;
    [SerializeField]
    private AudioSource sfxSource = null;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(StartUp());
    }

    IEnumerator StartUp()
    {
        while (UICamera.currentCamera == null)
            yield return new WaitForEndOfFrame();

        SceneManager.Inst.Initailze<TitleScene>(new TitleSceneInitData()
        {
            GameInstance = this
        });
        yield break;
    }
}
