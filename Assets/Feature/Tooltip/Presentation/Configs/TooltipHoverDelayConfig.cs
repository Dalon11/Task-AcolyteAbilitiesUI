using UnityEngine;

namespace Feature.Tooltip.Presentation.Configs
{

    /// <summary>
    /// Хранит параметры задержки и порога движения курсора для показа Tooltip.
    /// </summary>
    [CreateAssetMenu(
        fileName = nameof(TooltipHoverDelayConfig),
        menuName = "Game/Configs/Tooltip/" + nameof(TooltipHoverDelayConfig))]
    public sealed class TooltipHoverDelayConfig : ScriptableObject
    {
        [SerializeField] private float _hoverDelaySeconds = 1f;
        [SerializeField] private float _mouseMovementThreshold = 2f;

        public float HoverDelaySeconds => Mathf.Max(0f, _hoverDelaySeconds);

        public float MouseMovementThreshold => Mathf.Max(0f, _mouseMovementThreshold);
    }
}
