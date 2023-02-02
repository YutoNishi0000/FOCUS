using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// 次の日に行く
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="feeling"></param>
    public void NextDay(string sceneName, int feeling)
    {

        SceneManager.LoadScene(sceneName);
    }
}
