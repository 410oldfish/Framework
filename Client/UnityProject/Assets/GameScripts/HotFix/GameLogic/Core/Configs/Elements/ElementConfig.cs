// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.Configs.Types;
using UnityEngine;

namespace MergeIt.Core.Configs.Elements
{
    [CreateAssetMenu(fileName = "ElementConfig", menuName = "Merge Toolkit/Element")]
    public class ElementConfig : ScriptableObject
    {
        [SerializeField] 
        private string _id;
        
        [SerializeField]
        private ElementFuncType _funcType;
        
        [SerializeField]
        private ElementType _type;
        
        [SerializeField]
        private ElementCommonSettings _commonSettings;
        
        [SerializeField]
        private ElementGeneratorSettings _generatorSettings;

        public string Id
        {
            get => _id;
        }
        
        public ElementType Type
        {
            get => _type;
        }
        
        public ElementFuncType FuncType
        {
            get => _funcType;
        }
        
        public ElementCommonSettings CommonSettings
        {
            get => _commonSettings;
            set => _commonSettings = value;
        }
        
        public ElementGeneratorSettings GeneratorSettings
        {
            get => _generatorSettings;
            set => _generatorSettings = value;
        }

        public FieldElementIconComponent GetIconComponent()
        {
            return _commonSettings?.Icon;
        }
    }
}