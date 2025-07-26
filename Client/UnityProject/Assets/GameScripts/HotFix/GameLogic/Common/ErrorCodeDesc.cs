namespace GameLogic.GameScripts.HotFix.GameLogic.Common
{
    public static class ErrorCodeDesc
    {
        public static string GetErrorDesc(string respProto, uint errorCode)
        {
            var errorCodeCfg = ConfigSystem.Instance.Tables.TbErrorCode.Get(respProto);
            if (errorCodeCfg == null)
            {
                return "未知错误 ：未配置协议名";
            }

            switch (errorCode)
            {
                case 0:
                    return errorCodeCfg.Errorcode0;
                
                case 1:
                    return errorCodeCfg.Errorcode1;
                case 2:
                    return errorCodeCfg.Errorcode2;
                case 3:
                    return errorCodeCfg.Errorcode3;
                case 4:
                    return errorCodeCfg.Errorcode4;
                case 5:
                    return errorCodeCfg.Errorcode5;
                default:
                    return "未知错误 ：" + errorCode + "，未配置错误码";
            }
        }
    }
}