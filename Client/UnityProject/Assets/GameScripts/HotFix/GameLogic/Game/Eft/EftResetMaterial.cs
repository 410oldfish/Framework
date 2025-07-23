using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TEngine;
using UnityEngine;

namespace MergeIt.Game
{
    public class EftResetMaterial : MonoBehaviour
    {
        public string MaterialName;
        // Start is called before the first frame update
        void Start()
        {
            SetShader().Forget();
        }

        async UniTaskVoid SetShader()
        {
            var mat = await GameModule.Resource.LoadAssetAsync<Material>(MaterialName);
            
            if (mat != null)
            {
                Debug.Log(mat.name);
                var shaderName = mat.shader.name;
                var newShader = Shader.Find(shaderName);
                mat.shader = newShader;
            }
        }
    }
}
