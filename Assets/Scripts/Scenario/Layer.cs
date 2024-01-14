using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 画像表示クラス
public class Layer : MonoBehaviour
{
    RawImage _rawImage;

    // 画像更新
    public void UpdateTexture(Texture texture)
    {
        if (texture == null)
        {
            _rawImage.enabled = false;
            _rawImage.texture = null;
        }
        else
        {
            _rawImage.texture = texture;
            _rawImage.enabled = true;
            TextureResourceManager.Mark(texture.name);
        }
    }

    #region UNITY_CALLBACK
    private void Awake()
    {
        _rawImage = GetComponent<RawImage>();
        gameObject.tag = "Layer";

        _rawImage.enabled = false;
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
