using System;
using System.Collections;
using State_Machine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneIndex
{
    Start,
    Main,
    End
}

public class SceneSwitcher 
{
    private static SmoothMotion _smoothMotion = new();
    private static FadeImage _fadeImage;
    public static float FadeTime = 1.5f;

    public SceneSwitcher(FadeImage fadeImage)
    {
        SceneManager.sceneLoaded += FadeOut;
        
        _fadeImage = fadeImage;
    }
    
    public static void LoadScene(SceneIndex scene)
    {
        _fadeImage.StartCoroutine(Fade(1, ()=>SceneManager.LoadScene((int)scene)));
    }

    private static IEnumerator Fade(float transparency = 0, Action onEnd=null)
    {
        var startTransparency = _fadeImage.Image.color.a;
        
        for (float t = 0; t < FadeTime; t += Time.deltaTime)
        {
            var red = _fadeImage.Image.color.r;
            var green = _fadeImage.Image.color.g;
            var blue = _fadeImage.Image.color.b;
            
            _fadeImage.Image.color = new Color(red, green, blue, _smoothMotion.InterpolateFloat(startTransparency, transparency, FadeTime));
            yield return null;
        }
        _smoothMotion.Stop();
        onEnd?.Invoke();
    }

    private void FadeOut(Scene arg0, LoadSceneMode loadSceneMode)
    {
        _fadeImage.StartCoroutine(Fade());
    }
}
