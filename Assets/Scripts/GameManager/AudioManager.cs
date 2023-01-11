using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {
  public Sound[] sounds;

  void Awake() {
    foreach (Sound s in sounds) {
        s.source = gameObject.AddComponent<AudioSource>();
        s.source.clip = s.clip;
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
    }

    Sound bgm = Array.Find(sounds, sound => sound.name == "BGM");
    bgm.source.loop = true;
    bgm = Array.Find(sounds, sound => sound.name == "MenuBGM");
    bgm.source.loop = true;
  }

  public void Play(string name) {
    Sound s = Array.Find(sounds, sound => sound.name == name);
    s.source.Play();
  }

  public void Stop(string name) {
    Sound s = Array.Find(sounds, sound => sound.name == name);
    s.source.Stop();
  }
}
