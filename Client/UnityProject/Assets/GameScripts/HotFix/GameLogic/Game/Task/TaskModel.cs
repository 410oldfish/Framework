using System.Collections;
using System.Collections.Generic;
using GameConfig;
using UnityEngine;

namespace MergeIt.Game
{
    public class TaskModel
    {
        List<Task> _tasks = new List<Task>();
        public List<Task> Tasks => _tasks;
        
        public void AddTask(Task task)
        {
            _tasks.Add(task);
        }
        
        public void RemoveTask(Task task)
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
