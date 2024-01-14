using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CommandUpdateImage : ICommand, IPreCommand
{
    public string Tag
    {
        get { return "img"; }
    }

    public void PreCommand(Dictionary<string, string> command)
    {
        var fileName = command["image"];
        TextureResourceManager.Load(fileName);
    }

    public void Command(Dictionary<string, string> command)
    {
        var fileName = command["image"];
        var objectName = command["name"];

        var obj = Array.Find<GameObject>(GameObject.FindGameObjectsWithTag("Layer"), item => item.name == objectName);
        obj.GetComponent<Layer>().UpdateTexture(TextureResourceManager.Load(fileName));
    }
}
