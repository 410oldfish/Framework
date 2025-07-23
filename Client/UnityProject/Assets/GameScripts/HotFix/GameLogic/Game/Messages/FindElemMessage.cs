using MergeIt.Core.FieldElements;
using MergeIt.Core.Messages;
using UnityEngine;

namespace MergeIt.Game.Messages
{
    public class FindElementMessage : IMessage
    {
        public int ElementId { get; set; }
    }

}
