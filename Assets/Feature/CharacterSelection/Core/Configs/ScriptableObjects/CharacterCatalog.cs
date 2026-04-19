using System.Collections.Generic;
using UnityEngine;

namespace Feature.CharacterSelection.Core.Configs.ScriptableObjects
{

    [CreateAssetMenu(
        fileName = nameof(CharacterCatalog),
        menuName = "Game/Catalogs/CharacterSelection/" + nameof(CharacterCatalog))]
    /// <summary>
    /// Хранит каталог данных Character.
    /// </summary>
    public sealed class CharacterCatalog : ScriptableObject
    {
        [SerializeField] private CharacterConfig[] _characters = new CharacterConfig[0];

        public IReadOnlyList<CharacterConfig> Characters => _characters;
    }
}
