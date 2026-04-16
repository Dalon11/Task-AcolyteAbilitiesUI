using System.Collections.Generic;
using UnityEngine;

namespace Feature.Abilities.Configs.ScriptableObjects
{
    [CreateAssetMenu(
        fileName = "CharacterCatalog",
        menuName = "Game/Abilities/Configs/Character Catalog")]
    public sealed class CharacterCatalog : ScriptableObject
    {
        [SerializeField] private CharacterConfig[] _characters = new CharacterConfig[0];

        public IReadOnlyList<CharacterConfig> Characters => _characters;
    }
}
