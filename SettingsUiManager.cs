using Godot;
using System;
using System.Collections.Generic;

public class SettingsUiManager : Panel
{
    private List<Node> settings;
    public override void _Ready()
    {
        ///<summary>
        /// Music checkbox
        /// Sfx checkbox
        /// Tutorial enabled checkbox
        /// Advertisements enabled checkbox
        ///</summary>
        settings = new List<Node>
        {
            GetNode("CentrePanel").GetNode("Checkbox").GetNode<CheckBox>("Checkbox");
            GetNode("CentrePanel").GetNode("Checkbox2").GetNode<CheckBox>("Checkbox");
            GetNode("RightPanel").GetNode("Checkbox").GetNode<CheckBox>("Checkbox");
            GetNode("RightPanel").GetNode("Checkbox2").GetNode<CheckBox>("Checkbox");
            GetNode("RightPanel").GetNode("UsernameEdit");
        }
    }
}
