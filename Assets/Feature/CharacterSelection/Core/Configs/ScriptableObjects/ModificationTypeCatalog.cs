using System;
using System.Collections.Generic;
using Feature.CharacterSelection.Core.Enums;
using UnityEngine;

namespace Feature.CharacterSelection.Core.Configs.ScriptableObjects
{
    /// <summary>
    /// Каталог отображаемых параметров типов модификаторов для UI.
    /// </summary>
    [CreateAssetMenu(
        fileName = "ModificationTypeCatalog",
        menuName = "Game/Abilities/Configs/Modification Type Catalog")]
    public sealed class ModificationTypeCatalog : ScriptableObject
    {
        [SerializeField] private ModificationTypeEntry[] _entries = new ModificationTypeEntry[0];

        public IReadOnlyList<ModificationTypeEntry> Entries => _entries;

        /// <summary>
        /// Запись каталога с визуальными данными одного типа модификатора.
        /// </summary>
        [Serializable]
        public sealed class ModificationTypeEntry
        {
            [SerializeField] private ModificationType _type = ModificationType.Unknown;
            [SerializeField] private string _displayName;
            [SerializeField] private Sprite _iconSprite;
            [SerializeField] private Color _slotColor = Color.white;

            public ModificationType Type => _type;

            public string DisplayName => _displayName;

            public Sprite IconSprite => _iconSprite;

            public Color SlotColor => _slotColor;
        }
    }
}



