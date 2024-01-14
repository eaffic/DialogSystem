using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandJumpNextScenario : ICommand
{
    public string Tag
    {
        get { return "jump"; }
    }

    public void Command(Dictionary<string, string> command)
    {
        var scenario = ScenarioManager.Instance;
        var fileName = command["fileName"];
        scenario.UpdateLines(fileName);
    }
}
