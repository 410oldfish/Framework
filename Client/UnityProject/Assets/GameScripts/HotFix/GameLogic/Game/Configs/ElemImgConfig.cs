using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    [CreateAssetMenu(fileName = "ElemImgConfig", menuName = "Game/ElemImgConfig")]
    public class ElemImgConfig : ScriptableObject
    {
        [SerializeField] public int i;
        [SerializeField] public elemImgPair[] elemImgs;
    }
    
    [Serializable]
    public class elemImgPair
    {
        [SerializeField] public int elemId;
        [SerializeField] public Texture2D elemImg;
    }
}
