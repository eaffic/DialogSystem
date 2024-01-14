using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEditor.iOS;
using System;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TextController))]
public class ScenarioManager : UnitySingleton<ScenarioManager>
{
    public string LoadFileName;

    private string[] _scenarios;
    private int _currentLine = 0;
    private bool _isCallPrreload = false; // 事前の読み込みを呼び出すか

    private PlayerInputActions _playerInputActions;

    private TextController _textController;
    private CommandController _commandController;

    void RequestNextLine()
    {
        var currentText = _scenarios[_currentLine];
        _textController.SetNextLine(CommandProcess(currentText));
        _currentLine++;
        _isCallPrreload = false;
    }

    // シナリオデータの読み込み処理
    public void UpdateLines(string fileName)
    {
        var scenarioText = Resources.Load<TextAsset>("Scenario/" + fileName);

        if (scenarioText == null)
        {
            Debug.Log("シナリオファイルが見つかりませんでした");
            Debug.Log("ScenarioManagerを無効化します");
            enabled = false;
            return;
        }

        _scenarios = scenarioText.text.Split(new string[] { "@br" }, System.StringSplitOptions.None);
        _currentLine = 0;

        Resources.UnloadAsset(scenarioText);
    }

    private string CommandProcess(string line)
    {
        var lineReader = new StringReader(line);
        var lineBuilder = new StringBuilder();
        var text = string.Empty;
        while ((text = lineReader.ReadLine()) != null)
        {
            var commentCharacterCount = text.IndexOf("//");
            if (commentCharacterCount != -1)
            {
                text = text.Substring(0, commentCharacterCount);
            }

            if (!string.IsNullOrEmpty(text))
            {
                if (text[0] == '@' && _commandController.LoadCommand(text))
                {
                    continue;
                }
                lineBuilder.AppendLine(text);
            }
        }

        return lineBuilder.ToString();
    }

    private void PressedNextText(InputAction.CallbackContext ctx)
    {
        if (_textController.IsCompleteDisplayText)
        {
            if (_currentLine < _scenarios.Length)
            {
                RequestNextLine();
            }
        }
        else
        {
            _textController.ForceCompleteDisplayText();
        }
    }

    #region UNITY_CALLBACK
    // Start is called before the first frame update
    void Start()
    {
        _playerInputActions = GameManager.Instance.GetPlayerInputActions;
        if (_playerInputActions != null)
        {
            _playerInputActions.UI.Decide.performed += PressedNextText;
        }

        _textController = GetComponent<TextController>();
        _commandController = GetComponent<CommandController>();

        UpdateLines(LoadFileName);
        RequestNextLine();
    }

    // Update is called once per frame
    void Update()
    {
        if (_textController.IsCompleteDisplayText)
        {
            if (_currentLine < _scenarios.Length)
            {
                if (!_isCallPrreload)
                {
                    _commandController.PreloadCommand(_scenarios[_currentLine]);
                    _isCallPrreload = true;
                }
            }
        }
    }

    protected override void OnDestroy()
    {
        _playerInputActions.UI.Decide.performed -= PressedNextText;
        base.OnDestroy();
    }
    #endregion
}
