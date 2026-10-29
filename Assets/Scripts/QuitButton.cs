using System;
using UnityEngine;
using UnityEngine.UIElements;

public class QuitButton : MonoBehaviour
{
    private Button _quitButton;
    private void Awake()
    {
        _quitButton = GetComponent<UIDocument>().rootVisualElement.Q<Button>("QuitButton");
        _quitButton.clicked += Application.Quit;
    }
}
