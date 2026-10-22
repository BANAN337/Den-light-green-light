using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

public class HandsEnabler : MonoBehaviour
{
    [SerializeField] private int offset = -100;
    [SerializeField] private float duration = 0.05f;
    
    private UIDocument _uiDocument;
    private VisualElement _hands;
    
    private StyleLength _startingPosition;
    
    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        _hands = _uiDocument.rootVisualElement.Q<VisualElement>("Hands");

        _hands.style.transitionProperty = new List<StylePropertyName>
        {
            "top", "bottom",
        };
        _hands.style.transitionDuration = new List<TimeValue>
        {
            new(duration, TimeUnit.Second),
        };
        
    }

    public void MoveUp()
    {
        _hands.style.top = offset;
        offset *= -1;
    }

    public void ResetPosition()
    {
        _hands.style.top = StyleKeyword.Auto;
        _hands.style.bottom = StyleKeyword.Auto;
        _hands.style.left = StyleKeyword.Auto;
        _hands.style.right = StyleKeyword.Auto;
    }
}
