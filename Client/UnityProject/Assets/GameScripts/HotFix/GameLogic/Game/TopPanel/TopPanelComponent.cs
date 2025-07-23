// Copyright (c) 2024, Awessets

using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MergeIt.Core.Messages;
using MergeIt.Core.Services;
using MergeIt.Core.WindowSystem;
using MergeIt.Game.Configs.Services;
using MergeIt.Game.ElementsStock;
using MergeIt.Game.Messages;
using MergeIt.Game.Windows.ElementInfo;
using MergeIt.SimpleDI;
using TEngine;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using YooAsset;

namespace MergeIt.Game.TopPanel
{
    public class TopPanelComponent : MonoBehaviour
    {
        [SerializeField]
        private ElementsStockComponent _stockComponent;

        private IMessageBus _messageBus;
        public Transform taskContent;
        Dictionary<int, GameObject> taskObjs = new Dictionary<int, GameObject>();
        private TaskModel _taskModel;
        private void Start()
        {
            Log.Debug("TopPanelComponent Start");

            _messageBus = DiContainer.Get<IMessageBus>();
            _messageBus.AddListener<LoadedGameMessage>(OnLoadedGameMessageHandler);
            _messageBus.AddListener<FinishTaskSuccessMessage>( OnFinishTaskSuccessMessageHandler);
            _messageBus.AddListener<UpdateTaskMessage>(OnUpdateTaskMessage);
        }

        private void OnUpdateTaskMessage(UpdateTaskMessage msg)
        {
            var taskDic = msg.taskCanFinishDic;
            foreach (var task in taskDic)
            {
                if(taskObjs.TryGetValue(task.Key, out var taskObj))
                {
                    var btn = taskObj.transform.Find("CanvasTask").Find("finishBtn").GetComponent<Button>();
                    btn.interactable = task.Value;
                    btn.gameObject.SetActive(task.Value);
                }
            }
        }

        void OnFinishTaskSuccessMessageHandler(FinishTaskSuccessMessage message)
        {
            GameObject taskObj;
            if(taskObjs.TryGetValue(message.taskId, out taskObj))
            {
                Destroy(taskObj);
                taskObjs.Remove(message.taskId);
                _taskModel.RemoveTask(message.taskId);
            }
            
            CheckTaskCanFinishMessage checkTaskCanFinishMessage = new CheckTaskCanFinishMessage();
            _messageBus.Fire(checkTaskCanFinishMessage);
        }

        async UniTaskVoid InitTasks()
        {
            Log.Debug("TopPanelComponent InitTasks");
            foreach (var task in ConfigSystem.Instance.Tables.TbTask.DataList)
            {
                _taskModel.AddTask(task);
                
                var taskObj = await InitTask(task.Id);
                if (taskObj != null)
                {
                    taskObjs.Add(task.Id, taskObj);
                }
            }
        }
        
        async UniTask<GameObject> InitTask(int taskId)
        {
            var testTask = ConfigSystem.Instance.Tables.TbTask.Get(taskId);
            if(testTask == null)
            {
                return null;
            }
            var testImg = await GameModule.Resource.LoadAssetAsync<Sprite>(testTask.RoleImage);
            var testPrefab = await GameModule.Resource.LoadAssetAsync<GameObject>("TaskPrefab");
            var testTaskObj = Instantiate(testPrefab, taskContent) as GameObject;
            testTaskObj.transform.Find("role").GetComponent<Image>().sprite = testImg;
            testTaskObj.transform.Find("role").GetComponent<Image>().SetNativeSize();
            
            //need item
            var needItem = testTask.NeedItem;
            var needCount = testTask.NeedCount;
            
            Dictionary<int, int> taskItems = new Dictionary<int, int>();
            for(int i=0;i<needItem.Length;i++)
            {
                taskItems.Add(needItem[i], needCount[i]);
            }
            
            testTaskObj.transform.Find("CanvasTask").Find("finishBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                OnClickFinishBtn(taskId, taskItems);
            });
            testTaskObj.transform.Find("CanvasTask").Find("finishBtn").gameObject.SetActive(false);
            
            for (int i = 0; i < needItem.Length; i++)
            {
                var itemConfig = ConfigSystem.Instance.Tables.TbItem.Get(needItem[i]);
                var imgName = itemConfig.ImgName;
                var img = await GameModule.Resource.LoadAssetAsync<Sprite>(imgName);
                var needItemObj = this.FindChildInHierarchy(testTaskObj, $"CanvasTask/itemList/Grid/item_{i+1}/");
                needItemObj.SetActive(true);
                needItemObj.GetComponent<Image>().sprite = img;     
                needItemObj.transform.Find("count").GetComponent<Text>().text = needCount[i].ToString();
                
                //add listener
                needItemObj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    OnClickTaskItem(itemConfig.Id);
                });
                int index = i;
            }
            for(int i = needItem.Length; i < 3; i++)
            {
                this.FindChildInHierarchy(testTaskObj, $"CanvasTask/itemList/Grid/item_{i+1}/").gameObject.SetActive(false);
            }
            return testTaskObj;
        }
        
        private void OnDestroy()
        {
            _messageBus.RemoveListener<LoadedGameMessage>(OnLoadedGameMessageHandler);
        }
        
        private void OnLoadedGameMessageHandler(LoadedGameMessage _)
        {
            Log.Debug("TopPanelComponent OnLoadedGameMessageHandler");
            _stockComponent.Initialize();
            _taskModel = DiContainer.Get<TaskModel>();
            InitTasks();
            _messageBus.Fire(new CheckTaskCanFinishMessage());
        }

        void OnClickFinishBtn(int taskId,Dictionary<int, int> taskItems)
        {
            TryToFinishTask(taskId, taskItems);
        }
        
        void OnClickTaskItem(int itemId)//from zero
        {
            var infoArgs = new ElementInfoArgs {ElementConfig = DiContainer.Get<IConfigsService>().GetConfig(itemId.ToString())};

            DiContainer.Get<IWindowSystem>().OpenWindow<ElementInfoPresenter>(enableBlackout: true, args: infoArgs);
        }
        
        public GameObject FindChildInHierarchy(GameObject parent, string path)
        {
            var paths = path.Split('/');
            var child = parent.transform;
            for(int i = 0; i < paths.Length; i++)
            {
                child = child.Find(paths[i]);
            }
            return child.gameObject;
        }
        
        private void TryToFinishTask(int taskId,Dictionary<int, int> taskItems)
        {
            var message = new TryFinishTaskMessage()
            {
                taskId = taskId,
                taskItems = taskItems
            };

            _messageBus.Fire(message);
        }
    }
}