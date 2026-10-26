using System;
using UnityEngine;
using UnityEngine.UIElements;

public class StartingUI : MonoBehaviour
{
    [SerializeField] private string startButtonText;
    [SerializeField] private string quitButtonText;
    
    private string _originalStartButtonText;
    private string _originalQuitButtonText;
    
    private UIDocument _uiDocument;
    
    private VisualElement _root;
    
    private Button _startButton;
    private Button _quitButton;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        _root = _uiDocument.rootVisualElement.Q<VisualElement>("Buttons");
        
        _startButton = _root.Q<Button>("StartButton");
        _quitButton = _root.Q<Button>("QuitButton");
        
        _originalStartButtonText = _startButton.text;
        _originalQuitButtonText = _quitButton.text;
        
        _startButton.RegisterCallback<PointerEnterEvent>(_ => {ChangeText(_startButton, startButtonText);});        
        _quitButton.RegisterCallback<PointerEnterEvent>(_ => {ChangeText(_quitButton, quitButtonText);});    
        
        _startButton.RegisterCallback<PointerLeaveEvent>(_ => {ChangeText(_startButton, _originalStartButtonText);});   
        _quitButton.RegisterCallback<PointerLeaveEvent>(_ => {ChangeText(_quitButton, _originalQuitButtonText);});

        _startButton.clicked += ChangeSceneToMain;
    }

    private void ChangeSceneToMain()
    {
        SceneSwitcher.LoadScene(SceneIndex.Main);
    }

    private void ChangeText(Button button, string text)
    {
        button.text = text;
    }
}
