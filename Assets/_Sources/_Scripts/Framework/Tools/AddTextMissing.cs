
using System;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class AddTextMissing : MonoBehaviour
{
    public GameObject[] keys;

    [Button]
    public void AddKey()
    {
        foreach (var key in keys)
        {
            dict.Add(key, new TextConfig());
        }
    }

    public SerializableDictionaryBase<GameObject, TextConfig> dict;
    
    [Button]
    public void AddText()
    {
        foreach (var pair in dict)
        {
            var text = pair.Key.GetComponent<TextMeshProUGUI>();
            if (text == null)
            {
                text = pair.Key.AddComponent<TextMeshProUGUI>();
            }

            text.raycastTarget = pair.Value.raycastTarget;
            text.fontStyle = pair.Value.fontStyle;
            text.font = pair.Value.font;
            text.text = pair.Value.text;
            text.color = pair.Value.color;
            text.horizontalAlignment = pair.Value.hozAlignment;
            text.verticalAlignment = pair.Value.verAlignment;

            text.fontSize = pair.Value.fontSize;
            text.enableAutoSizing = pair.Value.autoSize;
        }
    }
}

[Serializable]
public class TextConfig
{
    public bool raycastTarget;
    [EnumToggleButtons] public FontStyles fontStyle = FontStyles.Bold;
    public TMP_FontAsset font;
    public string text;
    public Color color = Color.white;
    public HorizontalAlignmentOptions hozAlignment = HorizontalAlignmentOptions.Center;
    public VerticalAlignmentOptions verAlignment = VerticalAlignmentOptions.Capline;
    public float fontSize = 35;
    public bool autoSize;
}