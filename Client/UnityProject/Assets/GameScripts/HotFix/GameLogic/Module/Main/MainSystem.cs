using Cysharp.Threading.Tasks;
using GameConfig;
using QFramework;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class MainSystem : AbstractSystem
    {
        private FarmlandSystem _farmlandSystem;
        private FarmlandSystem FarmlandSystem => _farmlandSystem ??= this.GetSystem<FarmlandSystem>();
        
        protected override void OnInit()
        {
            RegisterEvent();
        }
        
        void RegisterEvent()
        {
            GameEvent.AddEventListener<string>(EventID.UI_COMMON_MSG, OnCommonMsg);
        }

        void OnCommonMsg(string msg)
        {
            GameModule.UI.ShowUI<Pfb_CommonMsg>(msg);
        }
        
        public void OpenModule(EGameModule module)
        {
            switch (module)
            {
                case EGameModule.Farmland:
                    FarmlandSystem.OpenModule();
                    break;
            }
        }
    }
}