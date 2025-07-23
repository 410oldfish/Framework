// Copyright (c) 2024, Awessets

using Cysharp.Threading.Tasks;
using MergeIt.Core.FieldElements;
using MergeIt.Core.Services;
using MergeIt.Game.Field;
using MergeIt.SimpleDI;
using TEngine;
using UnityEngine;
using YooAsset;

namespace MergeIt.Game.Factories.Field
{
    public class FieldFactory : IFieldFactory
    {
        public const string FieldGameObjectPath = "FieldContainer";
        public const string FieldCellObjectOddPath = "FieldCellOdd";
        public const string FieldCellObjectEvenPath = "FieldCellEven";
        
        [Introduce]
        private IResourcesLoaderService _resourcesLoaderService;

        public async UniTask<FieldPresenter> CreateField(Transform parent)
        {
            var fieldViewPrefab = await GameModule.Resource.LoadAssetAsync<GameObject>(FieldGameObjectPath);//_resourcesLoaderService.GetObject<FieldView>(FieldGameObjectPath);

            var fieldViewObject = Object.Instantiate(fieldViewPrefab.gameObject, parent);
            
            fieldViewObject.TryGetComponent(out FieldView fieldView);

            if (fieldView)
            {
                var presenter = new FieldPresenter();
                presenter.Initialize(fieldView);
                
                fieldView.Initialize();

                return presenter;
            }

            return null;
        }
        
        public async UniTask<FieldCellComponent> CreateFieldCell(Transform parent, GridPoint gridPoint)
        {
            int sum = (gridPoint.X + gridPoint.Y) % 2;
            var fieldCellPrefab = sum == 0 ? 
                await GameModule.Resource.LoadAssetAsync<GameObject>(FieldCellObjectEvenPath) :
                await GameModule.Resource.LoadAssetAsync<GameObject>(FieldCellObjectOddPath);

            var fieldCellObject = Object.Instantiate(fieldCellPrefab.gameObject, parent);

            fieldCellObject.TryGetComponent(out FieldCellComponent fieldCell);

            if (fieldCell)
            {
                fieldCell.Initialize(gridPoint);
            }
            
            return fieldCell;
        }
    }
}