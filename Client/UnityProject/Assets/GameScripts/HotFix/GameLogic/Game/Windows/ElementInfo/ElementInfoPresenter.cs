// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using MergeIt.Core.Configs.Data;
using MergeIt.Core.Configs.Elements;
using MergeIt.Core.Configs.Types;
using MergeIt.Core.Messages;
using MergeIt.Core.Schemes;
using MergeIt.Core.Services;
using MergeIt.Core.WindowSystem.Data;
using MergeIt.Core.WindowSystem.Windows;
using MergeIt.Game.Factories.ElementInfo;
using MergeIt.Game.Messages;
using MergeIt.SimpleDI;
using TEngine;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIt.Game.Windows.ElementInfo
{
    public class ElementInfoPresenter : WindowPresenter<ElementInfoWindow, ElementInfoModel>
    {
        const string elemLv_beverage = "elemLv_beverage";
        const string elemLv_food = "elemLv_food";
        const string elemLv_fruit = "elemLv_fruit";
        const string elemLv_fur = "elemLv_fur";
        const string elemLv_sea = "elemLv_sea";
        
        private IConfigsService _configsService;

        private readonly Dictionary<ElementInfoItemComponent, ElementConfig> _elementConfigs = new();
        private ElementInfoArgs _elementInfoArgs;
        private IElementInfoFactory _elementInfoFactory;
        private IEvolutionsService _evolutionsService;
        private RectTransform _prevSetUpPanel;
        private ElementConfig _selectedConfig;

        protected override void OnInitialize(IWindowArgs args = null)
        {
            base.OnInitialize(args);

            _elementInfoFactory = DiContainer.Get<IElementInfoFactory>();
            _configsService = DiContainer.Get<IConfigsService>();
            _evolutionsService = DiContainer.Get<IEvolutionsService>();

            _elementInfoArgs = args as ElementInfoArgs;

            if (_elementInfoArgs != null)
            {
                _selectedConfig = _elementInfoArgs.ElementConfig;
                SchemeObject evolutionConfig = _configsService.LevelConfig.EvolutionsScheme;
                EvolutionData evolutionData = evolutionConfig.Evolution.FirstOrDefault(x => x.Chain.Contains(_selectedConfig));

                View.WindowTitleText.text = evolutionData?.Name;
                SetupTitleImg(evolutionData?.Name);
                
                var elementChain = _evolutionsService.GetEvolutionChain(evolutionData);
                var generatorsChain = _evolutionsService.GetGeneratedBy(elementChain[0]);
                var generates = _evolutionsService.GetGenerates(_selectedConfig);

                bool lockedElements = _selectedConfig.Type == ElementType.Generator && _selectedConfig.GeneratorSettings.GenerateBlocked;

                SetupPanel(View.EvolutionPanel, elementChain, _prevSetUpPanel, evolutionData.Description, evolutionData, true);
                SetupPanel(View.GeneratesPanel, generates, _prevSetUpPanel, "Produce:", lockedItems: lockedElements);
                SetupPanel(View.CreatedFromPanel, generatorsChain, _prevSetUpPanel, "Created from:");

                CalculateScrollSize(View.EvolutionPanel.RectTransform, View.GeneratesPanel.RectTransform, View.CreatedFromPanel.RectTransform);
                
                View.FindButton.onClick.AddListener(() =>
                {
                    foreach (var v in generatorsChain)
                    {
                        SendFindElemMessage(int.Parse(v.Id));
                    }
                });
            }
        }

        private void SendFindElemMessage(int elemId)
        {
            FindElementMessage message = new FindElementMessage();
            message.ElementId = elemId;
            DiContainer.Get<IMessageBus>().Fire(message);
            
            WindowSystem.CloseWindow(this);
        }
        
        void SetupTitleImg(string s)
        {
            string targetImgPath = "";
            if(s.Contains("Beverage"))
            {
                targetImgPath = elemLv_beverage;
            }
            else if(s.Contains("Food"))
            {
                targetImgPath = elemLv_food;
            }
            else if(s.Contains("Fruit"))
            {
                targetImgPath = elemLv_fruit;
            }
            else if(s.Contains("Fur"))
            {
                targetImgPath = elemLv_fur;
            }
            else if(s.Contains("Sea"))
            {
                targetImgPath = elemLv_sea;
            }

            SetTitleImage(targetImgPath).Forget();
        }
        
        async UniTask SetTitleImage(string path)
        {
            //给view里的titleImg赋值
            //用gamemodule.resources.load加载图片
            //加载完成后赋值给titleImg
            var texture = await GameModule.Resource.LoadAssetAsync<Texture2D>(path);
            View.TitleImg.sprite =Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); 
        }
        
        protected override void OnDestroyWindow()
        {
            base.OnDestroyWindow();

            foreach (var elementConfig in _elementConfigs)
            {
                if (elementConfig.Key)
                {
                    elementConfig.Key.ClickEvent -= OnClickElement;
                }
            }
            View.FindButton.onClick.RemoveAllListeners();
            _elementConfigs.Clear();
        }

        private async UniTaskVoid SetupPanel(ElementInfoItemsPanelComponent panelComponent, List<ElementConfig> elementConfigs,
            RectTransform prevPanelTransform, string panelTitle, EvolutionData evolutionData = null, bool showNumbers = false,
            bool lockedItems = false)
        {
            if (elementConfigs.Count > 0)
            {
                panelComponent.gameObject.SetActive(true);
                panelComponent.SetSpacing(View.SpacingInsidePanels);
                panelComponent.SetTitle(panelTitle);

                //SetupPanelSize(panelComponent, elementConfigs.Count, prevPanelTransform);

                for (int i = 0; i < elementConfigs.Count; i++)
                {
                    ElementConfig elementConfig = elementConfigs[i];
                    ElementInfoItemComponent item = null;
                    bool isDiscovered = true;

                    if (evolutionData != null)
                    {
                        int index = evolutionData.Chain.IndexOf(elementConfig) + 1;
                        int progress = _evolutionsService.GetEvolutionProgress(evolutionData.Id);
                        isDiscovered = index <= progress;
                        
                        if (isDiscovered)
                        {
                            item = await CreateComponent(elementConfig, lockedItems);
                        }
                        else
                        {
                            item = await _elementInfoFactory.CreateUnknownElementWindowItem();
                        }
                    }
                    else
                    {
                        item = await CreateComponent(elementConfig, lockedItems);
                    }

                    if (item)
                    {
                        bool isGenerator = isDiscovered && elementConfig.Type == ElementType.Generator;
                        panelComponent.SetItem(item.transform);

                        item.Setup(isGenerator, elementConfig == _selectedConfig, showNumbers ? i + 1 : -1, i != elementConfigs.Count -1);
                    }
                }

                _prevSetUpPanel = panelComponent.RectTransform;
            }
            else
            {
                panelComponent.gameObject.SetActive(false);
            }
        }

        private async UniTask<ElementInfoItemComponent> CreateComponent(ElementConfig elementConfig, bool isLocked = false)
        {
            ElementInfoItemComponent item = await _elementInfoFactory.CreateElementWindowItem(elementConfig, isLocked: isLocked);
            item.ClickEvent += OnClickElement;

            _elementConfigs[item] = elementConfig;

            return item;
        }

        private void SetupPanelSize(ElementInfoItemsPanelComponent panel, int itemsCount, RectTransform prevPanelTransform)
        {
            GridLayoutGroup grid = panel.ItemsGrid;
            float cellHeight = grid.cellSize.y;
            int rows = Mathf.CeilToInt((float)itemsCount / grid.constraintCount);

            float resultSize = 0f;
            resultSize += panel.PanelTitle.textInfo.textComponent.rectTransform.rect.height;
            resultSize += View.SpacingInsidePanels;

            float gridSize = 0f;
            gridSize += cellHeight * rows;
            gridSize += grid.spacing.y * (rows - 1);
            gridSize += grid.padding.top + grid.padding.bottom;

            var gridRect = panel.ItemsGrid.GetComponent<RectTransform>();
            Vector2 panelSize = panel.RectTransform.sizeDelta;
            Vector2 gridLayoutSize = gridRect.sizeDelta;

            panelSize.y = resultSize + gridSize;
            gridLayoutSize.y = gridSize;

            gridRect.sizeDelta = gridLayoutSize;
            panel.RectTransform.sizeDelta = panelSize;

            if (prevPanelTransform != default)
            {
                float newPosition = prevPanelTransform.anchoredPosition.y - View.SpacingBetweenPanels - prevPanelTransform.sizeDelta.y;
                panel.RectTransform.anchoredPosition = new Vector2(panel.RectTransform.anchoredPosition.x, newPosition);
            }
        }

        private void CalculateScrollSize(params RectTransform[] panels)
        {
            float sumHeight =
                panels.Sum(x => x.gameObject.activeSelf ? x.sizeDelta.y : 0f) + panels.Length * View.SpacingBetweenPanels;

            View.ScrollContent.sizeDelta = new Vector2(View.ScrollContent.sizeDelta.x, sumHeight);
        }

        private void OnClickElement(ElementInfoItemComponent component)
        {
            if (_elementConfigs.TryGetValue(component, out ElementConfig config) &&
                config != _selectedConfig)
            {
                var args = new ElementInfoArgs
                {
                    ElementConfig = config
                };

                WindowSystem.OpenWindow<ElementInfoPresenter>(true, true, args);
            }
        }
    }
}