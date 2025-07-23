// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Types;

namespace MergeIt.Core.FieldElements
{
    public class InfoParameters : IInfoParameters
    {
        public int Id { get; set; }
        public GridPoint LogicPosition { get; set; }
        public bool IsBlocked { get; set; }
        public int SandCount { get; set; }
        public ElementType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}