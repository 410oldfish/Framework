using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace GameLogic
{
    public class WorkerModel : AbstractModel
    {
        private BindableProperty<Dictionary<int, WorkerData>> workerDataDic = new BindableProperty<Dictionary<int, WorkerData>>();
        protected override void OnInit()
        {
            
        } 
        
        public BindableProperty<Dictionary<int, WorkerData>> WorkerDataDic
        {
            get { return workerDataDic; }
        }
        
        public void SetWorkerDataDic(WorkerData data)
        {
            if (workerDataDic.Value == null)
            {
                workerDataDic.Value = new Dictionary<int, WorkerData>();
            }
            
            if (workerDataDic.Value.ContainsKey(data.Id))
            {
                workerDataDic.Value[data.Id] = data;
            }
            else
            {
                workerDataDic.Value.Add(data.Id, data);
            }
            
            workerDataDic.SetDirty();
        }
    }
}
