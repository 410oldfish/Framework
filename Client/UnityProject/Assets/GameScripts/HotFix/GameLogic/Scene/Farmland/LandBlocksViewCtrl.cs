using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace GameLogic
{
    /// <summary>
    /// 农场田地的管理器
    /// </summary>
    public class LandBlocksViewCtrl : MonoBehaviour, IController
    {
        Dictionary<int, LandBlock> _landBlocks = new Dictionary<int, LandBlock>();

        public void BindLandBlocks()
        {
            var children = transform.GetAllDirectChildren();
            foreach (var child in children)
            {
                int landId = int.Parse(child.name);
                var landBlock = new LandBlock(landId);
                _landBlocks[landBlock.LandId] = landBlock;
                landBlock.LandBlockObject = child.gameObject;
            }
        }

        private void OnInitFarmland()
        {
            var farmlandModel = this.GetModel<FarmlandModel>();
            var landDataDic = farmlandModel.LandDataDic;
            foreach (var data in landDataDic)
            {
                InitLandBlock(data.Value);
            }
        }

        private void InitLandBlock(Farm_LandData landData)
        {
            
        }

        public IArchitecture GetArchitecture()
        {
            return GameMgr.Interface;
        }
    }
}