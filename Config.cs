using Godot;
using System;
using System.Text.Json;

public class Config
{
    //NOTE: Skin should be chosen at start match time, it should be able to be bought in the shop.

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
