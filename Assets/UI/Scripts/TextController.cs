using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

// TextBlockのテキストを制御する
public class TextController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _nameText;
    [SerializeField]
    private TMP_Text _mainText;

    [SerializeField, Range(0.001f, 0.3f)]
    private float intervalForCharacterDisplay = 0.05f;  // 一文字の表示にかかる時間

    private string currentText = string.Empty;  // 現在の文字列
    private float timeUntilDisplay = 0;         // 表示にかかる時間
    private float timeElapsed = 0;              // 文字列の表示を開始した時間
    private int lastUpdateCharacter = -1;       // 表示中の文字数

    // 文字の表示が完了しているかどうか
    public bool IsCompleteDisplayText
    {
        get { return Time.time >= timeElapsed + timeUntilDisplay; }
    }

    // 強制的に全文表示する
    public void ForceCompleteDisplayText()
    {
        timeUntilDisplay = 0;
    }

    // 次のテキスト表示処理の初期設定
    public void SetNextLine(string text)
    {
        currentText = text;
        timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;
        timeElapsed = Time.time;
        lastUpdateCharacter = -1;
    }

    #region UNITY_CALLBACK
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
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
    #endregion
}
