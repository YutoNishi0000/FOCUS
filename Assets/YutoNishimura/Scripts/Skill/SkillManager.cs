using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//スキルに関する関数

//JudgeScoreコンポーネントを動的に追加したい <- ゲームシーンに移動したとき
public class SkillManager : Actor
{
    private bool addScoreFlag;         //スコア加算フラグ
    private bool targetMinimapFlag;     //ターゲットのミニマップを表示するかどうか
    private bool skillBlock_player;
    private bool skillBlock_addScore;
    private bool skillBlock_seeTarget;
    private float time;
    [SerializeField] private readonly float interval = 5.0f;
    private int shutterTimeStamp;
    private bool minimapSkillFlag;         //ターゲットのミニマップの表示フラグのために使う

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        targetMinimapFlag = false;
        skillBlock_player = true;
        skillBlock_addScore = true;
        skillBlock_seeTarget = true;
        addScoreFlag = false;
        minimapSkillFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        UnLockSkill();
        SkillImposition();
    }

    /// <summary>
    /// スキルロック解除
    /// </summary>
    private void UnLockSkill()
    {
        switch(GameManager.Instance.numSubShutter)
        {
            case 15:
                skillBlock_player = false;
                break;
            case 25:
                skillBlock_addScore = false;
                break;
            case 40:
                skillBlock_seeTarget = false;
                break;
        }
    }

    /// <summary>
    /// スキル発動関数
    /// </summary>
    public void SkillImposition()
    {
        if(skillBlock_player)
        {
            return;
        }

        PlayerSpeedUp();

        if(skillBlock_addScore)
        {
            return;
        }

        AddScore();

        if(skillBlock_seeTarget)
        {
            return;
        }

        TargetMinimapActivation();
    }

    /// <summary>
    /// スコア加算
    /// </summary>
    private void AddScore()
    {
        SetAddScoreFlag(true);
    }

    /// <summary>
    /// ターゲットのミニマップを表示する
    /// </summary>
    private void TargetMinimapActivation()
    {
        int count = GameManager.Instance.numSubShutter - 40;

        if(count % 10 == 0 && !minimapSkillFlag)
        {
            minimapSkillFlag = true;
        }

        if(!minimapSkillFlag)
        {
            return;
        }

        time += Time.deltaTime;

        if(time <= interval)
        {
            SetTargetMinimapFlag(true);
        }
        else if((time > interval) && (time <= interval + 10))
        {
            SetTargetMinimapFlag(false);
        }
        else
        {
            minimapSkillFlag = false; 
        }
    }

    /// <summary>
    /// プレイヤーの移動速度アップ
    /// </summary>
    private void PlayerSpeedUp()
    {
        playerInstance.SetPlayerSpeed(playerInstance.GetPlayerSpeed() * 1.5f);
    }

    //ターゲットのミニマップを表示するかどうか
    public void SetTargetMinimapFlag(bool flag) { targetMinimapFlag = flag; }

    public bool GetTargetMinimapFlag() { return targetMinimapFlag; }

    //スコア加算フラグ
    public void SetAddScoreFlag(bool flag) { addScoreFlag = flag; }

    public bool GetAddScoreFlag() { return addScoreFlag; }

}
