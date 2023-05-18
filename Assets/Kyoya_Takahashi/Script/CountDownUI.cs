using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownUI : MonoBehaviour
{
    //カウントダウンの長さ(秒)
    private float countDownTime = 3;

    Text countDownText;
    // Start is called before the first frame update
    void Start()
    {
        countDownText = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        countDownTime -= Time.deltaTime;
        countDownText.text = countDownTime.ToString();
        if(countDownTime < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
