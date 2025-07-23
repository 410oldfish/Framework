using System.Collections;
using System.Collections.Generic;
using MergeIt.Core.Messages;
using UnityEngine;

namespace MergeIt.Game.Messages
{
    public class TryFinishTaskMessage : IMessage
    {
        public int taskId { get; set; }
        public Dictionary<int, int> taskItems { get; set; }
    }
    
    public class FinishTaskSuccessMessage : IMessage
    {
        public int taskId { get; set; }
    }
    
    public class CheckTaskCanFinishMessage : IMessage
    {
       
    }
    
    public class UpdateTaskMessage : IMessage
    {
        public Dictionary<int, bool> taskCanFinishDic{ get; set; }
    }
}
