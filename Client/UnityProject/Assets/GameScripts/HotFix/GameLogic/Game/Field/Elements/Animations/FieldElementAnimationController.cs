// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MergeIt.Core.Animations;
using MergeIt.Core.FieldElements;
using Spine.Unity;
using TEngine;
using UnityEngine;

namespace MergeIt.Game.Field.Elements.Animations
{
    [RequireComponent(typeof(Animator))]
    public class FieldElementAnimationController : AnimationControllerBase
    {
        private IAnimationListener _listener;
        private Animator _animator;

        private static readonly Dictionary<FieldElementState, int> StatesHash = new();
        
        static FieldElementAnimationController()
        {
            StatesHash[FieldElementState.Idle] = Animator.StringToHash(FieldElementState.Idle.ToString());
            StatesHash[FieldElementState.Hint] = Animator.StringToHash(FieldElementState.Hint.ToString());
            StatesHash[FieldElementState.Find] = Animator.StringToHash(FieldElementState.Find.ToString());
        }

        public static int GetFieldElementState(FieldElementState state)
        {
            StatesHash.TryGetValue(state, out int hash);

            return hash;
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public override void Initialize(IAnimationListener listener)
        {
            _listener = listener;
        }

        public override void SetState<T>(T state)
        {
            var concreteState = (FieldElementState)(object)state;

            if (StatesHash.TryGetValue(concreteState, out int hash))
            {
                switch (concreteState)
                {
                    case FieldElementState.Idle:
                        _animator.Play(hash);
                        break;
                    
                    case FieldElementState.Hint:
                        _animator.SetTrigger(hash);
                        break;
                    case FieldElementState.Find:
                        var spineObj = transform.Find("DragElemEft").gameObject;
                        spineObj.SetActive(true);
                        var spineAni = spineObj.GetComponent<SkeletonAnimation>();
                        spineAni.AnimationState.SetAnimation(0, "1", true);
                        var mat = spineAni.GetComponent<Material>();
                        if (mat == null)
                        {
                            LoadEftMat().Forget();
                        }
                        
          
                        break;
                }
            }
        }

        async UniTask LoadEftMat()
        {
            var mat = await GameModule.Resource.LoadAssetAsync<Material>("ddz_fanpai_ani_Material");
            if (mat != null)
            {
                Debug.Log(mat.name);
                var shaderName = mat.shader.name;
                var newShader = Shader.Find(shaderName);
                mat.shader = newShader;
            }
        }

        private void OnDisable()
        {
            _listener?.ResetAnimationState();
        }
    }
}