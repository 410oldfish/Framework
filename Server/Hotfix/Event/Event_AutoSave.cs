using Hotfix.Common.Entity.Base;

namespace Hotfix;

public struct Event_AutoSave<T>  where T : PlayerDataBase
{
    public T Data;

    public Event_AutoSave(T data) => this.Data = data;
}