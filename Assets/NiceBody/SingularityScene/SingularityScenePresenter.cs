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
        Cursor.visible = false;
        // IQ 表示
        player_.IQ.Subscribe(iq =>
        {
            var progress = GetIqProgress(iq, player_.SingularityIq_);
            var time = GetProgressStamp(progress);
            sceneView.SetSingularityTime(time, progress);
        });

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
        player_.GetState<DeathState>().OnDeath.Subscribe(_ =>
        {
            Cursor.visible = true;
            sceneView.OnEnableDeathCanvas();
        }).AddTo(this);
    }

    private float GetNormalizedHealth(int current, int max)
    {
        if (max <= 0)
            return 0f;

        return Mathf.Clamp01((float)current / max);
    }

    private float GetIqProgress(IQ IQ, IQ SingularityIQ)
    {
        if (SingularityIQ == null || SingularityIQ.Value == 0f)
            return 1;

        var current = IQ.Value;
        var max = SingularityIQ.Value;

        return Mathf.Clamp01(current / max);
        
    }

    private TimeSpan GetProgressStamp(float progress)
    {
        float remainingHours = (1f - progress) * 24f;

        // 時間を秒に変換して TimeSpan に渡す
        var totalSeconds = Mathf.RoundToInt(remainingHours * 3600f);
        return TimeSpan.FromSeconds(totalSeconds);
    }
}
