using Feature.Abilities.Presentation.Binding.Contracts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Feature.Abilities.Presentation.Views
{
    /// <summary>
    /// Управляет layout-настройкой и расчетом ширины tooltip.
    /// </summary>
    public sealed class TooltipLayoutController
    {
        private const float MinWidth = 220f;
        private const float MaxWidth = 560f;
        private const float HorizontalPadding = 16f;
        private const float VerticalPadding = 14f;
        private const float HeaderDescriptionSpacing = 6f;

        private readonly RectTransform _panelRect;
        private readonly TMP_Text _headerText;
        private readonly TMP_Text _descriptionText;

        private VerticalLayoutGroup _layoutGroup;
        private ContentSizeFitter _contentSizeFitter;
        private LayoutElement _headerLayoutElement;
        private LayoutElement _descriptionLayoutElement;

        public TooltipLayoutController(RectTransform panelRect, TMP_Text headerText, TMP_Text descriptionText)
        {
            _panelRect = panelRect;
            _headerText = headerText;
            _descriptionText = descriptionText;
        }

        public void EnsureLayoutComponents()
        {
            if (_panelRect == null)
                return;

            _layoutGroup = _panelRect.GetComponent<VerticalLayoutGroup>();
            if (_layoutGroup == null)
                _layoutGroup = _panelRect.gameObject.AddComponent<VerticalLayoutGroup>();

            _layoutGroup.childAlignment = TextAnchor.UpperLeft;
            _layoutGroup.childControlWidth = true;
            _layoutGroup.childControlHeight = true;
            _layoutGroup.childForceExpandWidth = false;
            _layoutGroup.childForceExpandHeight = false;
            _layoutGroup.spacing = HeaderDescriptionSpacing;
            _layoutGroup.padding = new RectOffset(
                Mathf.RoundToInt(HorizontalPadding),
                Mathf.RoundToInt(HorizontalPadding),
                Mathf.RoundToInt(VerticalPadding),
                Mathf.RoundToInt(VerticalPadding));

            _contentSizeFitter = _panelRect.GetComponent<ContentSizeFitter>();
            if (_contentSizeFitter == null)
                _contentSizeFitter = _panelRect.gameObject.AddComponent<ContentSizeFitter>();

            _contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            _contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            if (_headerText != null)
            {
                _headerLayoutElement = _headerText.GetComponent<LayoutElement>();
                if (_headerLayoutElement == null)
                    _headerLayoutElement = _headerText.gameObject.AddComponent<LayoutElement>();
            }

            if (_descriptionText != null)
            {
                _descriptionLayoutElement = _descriptionText.GetComponent<LayoutElement>();
                if (_descriptionLayoutElement == null)
                    _descriptionLayoutElement = _descriptionText.gameObject.AddComponent<LayoutElement>();

                _descriptionText.enableWordWrapping = true;
                _descriptionText.overflowMode = TextOverflowModes.Overflow;
            }
        }

        public void ApplyLayoutDrivenTooltipSize(ITooltipContentViewModel content)
        {
            if (_panelRect == null)
                return;

            if (content == null)
                return;

            float safeMinWidth = Mathf.Max(1f, MinWidth);
            float safeMaxWidth = Mathf.Max(safeMinWidth, MaxWidth);
            float contentMaxWidth = Mathf.Max(1f, safeMaxWidth - (HorizontalPadding * 2f));
            float contentMinWidth = Mathf.Max(1f, safeMinWidth - (HorizontalPadding * 2f));

            float headerPreferredWidth = GetTextPreferredWidth(_headerText, content.Header);
            float descriptionPreferredWidth = GetTextPreferredWidth(_descriptionText, content.Description);
            float targetContentWidth = Mathf.Clamp(
                Mathf.Max(headerPreferredWidth, descriptionPreferredWidth),
                contentMinWidth,
                contentMaxWidth);

            if (_headerLayoutElement != null)
                _headerLayoutElement.preferredWidth = targetContentWidth;

            if (_descriptionLayoutElement != null)
                _descriptionLayoutElement.preferredWidth = targetContentWidth;

            LayoutRebuilder.ForceRebuildLayoutImmediate(_panelRect);
        }

        private float GetTextPreferredWidth(TMP_Text textComponent, string text)
        {
            if (textComponent == null || string.IsNullOrEmpty(text))
                return 0f;

            Vector2 preferred = textComponent.GetPreferredValues(text);
            return preferred.x;
        }
    }
}
