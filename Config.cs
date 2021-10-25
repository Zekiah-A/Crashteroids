using Godot;
using System;
using System.Text.Json;

public class Config
{
    //NOTE: Skin should be chosen at start match time, in editor only for preview

    public bool Music;
    public bool SoundEffects;
    public bool Advertisements;
    public int Money;
    public int GraphicsQualitySetting;
    public string Username;

    private void Save()
    {

    }
}
