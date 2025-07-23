// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.Utils;
using TEngine;
using UnityEngine;

namespace MergeIt.Game.Field.Elements.Generator
{
    public class FieldElementGeneratorView : FieldElementView
    {
        [SerializeField]
        private GeneratorTimerComponent _timer;
        
        public void SetTimer(Bindable<float> remainChargeTime, float fullRemainTime)
        { 
            _timer.StartTimer(remainChargeTime, fullRemainTime);
        }

        public void HideTimer()
        {
            _timer.gameObject.SetActive(false);
        }

        private void Start()
        {
            SetShader();
        }

        async void SetShader()
        {
            var generatorIcon = transform.Find("GeneratorIcon");
            if(!generatorIcon) return;
            if (generatorIcon.gameObject.activeSelf)
            {
                var mat = generatorIcon.transform.GetChild(0).GetChild(0).GetComponent<Material>();
                if (mat == null)
                {
                    mat = await GameModule.Resource.LoadAssetAsync<Material>("jinbi_Material-Additive");
                }
                var shaderName = mat.shader.name;
                var newShader = Shader.Find(shaderName);
                mat.shader = newShader;

            }
        }
    }
}