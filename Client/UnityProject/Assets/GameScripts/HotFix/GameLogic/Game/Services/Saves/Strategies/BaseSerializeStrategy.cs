// Copyright (c) 2024, Awessets

using Cysharp.Threading.Tasks;
using MergeIt.Core.Saves;

namespace MergeIt.Game.Services.Saves.Strategies
{
    public abstract class BaseSerializeStrategy : ISerializeStrategy
    {
        public virtual string SaveDir
        {
            get => "Saves";
        }

        public abstract UniTask Save<T>(T data) where T : class, ISavable;

        public abstract T Load<T>() where T : class, ISavable;
    }
}