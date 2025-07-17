using QFramework;

namespace GameLogic.GameScripts.HotFix.GameLogic.Module.Farmland.Cmd
{
    public class Farmland_UnlockFarmland_Cmd : AbstractCommand
    {
        public int farmlandId;
        
        public Farmland_UnlockFarmland_Cmd(int farmlandId)
        {
            this.farmlandId = farmlandId;
        }
        
        protected override void OnExecute()
        {
            //上传需要解锁的农田ID到服务器
            
            //等待服务器响应
            
            //如果服务器响应成功，更新本地农田状态
            
        }
    }
}