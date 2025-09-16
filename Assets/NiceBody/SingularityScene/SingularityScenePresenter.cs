using R3;
using System.Linq;
using UnityEngine;

public sealed class SingularityScenePresenter : MonoBehaviour
{
    [SerializeField] private SingularitySceneView sceneView;
    [SerializeField] private Player.Player player_;



    private void Awake()
    {
        // IQ 表示
        player_.IQ.Subscribe(iq =>
            sceneView.NeedCalculateTime.text = GetIqLevel24(iq, player_.SingularityIq_).ToString());

        // スキル選択表示
        player_.LearnedSkillGroup_.OnSelectLearnSkill.Subscribe(sceneView.OnOpenSelectLearnSkill);

        // スキル選択クリック処理
        sceneView.SelectLearnSkillUIs
            .ToList()
            .Select(ui => ui.OnClicked)
            .ToList()
            .ForEach(onClicked =>
            {
                onClicked.Subscribe(player_.LearnedSkillGroup_.LearnSkill);
                sceneView.OnCloseSelectLearnSkill();
            });

        // Health 表示更新
        player_.Health.Subscribe(health =>
        {
            sceneView.HealthSlider.value = GetNormalizedHealth(health.Value, player_.MaxHealth.Value);
        });
    }

    private float GetNormalizedHealth(int current, int max)
    {
        if (max <= 0) return 0f;
        return Mathf.Clamp01((float)current / max);
    }

    private float GetIqLevel24(IQ IQ, IQ SingularityIQ)
    {
        if (SingularityIQ == null || SingularityIQ.Value == 0f)
            return 0f;

        float current = IQ.Value;
        float max = SingularityIQ.Value;

        return Mathf.Clamp01(current / max) * 24f;
    }
}
