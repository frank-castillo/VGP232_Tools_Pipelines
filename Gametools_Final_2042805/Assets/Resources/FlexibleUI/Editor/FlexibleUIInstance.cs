using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FlexibleUIInstance : Editor
{
    static GameObject clickedObject;

    [MenuItem("GameObject/Flexible UI/AutomaticButton", priority = 0)]
    public static void AddButton()
    {
        Create("AutomaticButton");
    }

    [MenuItem("GameObject/Flexible UI/AutomaticToggleButton", priority = 0)]
    public static void AddToggleButton()
    {
        Create("AutomaticToggleButton");
    }

    private static GameObject Create(string objectName)
    {
        GameObject instance = Instantiate(Resources.Load(objectName)) as GameObject;
        instance.name = objectName;
        clickedObject = UnityEditor.Selection.activeObject as GameObject;

        if (clickedObject != null)
        {
            instance.transform.SetParent(clickedObject.transform, false);
        }

        return instance;
    }
}