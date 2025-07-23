// Copyright (c) 2024, Awessets

using MergeIt.Core.Animations;
using MergeIt.Core.Configs.Elements;
using MergeIt.Core.FieldElements;
using MergeIt.Core.MVP;
using TEngine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MergeIt.Game.Field.Elements
{
    public class FieldElementView : View, IFieldElementView
    {
        public UnityAction ResetStateEvent;
        
        [SerializeField]
        private GameObject _blocker;

        [SerializeField] private Image _sand_1;
        [SerializeField] private Image _sand_2;
        
        private IAnimationController _animationController;
        
        public GraphicRaycaster GraphicRaycaster => GetComponent<GraphicRaycaster>();

        public RectTransform RectTransform
        {
            get
            {
                if (!_rectTransform)
                {
                    _rectTransform = GetComponent<RectTransform>();
                }

                return _rectTransform;
            }
        }

        public Canvas Canvas
        {
            get
            {
                if (!_canvas)
                {
                    _canvas = GetComponent<Canvas>();
                }

                return _canvas;
            }
        }
        
        public IAnimationController AnimationController
        {
            get { return _animationController ??= GetComponent<IAnimationController>(); }
        }

        private RectTransform _rectTransform;
        private Canvas _canvas;
        private FieldElementModel _model;
        private FieldElementIconComponent _iconComponent;
        
        // private void Start()
        // {
        //     RectTransform rectTransform = GetComponent<RectTransform>();
        //     rectTransform.Stretch();
        // }

        public void Lock(bool isLocked)
        {
            if (!_iconComponent)
            {
                _iconComponent = GetComponentInChildren<FieldElementIconComponent>();
            }
            
            _iconComponent.SetBlocked(isLocked);
             _blocker.SetActive(isLocked);
        }

        public void SetUpSand(int sandCount)
        {
            //_model.SandCount = sandCount;
            if (sandCount == 0)
            {
                _sand_1.gameObject.SetActive(false);
                _sand_2.gameObject.SetActive(false);
            }
            else if (sandCount == 1)
            {
                _sand_1.gameObject.SetActive(true);
                _sand_2.gameObject.SetActive(false);
            }
            else if (sandCount == 2)
            {
                _sand_1.gameObject.SetActive(true);
                _sand_2.gameObject.SetActive(true);
                SetRandomSandFullImg();
            }
        }

        private async void SetRandomSandFullImg()
        {
            int index = Random.Range(1, 8);
            string path = $"img_SandFull_{index}";
            var img = await GameModule.Resource.LoadAssetAsync<Texture2D>(path);
            _sand_2.sprite = Sprite.Create(img, new Rect(0, 0, img.width, img.height), Vector2.zero);
        }

        public void ResetState()
        {
            ResetStateEvent?.Invoke();
        }
    }
}