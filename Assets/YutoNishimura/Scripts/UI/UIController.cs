using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using UnityEditor;


//UIの基底クラス
//このクラスは特に継承必要がないとき、UI自身にアタッチするものとする
public class UIController : MonoBehaviour
{
    //trueだとシーン切り替えをしない
    public bool blockSwithScene = true;
    //シーン切り替え時のアニメーションプレハブのアドレス
    private string address = "Assets/Kyoya_Takahashi/Prefabs/OutGame/Animation/SwichAnimationEnd.prefab";
    //シーン切り替え時のアニメーションプレハブ
    public GameObject endAnimation = null;
    private void Awake()
    {
        endAnimation = AssetDatabase.LoadAssetAtPath<GameObject>(address);
    }
    //シーン移動
    public void MoveScene(string scene)
    {
        blockSwithScene = true;
        SceneManager.LoadScene(scene); 
    }
    public void PlaySE()
    {
        //サウンドを鳴らす
    }
    public void UnBlockSwithScene()
    {
        blockSwithScene = false;
    }
}
