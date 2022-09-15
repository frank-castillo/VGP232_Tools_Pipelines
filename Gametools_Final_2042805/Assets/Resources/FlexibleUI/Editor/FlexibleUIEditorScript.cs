using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FlexibleUIData))]
public class FlexibleUIEditorScript : Editor
{
    private bool _showDefaultControls = false;
    private bool _showConfirmControls = false;
    private bool _showDeclineControls = false;
    private bool _showWarningControls = false;
    private bool _showBackControls = false;
    private bool _showToggleControls = false;

    private FlexibleUIData _data;
    private SerializedObject _serializedDataObject;

    private SerializedProperty m_ButtonSprite;
    private SerializedProperty m_ButtonSpriteState;
    private SerializedProperty m_UseButtonIcon;
    private SerializedProperty m_UseButtonText;

    private SerializedProperty m_DefaultButtonColor;
    private SerializedProperty m_DefaultButtonSprite;
    private SerializedProperty m_DefaultButtonLabel;

    private SerializedProperty m_ConfirmButtonColor;
    private SerializedProperty m_ConfirmButtonSprite;
    private SerializedProperty m_ConfirmButtonLabel;

    private SerializedProperty m_DeclineButtonColor;
    private SerializedProperty m_DeclineButtonSprite;
    private SerializedProperty m_DeclineButtonLabel;

    private SerializedProperty m_WarningButtonColor;
    private SerializedProperty m_WarningButtonSprite;
    private SerializedProperty m_WarningButtonLabel;

    private SerializedProperty m_BackButtonColor;
    private SerializedProperty m_BackButtonSprite;
    private SerializedProperty m_BackButtonLabel;

    private SerializedProperty m_ToggleButtonSprite;
    private SerializedProperty m_ToggleButtonColor;
    private SerializedProperty m_ToggleButtonCheckSprite;
    private SerializedProperty m_ToggleButtonCheckColor;
    private SerializedProperty m_ToggleButtonLabel;


    public void OnEnable()
    {
        _data = (FlexibleUIData)target;
        _serializedDataObject = new SerializedObject(_data);

        m_ButtonSprite = _serializedDataObject.FindProperty("buttonSprite");
        m_ButtonSpriteState = _serializedDataObject.FindProperty("buttonSpriteState");
        m_UseButtonIcon = _serializedDataObject.FindProperty("useIcon");
        m_UseButtonText = _serializedDataObject.FindProperty("useText");

        m_DefaultButtonColor = _serializedDataObject.FindProperty("defaultButtonColor");
        m_DefaultButtonSprite = _serializedDataObject.FindProperty("defaultButtonIcon");
        m_DefaultButtonLabel = _serializedDataObject.FindProperty("defaultButtonText");

        m_ConfirmButtonColor = _serializedDataObject.FindProperty("confirmButtonColor");
        m_ConfirmButtonSprite = _serializedDataObject.FindProperty("confirmButtonIcon");
        m_ConfirmButtonLabel = _serializedDataObject.FindProperty("confirmButtonText");

        m_DeclineButtonColor = _serializedDataObject.FindProperty("declineButtonColor");
        m_DeclineButtonSprite = _serializedDataObject.FindProperty("declineButtonIcon");
        m_DeclineButtonLabel = _serializedDataObject.FindProperty("declineButtonText");

        m_WarningButtonColor = _serializedDataObject.FindProperty("warningButtonColor");
        m_WarningButtonSprite = _serializedDataObject.FindProperty("warningButtonIcon");
        m_WarningButtonLabel = _serializedDataObject.FindProperty("warningButtonText");

        m_BackButtonColor = _serializedDataObject.FindProperty("backButtonColor");
        m_BackButtonSprite = _serializedDataObject.FindProperty("backButtonIcon");
        m_BackButtonLabel = _serializedDataObject.FindProperty("backButtonText");

        m_ToggleButtonSprite = _serializedDataObject.FindProperty("toggleButtonImage");
        m_ToggleButtonColor = _serializedDataObject.FindProperty("toggleButtonColor");
        m_ToggleButtonCheckSprite = _serializedDataObject.FindProperty("toggleButtonCheckImage");
        m_ToggleButtonCheckColor = _serializedDataObject.FindProperty("toggleButtonCheckColor");
        m_ToggleButtonLabel = _serializedDataObject.FindProperty("toggleButtonText");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Use this custom view to modify the settings of each element that belongs to the Flexible UI", MessageType.Info);

        EditorGUILayout.PropertyField(m_ButtonSprite, new GUIContent("Button Image"));
        EditorGUILayout.PropertyField(m_ButtonSpriteState, new GUIContent("Button State"));
        EditorGUILayout.PropertyField(m_UseButtonIcon, new GUIContent("Use Button Icon"));
        EditorGUILayout.PropertyField(m_UseButtonText, new GUIContent("Use Button Text"));

        _showDefaultControls = EditorGUILayout.Foldout(_showDefaultControls, "Default Controls");
        if (_showDefaultControls)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_DefaultButtonColor, new GUIContent("Default Button Color"));
            if (_data.useIcon)
            {
                EditorGUILayout.PropertyField(m_DefaultButtonSprite, new GUIContent("Default Button Sprite"));
            }

