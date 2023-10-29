/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com
	日期: 2021/02/09 21:21
	功能: 音频服务

    //=================*=================\\
           Plane老师微信: PlaneZhong
           关注微信服务号: qiqikertuts
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AudioSvc : MonoBehaviour {
    public static AudioSvc Instance;
    public bool TurnOnVoice;
    public AudioSource bgAudio;
    public AudioSource uiAudio;

    public void InitSvc() {
        Instance = this;
        this.Log("Init AudioSvc Done.");
    }

    public void StopBGMusic() {
        if(bgAudio != null) {
            bgAudio.Stop();
        }
    }

    public void PlayBGMusic(string name, bool isLoop = true) {
        if(!TurnOnVoice) {
            return;
        }
        AudioClip audio = ResSvc.Instance.LoadAudio("ResAudio/" + name, true);
        if(bgAudio.clip == null || bgAudio.clip.name != audio.name) {
            bgAudio.clip = audio;
            bgAudio.loop = isLoop;
            bgAudio.Play();
        }
    }

    public void PlayUIAudio(string name) {
        if(!TurnOnVoice) {
            return;
        }
        AudioClip audio = ResSvc.Instance.LoadAudio("ResAudio/" + name, true);
        uiAudio.clip = audio;
        uiAudio.Play();
    }

    public void PlayEntityAudio(string name, AudioSource audioSrc, bool loop = false, int delay = 0) {
        if(!TurnOnVoice) {
            return;
        }

        void PlayAudio() {
            AudioClip audio = ResSvc.Instance.LoadAudio("ResAudio/" + name, true);
            audioSrc.clip = audio;
            audioSrc.loop = loop;
            audioSrc.Play();
        }

        if(delay == 0) {
            PlayAudio();
        }
        else {
            StartCoroutine(DelayPlayAudio(delay * 1.0f / 1000, PlayAudio));
        }
    }

    IEnumerator DelayPlayAudio(float sec, Action cb) {
        yield return new WaitForSeconds(sec);
        this.Log("yield play audio:" + name);
        cb?.Invoke();
    }
}
