// Copyright (c) 2024, Awessets

using Cysharp.Threading.Tasks;
using MergeIt.Core.Commands;
using MergeIt.Core.Services;
using MergeIt.Game.User;
using MergeIt.SimpleDI;
using TEngine;

namespace MergeIt.Game.Commands
{
    public class PrepareUserCommand : Command
    {
        private readonly IGameLoadService _gameLoadService = DiContainer.Get<IGameLoadService>();
        private readonly IUserService _userService = DiContainer.Get<IUserService>();
        
        public override void Execute()
        {
            Log.Debug("PrepareUserCommand");
            var userData = _gameLoadService.Load<UserData>();

            if (userData == null)
            {
                _userService.CreateUser();
            }
            else
            {
                _userService.SetupUser(userData);
            }
        }
    }
}