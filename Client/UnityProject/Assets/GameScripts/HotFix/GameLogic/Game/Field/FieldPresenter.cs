// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameConfig;
using GameLogic;
using MergeIt.Core.FieldElements;
using MergeIt.Core.Messages;
using MergeIt.Core.MVP;
using MergeIt.Core.Services;
using MergeIt.Game.Effects;
using MergeIt.Game.Effects.Controllers;
using MergeIt.Game.Effects.Parameters;
using MergeIt.Game.Factories.Field;
using MergeIt.Game.Factories.FieldElement;
using MergeIt.Game.Field.Elements;
using MergeIt.Game.Messages;
using MergeIt.SimpleDI;
using TEngine;
using UnityEngine;

namespace MergeIt.Game.Field
{
    public class FieldPresenter : Presenter<FieldView, FieldModel>
    {
        private readonly FieldLogicModel _fieldLogicModel;
        private readonly IFieldFactory _fieldFactory;
        private readonly IFieldElementVisualFactory _fieldElementVisualFactory;
        private readonly IMessageBus _messageBus;
        private readonly IGameSaveService _saveService;
        private readonly IEffectsFactory _effectsFactory;

        private Dictionary<GridPoint, FieldCellComponent> _cellComponents;
        public FieldPresenter()
        {
            _fieldLogicModel = DiContainer.Get<FieldLogicModel>();
            _fieldFactory = DiContainer.Get<IFieldFactory>();
            _fieldElementVisualFactory = DiContainer.Get<IFieldElementVisualFactory>();
            _messageBus = DiContainer.Get<IMessageBus>();
            _saveService = DiContainer.Get<IGameSaveService>();
            _effectsFactory = DiContainer.Get<IEffectsFactory>();
        }

        protected override void OnDispose()
        {
            _messageBus.RemoveListener<SelectedElementMessage>(OnSelectedElementMessageHandler);
            _messageBus.RemoveListener<ResetSelectionMessage>(OnResetSelectionMessageHandler);
            _messageBus.RemoveListener<MergeElementsMessage>(OnMergeElementsMessageHandler);
            _messageBus.RemoveListener<UnlockElementMessage>(OnUnlockElementMessageHandler);
            _messageBus.RemoveListener<ClearSandMessage>(OnClearSandMessageHandler);
            _messageBus.RemoveListener<SplitElementMessage>(OnSplitResultMessageHandler);
            _messageBus.RemoveListener<ResetPositionMessage>(OnResetPositionsMessageHandler);
            _messageBus.RemoveListener<SwapElementsMessage>(OnSwapElementsMessageHandler);
            _messageBus.RemoveListener<CreateElementMessage>(OnCreateElementMessageHandler);
            _messageBus.RemoveListener<RemoveElementMessage>(OnRemoveElementMessageHandler);
        }

        public async UniTaskVoid Initialize()
        {
            _messageBus.AddListener<SelectedElementMessage>(OnSelectedElementMessageHandler);
            _messageBus.AddListener<ResetSelectionMessage>(OnResetSelectionMessageHandler);
            _messageBus.AddListener<MergeElementsMessage>(OnMergeElementsMessageHandler);
            _messageBus.AddListener<SplitElementMessage>(OnSplitResultMessageHandler);
            _messageBus.AddListener<UnlockElementMessage>(OnUnlockElementMessageHandler);
            _messageBus.AddListener<ClearSandMessage>(OnClearSandMessageHandler);
            _messageBus.AddListener<ResetPositionMessage>(OnResetPositionsMessageHandler);
            _messageBus.AddListener<SwapElementsMessage>(OnSwapElementsMessageHandler);
            _messageBus.AddListener<CreateElementMessage>(OnCreateElementMessageHandler);
            _messageBus.AddListener<RemoveElementMessage>(OnRemoveElementMessageHandler);
            _messageBus.AddListener<TryFinishTaskMessage>( OnTryFinishTaskMessageHandler);
            
            _cellComponents = _fieldLogicModel.CellComponents;
            
            var fieldRect = View.CellsContainer.rect;
            var fieldRectSize = fieldRect.size;

            _fieldLogicModel.FieldRect = fieldRect;

            int fieldWidth = _fieldLogicModel.FieldWidth;
            int fieldHeight = _fieldLogicModel.FieldHeight;

            float sizeByWidth = (fieldRectSize.x - (fieldWidth - 1) * View.CellSpacing) / fieldWidth;
            float sizeByHeight = (fieldRectSize.y - (fieldHeight - 1) * View.CellSpacing) / fieldHeight;

            float cellSize = Mathf.Min(sizeByHeight, sizeByWidth);

            bool horizontalAlign = Mathf.Approximately(cellSize, sizeByHeight);
            float offset = 0f;

            if (horizontalAlign)
            {
                offset = (fieldRectSize.x - ((fieldWidth - 1) * View.CellSpacing + cellSize * fieldWidth)) / 2f;
            }
            else
            {
                offset = (fieldRectSize.y - ((fieldHeight - 1) * View.CellSpacing + cellSize * fieldHeight)) / 2f;
            }

            _fieldLogicModel.CellSize = cellSize;
            _fieldLogicModel.PositionOffset = offset;
            _fieldLogicModel.CellSpacing = View.CellSpacing;

            for (int i = 0; i < fieldHeight; i++)
            {
                for (int j = 0; j < fieldWidth; j++)
                {
                    var point = GridPoint.Create(i, j);
                    var cell = await _fieldFactory.CreateFieldCell(View.CellsContainer.transform, point);
                    cell.RectTransform.localScale = Vector3.one;
                    cell.RectTransform.pivot = Vector2.zero;
                    cell.RectTransform.sizeDelta = new Vector2(cellSize, cellSize);
                    cell.RectTransform.anchoredPosition = horizontalAlign
                        ? new Vector2(offset + j * (cellSize + View.CellSpacing), (fieldHeight - 1 - i) * (cellSize + View.CellSpacing))
                        : new Vector2(j * (cellSize + View.CellSpacing), offset + i * (cellSize + View.CellSpacing));

                    cell.gameObject.name = $"FieldCell [{point.X}, {point.Y}]";

                    _cellComponents[point] = cell;

                    if (_fieldLogicModel.FieldElements.TryGetValue(point, out var fieldElement))
                    {
                        var fieldElementPresenter = _fieldElementVisualFactory.CreateFieldElement(fieldElement);
                        cell.AttachObject(fieldElementPresenter);
                    }
                }
            }
        }

