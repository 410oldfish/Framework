using System.Collections;
using System.Collections.Generic;
using MergeIt.Core.Messages;
using MergeIt.SimpleDI;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIt.Game
{
    public class HallComponent : MonoBehaviour
    {
        [Introduce]
        private IMessageBus _messageBus;
        public GameObject HallObj;
        public Button HallBtn;
        // Start is called before the first frame update
        void Start()
        {
            _messageBus = DiContainer.Get<IMessageBus>();
            HallBtn.onClick.AddListener(() =>
            {
                SetHallObj(false);
            });
            
            _messageBus.AddListener<BackToHallMessage>(OnBackToHallMessageHandler);
        }

        private void OnBackToHallMessageHandler(BackToHallMessage obj)
        {
            SetHallObj(true);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SetHallObj(bool show)
        {
            HallObj.SetActive(show);
        }
    }
}
