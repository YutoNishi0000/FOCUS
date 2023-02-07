using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMove : MonoBehaviour
{
    [SerializeField] string SceneName;
    public void LoadScene()
    {
        //指定秒数待ってから指定したsceneに移動
        FadeManager.Instance.LoadScene(SceneName, 2.0f);
    }
}
