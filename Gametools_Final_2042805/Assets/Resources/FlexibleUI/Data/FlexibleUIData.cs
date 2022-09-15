using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Flexible UI Data")]
public class FlexibleUIData : ScriptableObject
{
    public Sprite buttonSprite;
    public SpriteState buttonSpriteState;
    public bool useIcon;
    public bool useText;

    // Default Button Parameters
    public Color defaultButtonColor;
    public Sprite defaultButtonIcon;
    public string defaultButtonText;

    // Confirm Button Parameters
    public Color confirmButtonColor;
    public Sprite confirmButtonIcon;
    public string confirmButtonText;

    // Decline Button Parameters
    public Color declineButtonColor;
    public Sprite declineButtonIcon;
    public string declineButtonText;

    // Warning Button Parameters
    public Color warningButtonColor;
    public Sprite warningButtonIcon;
    public string warningButtonText;

    // Back Button Parameters
    public Color backButtonColor;
    public Sprite backButtonIcon;
    public string backButtonText;

    // Toggle Button Parameters
    public Color toggleButtonColor;
    public Sprite toggleButtonImage;
    public Color toggleButtonCheckColor;
    public Sprite toggleButtonCheckImage;
    public string toggleButtonText;
}