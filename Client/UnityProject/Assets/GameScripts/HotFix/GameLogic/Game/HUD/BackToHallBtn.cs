using System.Collections;
using System.Collections.Generic;
using MergeIt.Core.Messages;
using MergeIt.SimpleDI;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIt.Game
{
    public class BackToHallBtn : MonoBehaviour
    {
        [Introduce]
        private IMessageBus _messageBus;
        public Button BackToHallBtnObj;
        // Start is called before the first frame update
        void Start()
        {
            _messageBus = DiContainer.Get<IMessageBus>();
            BackToHallBtnObj.onClick.AddListener(() =>
            {
                _messageBus.Fire(new BackToHallMessage());
            });
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
