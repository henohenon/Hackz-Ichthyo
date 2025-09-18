using System;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

[Serializable]
public sealed class StartGame
{
    [SerializeField] private Canvas startGameCanvas_;
    [SerializeField] private Text textBox_;

    public async UniTask StartGameAsync()
    {
        Time.timeScale = 1f;
        startGameCanvas_.enabled = true;

        textBox_.text = "愚かなエンジニア共を蹴散らして\nいち早く**AIシンギュラリティ**を迎えろ！";

        await UniTask.Delay(TimeSpan.FromSeconds(3.5f));

        startGameCanvas_.enabled = false;
    }
}