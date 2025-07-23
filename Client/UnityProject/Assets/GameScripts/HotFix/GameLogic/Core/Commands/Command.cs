// Copyright (c) 2024, Awessets

using System;
using Cysharp.Threading.Tasks;

namespace MergeIt.Core.Commands
{
    public abstract class Command : ICommand, IDisposable
    {
        public event Action<ICommand> Finished;
        
        public virtual void Execute()
        {
            
        }

        public virtual async UniTask ExecuteAsync()
        {
            Execute();
        }
        
        public void Dispose()
        {
            OnDispose();
        }

        protected virtual void Finish()
        {
            Finished?.Invoke(this);
        }

        protected virtual void OnDispose()
        {
            
        }
    }
}