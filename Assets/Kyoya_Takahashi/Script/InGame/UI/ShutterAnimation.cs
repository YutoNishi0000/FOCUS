using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ShutterAnimation : MonoBehaviour
{
    string addressOther = "Assets/Kyoya_Takahashi/Prefabs/Animation/OtherAnimation.prefab";
    string addressTarget = "Assets/Kyoya_Takahashi/Prefabs/Animation/TargetAnimation.prefab";
    string addressNone = "Assets/Kyoya_Takahashi/Prefabs/Animation/NoneAnimation.prefab";
    //0:other, 1:target, 2:other
    static GameObject[] Animation = new GameObject[3];
    private void Start()
    {
        Animation[0] = AssetDatabase.LoadAssetAtPath<GameObject>(addressOther);
        Animation[1] = AssetDatabase.LoadAssetAtPath<GameObject>(addressTarget);
        Animation[2] = AssetDatabase.LoadAssetAtPath<GameObject>(addressNone);
    }


    
    /// <summary>
    /// 異質なもの撮影アニメーション開始
    /// </summary>
    public static void OtherAnimationStart()
    {
        Debug.Log("異質");
        Instantiate(Animation[0]);
    }
    /// <summary>
    /// ターゲット撮影アニメーション開始
    /// </summary>
    public static void TargetAnimationStart()
    {
        Debug.Log("ターゲット");
        Instantiate(Animation[1]);
    }
    /// <summary>
    /// 何も撮影できなかったアニメーション開始
    /// </summary>
    public static void NoneAnimationStart()
    {
        Debug.Log("何も");
        Instantiate(Animation[2]);
    }
}