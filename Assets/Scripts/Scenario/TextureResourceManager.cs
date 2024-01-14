using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using UnityEngine;

// 画像データ管理クラス
public class TextureResourceManager : UnitySingleton<TextureResourceManager>
{
    public int Max = 5;
    private List<Texture2D> _textureList = new List<Texture2D>();

    public static void Mark(string textureName)
    {
        var tex = Instance._textureList.Find(itme => itme.name == textureName);
        if (tex != null)
        {
            Instance._textureList.Remove(tex);
            Instance._textureList.Add(tex);
        }
    }

    // 画像データの読み込み
    public static Texture Load(string textureName)
    {
        var tex = Instance._textureList.Find(item => item.name == textureName);
        if (tex == null)
        {
            tex = Instance._textureList[0];
            var res = Resources.Load<Texture2D>("Image/" + textureName);
            tex = res;
            //var res = Resources.Load<TextAsset>("Image/" + textureName);
            //tex.LoadImage(res.bytes);

            tex.name = textureName;
            Resources.UnloadAsset(res);
        }

        Instance._textureList.Remove(tex);
        Instance._textureList.Add(tex);

        return tex;
    }

    #region UNITY_CALLBACK
    private void OnEnable()
    {
        for (int i = 0; i < Instance.Max; ++i)
        {
            var tex2D = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            tex2D.Apply(false, true);
            Instance._textureList.Add(tex2D);
        }
    }

    private void OnDisable()
    {
        foreach (var tex in _textureList)
        {
            Destroy(tex);
        }
        _textureList.Clear();
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