            if (_data.useText)
            {
                EditorGUILayout.PropertyField(m_DefaultButtonLabel, new GUIContent("Default Button Label"));
            }

            EditorGUI.indentLevel--;
        }

        _showConfirmControls = EditorGUILayout.Foldout(_showConfirmControls, "Confirm Controls");
        if (_showConfirmControls)
        {
            EditorGUILayout.PropertyField(m_ConfirmButtonColor, new GUIContent("Confirm Button Color"));
            if (_data.useIcon)
                EditorGUILayout.PropertyField(m_ConfirmButtonSprite, new GUIContent("Confirm Button Sprite"));
            if (_data.useText)
                EditorGUILayout.PropertyField(m_ConfirmButtonLabel, new GUIContent("Confirm Button Label"));
        }

        _showDeclineControls = EditorGUILayout.Foldout(_showDeclineControls, "Decline Controls");
        if (_showDeclineControls)
        {
            EditorGUILayout.PropertyField(m_DeclineButtonColor, new GUIContent("Decline Button Color"));
            if (_data.useIcon)
                EditorGUILayout.PropertyField(m_DeclineButtonSprite, new GUIContent("Decline Button Sprite"));
            if (_data.useText)
                EditorGUILayout.PropertyField(m_DeclineButtonLabel, new GUIContent("Decline Button Label"));
        }

        _showWarningControls = EditorGUILayout.Foldout(_showWarningControls, "Warning Controls");
        if (_showWarningControls)
        {
            EditorGUILayout.PropertyField(m_WarningButtonColor, new GUIContent("Warning Button Color"));
            if (_data.useIcon)
                EditorGUILayout.PropertyField(m_WarningButtonSprite, new GUIContent("Warning Button Sprite"));
            if (_data.useText)
                EditorGUILayout.PropertyField(m_WarningButtonLabel, new GUIContent("Warning Button Label"));
        }

        _showBackControls = EditorGUILayout.Foldout(_showBackControls, "Back Controls");
        if (_showBackControls)
        {
            EditorGUILayout.PropertyField(m_BackButtonColor, new GUIContent("Back Button Color"));
            if (_data.useIcon)
                EditorGUILayout.PropertyField(m_BackButtonSprite, new GUIContent("Back Button Sprite"));
            if (_data.useText)
                EditorGUILayout.PropertyField(m_BackButtonLabel, new GUIContent("Back Button Label"));
        }

        _showToggleControls = EditorGUILayout.Foldout(_showToggleControls, "Toggle Controls");
        if (_showToggleControls)
        {
            EditorGUILayout.PropertyField(m_ToggleButtonSprite, new GUIContent("Toggle Button Sprite"));
            EditorGUILayout.PropertyField(m_ToggleButtonColor, new GUIContent("Toggle Button Color"));
            EditorGUILayout.PropertyField(m_ToggleButtonCheckSprite, new GUIContent("Toggle Button Checkmark Sprite"));
            EditorGUILayout.PropertyField(m_ToggleButtonCheckColor, new GUIContent("Toggle Button Checkmark Color"));
        }

        _serializedDataObject.ApplyModifiedProperties();
    }
}