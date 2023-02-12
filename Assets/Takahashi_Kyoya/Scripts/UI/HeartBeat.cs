using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartBeat : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;
    //拍動SEどっくんのどっ
    [SerializeField] private AudioClip heartBeatF;
    //拍動SEどっくんのくん
    [SerializeField] private AudioClip heartBeatS;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    public void FastHeartBeat()
    {
        animator.SetBool("IsFast", true);
    }
    public void IdleHeartBeat()
    {
        animator.SetBool("IsFast", false);
    }
    /// <summary>
    /// 拍動SEのどっを鳴らす
    /// </summary>
    public void HeartBeatSEF()
    {
        audioSource.PlayOneShot(heartBeatF);
    }
    /// <summary>
    /// 拍動SEのくんを鳴らす
    /// </summary>
    public void HeartBeatSES()
    {
        audioSource.PlayOneShot(heartBeatS);
    }
}
