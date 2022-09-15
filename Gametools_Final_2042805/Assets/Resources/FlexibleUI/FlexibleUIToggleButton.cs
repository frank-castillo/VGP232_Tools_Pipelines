using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
[RequireComponent(typeof(Image))]
public class FlexibleUIToggleButton : FlexibleUI
{
    [SerializeField] private Image _backgroundImage = null;
    [SerializeField] private Toggle _toggle = null;
    [SerializeField] private Image _checkboxImage = null;
    [SerializeField] private TextMeshProUGUI _label = null;

    public override void Awake()
    {
        if (_backgroundImage != null && _toggle != null && _checkboxImage != null && _label != null)
            return;

        var checkObject = new GameObject("Checkmark");
        checkObject.AddComponent<Image>();
        checkObject.gameObject.transform.SetParent(transform, false);

        var textObject = new GameObject("Label");
        textObject.AddComponent<TextMeshProUGUI>();
        textObject.gameObject.transform.SetParent(transform, false);

        _backgroundImage = GetComponent<Image>();
        _toggle = GetComponent<Toggle>();
        _checkboxImage = checkObject.GetComponent<Image>();
        _label = textObject.GetComponent<TextMeshProUGUI>();
        _toggle.transition = Selectable.Transition.None;
        _skinData = (FlexibleUIData)Resources.Load("Default Skin");

        this.gameObject.name = "AutomaticToggleButton";
    }

    protected override void OnSkinUI()
    {
        base.OnSkinUI();

        _toggle.graphic = _checkboxImage;
        _backgroundImage.sprite = _skinData.toggleButtonImage;
        _backgroundImage.type = Image.Type.Sliced;
        _backgroundImage.color = _skinData.toggleButtonColor;
        _checkboxImage.sprite = _skinData.toggleButtonCheckImage;
        _checkboxImage.type = Image.Type.Sliced;
        _checkboxImage.color = _skinData.toggleButtonCheckColor;
    }
}