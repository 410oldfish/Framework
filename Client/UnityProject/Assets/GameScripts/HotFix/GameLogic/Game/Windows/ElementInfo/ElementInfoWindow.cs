// Copyright (c) 2024, Awessets

using MergeIt.Core.WindowSystem.Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIt.Game.Windows.ElementInfo
{
    public class ElementInfoWindow : WindowBase
    {
        [SerializeField]
        private Text _windowTitleText;

        [SerializeField]
        private float _spacingBetweenPanels;

        [SerializeField]
        private float _spacingInsidePanels;

        [SerializeField]
        private ElementInfoItemsPanelComponent _evolutionPanel;

        [SerializeField]
        private ElementInfoItemsPanelComponent _generatesPanel;

        [SerializeField]
        private ElementInfoItemsPanelComponent _createdFromPanel;

        [SerializeField]
        private RectTransform _scrollContent;

        [SerializeField]
        private Button _findButton;
        
        [SerializeField]
        private Image _titleImg;
        
        
        public Text WindowTitleText
        {
            get => _windowTitleText;
        }

        public float SpacingBetweenPanels
        {
            get => _spacingBetweenPanels;
        }

        public float SpacingInsidePanels
        {
            get => _spacingInsidePanels;
        }

        public ElementInfoItemsPanelComponent EvolutionPanel
        {
            get => 
                _evolutionPanel;
        }

        public ElementInfoItemsPanelComponent GeneratesPanel
        {
            get => _generatesPanel;
        }

        public ElementInfoItemsPanelComponent CreatedFromPanel
        {
            get => _createdFromPanel;
        }

        public RectTransform ScrollContent
        {
            get => _scrollContent;
        }
        
        public Button FindButton
        {
            get => _findButton;
        }
        
        public Image TitleImg
        {
            get => _titleImg;
        }
    }
}