using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using SdlDotNet;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Core;
using SdlDotNet.Input;
using System.Drawing;

namespace Bejeweled
{

    class Button : Sprite
    {
        private SdlDotNet.Graphics.Font textFont;
        private Surface fontSurface;
        private Surface background;

        /* Constructor */
        internal Button()
        {
            try
            {
                textFont = new SdlDotNet.Graphics.Font(@"c:\Windows\Fonts\trebuc.ttf", 16);
            }
            catch (SdlException)
            {
                textFont = new SdlDotNet.Graphics.Font(@"c:\Windows\Fonts\arial.ttf", 14);
            }
        }

        /* Button Properties Methods */
        internal void SetImage(Surface surface, Point position)
        {
            background = surface;
            this.Surface = background;
            this.X = position.X;
            this.Y = position.Y;
            this.Surface.Transparent = true;
            this.Surface.TransparentColor = Color.FromArgb(255, 000, 255);
        }

        internal void SetText(string text)
        {
            fontSurface = textFont.Render(text, Color.Black, false);

            int px = this.Rectangle.Width / 2 - (fontSurface.Width / 2);
            int py = this.Rectangle.Height / 2 - (fontSurface.Height / 2);

            this.Surface.Blit(fontSurface, new Point(px, py));
        }

        #region IDisposable
        private bool disposed;

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (!this.disposed)
                {
                    if (textFont != null)
                    {
                        textFont.Dispose();
                    }
                    this.disposed = true;
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
        #endregion IDisposable
    }
}
