using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーの状態に応じてステートを管理する
public class PlayerStateController : MonoBehaviour
{
    public enum PlayerState
    {
        Move,                    //移動状態
        ViewportLocked,          //視点が固定されている状態
        Voyeurism,               //盗撮状態
        TalkEvent                //会話イベントが発生したときの状態
    }

    private PlayerState playerState;

    // Start is called before the first frame update
    void Start()
    {
        //インスタンス生成
        playerState = new PlayerState();
    }

    // Update is called once per frame
    void Update()
    {
        if(RaycastController.Lockon)
        {
            SetPlayerState(PlayerState.ViewportLocked);
        }
        else if(ChangeCameraAngle._voyeurism)
        {
            SetPlayerState(PlayerState.Voyeurism);
        }
        else if(UIController._talkStart)
        {
            SetPlayerState(PlayerState.TalkEvent);
        }
        else
        {
            SetPlayerState(PlayerState.Move);
        }
    }

    //セッター
    public void SetPlayerState(PlayerState state)
    {
        playerState = state;
    }

    //ゲッター
    public PlayerState GetPlayerState()
    {
        return playerState;
    }
}
