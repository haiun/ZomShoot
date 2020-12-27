using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum BgmEnum
{
    Title, Game, GameAmbient, Max
}

[Serializable]
public class BgmSource
{
    public BgmEnum BgmEnum = BgmEnum.Title;
    public AudioSource AudioSource = null;
}

public enum SfxEnum
{
    None, Shoot, Reload, Mag, Kill, Impact,
    BulletHit_Con, BulletHit_Metal, BulletHit_Dirt,
    Max
}

[Serializable]
public class SfxClip
{
    public SfxEnum SfxEnum = SfxEnum.None;
    public AudioClip Clip = null;
}