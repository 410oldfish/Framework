// Copyright (c) 2024, Awessets

using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MergeIt.Core.Configs.Elements;
using MergeIt.Core.FieldElements;
using MergeIt.Core.Helpers;
using MergeIt.Core.Messages;
using MergeIt.Core.Services;
using MergeIt.Game.Factories.Field;
using MergeIt.Game.Factories.FieldElement;
using MergeIt.Game.Field;
using MergeIt.Game.Messages;
using MergeIt.SimpleDI;
using MergeIt.SimpleDI.ReservedInterfaces;
using TEngine;

namespace MergeIt.Game.Services
{
    public class GameFieldService : IGameFieldService, IInitializable, IDisposable
    {
        [Introduce]
        private IConfigsService _configsService;

        [Introduce]
        private IFieldElementFactory _fieldElementFactory;

        [Introduce]
        private IFieldFactory _fieldFactory;

        [Introduce]
        private FieldLogicModel _fieldLogicModel;

        [Introduce]
        private GameServiceModel _gameServiceModel;

        [Introduce]
        private IMessageBus _messageBus;

        public void Dispose()
        {
            _messageBus.RemoveListener<LoadedGameMessage>(OnLoadedGameMessageHandler);
            _messageBus.RemoveListener<CheckTaskCanFinishMessage>(OnCheckTaskCanFinishMessage);
        }

        public GridPoint? GetFreeCell(GridPoint centerPoint)
        {
            int fieldHeight = _fieldLogicModel.FieldHeight;
            int fieldWidth = _fieldLogicModel.FieldWidth;

            // 方向向量 (上, 下, 左, 右)
            int[] dx = { -1, 1, 0, 0 };
            int[] dy = { 0, 0, -1, 1 };

            // BFS队列
            Queue<(int, int)> queue = new Queue<(int, int)>();
            HashSet<(int, int)> visited = new HashSet<(int, int)>();

            int centerX = centerPoint.X;
            int centerY = centerPoint.Y;
            
            // 初始化
            queue.Enqueue((centerX, centerY));
            visited.Add((centerX, centerY));

            // BFS查找
            while (queue.Count > 0)
            {
                var (x, y) = queue.Dequeue();

                // 检查是否为空闲格
                GridPoint point = new GridPoint(x, y);
                if (!_fieldLogicModel.FieldElements.ContainsKey(point))
                {
                    return point;
                }

                // 遍历四个方向
                for (int i = 0; i < 4; i++)
                {
                    int nx = x + dx[i];
                    int ny = y + dy[i];

                    // 判断是否在棋盘范围内且未访问过
                    if (nx >= 0 && nx < fieldWidth && ny >= 0 && ny < fieldHeight && !visited.Contains((nx, ny)))
                    {
                        queue.Enqueue((nx, ny));
                        visited.Add((nx, ny));
                    }
                }
            }
            
            return null;
        }

        public IFieldElement CreateNewElement(ElementConfig config, GridPoint point, bool blocked = false)
        {
            IFieldElement newElement =
                _fieldElementFactory.CreateFieldElement(config, point, blocked);

            return newElement;
        }

        public void Initialize()
        {
            Log.Debug("GameFieldService Initialize");
            _messageBus.AddListener<LoadedGameMessage>(OnLoadedGameMessageHandler);
            _messageBus.AddListener<CheckTaskCanFinishMessage>(OnCheckTaskCanFinishMessage);
        }

        private async void OnLoadedGameMessageHandler(LoadedGameMessage message)
        {
            Log.Debug("GameFieldService OnLoadedGameMessageHandler");
            FieldPresenter field = await _fieldFactory.CreateField(_gameServiceModel.MainCanvas.transform);
            field.Initialize();
        }

        public void OnCheckTaskCanFinishMessage(CheckTaskCanFinishMessage msg)
        {
            var taskModel = DiContainer.Get<TaskModel>();
            var fieldModel = DiContainer.Get<FieldLogicModel>();
            var taskCanFinishDic = new Dictionary<int, bool>();
            foreach (var task in taskModel.Tasks) // every task
            {
                taskCanFinishDic[task.Id] = true;
                var needItem = task.NeedItem;
                //都只需要一个
                foreach (var itemId in needItem) // every need item
                {
                    if (!fieldModel.IsContainElement(itemId))
                    {
                        taskCanFinishDic[task.Id] = false;
                        break;
                    }
                }
            }

            UpdateTaskMessage updateTaskMessage = new UpdateTaskMessage();
            updateTaskMessage.taskCanFinishDic = taskCanFinishDic;
            _messageBus.Fire(updateTaskMessage);
        }
    }
}