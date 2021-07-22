// TextureChooser.cs
// Copyright Karel Kroeze, 2018-2018

using System;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace FluffyUI
{
    [StaticConstructorOnStartup]
    public class TextureChooser
    {
        private readonly Texture2D[] options;
        private int _curIndex;
        private const int DefaultMargin = 6;
        public Texture2D Choice => options[_curIndex];
        private static Texture2D LeftArrow, RightArrow;

        static TextureChooser()
        {
            try
            {
                LeftArrow = ContentFinder<Texture2D>.Get( "UI/Icons/LeftArrow", false );
                RightArrow = ContentFinder<Texture2D>.Get( "UI/Icons/RightArrow", false );
            }
            catch ( Exception ex )
            {
                throw new Exception( "Include left/right arrows at 'UI/Icons/[Left|Right]Arrow' when using TextureChooser." + ex );
            }
        }

        public TextureChooser( Texture2D[] options, int defaultIndex = 0 )
        {
            if ( options == null || !options.Any() )
                throw new ArgumentOutOfRangeException( nameof( options ) );
            this.options = options;
            _curIndex = defaultIndex;
        }

        public void DrawAt( Rect canvas, float margin = DefaultMargin,
            bool highlight = true )
        {
            // create main icon rect
            var grid = new Grid( canvas, 5, gutters: Vector2.zero );
            var backCell = grid.Column( 1 ).Rect;
            var iconCell = grid.Column( 3 ).Rect;
            var nectCell = grid.Column( 1 ).Rect;

            var buttonSize = Mathf.Min( backCell.width, backCell.height * 2 / 3f );
            var buttonRect = new Rect( 0f, 0f, buttonSize, buttonSize );
            var iconSize = Mathf.Min( iconCell.width, iconCell.height ) - 6f;
            var iconRect = new Rect( 0f, 0f, iconSize, iconSize ).CenteredIn( iconCell );

            // draw icon and buttons
            GUI.DrawTexture( iconRect, Choice );
            if ( Verse.Widgets.ButtonImage( buttonRect.CenteredIn( backCell ), LeftArrow ) )
                Previous();
            if ( Verse.Widgets.ButtonImage( buttonRect.CenteredIn( nectCell ), RightArrow ) )
                Next();
            
            // highlight
            if ( highlight )
                Verse.Widgets.DrawHighlightIfMouseover( canvas );
            
            // scrowheel selection
            if ( Mouse.IsOver( canvas ) && Event.current.type == EventType.ScrollWheel )
            {
                if ( Event.current.delta.y > 0f )
                    Next();
                if ( Event.current.delta.y < 0f )
                    Previous();
                Event.current.Use();
            }
        }

        private void Next()
        {
            SoundDefOf.Tick_High.PlayOneShotOnCamera();
            _curIndex = ( _curIndex + 1 ) % options.Length;
        }

        private void Previous()
        {
            SoundDefOf.Tick_Low.PlayOneShotOnCamera();
            _curIndex--;
            if ( _curIndex < 0 )
                _curIndex = options.Length - 1;
        }
    }
}