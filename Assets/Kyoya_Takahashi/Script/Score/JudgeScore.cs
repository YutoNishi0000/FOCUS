using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeScore : ScoreManger
{
    //�r���[���W�ɕϊ��������I�u�W�F�N�g�|�W�V����
    [SerializeField] private GameObject obj = null;
    //[SerializeField] private GameObject obj2 = null;

    [SerializeField] private Camera cam = null;

    //���ꂼ��̃X�R�A�̒l
    enum ScoreType
    {
        high = 50,
        midle = 30,
        low = 10,
        outOfScreen = 0,
    };
    //���ꂼ��̔���̕�
    private float areaWidth = 192;      //(960 / 5) ���5����
    private float areaHeight = 216;     //(1080 / 5) ���5����
    //��ʂ̒��S
    private Vector3 center = Vector3.zero;

    private void Start()
    {
        //��ʂ̒��S�����߂�
        center = new Vector3(
            areaWidth * 2 + areaWidth * 0.5f,
            areaHeight * 2 + areaHeight * 0.5f,
            0.0f);
    }
    private void Update()
    {
        //obj2.transform.position = WorldToScreenPoint(cam, DestroyTarget.target.transform.position);
        Debug.Log(ScoreManger.Score);
        if (GameManager.Instance.IsPhoto)
        {
            Debug.Log("�ʂ��Ă�");
            ScoreManger.Score += checkScore(WorldToScreenPoint(cam, DestroyTarget.target.transform.position));
            GameManager.Instance.IsPhoto = false;
        }
    }
    /// <summary>
    /// ���[���h���W���X�N���[�����W��
    /// </summary>
    /// <param name="cam">�J�����I�u�W�F�N�g</param>
    /// <param name="worldPosition">���[���h���W</param>
    /// <returns>�X�N���[�����W</returns>
    public static Vector3 WorldToScreenPoint(Camera cam, Vector3 worldPosition)
    {
        // �J������Ԃɕϊ�(�J�������猩�����W�ɕϊ�)
        Vector3 cameraSpace = cam.worldToCameraMatrix.MultiplyPoint(worldPosition);

        // �N���b�s���O��Ԃɕϊ�(cameraSpace�����͈̔͂ɍi���Ă�)
        Vector4 clipSpace = cam.projectionMatrix * new Vector4(cameraSpace.x, cameraSpace.y, cameraSpace.z, 1f);

        //�f�o�C�X���W�F�����[1�@�E��{1
        //�����Ă�̂͐��K
        // �f�o�C�X���W�n�ɕϊ�
        Vector3 deviceSpace = new Vector3(clipSpace.x / clipSpace.w, clipSpace.y / clipSpace.w, clipSpace.z / clipSpace.w);

        // �X�N���[�����W�n�ɕϊ�
        Vector3 screenSpace = new Vector3((deviceSpace.x + 1f) * 0.5f * Screen.width, (deviceSpace.y + 1f) * 0.5f * Screen.height, deviceSpace.z);

        return screenSpace;
    }
    /// <summary>
    /// �����ɂǂꂾ���߂�������
    /// </summary>
    /// <param name="scrPoint">�X�N���[�����W</param>
    /// <returns>�X�R�A�̒l</returns>
    private int checkScore(Vector3 scrPoint)
    {
        return lower(checkScoreHori(scrPoint), checkScoreVart(scrPoint));
    }
    /// <summary>
    /// �X�R�A�̔���(��)
    /// </summary>
    /// <param name="scrPoint">�X�N���[�����W</param>
    /// <param name="score">�^�e�����Ō����Ƃ��̃X�R�A</param>
    /// <returns>�X�R�A</returns>
    private int checkScoreHori(Vector3 scrPoint)
    {
        if (Mathf.Abs(scrPoint.x - center.x) < areaWidth / 2)
        {
            return (int)ScoreType.high;
        }
        if (Mathf.Abs(scrPoint.x - center.x) < areaWidth / 2 + areaWidth)
        {
            return (int)ScoreType.midle;
        }
        if (Mathf.Abs(scrPoint.x - center.x) < areaWidth / 2 + areaWidth * 2)
        {
            return (int)ScoreType.low;
        }
        return (int)ScoreType.outOfScreen;
    }
    /// <summary>
    /// �X�R�A�̔���(�c)
    /// </summary>
    /// <param name="scrPoint">�X�N���[�����W</param>
    /// <param name="score">�^�e�����Ō����Ƃ��̃X�R�A</param>
    /// <returns>�X�R�A</returns>
    private int checkScoreVart(Vector3 scrPoint)
    {
        if (Mathf.Abs(scrPoint.y - center.y) < areaHeight / 2)
        {
            return (int)ScoreType.high;
        }
        if (Mathf.Abs(scrPoint.y - center.y) < areaHeight / 2 + areaHeight)
        {
            return (int)ScoreType.midle;
        }
        if (Mathf.Abs(scrPoint.y - center.y) < areaHeight / 2 + areaHeight * 2)
        {
            return (int)ScoreType.low;
        }
        return (int)ScoreType.outOfScreen;
    }
    /// <summary>
    /// �Ⴂ�ق��̒l�������߂�
    /// </summary>
    /// <param name="v">�Е��̃X�R�A</param>
    /// <returns>�X�R�A</returns>
    private int lower(int v1, int v2)
    {
        if(v1 < v2)
        {
            return v1;
        }
        else
        {
            return v2;
        }
    }

}