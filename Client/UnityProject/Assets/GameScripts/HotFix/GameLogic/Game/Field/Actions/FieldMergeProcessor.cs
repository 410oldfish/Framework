// Copyright (c) 2024, Awessets

using System;
using MergeIt.Core.Configs.Elements;
using MergeIt.Core.Configs.Types;
using MergeIt.Core.FieldElements;
using MergeIt.Core.Saves;
using MergeIt.Core.Services;
using MergeIt.Game.Factories.FieldElement;
using MergeIt.Game.Helpers;
using MergeIt.Game.Messages;
using MergeIt.SimpleDI;
using UnityEngine;

namespace MergeIt.Game.Field.Actions
{
    public class FieldMergeProcessor : FieldActionProcessorBase
    {
        private readonly IFieldElementFactory _fieldElementFactory = DiContainer.Get<IFieldElementFactory>();
        private readonly IEvolutionsService _evolutionsService = DiContainer.Get<IEvolutionsService>();
        private readonly FieldLogicModel _fieldLogicModel = DiContainer.Get<FieldLogicModel>();

        public override void ProcessClick(FieldCellComponent cellComponent)
        {
            base.ProcessClick(cellComponent);

            var presenter = cellComponent.FieldElementPresenter;
            if (presenter != null)
            {
                presenter.GetModel().ClicksCount++;

                if (!presenter.IsSanded() && !presenter.GetModel().Selected)
                {
                    presenter.Select(true);
                }
            }
        }

