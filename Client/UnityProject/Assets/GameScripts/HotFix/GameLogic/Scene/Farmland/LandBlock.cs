using System;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class LandBlock : ISceneClickAble
    {
        public LandBlock(int landId)
        {
            this._landId = landId;
        }

        private int _landId;
        public int LandId
        {
            get => _landId;
            set => _landId = value;
        }
        
        private GameObject _landBlockObject;
        public GameObject LandBlockObject
        {
            get => _landBlockObject;
            set => _landBlockObject = value;
        }

        public void Init(Farm_LandData landData)
        {
            
        }

        public void OnClick()
        {
            GameEvent.Send(EventID.FARMLAND_LAND_BLOCK_CLICK, _landId);
        }
    }
}