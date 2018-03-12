using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Skyunion
{
    public class SoundService : GameModule<SoundService>
    {
        GameObject music;
        GameObject effect;
        public AudioSource backsoundSource = null;
        private Dictionary<string, AudioClip> clipCache = new Dictionary<string, AudioClip>();
        private Dictionary<string, AudioSource> effectsounCache = new Dictionary<string, AudioSource>();

        void Start()
        {
            music = new GameObject("Music");
            music.transform.SetParent(transform);
            backsoundSource = music.AddComponent<AudioSource>();

            effect = new GameObject("Effect");
            effect.transform.SetParent(transform);
        }

        public void PlayMusic(string name, bool loop = true)
        {
            if (backsoundSource.clip != null && name.IndexOf(this.backsoundSource.clip.name) > -1)
            {
                return ;
            }
            backsoundSource.loop = loop;
            backsoundSource.clip = LoadAudioClip(name);
            if(backsoundSource.clip != null)
            {
                backsoundSource.Play();
            }
        }
        public void StopMusic()
        {
            backsoundSource.Stop();
            backsoundSource.clip = null;
        }

        public AudioClip LoadAudioClip(string name)
        {
            AudioClip ac = null;
            clipCache.TryGetValue(name, out ac);
            if (ac == null)
            {
                ac = AssetsManager.Load<AudioClip>("Sound/" + name);
                if (ac != null) clipCache.Add(name, ac);
            }

            return ac;
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        public void PlayEffect(string name)
        {
            if (effectsounCache.ContainsKey(name))
            {
                //杜绝重复播放
                return;
            }

            GameObject gameObject = new GameObject(name);
            gameObject.transform.SetParent(effect.transform);

            AudioSource audioSource = (AudioSource)gameObject.AddComponent<AudioSource>();
            var clip = LoadAudioClip(name);
            audioSource.clip = clip;
            //audioSource.spatialBlend = 1f;
            //audioSource.volume = 1;
            audioSource.Play();
            effectsounCache.Add(name, audioSource);
            var clearTime = clip.length * ((Time.timeScale >= 0.01f) ? Time.timeScale : 0.01f);

            StartCoroutine(DelayToInvokeDo(() =>
            {
                GameObject.Destroy(gameObject);
                effectsounCache.Remove(name);
            }, clearTime));
        }

        IEnumerator DelayToInvokeDo(Action action, float delaySeconds)
        {
            yield return new WaitForSeconds(delaySeconds);
            action();
        }

        public void Play(string name)
        {
            AudioSource.PlayClipAtPoint(LoadAudioClip(name), new Vector3(), 1);
        }
    }
}