// Copyright (c) 2024, Awessets

using Cysharp.Threading.Tasks;
using MergeIt.Core.Commands;
using MergeIt.Core.Services;
using MergeIt.SimpleDI;
using TEngine;

namespace MergeIt.Game.Commands
{
    public class LoadConfigsCommand : Command
    {
        private readonly IConfigsService _configsService = DiContainer.Get<IConfigsService>();

        public override void Execute()
        {
            Log.Debug("LoadConfigsCommand Execute");
            _configsService.Load();
        }
        
        public override async UniTask ExecuteAsync()
        {
            Log.Debug("LoadConfigsCommand ExecuteAsync");
            await _configsService.Load();
        }
    }
}