using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum musicLayer { Cubert, Enemy, SirKel, Treasure, Energy1, Attack1, Laser, Energy2, Attack2, Health1, Health2}

[CreateAssetMenu(fileName = "MusicElement", menuName = "Music/Create new Music Element")]
public class MusicElement : ScriptableObject
{
    public musicLayer layer;
    public audioState state;
    public int intensity;
    public int bpm;
    public float volume;
    public float pitch;
    public AudioClip clip;
    public AudioSource[] audioSource = new AudioSource[2];
    public int toggle;
}
