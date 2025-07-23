// Copyright (c) 2024, Awessets

using MergeIt.Core.Helpers;
using UnityEngine;

namespace MergeIt.SimpleDI
{
    public abstract class DiRoot : MonoBehaviour
    {
        protected abstract void OnInstall();
        
        protected virtual void Run()
        {
            
        }
        
        private void Awake()
        {
            Debug.Log("11111111111111");
            _ = MainThreadDispatcher.Instance;
            Install();
            Debug.Log("222222222222222");
            Run();
            Debug.Log("333333333333333");
        }

        private void Update()
        {
            DiContainer.Update();
        }

        private void Install()
        {
            OnInstall();
            PostInstall();
        }

        private void PostInstall()
        {
            DiContainer.PostProcess();
        }
    }
}