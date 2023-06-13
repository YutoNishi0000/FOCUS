using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PoseUIManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>選択肢ボタン </summary>    
    private Button _button;

    /// <summary>イメージ </summary>    
    private Image _image;

    /// <summary>テキスト </summary>    
    private Text _text;

    /// <summary>ボタンクラスを取得 </summary>
    public Button Button => _button;
    
    private void Start()
    {
        _button = GetComponent<Button>();
        _image　= GetComponent<Image>();
        _text = transform.GetChild(0).GetComponent<Text>();

        // 選択されていないときはボタンを暗くする
        Color disabledColor = Button.colors.disabledColor;
        Button.image.color = disabledColor;
    }
    /// <summary>UI表示</summary>
    public void ShowUI()
    {
        //_button.enabled = true;
        _image.enabled = true;
        _text.enabled = true;
    }
    /// <summary>UI非表示</summary>
    public void HideUI()
    {
        //_button.enabled = false;
        _image.enabled = false;
        _text.enabled = false;
    }
    /// <summary>選択されてるときはボタンを明るくする </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        Color normalColor = Button.colors.normalColor;
        Button.image.color = normalColor;
    }
    /// <summary>選択されていないときはボタンを暗くする </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        Color disabledColor = Button.colors.disabledColor;
        Button.image.color = disabledColor;
    }
}
