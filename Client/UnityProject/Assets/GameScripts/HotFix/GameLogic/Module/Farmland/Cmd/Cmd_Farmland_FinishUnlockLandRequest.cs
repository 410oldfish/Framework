using System;
using System.Windows.Input;
using Fantasy;
using GameConfig.farm;
using QFramework;
using TEngine;
using ICommand = QFramework.ICommand;

namespace GameLogic
{
    /// <summary>
    /// 在解锁时间到达后，手动完成解锁
    /// </summary>
    public class Cmd_Farmland_FinishUnlockLandRequest : ICommand
    {
        public int LandId;
        public IArchitecture GetArchitecture()
        {
            return GameMgr.Interface;
        }

        public void SetArchitecture(IArchitecture architecture)
        {
        }

        public async void Execute()
        {
            var resp = await GameModule.Network.CallRoute<C2Center_Farmland_FinishUnlockLand_Req, Center2C_Farmland_FinishUnlockLand_Resp>(
                new C2Center_Farmland_FinishUnlockLand_Req(){LandId = this.LandId});
            if (resp.ErrorCode != 0)
            {
                Log.Error("完成解锁失败, 错误码: " + resp.ErrorCode);
                return;
            }
            
            ELandType landType = (ELandType)resp.LandType;
            var farmlandModel = this.GetModel<FarmlandModel>();
            farmlandModel.UpdateLandType(LandId, landType);
            GameEvent.Send(EventID.FARMLAND_LAND_TYPE_CHANGE, LandId, landType);
            GameEvent.Send(EventID.FARMLAND_LAND_UNLOCK_FINISH, LandId);
        }
    }
}