
using Cysharp.Threading.Tasks;
using QFramework;
using TEngine;

namespace GameLogic
{
    public class PlayerCtrl : AbstractSystem
    {
        protected override void OnInit()
        {
            
        }

        //请求所有模块的数据
        public async void RequestAllModuleData()
        {
            InitUI();
            
            var playerDataTask = this.SendCommand(new Cmd_Player_GetPlayerDataRequest());
            var  inventoryDataTask = this.SendCommand(new Cmd_Inventory_GetInventoryData_Request());
            var getDataSuccess = await UniTask.WhenAll(playerDataTask, inventoryDataTask);
            
            if(getDataSuccess.Item1 && getDataSuccess.Item2)
            {
                Log.Debug("所有模块数据请求成功");
            }
            else
            {
                Log.Error("部分模块数据请求失败");
            }
        }

        public void InitUI()
        {
            GameModule.UI.CloseUI<Pfb_LoginCenterServer>();
            GameModule.UI.ShowUI<Pfb_MainUI>();
        }
    }
}