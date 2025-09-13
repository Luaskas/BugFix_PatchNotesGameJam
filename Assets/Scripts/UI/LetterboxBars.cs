using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LetterboxBars : MonoBehaviour
{
    public RectTransform topBar;
    public RectTransform bottomBar;
    public float targetHight = 200f;
    public float speed = 500f;
    
    private Coroutine currentRoutine;

    public void ShowBars()
    {
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(AnimateBars(targetHight));
    }

    public void HideBars()
    {
        if(currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(AnimateBars(0f));
    }

    private IEnumerator AnimateBars(float endHight)
    {
        while (Mathf.Abs(topBar.sizeDelta.y - endHight) > 0.5f)
        {
            float newHeight = Mathf.MoveTowards(topBar.sizeDelta.y, endHight, speed * Time.deltaTime);
            
            topBar.sizeDelta = new Vector2(topBar.sizeDelta.x, newHeight);
            bottomBar.sizeDelta = new Vector2(bottomBar.sizeDelta.x, newHeight);
            
            yield return null;
        }
        
        topBar.sizeDelta = new Vector2(topBar.sizeDelta.x, endHight);
        bottomBar.sizeDelta = new Vector2(bottomBar.sizeDelta.x, endHight);
        
    }
}
