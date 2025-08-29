using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameConfig.farm;
using QFramework;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class FarmlandView : MonoBehaviour, IController
    {
        Dictionary<int, FarmlandBlockView> _landsMap = new Dictionary<int, FarmlandBlockView>();
        private FarmlandModel _farmlandModel;

        private void Start()
        {
            Init();
            RegisterEvents();
        }

        public void Init()
        {
            BindLands();
            _farmlandModel = this.GetModel<FarmlandModel>();
        }

        void BindLands()
        {
            var landBlockTileMap = this.transform.GetComponentInTargetChild<Transform>("Tilemap_LandBlocks");
            var landBlockTiles = landBlockTileMap.GetAllDirectChildren();
            foreach (var tile in landBlockTiles)
            {
                _landsMap[int.Parse(tile.name)] = tile.GetComponent<FarmlandBlockView>();
            }
        }
        
        void RegisterEvents()
        {
            GameEvent.AddEventListener(EventID.FARMLAND_LAND_UPDATE_DATA_ALL, RefreshAllLands);
            GameEvent.AddEventListener<int>(EventID.FARMLAND_LAND_UPDATE_DATA, RefreshLand);
            GameEvent.AddEventListener<int, ELandType>(EventID.FARMLAND_LAND_TYPE_CHANGE, OnLandTypeChange);
        }

        private void OnLandTypeChange(int landId, ELandType landType)
        {
            this._landsMap[landId].ChangeLandType(landType);
        }

        public void RefreshAllLands()
        {
            var landIds = _farmlandModel.GetAllLandIds();
            var allLandIds = new List<int>(_landsMap.Keys);
            //有数据的农田块
            foreach (var Id in landIds)
            {
                RefreshLand(Id);
            }
            //未解锁的农田块
            var lockedLandIds = allLandIds.FindAll(id => !landIds.Contains(id));
            foreach (var Id in lockedLandIds)
            {
                this._landsMap[Id].SetLockedBlock(Id).Forget();
            }
        }

        public void RefreshLand(int landId)
        {
            if (!_landsMap.ContainsKey(landId))
            {
                Log.Error($"没有找到对应的农田块, landId: {landId}");
                return;
            }
            var farmlandBlockData = _farmlandModel.GetLandData(landId);
            this._landsMap[landId].SetBlock(landId, farmlandBlockData).Forget();
        }

        public IArchitecture GetArchitecture()
        {
            return GameMgr.Interface;
        }
    }
}