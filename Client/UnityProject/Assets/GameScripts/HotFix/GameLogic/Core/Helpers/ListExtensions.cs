// Copyright (c) 2024, Awessets

using System;
using System.Collections.Generic;
using System.Drawing;
using Sirenix.Utilities;

namespace MergeIt.Core.Helpers
{
    public static class ListExtensions
    {
        public static IList<int> GenerateShuffledArray(int size)
        {
            IList<int> array = new int[size];

            for (int i = 0; i < size; i++)
            {
                array[i] = i;
            }

            array.Shuffle();

            return array;
        }
        
        public static void Shuffle<TSource>(this IList<TSource> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);

                (list[i], list[j]) = (list[j], list[i]);
            }
        }
        
        /// <summary>
        /// 从小到大生成数组
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IList<int> GenerateArrayFromSToL(int size)
        {
            IList<int> array = new int[size];

            for (int i = 0; i < size; i++)
            {
                array[i] = i;
            }

            return array;
        }
        
        public static IList<int> GenerateArrayCloseToTarget(int size, int center)
        {
            //生成数组，按照距离中心点的距离排序
            IList<int> array = new int[size];
             
            for (int i = 0; i < size; i++)
            {
                array[i] = i;
            }
             
            array.Sort((a, b) => Math.Abs(a - center).CompareTo(Math.Abs(b - center)));
            
            return array;
        }
        
    }
}