using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DivineLight : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.5f;
    private Image _divineLightImage;
    private Coroutine _fadeCoroutine;


    private void Awake()
    {
        _divineLightImage = GetComponent<Image>();
    }

    public void Flash()
    {
        if (_divineLightImage == null) return;
        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(FadeOutCoroutine(fadeDuration));
    }

    private IEnumerator FadeOutCoroutine(float duration)
    {
        Color col = _divineLightImage.color;
        col.a = 0.6f;
        _divineLightImage.color = col;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0.6f, 0f, t / Mathf.Max(0.0001f, duration));
            col.a = alpha;
            _divineLightImage.color = col;
            yield return null;
        }

        col.a = 0f;
        _divineLightImage.color = col;
        _fadeCoroutine = null;
    }
}
