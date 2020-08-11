// FloatMenuOption_Aligned.cs
// Copyright Karel Kroeze, 2018-2018

using System;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace FluffyUI.FloatMenu
{
    public class FloatMenuOption_Aligned: FloatMenuOption
    {
        private const float HorizontalMargin = 6f;
        private const float VerticalMargin = 4f;
        private const float HoverLabelShift = 4f;

        private static readonly Color BGActive = new ColorInt( 21, 25, 29 ).ToColor;
        private static readonly Color BGHover = new ColorInt( 29, 45, 50 ).ToColor;
        private static readonly Color BGDisabled = new ColorInt( 40, 40, 40 ).ToColor;
        private static readonly Color TextActive = Color.white;
        private static readonly Color TextDisabled = new Color( .9f, .9f, .9f );

        private Direction extraPartAlignment;

        public FloatMenuOption_Aligned(
            string label,
            Action action,
            MenuOptionPriority priority = MenuOptionPriority.Default,
            Action mouseoverGuiAction = null,
            Thing revalidateClickTarget = null,
            float extraPartWidth = 0,
            Func<Rect, bool> extraPartOnGUI = null,
            Direction extraPartAlignment = Direction.Right,
            WorldObject revalidateWorldClickTarget = null ) : base( label, action, priority, mouseoverGuiAction,
            revalidateClickTarget, extraPartWidth, extraPartOnGUI, revalidateWorldClickTarget )
        {
            if ( extraPartAlignment != Direction.Left && extraPartAlignment != Direction.Right )
                throw new InvalidOperationException( "extraPartAlignment can only be Left or Right" );
            this.extraPartAlignment = extraPartAlignment;
        }

        public override bool DoGUI( Rect canvas, bool colonistOrdering, Verse.FloatMenu floatMenu )
        {
            var optionRect = canvas;
            optionRect.height -= 1f;
            var hovered = !Disabled && Mouse.IsOver( optionRect );

            // create label rect
            var labelRect = canvas;
            labelRect.xMin += HorizontalMargin;
            labelRect.xMax -= HorizontalMargin;
            if ( !hovered )
                labelRect.x -= HoverLabelShift;

            // create and position extra part rect
            bool doExtraPart = extraPartOnGUI != null && extraPartWidth > 0f;
            bool extraPartHovered = false;
            Rect extraPartRect = Rect.zero;
            if ( doExtraPart )
            {
                extraPartRect = new Rect(
                    canvas.xMin,
                    canvas.yMin,
                    extraPartWidth,
                    ExtraPartHeight );
                if ( extraPartAlignment == Direction.Left )
                {
                    extraPartRect.x = labelRect.x;
                    labelRect.x += extraPartWidth + HorizontalMargin;
                }
                else
                {
                    extraPartRect.x = canvas.xMax - extraPartWidth - HorizontalMargin;
                }
                extraPartHovered = Mouse.IsOver( extraPartRect );
            }

            // UI effects
            if (!Disabled)
                MouseoverSounds.DoRegion( optionRect );
            if ( hovered )
                mouseoverGuiAction?.Invoke();
            if ( !tutorTag.NullOrEmpty() )
                UIHighlighter.HighlightOpportunity( canvas, tutorTag );

            // background
            Color color = GUI.color;
            if ( Disabled )
                GUI.color = BGDisabled * color;
            else if ( hovered )
                GUI.color = BGHover * color;
            else
                GUI.color = BGActive * color;
            GUI.DrawTexture( optionRect, BaseContent.WhiteTex );
            GUI.color = Disabled ? TextDisabled : TextActive;
            Verse.Widgets.DrawAtlas( optionRect, TexUI.FloatMenuOptionBG );

            // label
            var textColor = ( Disabled ? TextDisabled : TextActive ) * color;
            Widgets.Label( labelRect, Label, textColor, TextAnchor.MiddleLeft );


            var result = doExtraPart && extraPartOnGUI( extraPartRect )
                      || Verse.Widgets.ButtonInvisible( optionRect );
            GUI.color = color;
            if ( !result ) return false;

            // let floatmenu and tutor system know we made a choice
            Chosen( colonistOrdering, floatMenu );
            if ( !tutorTag.NullOrEmpty() )
                TutorSystem.Notify_Event( tutorTag );
            return true;
        }
    }
}