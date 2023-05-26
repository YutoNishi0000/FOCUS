using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using DG.Tweening;

public class ScreenShot : MonoBehaviour
{
    //入力が必要なもの
    [SerializeField] private RawImage targetImage;     //テクスチャを表示するためのRawImage
    [SerializeField] private Image point1;             //スクショした画像の１番目の移動先
    [SerializeField] private Image point2;             //スクショした画像の２番目の移動先

    //内部処理で使うもの
    private Camera cam;                                //プレイヤーのカメラ
    private string screenShotPath;                     //スクリーンショットして生成されたテクスチャのファイルパス
    private string timeStamp;                          //現在時刻を表すためのもの
    private const float firstScale = 0.8f;             //一回目移動するときにどれだけRawImnageが縮小されるか（何倍の大きさになるか）
    private const float secondScale = 0.2f;            //二回目縮小するときにどれだけRawImageが縮小されるか（何倍の大きさになるか）
    private Vector3 InitialPrevPos;                    //RawImageの初期位置
    private Vector3 InitialPrevscale;                  //RawImageの初期スケール
    private List<GameObject> setterObj;                //毎フレーム送られてくる異質なものの情報を取得するためのもの
    private List<int> destroyStrangeList;
    [SerializeField]private GameObject mimic = null;    //対象のモデル
    private bool noneStrangeFlag;
    private bool noneTargetFlag;
    private Player player;

    public static int[] abc = new int[10];

    void Awake()
    {
        setterObj = new List<GameObject>();
        destroyStrangeList = new List<int>();
        InitialPrevPos = targetImage.rectTransform.position;
        InitialPrevscale = targetImage.rectTransform.localScale;
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        targetImage.enabled = false;
        noneStrangeFlag = true;
        noneTargetFlag = true;
        player = GameObject.FindObjectOfType<Player>();
    }

    private void Update()
    {
        if(Shutter.isFilming)
        {
            //フラグを初期化
            noneTargetFlag = true;
            noneStrangeFlag = true;

            Instantiate(mimic,
                new Vector3(
                RespawTarget.GetCurrentTargetObj().transform.position.x,
                RespawTarget.GetCurrentTargetObj().transform.position.y,
                RespawTarget.GetCurrentTargetObj().transform.position.z),
                Quaternion.Euler(
                RespawTarget.GetCurrentTargetObj().transform.rotation.x,
                RespawTarget.GetCurrentTargetObj().transform.rotation.y,
                RespawTarget.GetCurrentTargetObj().transform.rotation.z));
            InitializeRawImage();
            ClickShootButton();
            Invoke(nameof(FirstMovePreview), 1f);
            //GameManager.Instance.IsPhoto = true;

            //サブカメラ撮影判定がオンだったときの判定
            for (int i = 0; i < setterObj.Count; i++)
            {
                if (setterObj[i] != null && setterObj[i].GetComponent<HeterogeneousController>().GetEnableTakePicFlag())
                {
                    noneStrangeFlag = false;
                    //サブカメラカウントをインクリメント
                    GameManager.Instance.numSubShutter++;
                    Debug.Log("1");
                    //スコアを加算
                    ScoreManger.Score += 10;
                    Debug.Log("2");
                    //tempList[i]のオブジェクトの消滅フラグをオンにする
                    setterObj[i].GetComponent<HeterogeneousController>().SetTakenPicFlag(true);
                    Debug.Log("処理完了");
                    //リストにこの配列のインデックスを追加
                    //destroyStrangeList.Add(i);
                }
            }

            //空撮り（異質なもの、ターゲットが撮影されていない）していたら
            if (noneTargetFlag && noneStrangeFlag && Shutter.isFilming)
            {
                Debug.Log("時間を失いました");
                CountDownTimer.DecreaceTime();
                player.Shake(1.5f, 1);
            }
        }
    }

    private void InitializeRawImage()
    {
        targetImage.texture = null;
        targetImage.enabled = false;
        targetImage.rectTransform.position = InitialPrevPos;
        targetImage.rectTransform.localScale = InitialPrevscale;
    }

    private string GetScreenShotPath()
    {
        //string path = "Assets/Pictures/" + timeStamp + ".png";
        string path = GameManager.Instance.GetPicturesFilePath() + timeStamp + ".png";

        return path;
    }

    private IEnumerator CreateScreenShot()
    {
        DateTime date = DateTime.Now;
        timeStamp = date.ToString("yyyy-MM-dd-HH-mm-ss-fff");
        // レンダリング完了まで待機
        yield return new WaitForEndOfFrame();

        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cam.targetTexture = renderTexture;




        //================================================================================================================================================
        //
        //   もし。テクスチャが縮められている状態だったら下のコメント化している部分（「 / 2」）のところをはずしてみて）
        //
        //================================================================================================================================================




        Texture2D texture = new Texture2D(cam.targetTexture.width / 2, cam.targetTexture.height, TextureFormat.RGB24, false);

        texture.ReadPixels(new Rect(0, 0, cam.targetTexture.width / 2, cam.targetTexture.height), 0, 0);
        texture.Apply();

        // 保存する画像のサイズを変えるならResizeTexture()を実行
        //		texture = ResizeTexture(texture,320,240);

        byte[] pngData = texture.EncodeToPNG();
        screenShotPath = GetScreenShotPath();

        // ファイルとして保存するならFile.WriteAllBytes()を実行
        File.WriteAllBytes(screenShotPath, pngData);

        cam.targetTexture = null;

        //生成したテクスチャファイルから情報を読み込んでRawImageに出力
        ShowSSImage();
    }

    public void ClickShootButton()
    {
        StartCoroutine(CreateScreenShot());
    }

    public void ShowSSImage()
    {
        if (!String.IsNullOrEmpty(screenShotPath))
        {
            byte[] image = File.ReadAllBytes(screenShotPath);

            Texture2D tex = new Texture2D(0, 0);
            tex.LoadImage(image);

            // NGUI の UITexture に表示
            RawImage target = targetImage.GetComponent<RawImage>();
            target.texture = tex;

            //テクスチャ情報を読み込んだ後でRawImageを表示する
            targetImage.enabled = true;
        }
    }

    private void FirstMovePreview()
    {
        targetImage.rectTransform.DOScale(transform.localScale * firstScale, 0.5f);
        Invoke(nameof(SecondMovePreview), 0.5f);
    }

    private void SecondMovePreview()
    {
        targetImage.rectTransform.DOScale(transform.localScale * secondScale, 0.5f);
        targetImage.rectTransform.DOMove(point1.rectTransform.position, 0.5f);
        Invoke(nameof(SlideMovePreview), 1f);
    }

    private void SlideMovePreview()
    {
        targetImage.transform.DOMoveX(point2.rectTransform.position.x, 0.3f);
    }

    public void SetList(List<GameObject> list) { setterObj = list; }

    public void SetDestroyList(List<int> list) { destroyStrangeList = list; }

    public List<int> GetDestroyList() { return destroyStrangeList; }

    public void SetPhotographTargetFlag(bool flag) { noneTargetFlag = flag; }
}
