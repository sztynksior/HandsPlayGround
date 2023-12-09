using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Display : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private MeshRenderer _meshRenderer;

    public void SetText(string text)
    {
        this._text.text = text;
    }

    public void AddText(string text)
    {
        this._text.text += text;
    }
}
