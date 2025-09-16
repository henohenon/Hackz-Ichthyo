using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public sealed class DamageText : MonoBehaviour
{
    [SerializeField] Canvas onChangeDisplayCanvas;
    [SerializeField] float moveChangeParameterTextRange;
    [SerializeField] float moveChangeParameterTextDuration_secs;
    [SerializeField] Text changeParameterTextBox;


    /// <summary>
    /// 実行されて一定時間経つとオブジェクト自体が消える。
    /// </summary>
    /// <param name="changeValue"></param>
    public async void OnShowTextAsync(int changeValue)
    {
        changeParameterTextBox.text = changeValue.ToString();

        await onChangeDisplayCanvas.transform
            .DOMove(moveChangeParameterTextRange * Vector2.up, moveChangeParameterTextDuration_secs)
            .SetRelative(true)
            .SetLink(gameObject)
            .AsyncWaitForCompletion();

        Destroy(gameObject);
    }
}