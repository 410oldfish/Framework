using MongoDB.Bson.Serialization.Attributes;

namespace Hotfix.Common.Entity.Database;

//玩家基本数据实体
public class PlayerBaseData : Fantasy.Entitas.Entity
{
    public long PlayerId;
    public string NickName;
    public int Lv;
    public int Exp;
}