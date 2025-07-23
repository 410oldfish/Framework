using System.Collections;
using System.Collections.Generic;
using MergeIt.Core.Commands;
using TEngine;
using UnityEngine;
using AudioType = TEngine.AudioType;

namespace MergeIt.Game
{
    public class PlayBgmCommand : Command
    {
        string bgm1Name = "bgm1";
        string bgm2Name = "bgm2";
        public override void Execute()
        {
            //GameModule.Audio.Play(AudioType.Music, bgm1Name, true, 1.0f, true, true);
            //PlayBgm();
        }

        async void PlayBgm()
        {
            var bgm = await GameModule.Resource.LoadAssetAsync<AudioClip>(bgm1Name);
            var source = GameObject.Find("AudioSource").GetComponent<AudioSource>();
            source.clip = bgm;
            source.loop = true;
            source.Play();
        }
    }
}
