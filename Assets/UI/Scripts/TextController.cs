using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class TextController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _nameText;
    [SerializeField]
    private TMP_Text _mainText;

    [SerializeField]
    private string _charaName;
    [SerializeField]
    private string[] _scenarios;

    [SerializeField, Range(0.001f, 0.3f)]
    private float intervalForCharacterDisplay = 0.05f;  // 一文字の表示にかかる時間

    private PlayerInputActions _playerInputActions;
    private int _currentLine = 0;
    private string currentText = string.Empty;  // 現在の文字列
    private float timeUntilDisplay = 0;         // 表示にかかる時間
    private float timeElapsed = 0;              // 文字列の表示を開始した時間
    private int lastUpdateCharacter = -1;       // 表示中の文字数

    // 文字の表示が完了しているかどうか
    public bool IsCompleteDisplayText
    {
        get { return Time.time >= timeElapsed + timeUntilDisplay; }
    }

    private void OnDestroy()
    {
        _playerInputActions.UI.Decide.performed -= PressedNextText;
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerInputActions = GameManager.Instance.GetPlayerInputActions;
        _playerInputActions.UI.Decide.performed += PressedNextText;

        StartNextLine();
    }

    // Update is called once per frame
    void Update()
    {
        // クリックから経過した時間が想定表示時間の何%か確認し、表示文字数を出す
        int displayCharacterCount = (int)(Mathf.Clamp01((Time.time - timeElapsed) / timeUntilDisplay) * currentText.Length);

        // 表示文字数が前回の表示文字数と異なるならテキストを更新する
        if (displayCharacterCount != lastUpdateCharacter)
        {
            _mainText.text = currentText.Substring(0, displayCharacterCount);
            lastUpdateCharacter = displayCharacterCount;
        }
    }

    void PressedNextText(InputAction.CallbackContext ctx)
    {
        if (ctx.interaction is PressInteraction)
        {
            StartNextLine();
        }
    }

    void StartNextLine()
    {
        // 文字の表示が完了してないなら文字をすべて表示する
        if (!IsCompleteDisplayText)
        {
            timeUntilDisplay = 0;
        }
        else
        {
            _nameText.text = _charaName;

            if (_currentLine == _scenarios.Length)
            {
                _currentLine = 0;
            }

            currentText = _scenarios[_currentLine];
            _currentLine++;

            // 想定表示時間と現在の時刻をキャッシュ
            timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;
            timeElapsed = Time.time;

            // 文字カウントを初期化
            lastUpdateCharacter = -1;
        }
    }
}
