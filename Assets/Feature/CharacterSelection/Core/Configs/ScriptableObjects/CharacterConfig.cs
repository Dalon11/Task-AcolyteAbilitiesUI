using UnityEngine;

namespace Feature.CharacterSelection.Core.Configs.ScriptableObjects
{
    /// <summary>
    /// Конфиг персонажа с параметрами, способностями и модификаторами.
    /// </summary>
    [CreateAssetMenu(
        fileName = "CharacterConfig",
        menuName = "Game/Abilities/Configs/Character Config")]
    public sealed class CharacterConfig : ScriptableObject
    {
        [SerializeField] private string _characterName;
        [SerializeField] private Sprite _characterSprite;
        [SerializeField] private Sprite _iconSprite;
        [SerializeField] private int _hp;
        [SerializeField] private int _armor;
        [SerializeField] [TextArea(3, 6)] private string _description;
        [SerializeField] private AbilityConfig[] _abilities = new AbilityConfig[0];
        [SerializeField] private ModificationConfig[] _modifications = new ModificationConfig[0];

        public string CharacterName => _characterName;

        public Sprite CharacterSprite => _characterSprite;

        public Sprite IconSprite => _iconSprite;

        public int Hp => _hp;

        public int Armor => _armor;

        public string Description => _description;

        public AbilityConfig[] Abilities => _abilities;

        public ModificationConfig[] Modifications => _modifications;
    }
}



