using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//シャッターチャンスかどうかを管理するクラス
public class ShutterChanceController : MonoBehaviour
{
    public static bool _shutterChance;      //シャッターチャンスかどうかを管理するフラグ

    // Start is called before the first frame update
    void Start()
    {
        _shutterChance = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(_shutterChance)
        {
            Debug.Log("シャッターチャンス");
        }
        else
        {
            Debug.Log("のっとシャッターチャンス");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //もしもシャッターチャンスエリアにターゲットが進入したら
        if(other.gameObject.CompareTag("ShutterChanceArea"))
        {
            //フラグをオンに
            _shutterChance = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //もしもシャッターチャンスエリアからターゲットが出たら
        if (other.gameObject.CompareTag("ShutterChanceArea"))
        {
            //フラグをオフに
            _shutterChance = false;
        }
    }
}
