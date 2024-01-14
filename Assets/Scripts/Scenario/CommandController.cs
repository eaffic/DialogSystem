using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

// シナリオテキストのコマンド制御
public class CommandController : UnitySingleton<CommandController>
{
    // 文字を解析しながら呼び出すコマンド
    private readonly List<ICommand> _commandList = new List<ICommand>()
    {
        new CommandUpdateImage(),       // name=オブジェクト名 image=イメージ名
        new CommandJumpNextScenario(),  // fileName=シナリオ名
    };

    // 文字の表示が完了したタイミングで呼ばれる処理(このタイミングはロード負荷は気にならない)
    private List<IPreCommand> _preCommandList = new List<IPreCommand>();

    // 次の文字表示前のコマンドを読み込み
    public void PreloadCommand(string line)
    {
        var dic = CommandAnalytic(line);
        foreach (var command in _preCommandList)
        {
            if (command.Tag == dic["tag"])
            {
                command.PreCommand(dic);
            }
        }
    }

    // コマンドを読み込み
    public bool LoadCommand(string line)
    {
        var dic = CommandAnalytic(line);
        foreach (var command in _commandList)
        {
            if (command.Tag == dic["tag"])
            {
                command.Command(dic);
                return true;
            }
        }
        return false;
    }

    // コマンドを解析
    private Dictionary<string, string> CommandAnalytic(string line)
    {
        Dictionary<string, string> command = new Dictionary<string, string>();

        // コマンド名を取得
        var tag = Regex.Match(line, "@(\\S+)\\s");
        command.Add("tag", tag.Groups[1].ToString());

        // コマンドのパラメータを取得
        Regex regex = new Regex("(\\S+)=(\\S+)");
        var matches = regex.Matches(line);
        foreach (Match match in matches)
        {
            command.Add(match.Groups[1].ToString(), match.Groups[2].ToString());
        }

        return command;
    }

    #region UNITY_CALLBACK
    protected override void Awake()
    {
        base.Awake();

        // PreCommandを取得
        foreach (var command in _commandList)
        {
            if (command is IPreCommand)
            {
                _preCommandList.Add((IPreCommand)command);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion
}
