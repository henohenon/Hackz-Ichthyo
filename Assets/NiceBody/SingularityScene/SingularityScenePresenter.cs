using Player.State;
using R3;
using System;
using System.Linq;
using UnityEngine;

public sealed class SingularityScenePresenter : MonoBehaviour
{
    [SerializeField] private SingularitySceneView sceneView;
    [SerializeField] private Player.Player player_;



    private void Awake()
    {
        // IQ 表示
        player_.IQ.Subscribe(iq => sceneView.NeedCalculateTime.text = "必要計算時間: " + GetIqLevel24Formatted(iq, player_.SingularityIq_));

        // スキル選択表示
        player_.LearnedSkillGroup_.OnSelectLearnSkill.Subscribe(sceneView.OnOpenSelectLearnSkill).AddTo(this);

        // スキル選択クリック処理
        sceneView.SelectLearnSkillUIs
            .ToList()
            .Select(ui => ui.OnClicked)
            .ToList()
            .ForEach(onClicked => onClicked.Subscribe(_ =>
                {
                    player_.LearnedSkillGroup_.LearnSkill(_);
                    sceneView.OnCloseSelectLearnSkill();
                }).AddTo(this)
            );

        // Health 表示更新
        player_.Health.Subscribe(health => sceneView.HealthSlider.value = GetNormalizedHealth(health.Value, player_.MaxHealth.Value)).AddTo(this);
        player_.LearnedSkillGroup_.OnLearnSkill.Subscribe(skill => sceneView.AddLearnSkill(skill.Skill)).AddTo(this);
        player_.GetState<DeathState>().OnDeath.Subscribe(_ => sceneView.OnEnableDeathCanvas()).AddTo(this);
    }

    private float GetNormalizedHealth(int current, int max)
    {
        if (max <= 0)
            return 0f;

        return Mathf.Clamp01((float)current / max);
    }

    private string GetIqLevel24Formatted(IQ IQ, IQ SingularityIQ)
    {
        if (SingularityIQ == null || SingularityIQ.Value == 0f)
            return "24時間0分0秒";

        float current = IQ.Value;
        float max = SingularityIQ.Value;

        float progress = Mathf.Clamp01(current / max);
        float remainingHours = (1f - progress) * 24f;

        // 時間を秒に変換して TimeSpan に渡す
        var totalSeconds = Mathf.RoundToInt(remainingHours * 3600f);
        var timeSpan = TimeSpan.FromSeconds(totalSeconds);

        return $"{timeSpan.Hours}時間{timeSpan.Minutes}分{timeSpan.Seconds}秒";
    }
}
