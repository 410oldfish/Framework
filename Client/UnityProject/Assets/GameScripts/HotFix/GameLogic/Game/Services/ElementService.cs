// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Elements;
using MergeIt.Core.FieldElements;
using MergeIt.Core.Messages;
using MergeIt.Core.Saves;
using MergeIt.Core.Services;
using MergeIt.Game.Factories.FieldElement;
using MergeIt.Game.Field;
using MergeIt.Game.Helpers;
using MergeIt.Game.Messages;
using MergeIt.SimpleDI;

namespace MergeIt.Game.Services
{
    public class ElementService : IElementService
    {
        [Introduce]
        private ICurrencyService _currencyService;

        [Introduce]
        private IFieldElementFactory _fieldElementFactory;

        [Introduce]
        private FieldLogicModel _fieldLogicModel;

        [Introduce]
        private IGameFieldService _gameFieldService;

        [Introduce]
        private IMessageBus _messageBus;

        [Introduce]
        private IGameSaveService _saveService;

        [Introduce]
        private UserServiceModel _userServiceModel;

        public void TrySell(IFieldElement fieldElement)
        {
            CurrencySettings costSettings = fieldElement.ConfigParameters.ElementConfig.CommonSettings.SellCostSettings;

            _currencyService.Sell(costSettings);

            GridPoint point = fieldElement.InfoParameters.LogicPosition;

            var removeMessage = new RemoveElementMessage {RemoveAtPoint = point};
            _messageBus.Fire(removeMessage);
            _messageBus.Fire<ResetSelectionMessage>();

            _saveService.Save(GameSaveType.Field);
        }

        public void TryUnlock(IFieldElement fieldElement)
        {
            CurrencySettings costSettings = fieldElement.ConfigParameters.ElementConfig.CommonSettings.UnlockCostSettings;

            if (_currencyService.TryPay(costSettings))
            {
                fieldElement.InfoParameters.IsBlocked = false;

                _saveService.Save(GameSaveType.Field);

                var unlockMessage = new UnlockElementMessage {Element = fieldElement};
                _messageBus.Fire(unlockMessage);
            }
        }

        public void TrySplit(IFieldElement element)
        {
            var freeCellPoint = _gameFieldService.GetFreeCell(element.InfoParameters.LogicPosition);
            if (freeCellPoint == null)
            {
                return;
            }

            GridPoint secondPoint = freeCellPoint.Value;

            var splitResult = TrySplit(element, secondPoint);
            if (splitResult != null)
            {
                CurrencySettings costSettings = element.ConfigParameters.ElementConfig.CommonSettings.SplitCostSettings;

                if (_currencyService.TryPay(costSettings))
                {
                    SendSplitMessage(splitResult.Value);

                    _saveService.Save(GameSaveType.Field);
                    _messageBus.Fire<ResetSelectionMessage>();
                }
            }
        }
        
        public bool TrySplitByFuncItem(IFieldElement element)
        {
            var freeCellPoint = _gameFieldService.GetFreeCell(element.InfoParameters.LogicPosition);
            if (freeCellPoint == null)
            {
                return false;
            }

            GridPoint secondPoint = freeCellPoint.Value;

            var splitResult = TrySplit(element, secondPoint);
            if (splitResult != null)
            {
                SendSplitMessage(splitResult.Value);

                _saveService.Save(GameSaveType.Field);
                _messageBus.Fire<ResetSelectionMessage>();
                
                return true;
            }
            return false;
        }

        public bool TryCopy(IFieldElement element)
        {
            var freeCellPoint = _gameFieldService.GetFreeCell(element.InfoParameters.LogicPosition);
            if (freeCellPoint == null)
            {
                return false;
            }

            GridPoint secondPoint = freeCellPoint.Value;

            var message = new CreateElementMessage
            {
                FromPosition = _fieldLogicModel.GetPosition(element.InfoParameters.LogicPosition),
                ToPoint = secondPoint,
                NewElement = element
            };
            
            _messageBus.Fire(message);
            _saveService.Save(GameSaveType.Field);
            _messageBus.Fire<ResetSelectionMessage>();
            return true;
        }

        private (IFieldElement element1, IFieldElement element2)? TrySplit(IFieldElement fieldElement, GridPoint point2)
        {
            ElementConfig previousInEvolution = fieldElement.GetPreviousInEvolution();
            if (previousInEvolution)
            {
                IFieldElement splitElement1 =
                    _fieldElementFactory.CreateFieldElement(previousInEvolution, fieldElement.InfoParameters.LogicPosition);
                IFieldElement splitElement2 = _fieldElementFactory.CreateFieldElement(previousInEvolution, point2);

                return (splitElement1, splitElement2);
            }

            return null;
        }

        private void SendSplitMessage((IFieldElement element1, IFieldElement element2) splitResult)
        {
            var splitResultMessage = new SplitElementMessage
            {
                SplitElement1 = splitResult.element1,
                SplitElement2 = splitResult.element2
            };

            _messageBus.Fire(splitResultMessage);
        }
    }
}