using System;
using UnityEngine;
using UnityEngine.UI;

namespace State_Machine
{
    public class FadeImage : MonoBehaviour
    {
        public static FadeImage Instance;
        public Image Image { get; private set; }

        private SceneSwitcher _sceneSwitcher;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            
            Image = GetComponentInChildren<Image>();
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            _sceneSwitcher = new SceneSwitcher(this);
        }
    }
}