        private void OnTryFinishTaskMessageHandler(TryFinishTaskMessage message)
        {
            var taskItems = message.taskItems;
            var findElem = new List<GridPoint>();
            foreach (var item in taskItems)
            {
               var curFindElems = FindTaskItem(item.Key, item.Value);
               if (curFindElems == null)
               {
                   Log.Debug("not enough elements : " + item.Key);
                   return;
               }
               findElem.AddRange(curFindElems);
            }

            foreach (var elem in findElem)
            {
                RemoveElement(elem);
            }
            _messageBus.Fire(new FinishTaskSuccessMessage() {taskId = message.taskId});
        }
        
        public List<GridPoint> FindTaskItem(int id, int count)
        {
            int findCount = 0;
            List<GridPoint> findPoints = new List<GridPoint>();
            foreach (var elem in _fieldLogicModel.FieldElements)
            {
                if (elem.Value.InfoParameters.Id == id)
                {
                    findPoints.Add(elem.Key);
                    findCount++;
                }
                if(findCount == count)
                {
                    break;
                }
            }
            if (findCount == count)
            {
                return findPoints;
            }
            return null;
        }

        private void OnSelectedElementMessageHandler(SelectedElementMessage message)
        {
            UpdateSelection(message.Point);
        }

        private void OnResetSelectionMessageHandler(ResetSelectionMessage message)
        {
            UpdateSelection(GridPoint.Default);
        }

        private void OnMergeElementsMessageHandler(MergeElementsMessage message)
        {
            GridPoint toPoint = message.NewElement.InfoParameters.LogicPosition;
            FieldCellComponent toCell = _cellComponents[toPoint];

            toCell.DetachObject();
            _fieldLogicModel.FieldElements.Remove(message.From);
            
            var fieldElementPresenter = _fieldElementVisualFactory.CreateFieldElement(message.NewElement);
            toCell.AttachObject(fieldElementPresenter);

            fieldElementPresenter.Select(true);
            _fieldLogicModel.FieldElements[toPoint] = message.NewElement;

            DiContainer.Get<EffectHelper>().LoadAndInstantiateEffect("MergeEft", fieldElementPresenter.Transform,
                Vector2.zero, Vector2.one * 30, "img_eft_action", "action_Material-Additive").Forget();
        }

