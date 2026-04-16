using Feature.Abilities.Configs.Enums;
using UnityEngine;

namespace Feature.Abilities.Configs.ScriptableObjects
{
    [CreateAssetMenu(
        fileName = "AbilityConfig",
        menuName = "Game/Abilities/Configs/Ability Config")]
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
