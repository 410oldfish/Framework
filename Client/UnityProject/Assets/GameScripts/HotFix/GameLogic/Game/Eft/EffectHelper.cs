using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MergeIt.SimpleDI.ReservedInterfaces;
using Spine.Unity;
using TEngine;
using UnityEngine;
using YooAsset;

namespace GameLogic
{
    public class EffectHelper : IUpdatable
    {
        public async UniTask<GameObject> LoadEffect(string effectName)
        {
            var effect = await GameModule.Resource.LoadAssetAsync<GameObject>(effectName);
            return effect;
        }
        
        public async UniTask<GameObject> LoadAndInstantiateEffect(string effectName, Transform parent, Vector2 pos, Vector2 scale, string aniName, string matName)
        {
            var effect = await LoadEffect(effectName);
            var instance = GameObject.Instantiate(effect, parent.transform);
            instance.transform.localPosition = pos;
            instance.transform.localScale = scale;
            //Play Spine
            var spineAni = instance.GetComponent<SkeletonAnimation>();
            spineAni.AnimationState.SetAnimation(0, aniName, false);

            
            var mat = instance.GetComponent<Material>();
            if (mat == null)
            {
                mat = await GameModule.Resource.LoadAssetAsync<Material>(matName);
            }

            if (mat != null)
            {
                Debug.Log(mat.name);
                var shaderName = mat.shader.name;
                var newShader = Shader.Find(shaderName);
                mat.shader = newShader;
            }
            
            return instance;
        }
        
        public async UniTask<GameObject> LoadAndInstantiateEffect(string effectName, string parentName, Vector2 pos, Vector2 scale, string aniName)
        {
            var effect = await LoadEffect(effectName);
            var parent = GameObject.Find(parentName);
            var instance = GameObject.Instantiate(effect, parent.transform);
            instance.transform.position = pos;
            instance.transform.localScale = scale;
            //Play Spine
            var spineAni = instance.GetComponent<SkeletonAnimation>();
            spineAni.AnimationState.SetAnimation(0, aniName, false);

            /*
            var mat = instance.GetComponent<Material>();
            if (mat == null)
            {
                mat = await GameModule.Resource.LoadAssetAsync<Material>("mj_hdbq_dianzan_ani_Material-Additive");
            }

            if (mat != null)
            {
                Debug.Log(mat.name);
                var shaderName = mat.shader.name;
                var newShader = Shader.Find(shaderName);
                mat.shader = newShader;
            }
            */
            
            return instance;
        }

        public void Update()
        {
            //On Click
            /*
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                Debug.Log("Click!");
                var mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                LoadAndInstantiateEffect("ClickEft", "RootCanvas", mousepos, new Vector2(80, 80), "mj_hdbq_dianzan_ani").Forget();
            }
            */
        }
    }
}
