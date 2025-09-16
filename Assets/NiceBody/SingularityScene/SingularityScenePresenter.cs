using R3;
using UnityEngine;

public sealed class SingularityScenePresenter : MonoBehaviour
{
    [SerializeField] private SingularitySceneView sceneView;
    [SerializeField] private Player.Player player_;

        

    private void Awake()
    {
        player_.IQ.Subscribe(iq => sceneView.NeedCalculateTime.text = GetIqLevel24(iq, player_.SingularityIq_).ToString());
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