        public override void ProcessEndDrag(GridPoint fromPoint, GameObject toGameObject)
        {
            base.ProcessEndDrag(fromPoint, toGameObject);

            GridPoint toPoint = GridPoint.Default;

            if (toGameObject)
            {
                if (toGameObject.TryGetComponent(out FieldCellComponent toCell))
                {
                    toPoint = toCell.Point;

                    if (fromPoint.Equals(toPoint))
                    {
                        SendResetPositionMessage(fromPoint);

                        return;
                    }

                    IFieldElementPresenter toPresenter = toCell.FieldElementPresenter;

                    if (toPresenter != null)
                    {
                        if (toPresenter.GetModel() != null && toPresenter.GetModel().SandCount >= 2)
                        {
                            SendResetPositionMessage(fromPoint);
                            return;
                        }

                        //Func Block Check
                        if (TrySplitAndCopy(fromPoint, toPoint))
                        {
                            SendRemoveElement(fromPoint);
                            return;
                        }

                        var mergedResult = TryMerge(fromPoint, toPoint);
                        if (mergedResult != null)
                        {
                            SendRemoveElement(fromPoint);
                            SendRemoveElement(toPoint);
                            SendMergeMessage(fromPoint, mergedResult);
                            if (toPresenter.GetModel() != null && toPresenter.GetModel().SandCount == 1)
                            {
                                toPresenter.ClearSandByMerge();
                                //必须是当前合成格有一个沙块，才会触发周围沙块的清理
                                //遍历周围4个格子，如果sandcount大于0，就减1
                                for (int i = -1; i <= 1; i++)
                                {
                                    for (int j = -1; j <= 1; j++)
                                    {
                                        if (Mathf.Abs(i) + Mathf.Abs(j) != 1)
                                        {
                                            continue;
                                        }

                                        GridPoint point = new GridPoint(toPoint.X + i, toPoint.Y + j);
                                        if (point.IsValid(_fieldLogicModel.FieldHeight, _fieldLogicModel.FieldHeight))
                                        {
                                            if (_fieldLogicModel.CellComponents.TryGetValue(point, out FieldCellComponent element))
                                            {
                                                if(element.FieldElementPresenter != null && element.FieldElementPresenter.GetModel() !=null && element.FieldElementPresenter.GetModel().SandCount > 0)
                                                {
                                                    element.FieldElementPresenter.ClearSand();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            
                            SaveService.Save(GameSaveType.Field);
                        }
                        else if (!toPresenter.IsLocked && !toPresenter.IsSanded())
                        {
                            SendSwapMessage(fromPoint, toPoint);
                            SaveService.Save(GameSaveType.Field);
                        }
                        else
                        {
                            SendResetPositionMessage(fromPoint);
                        }
                    }
                    else
                    {
                        SendSwapMessage(fromPoint, toPoint);
                        SaveService.Save(GameSaveType.Field);
                    }
                }
                else
                {
                    SendResetPositionMessage(fromPoint);
                }
            }
            else
            {
                SendResetPositionMessage(fromPoint);
            }
        }

        private bool TrySplitAndCopy(GridPoint fromPoint, GridPoint toPoint)
        {
            IFieldElement firstElement = FieldLogicModel.FieldElements[fromPoint];
            IFieldElement secondElement = FieldLogicModel.FieldElements[toPoint];
            
            var isFuncBlock = firstElement.ConfigParameters.ElementConfig.FuncType == ElementFuncType.Split || firstElement.ConfigParameters.ElementConfig.FuncType == ElementFuncType.Copy;
            if (!isFuncBlock) return false;

            if (firstElement.ConfigParameters.ElementConfig.FuncType == ElementFuncType.Split)
            {
                IElementService elementService = DiContainer.Get<IElementService>();
                bool ret = elementService.TrySplitByFuncItem(secondElement);
                return ret;
            }

            if (firstElement.ConfigParameters.ElementConfig.FuncType == ElementFuncType.Copy)
            {
                IElementService elementService = DiContainer.Get<IElementService>();
                bool ret = elementService.TryCopy(secondElement);
                return ret;
            }

            return false;
        }


        private IFieldElement TryMerge(GridPoint fromPoint, GridPoint toPoint)
        {
            IFieldElement firstElement = FieldLogicModel.FieldElements[fromPoint];
            IFieldElement secondElement = FieldLogicModel.FieldElements[toPoint];

            ElementConfig firstId = firstElement.ConfigParameters.ElementConfig;
            ElementConfig secondId = secondElement.ConfigParameters.ElementConfig;

            if (firstId == secondId)
            {
                ElementConfig nextInEvolution = firstElement.GetNextInEvolution();
                if (nextInEvolution)
                {
                    IFieldElement newElement = _fieldElementFactory.CreateFieldElement(nextInEvolution, toPoint);
                    _evolutionsService.UpdateProgress(newElement.ConfigParameters.ElementConfig);

                    if (firstElement.InfoParameters.Type == ElementType.Generator &&
                        newElement.InfoParameters.Type == ElementType.Generator)
                    {
                        SetupNewGenerator(firstElement, secondElement, newElement);
                    }

                    return newElement;
                }

                return null;
            }

            return null;
        }

        private void SendResetPositionMessage(GridPoint fromPoint)
        {
            var resetMessage = new ResetPositionMessage
            {
                From = fromPoint
            };

            MessageBus.Fire(resetMessage);
        }

        private void SendMergeMessage(GridPoint fromPoint, IFieldElement newElement)
        {
            var resetMessage = new MergeElementsMessage
            {
                From = fromPoint,
                NewElement = newElement
            };

            MessageBus.Fire(resetMessage);
        }

        private void SendSwapMessage(GridPoint fromPoint, GridPoint toPoint)
        {
            var resetMessage = new SwapElementsMessage
            {
                From = fromPoint,
                To = toPoint
            };

            MessageBus.Fire(resetMessage);
        }

        private void SendRemoveElement(GridPoint elementPosition)
        {
            var message = new RemoveElementMessage
            {
                RemoveAtPoint = elementPosition
            };

            MessageBus.Fire(message);
        }

        private void SetupNewGenerator(IFieldElement generator1, IFieldElement generator2, IFieldElement newGenerator)
        {
            if (!newGenerator.ConfigParameters.ElementConfig.GeneratorSettings.Charged)
            {
                var parameters1 = generator1.GeneratorParameters;
                var parameters2 = generator2.GeneratorParameters;

                int chargedCount = parameters1.ChargedCount + parameters2.ChargedCount;
                int availableCount = parameters1.AvailableToDrop + parameters2.AvailableToDrop;
                int allCount = chargedCount + availableCount;

                newGenerator.GeneratorParameters.ChargedCount = allCount;
                newGenerator.UpdateGenerator();
            }
        }
    }
}