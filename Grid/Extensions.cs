// Extensions.cs
// Copyright Karel Kroeze, 2018-2018

using UnityEngine;
using Verse;

namespace FluffyUI
{
    public static class Extensions
    {
        public static Rect ContractedBy( this Rect rect, Vector2 vector )
        {
            return new Rect( 
                rect.xMin + vector.x,
                rect.yMin + vector.y,
                rect.width - vector.x * 2,
                rect.height - vector.y * 2 );
        }

        public static Rect CenteredIn( this Rect rect, Rect parent )
        {
            return rect.CenteredOnXIn( parent ).CenteredOnYIn( parent );
        }
    }
}