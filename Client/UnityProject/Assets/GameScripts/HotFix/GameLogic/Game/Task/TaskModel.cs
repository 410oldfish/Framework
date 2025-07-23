using System.Collections;
using System.Collections.Generic;
using GameConfig;
using UnityEngine;

namespace MergeIt.Game
{
    public class TaskModel
    {
        List<MergeTask> _tasks = new List<MergeTask>();
        public List<MergeTask> Tasks => _tasks;
        
        public void AddTask(MergeTask task)
        {
            _tasks.Add(task);
        }
        
        public void RemoveTask(MergeTask task)
        {
            _tasks.Remove(task);
        }
        
        public void RemoveTask(int taskId)
        {
            var task = _tasks.Find(t => t.Id == taskId);
            if(task != null)
            {
                _tasks.Remove(task);
            }
        }
    }
}
