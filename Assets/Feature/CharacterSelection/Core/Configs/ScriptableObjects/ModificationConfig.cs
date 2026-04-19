using Feature.CharacterSelection.Core.Enums;
using UnityEngine;

namespace Feature.CharacterSelection.Core.Configs.ScriptableObjects
{

    [CreateAssetMenu(
        fileName = nameof(ModificationConfig),
        menuName = "Game/Configs/CharacterSelection/" + nameof(ModificationConfig))]
    /// <summary>
    /// Описывает конфигурацию Modification.
    /// </summary>
    public sealed class ModificationConfig : ScriptableObject
    {
        [SerializeField] private string _modificationName;
        [SerializeField] private Sprite _iconSprite;
        [SerializeField] private ModificationType _modificationType = ModificationType.Unknown;
        [SerializeField] [TextArea(3, 6)] private string _description;

        public string ModificationName => _modificationName;

        public Sprite IconSprite => _iconSprite;

        public ModificationType ModificationType => _modificationType;

        public string Description => _description;
    }
}
