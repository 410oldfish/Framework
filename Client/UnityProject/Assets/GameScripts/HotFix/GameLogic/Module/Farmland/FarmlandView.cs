using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using QFramework;
using UnityEngine;

namespace GameLogic
{
    public class FarmlandView : MonoBehaviour, IController
    {
        Dictionary<int, GameObject> _landsMap = new Dictionary<int, GameObject>();
        private FarmlandModel _farmlandModel;

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
                _landsMap[int.Parse(tile.name)] = tile.gameObject;
            }
        }

        public void RefreshAllLands()
        {
            var landsData = _farmlandModel.LandDataDic;
            foreach (var data in landsData)
            {
                RefreshLand(data.Key, data.Value);
            }
        }

        public async UniTask RefreshLand(int landId, Farm_LandData landData)
        {
            var landObj = _landsMap[landId];
            if (landObj == null)
            {
                Debug.LogError($"Land with ID {landId} not found in the map.");
                return;
            }
            //土地类型
            string landTypeImgPath = Misc.GetLandTypeImgPath(landData.LandType);
            landObj.transform.GetComponentInTargetChild<SpriteRenderer>("Land").sprite = await GameModule.Resource.LoadAssetAsync<Sprite>("landTypeImgPath");

            //种子状态
        }

        public IArchitecture GetArchitecture()
        {
            return GameMgr.Interface;
        }
    }
}