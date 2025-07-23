// Copyright (c) 2024, Awessets

using MergeIt.Core.Animations;
using MergeIt.Core.FieldElements;
using MergeIt.Core.Helpers;
using MergeIt.Core.MVP;
using TEngine;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MergeIt.Game.Field.Elements
{
    public class FieldElementPresenterBase<TView, TModel> : Presenter<TView, TModel>, IFieldElementPresenter, IAnimationListener
        where TView : FieldElementView
        where TModel : FieldElementModel, new()
    {
        public IFieldElement FieldElement;
        private Transform _transform;
        private RectTransform _rectTransform;
        private Canvas _canvas;
        private FieldElementState _state;

        public FieldElementState State
        {
            get => _state;
        }

        public Transform Transform
        {
            get
            {
                if (!_transform)
                {
                    _transform = View.transform;
                }

                return _transform;
            }
        }

        public bool IsAvailable
        {
            get => !Model.IsBusy && !Model.IsLocked && (Model.SandCount == 0);
        }

        public bool IsBusy
        {
            get => Model.IsBusy;
        }

        public bool IsLocked
        {
            get => Model.IsLocked;
        }
        
        public int SandCount
        {
            get => Model.SandCount;
        }

        public RectTransform RectTransform
        {
            get
            {
                if (!_rectTransform)
                {
                    _rectTransform = View.RectTransform;
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
                    _canvas = View.Canvas;
                }

                return _canvas;
            }
        }

        public virtual void Update(IFieldElement fieldElement)
        {
            FieldElement = fieldElement;
            Model.Id = FieldElement.InfoParameters.Id;
            Model.Point = FieldElement.InfoParameters.LogicPosition;
            Model.IsLocked = FieldElement.InfoParameters.IsBlocked;
            Model.SandCount = FieldElement.InfoParameters.SandCount;

            View.GameObject.name = $"[{Model.Point.X}, {Model.Point.Y}] {FieldElement.InfoParameters.Name}";

            View.Lock(Model.IsLocked);
            View.SetUpSand(Model.SandCount);
            View.ResetState();
        }

        public virtual void Activate(bool isActive)
        {
            Model.Selected = false;
            View.GameObject.SetActive(isActive);
        }

        public virtual void Release()
        {
            
        }

        public void ClearSand()
        {
            Log.Debug("ClearSand : {0}", Model.Point);
            if (Model.SandCount >= 2)
            {
                Model.SandCount = 1;
            }

            View.SetUpSand(Model.SandCount);
        }

        public void ClearSandByMerge()
        {
            if (Model.SandCount == 1)
            {
                Model.SandCount = 0;
            }

            View.SetUpSand(Model.SandCount);
        }
        
        public bool IsSanded()
        {
            return Model.SandCount > 0;
        }

        public virtual void Remove()
        {
            Model.Selected = false;
            View.ResetStateEvent -= OnResetState;

            Object.Destroy(View.GameObject);
        }

        public virtual void SetParent(Transform parent, bool resetPosition = true)
        {
            Transform.SetParent(parent);
            RectTransform.Stretch();

            if (resetPosition)
            {
                RectTransform.anchoredPosition = Vector2.zero;
            }
            
            Transform.localScale = Vector3.one;
        }

        public virtual void ResetPosition()
        {
            RectTransform.anchoredPosition = Vector2.zero;
        }

        public virtual void SetPoint(GridPoint point)
        {
            Model.Point = point;
        }

        public virtual void SetLock(bool block)
        {
            Model.IsLocked = block;
            View.Lock(block);
        }

        public virtual void Select(bool select)
        {
            Model.Selected = select;
        }

        public virtual void StartDrag()
        {
            View.Canvas.sortingOrder = 2;
            View.GraphicRaycaster.enabled = false;
        }

        public virtual void EndDrag()
        {
            View.Canvas.sortingOrder = 1;
            View.GraphicRaycaster.enabled = true;
        }

        public virtual void SetBusy(bool isBusy)
        {
            Model.IsBusy = isBusy;
        }

        public virtual IFieldElementView GetView()
        {
            return View;
        }

        public virtual IFieldElementModel GetModel()
        {
            return Model;
        }

        public virtual void SetState(FieldElementState state)
        {
            _state = state;
            View.AnimationController.SetState(state);
        }

        public virtual void ResetAnimationState()
        {
            View.ResetState();
        }

        protected override void OnInitialize(TView view)
        {
            base.OnInitialize(view);

            View.ResetStateEvent += OnResetState;
            
            View.AnimationController.Initialize(this);
        }
        
        private void OnResetState()
        {
            _state = FieldElementState.Idle;
        }
    }
}