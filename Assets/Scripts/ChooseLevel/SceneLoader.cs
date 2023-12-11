using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] GameObject _fadePanel;
    [SerializeField] float _fadeTime;

    private CanvasRenderer fadePanelCanvasRenderer;

    private void Awake()
    {
        this._fadePanel.SetActive(true);
        this.fadePanelCanvasRenderer = this._fadePanel.GetComponent<CanvasRenderer>();
        this.fadePanelCanvasRenderer.SetColor(new Color(0, 0, 0, 1));
        DOTween.To(() => this.fadePanelCanvasRenderer.GetColor(), color => this.fadePanelCanvasRenderer.SetColor(color), new Color(0, 0, 0, 0), this._fadeTime);
    }

    public void LoadScene(int sceneBuildIndex)
    {
        DOTween.To(() => this.fadePanelCanvasRenderer.GetColor(), color => this.fadePanelCanvasRenderer.SetColor(color), new Color(0, 0, 0, 1), this._fadeTime);
        StartCoroutine(this.ChangeSceneAfterDelay(sceneBuildIndex));
    }

    private IEnumerator ChangeSceneAfterDelay(int sceneBuildIndex)
    {
        while (true)
        {
            yield return new WaitForSeconds(this._fadeTime);
            SceneManager.LoadScene(sceneBuildIndex);
        }
    }
}
