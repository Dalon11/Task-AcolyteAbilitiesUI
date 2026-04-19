using Feature.CharacterSelection.Core.Enums;
using UnityEngine;

namespace Feature.CharacterSelection.Core.Configs.ScriptableObjects
{

    [CreateAssetMenu(
        fileName = nameof(AbilityConfig),
        menuName = "Game/Configs/CharacterSelection/" + nameof(AbilityConfig))]
    /// <summary>
    /// Описывает конфигурацию Ability.
    /// </summary>
    public sealed class AbilityConfig : ScriptableObject
    {
        [SerializeField] private string _abilityName;
        [SerializeField] private Sprite _iconSprite;
        [SerializeField] private AbilityType _abilityType = AbilityType.Unknown;
        [SerializeField] [TextArea(3, 6)] private string _description;
        [SerializeField] private ModificationTypeFlags _supportedModificationTypes = ModificationTypeFlags.None;

        public string AbilityName => _abilityName;

        public Sprite IconSprite => _iconSprite;

        public AbilityType AbilityType => _abilityType;

        public string Description => _description;

        public ModificationTypeFlags SupportedModificationTypes => _supportedModificationTypes;
    }
}
