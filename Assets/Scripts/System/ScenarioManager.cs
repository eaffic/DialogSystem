using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

[RequireComponent(typeof(TextController))]
public class ScenarioManager : UnitySingleton<ScenarioManager>
{
    public string LoadFileName;

    private string[] _scenarios;
    private int _currentLine = 0;
    private bool _isCallPrreload = false;

    private TextController _textController;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