        private void OnSplitResultMessageHandler(SplitElementMessage message)
        {
            _messageBus.Fire<ResetHintsMessage>();
            
            IFieldElement splitElement1 = message.SplitElement1;
            IFieldElement splitElement2 = message.SplitElement2;

            GridPoint initPoint = splitElement1.InfoParameters.LogicPosition;
            GridPoint secondPoint = splitElement2.InfoParameters.LogicPosition;

            FieldCellComponent initialCell = _cellComponents[initPoint];
            FieldCellComponent secondCell = _cellComponents[secondPoint];

            IFieldElementPresenter fromPresenter = initialCell.FieldElementPresenter;
            Vector3 fromPosition = initialCell.ObjectContainer.position;

            fromPresenter.Remove();
            initialCell.DetachObject();
            _fieldLogicModel.FieldElements.Remove(initPoint);
            
            IFieldElementPresenter fieldElementPresenter1 = _fieldElementVisualFactory.CreateFieldElement(splitElement1);
            initialCell.AttachObject(fieldElementPresenter1);
            _fieldLogicModel.FieldElements[initPoint] = splitElement1;
            
            IFieldElementPresenter fieldElementPresenter2 = _fieldElementVisualFactory.CreateFieldElement(splitElement2);
            secondCell.AttachObject(fieldElementPresenter2, false);
            _fieldLogicModel.FieldElements[secondPoint] = splitElement2;
            fieldElementPresenter2.Transform.position = fromPosition;

            var moveParameters = new MoveEffectParameters(fromPosition, secondCell.ObjectContainer.position);
            _effectsFactory.CreateEffect<MoveEffectWithPresenter>(fieldElementPresenter2, moveParameters);
            _effectsFactory.CreateEffect<JumpEffectWithPresenter>(fieldElementPresenter1);
        }

        private void OnUnlockElementMessageHandler(UnlockElementMessage message)
        {
            GridPoint point = message.Element.InfoParameters.LogicPosition;

            if (_cellComponents.TryGetValue(point, out FieldCellComponent cellComponent) &&
                cellComponent.FieldElementPresenter != null)
            {
                cellComponent.FieldElementPresenter.SetLock(false);
            }
        }
        
        private void OnClearSandMessageHandler(ClearSandMessage message)
        {
            GridPoint point = message.Element.InfoParameters.LogicPosition;
            if (_cellComponents.TryGetValue(point, out FieldCellComponent cellComponent) &&
                cellComponent.FieldElementPresenter != null)
            {
                cellComponent.FieldElementPresenter.ClearSand();
            }
        }

        private void OnSwapElementsMessageHandler(SwapElementsMessage message)
        {
            GridPoint fromPoint = message.From;
            GridPoint toPoint = message.To;

            FieldCellComponent fromCell = _cellComponents[fromPoint];
            FieldCellComponent toCell = _cellComponents[toPoint];

            IFieldElementPresenter fromPresenter = fromCell.FieldElementPresenter;
            IFieldElementPresenter toPresenter = toCell.FieldElementPresenter;

            fromCell.DetachObject();
            toCell.DetachObject();

            fromCell.AttachObject(toPresenter);
            toCell.AttachObject(fromPresenter);
            
            toPresenter?.Select(false);
            fromPresenter?.Select(true);
        }

        private void OnResetPositionsMessageHandler(ResetPositionMessage message)
        {
            GridPoint fromPoint = message.From;
            FieldCellComponent fromCell = _cellComponents[fromPoint];
            IFieldElementPresenter fromPresenter = fromCell.FieldElementPresenter;

            fromPresenter.ResetPosition();
            fromPresenter.Select(true);
        }

        private void OnCreateElementMessageHandler(CreateElementMessage message)
        {
            if (!_cellComponents.ContainsKey(message.ToPoint))
            {
                return;
            }
            FieldCellComponent toCell = _cellComponents[message.ToPoint];

            IFieldElementPresenter fieldElementPresenter = _fieldElementVisualFactory.CreateFieldElement(message.NewElement);
            toCell.AttachObject(fieldElementPresenter, false);
            _fieldLogicModel.FieldElements[message.ToPoint] = message.NewElement;
            
            Transform transform = fieldElementPresenter.Transform;

            if (message.FromPosition != null)
            {
                transform.position = message.FromPosition.Value;

                var parameters = new MoveEffectParameters(transform.position, toCell.ObjectContainer.position);
                _effectsFactory.CreateEffect<MoveEffectWithPresenter>(fieldElementPresenter, parameters);
            }
        }

        private void OnRemoveElementMessageHandler(RemoveElementMessage message)
        {
            RemoveElement(message.RemoveAtPoint);
        }

        //Find Task Item
        public void OnClickTaskItem(int id, int count)
        {
            
        }
        
        private void RemoveElement(GridPoint point)
        {
            FieldCellComponent cell = _cellComponents[point];
            IFieldElementPresenter cellPresenter = cell.FieldElementPresenter;
            cell.ToggleSelector(false);
            cellPresenter.Remove();
            cell.DetachObject();
            _fieldLogicModel.FieldElements.Remove(point);
        }

        private void UpdateSelection(GridPoint point)
        {
            var cells = _cellComponents;

            if (cells != null)
            {
                foreach (var fieldCellComponent in cells)
                {
                    bool activateCell = fieldCellComponent.Key.Equals(point);
                    fieldCellComponent.Value.ToggleSelector(activateCell);
                }
            }
        }
    }
}