// Label.cs
// Copyright Karel Kroeze, 2018-2018

using UnityEngine;
using Verse;

namespace FluffyUI
{
    public static partial class Widgets
    {
        private static readonly Color DefaultColor = Color.white;

        public static void Label( Rect canvas, string label, Color? color = null,
            TextAnchor anchor = TextAnchor.UpperLeft, GameFont font = GameFont.Small )
        {
            var _color = GUI.color;
            var _anchor = Text.Anchor;
            var _font = Text.Font;
            GUI.color = color ?? DefaultColor;
            Text.Anchor = anchor;
            Text.Font = font;
            Verse.Widgets.Label( canvas, label );
            Text.Font = _font;
            Text.Anchor = _anchor;
            GUI.color = _color;
        }
    }
}