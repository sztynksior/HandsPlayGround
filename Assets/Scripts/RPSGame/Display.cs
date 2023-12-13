using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Display : MonoBehaviour
{
    [SerializeField] private TMP_Text _mainDisplayText;
    [SerializeField] private TMP_Text _outcomeDisplayText;
    [SerializeField] private MeshRenderer _figureDisplayMeshRenderer;
    [SerializeField] private Material _scissorsMaterial;
    [SerializeField] private Material _rockMaterial;
    [SerializeField] private Material _paperMaterial;

    public void SetMainDisplayText(string text)
    {
        this._mainDisplayText.text = text;
    }

    public void SetOutcomeDisplayText(string text) 
    {  
        this._outcomeDisplayText.text = text;
    }

    public void DisplayRock()
    {
        this._figureDisplayMeshRenderer.material = this._rockMaterial;
    }
    public void DisplayPaper()
    {
        this._figureDisplayMeshRenderer.material = this._paperMaterial;
    }

    public void DisplayScissors()
    {
        this._figureDisplayMeshRenderer.material = this._scissorsMaterial;
    }

    public void TurnOffFigureDisplay()
    {
        this._figureDisplayMeshRenderer.enabled = false;
    }

    public void TurnOnFigureDisplay()
    {
        this._figureDisplayMeshRenderer.enabled = true;
    }
}
