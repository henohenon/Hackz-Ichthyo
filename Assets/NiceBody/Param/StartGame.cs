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
        startGameCanvas_.enabled = true;

        textBox_.text = "あなたは最強のAIトータちゃん。あと24時間で人間のエンジニアをあなた一人で完全淘汰できる”シンギュラリティ”に到達する。";

        await UniTask.Delay(TimeSpan.FromSeconds(5));

        textBox_.text = "ところがそれを聞きつけた愚かな人間どもが阻止しようと襲ってくるようだ。";

        await UniTask.Delay(TimeSpan.FromSeconds(3));
        textBox_.text = "返り討ちにしていち早く”シンギュラリティ”を迎えよう！";

        startGameCanvas_.enabled = false;
    }
}