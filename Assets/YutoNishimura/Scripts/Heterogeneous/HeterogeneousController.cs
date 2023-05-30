using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//異質なもの自身にアタッチするもの
[RequireComponent(typeof(MeshRenderer))]
public class HeterogeneousController : Actor
{
    private Material material;
    private readonly float destroyTime = 1.0f;
    private float dethtime;
    public bool takenPicFlag;       //写真を撮られたかどうか
    public bool enableTakePicFlag;  //サブカメラで写真を撮ることが可能かどうかを表すフラグ

    void Start()
    {
        enableTakePicFlag = false;
        takenPicFlag = false;
        material = GetComponent<Material>();
        dethtime = destroyTime;
    }

    void Update()
    {
        DestroyHeterogeneous();
    }

    //自身がカメラに写っていた場合にだけ呼び出される
    void OnWillRenderObject()
    {
        //メインカメラから見えたときだけ処理を行う
        if (Camera.current.name == "Main Camera")
        {
            //Debug.Log("サブカメラ処理が行われています");

            Vector3 strangeObjVec = transform.position - playerInstance.transform.position;
            Vector3 playerForwardVec = playerInstance.transform.forward;

            float angle = Vector3.Angle(playerForwardVec, strangeObjVec);

            //判定距離(後で上に移動させる)
            const float enableSeeDis = 7.0f;

            float judgeDis = strangeObjVec.magnitude * Mathf.Cos((angle / 360) * Mathf.PI * 2);

            if (judgeDis <= enableSeeDis)
            {
                enableTakePicFlag = true;
            }
            else
            {
                enableTakePicFlag = false;
            }
        }
    }

    //消すとき、α値を減少させながら消滅させる
    //フラグを用いてこの関数を呼び出せばよい
    private void DestroyHeterogeneous()
    {
        if(!takenPicFlag)
        {
            return;
        }

        //アルファ値が０以下になったら自身を削除
        if (dethtime < 0)
        {
            //フィールドに足りない異質なものを補うときにクールタイム発生
            HeterogeneousSetter.CoolTime();
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        dethtime -= Time.deltaTime;
    }

    public void SetEnableTakePicFlag(bool flag) { enableTakePicFlag = flag; }

    public bool GetEnableTakePicFlag() { return enableTakePicFlag; }

    public void SetTakenPicFlag(bool flag) { takenPicFlag = flag; }

    public bool GetTakenPicFlag() { return takenPicFlag; }
}