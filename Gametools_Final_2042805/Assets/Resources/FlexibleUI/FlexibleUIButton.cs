using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class FlexibleUIButton : FlexibleUI
{
    [SerializeField] private enum ButtonType { Default, Confirm, Decline, Warning, Back}

    [SerializeField] private Image _image = null;
    [SerializeField] private Button _button = null ;
    [SerializeField] private Image _icon = null;
    [SerializeField] private TextMeshProUGUI _label = null;
    [SerializeField] private ButtonType _buttonType = ButtonType.Default;
    private GameObject _checkObject = null;
    private GameObject _textObject = null;

    public override void Awake()
    {
        if (_image != null && _button != null && _icon != null && _label != null) 
            return;

        _checkObject = new GameObject("Icon");
        _checkObject.AddComponent<Image>();
        _checkObject.gameObject.transform.SetParent(transform, false);

        _textObject = new GameObject("Label");
        _textObject.AddComponent<TextMeshProUGUI>();
        _textObject.gameObject.transform.SetParent(transform, false);

        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
        _icon = _checkObject.GetComponent<Image>();
        _label = _textObject.GetComponent<TextMeshProUGUI>();
        _button.transition = Selectable.Transition.SpriteSwap;
        _skinData = (FlexibleUIData)Resources.Load("Default Skin");

        this.gameObject.name = "AutomaticButton";
    }

    protected override void OnSkinUI()
    {
        base.OnSkinUI();

        if (!_skinData.useText)
        {
            _label.transform.gameObject.SetActive(false);
        }
        else
        {
            _label.transform.gameObject.SetActive(true);
        }

        if (!_skinData.useIcon)
        {
            _icon.transform.gameObject.SetActive(false);
        }
        else
        {
            _icon.transform.gameObject.SetActive(true);
        }

        _button.targetGraphic = _image;
        _image.sprite = _skinData.buttonSprite;
        _image.type = Image.Type.Sliced;
        _button.spriteState = _skinData.buttonSpriteState;

        switch (_buttonType)
        {
            case ButtonType.Default:
                _image.color = _skinData.defaultButtonColor;
                _icon.sprite = _skinData.defaultButtonIcon;
                _icon.type = Image.Type.Sliced;
                _label.text = _skinData.defaultButtonText;
                break;
            case ButtonType.Confirm:
                _image.color = _skinData.confirmButtonColor;
                _icon.sprite = _skinData.confirmButtonIcon;
                _icon.type = Image.Type.Sliced;
                _label.text = _skinData.confirmButtonText;
                break;
            case ButtonType.Decline:
                _image.color = _skinData.declineButtonColor;
                _icon.sprite = _skinData.declineButtonIcon;
                _icon.type = Image.Type.Sliced;
                _label.text = _skinData.declineButtonText;
                break;
            case ButtonType.Warning:
                _image.color = _skinData.warningButtonColor;
                _icon.sprite = _skinData.warningButtonIcon;
                _icon.type = Image.Type.Sliced;
                _label.text = _skinData.warningButtonText;
                break;
            case ButtonType.Back:
                _image.color = _skinData.backButtonColor;
                _icon.sprite = _skinData.backButtonIcon;
                _icon.type = Image.Type.Sliced;
                _label.text = _skinData.backButtonText;
                break;
        }
    }
}