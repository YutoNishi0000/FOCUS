using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class UniTaskController : MonoBehaviour
{
    /// <summary>
    /// UniTaskのキャンセル方法
    /// </summary>
    protected enum UniTaskCancellMode
    {
        Auto,         //自動
        Manual        //手動
    }

    protected CancellationTokenSource cts;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    protected void Awake()
    {
        cts = new CancellationTokenSource();
    }

    /// <summary>
    /// UniTaskを用いて独自のフレームワークを持つ関数（マルチスレッドでの実行が可能）
    /// </summary>
    /// <param name="start">この関数が呼ばれた瞬間呼び出す関数</param>
    /// <param name="update">毎フレーム呼び出す関数</param>
    /// <param name="unlockUpdate">このループを抜け出す出す条件</param>
    /// <param name="token">キャンセルトークン</param>
    /// <param name="mode">UniTaskのキャンセル方法</param>
    /// <returns></returns>
    protected async UniTask UniTaskUpdate(UnityAction start, UnityAction update, Func<bool> unlockFunc, CancellationToken token, UniTaskCancellMode mode)
    {
        //キャンセルトークンをセット
        SetCancellToken(ref token, mode);

        //UnityEventのインスタンスを生成
        UnityEvent UniTaskExecute = new UnityEvent();

        //startで受け渡された関数を実行
        UniTaskExecute?.AddListener(start);
        UniTaskExecute?.Invoke();
        //関数をリセット
        UniTaskExecute?.RemoveAllListeners();
        //引数で渡されたupdateを実行
        UniTaskExecute?.AddListener(update);

        while (true)
        {
            //UniTaskExecuteがnullなければ実行する
            UniTaskExecute?.Invoke();

            if(unlockFunc.Invoke() && unlockFunc != null)
            {
                Debug.Log("抜け出す");
                //関数をリセット
                UniTaskExecute?.RemoveAllListeners();
                //Unitaskを強制的に中止
                CancelUniTask();
                break;
            }

            //１フレーム待つ
            await UniTask.DelayFrame(1, PlayerLoopTiming.Update);
        }
    }

    /// <summary>
    /// キャンセルトークンにインスタンスをキャンセル方法に応じて設定
    /// </summary>
    /// <param name="token"></param>
    /// <param name="mode"></param>
    private void SetCancellToken(ref CancellationToken token, UniTaskCancellMode mode)
    {
        switch(mode)
        {
            case UniTaskCancellMode.Auto:
                token = this.GetCancellationTokenOnDestroy();
                break;
            case UniTaskCancellMode.Manual:
                token = cts.Token;
                break;
        }
    }

    /// <summary>
    /// Unitaskを強制的にキャンセルする関数
    /// </summary>
    protected void CancelUniTask()
    {
        cts.Cancel();
    }

    /// <summary>
    /// 仮想関数(Update)
    /// </summary>
    public virtual void UpdateUniTask(){ }

    /// <summary>
    /// 仮想関数(Start)
    /// </summary>
    public virtual void StartUniTask() { }
}
