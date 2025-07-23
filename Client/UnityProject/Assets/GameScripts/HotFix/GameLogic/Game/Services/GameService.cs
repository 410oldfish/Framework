// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.Commands;
using MergeIt.Core.Messages;
using MergeIt.Core.Services;
using MergeIt.Game.Commands;
using MergeIt.Game.Converters;
using MergeIt.Game.Factories.Field;
using MergeIt.Game.Messages;
using MergeIt.SimpleDI;
using MergeIt.SimpleDI.ReservedInterfaces;
using TEngine;

namespace MergeIt.Game.Services
{
    public class GameService : IGameService, IInitializable, IDisposable
    {
        [Introduce]
        private IConfigProcessor _configProcessor;

        [Introduce]
        private IConfigsService _configsService;

        [Introduce]
        private IFieldFactory _fieldFactory;

        [Introduce]
        private IGameLoadService _gameLoadService;

        [Introduce]
        private GameServiceModel _gameServiceModel;

        [Introduce]
        public IMessageBus _messageBus;

        [Introduce]
        private IGameSaveService _saveService;

        public void Dispose()
        {
            _messageBus.RemoveListener<StartGameMessage>(StartGameMessageHandler);
        }

        public void Initialize()
        {
            _messageBus.AddListener<StartGameMessage>(StartGameMessageHandler);
        }

        private async void StartGameMessageHandler(StartGameMessage message)
        {
            Log.Debug("StartGameMessageHandler?????????");
            var manager = new CommandManager();
            Log.Debug("111111111111111111");
            manager.Add(new LoadConfigsCommand());
            Log.Debug("2222222222");

            manager.Add(new PrepareUserCommand());
            Log.Debug("33333333333");

            manager.Add(new PrepareEnergyCommand());
            Log.Debug("4444444444444444");

            manager.Add(new PrepareStockCommand());
            Log.Debug("55555555555555");

            manager.Add(new PrepareInventoryCommand());
            Log.Debug("66666666666666");

            manager.Add(new PrepareFieldCommand());
            Log.Debug("7777777777777");

            manager.Add(new CheckEvolutionsProgressCommand());
            Log.Debug("8888888888888");
            manager.Add(new PlayBgmCommand());
            
            await manager.RunAsync();
            Log.Debug("Fire LoadedGameMessage");

            _messageBus.Fire<LoadedGameMessage>();
        }
    }
}