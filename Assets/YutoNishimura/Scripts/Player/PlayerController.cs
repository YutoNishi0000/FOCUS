using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : Human
{
    Vector3 moveDirection = Vector3.zero;
    Vector3 targetDirection;        //移動する方向のベクトル
    private CharacterController controller;
    [SerializeField] private float speed = 3f;
    private PlayerStateController playerState;

    public GameObject cam;
    Quaternion cameraRot, characterRot;
    float Xsensityvity = 3f, Ysensityvity = 3f;

    bool cursorLock = true;

    public GameObject targetCenter;

    //変数の宣言(角度の制限用)
    float minX = -40f, maxX = 40f;
    Vector3 initialCamPos;

    private void Start()
    {
        cameraRot = cam.transform.localRotation;
        characterRot = transform.localRotation;
        initialCamPos = cam.transform.localPosition;
        controller = GetComponent<CharacterController>();
        playerState = GetComponent<PlayerStateController>();
    }

    private void Update()
    {
        switch (playerState.GetPlayerState())
        {
            case PlayerStateController.PlayerState.Move:
                RotateControl();
                break;
            case PlayerStateController.PlayerState.ViewportLocked:
                lockOnTargetObject(targetCenter);
                break;
        }
    }

    private void FixedUpdate()
    {
        MoveControl();
    }

    public override void MoveControl()
    {
        //キーボード入力を取得
        float v = Input.GetAxisRaw("Vertical");         //InputManagerの↑↓の入力       
        float h = Input.GetAxisRaw("Horizontal");       //InputManagerの←→の入力 

        //カメラの正面方向ベクトルからY成分を除き、正規化してキャラが走る方向を取得
        Vector3 forward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 right = Camera.main.transform.right; //カメラの右方向を取得

        //カメラの方向を考慮したキャラの進行方向を計算
        targetDirection = h * right + v * forward;

        AnimatioControl(v, h);

        //進行方向から正規化されたベクトルを取得
        moveDirection = Vector3.Scale(targetDirection, new Vector3(1, 0, 1)).normalized;

        //スピードを掛け合わせる
        moveDirection *= speed;

        //最終的な移動処理
        controller.Move(moveDirection * Time.deltaTime);
    }

    public void AnimatioControl(float vertical, float horizon)
    {
        if (vertical > .1 || vertical < -.1 || horizon > .1 || horizon < -.1) //(移動入力があると)
        {
            cam.transform.localPosition = new Vector3(initialCamPos.x, initialCamPos.y + Mathf.PingPong(Time.time / 2, 0.2f), initialCamPos.z);
        }
        else
        {
            cam.transform.localPosition = initialCamPos;
        }
    }

    void RotateControl()
    {
        float xRot = Input.GetAxis("Mouse X") * Ysensityvity;
        float yRot = Input.GetAxis("Mouse Y") * Xsensityvity;

        cameraRot *= Quaternion.Euler(-yRot, 0, 0);
        characterRot *= Quaternion.Euler(0, xRot, 0);

        //Updateの中で作成した関数を呼ぶ
        cameraRot = ClampRotation(cameraRot);

        cam.transform.localRotation = cameraRot;
        transform.localRotation = characterRot;

        //UpdateCursorLock();
    }

    public void UpdateCursorLock()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cursorLock = false;
        }
        else if (Input.GetMouseButton(0))
        {
            cursorLock = true;
        }


        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (!cursorLock)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    //角度制限関数の作成
    public Quaternion ClampRotation(Quaternion q)
    {
        //q = x,y,z,w (x,y,zはベクトル（量と向き）：wはスカラー（座標とは無関係の量）)

        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1f;

        float angleX = Mathf.Atan(q.x) * Mathf.Rad2Deg * 2f;

        angleX = Mathf.Clamp(angleX, minX, maxX);

        q.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);

        return q;
    }

    private void lockOnTargetObject(GameObject target)
    {
        transform.LookAt(target.transform.position);
    }
}



//人型クラスの基底クラス
public class Human : MonoBehaviour
{
    //プレイヤーのインスタンス
    protected PlayerController playerInstance;

    private void Awake()
    {
        //ゲーム開始時プレイヤーインスタンスを取得
        playerInstance = FindObjectOfType<PlayerController>();
    }

    public virtual void MoveControl() { }
    public virtual void AnimationControl() { }

    /// <summary>
    /// 指定したゲームオブジェクトを指定した方向に向かせる
    /// </summary>
    /// <param name="gameObject">　指定した方向に向かせたいゲームオブジェクト </param>
    /// <param name="direction"> ゲームオブジェクトを向かせたい方向 </param>
    public void CorrectRotation(GameObject gameObject, Vector3 direction)
    {
        gameObject.transform.DORotate(direction, 0.5f);
    }
}
