using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartBeat : MonoBehaviour
{
    //大きくなるスピード
    private float largeSpeed = 0.01f;
    //小さくなるスピード
    private float smallSpeed = 0.01f;
    //止まる時間
    private float stopTime = 0.01f;
    //止まる時間の経過時間
    private float time;
    //最小の大きさ
    private Vector3 minSize = new Vector3(1.0f, 1.0f, 1.0f);
    //最大の大きさ
    private Vector3 maxSize = new Vector3(2.0f, 2.0f, 2.0f);
    private enum STATE
    {
        large,
        stop,
        small,
    }
    private STATE state;

    private void Update()
    {
        BeatUpdate();
        FastBeat();
    }

    /// <summary>
    /// UIを拍動させる
    /// </summary>
    void BeatUpdate()
    {
        //大きくなって少し止まって小さくなる
        switch (state)
        {
            case STATE.large:
                LargeUpdate();
                break;
            case STATE.stop:
                StopUpdate();
                break;
            case STATE.small:
                SmallUpdate();
                break;
        }
    }

    /// <summary>
    /// 大きくする
    /// </summary>
    void LargeUpdate()
    {
        this.transform.localScale += new Vector3(largeSpeed, largeSpeed, largeSpeed);
        if (this.transform.localScale.x > maxSize.x)
        {
            state = STATE.stop;
            time = Time.time;
        }
    }
    /// <summary>
    /// サイズ変更を止める
    /// </summary>
    void StopUpdate()
    {
        if (Time.time - time > stopTime)
        {
            state = STATE.small;
        }
    }
    /// <summary>
    /// 小さくする
    /// </summary>
    void SmallUpdate()
    {
        this.transform.localScale -= new Vector3(smallSpeed, smallSpeed, smallSpeed);
        if (this.transform.localScale.x < minSize.x)
        {
            state = STATE.large;
        }
    }
    /// <summary>
    /// 拍動を早くする
    /// </summary>
    void FastBeat()
    {
        largeSpeed = 0.01f + ((RayTest.lockonTime / 3.0f) / 100);
        smallSpeed = 0.01f + ((RayTest.lockonTime / 3.0f) / 100);
        stopTime = 0.1f - (((RayTest.lockonTime * 5.0f) / 3.0f) / 100);
    }
}
