using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace FreeCell
{
    internal struct IconInfo
    {
        private int HotspotX;
        private int HotspotY;
        private bool FIcon;
        internal bool fIcon
        {
            get { return FIcon; }
            set { FIcon = value; }
        }
        internal int hotspotX
        {
            get { return HotspotX; }
            set { HotspotX = value; }
        }
        internal int hotspotY
        {
            get { return HotspotY; }
            set { HotspotY = value; }
        }
        private IntPtr hbmMask;
        private IntPtr hbmColor;
    }

    internal static class UnsafeNativeMethods
    {
        /* Windows Methods to replace the cursor */
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);

        [DllImport("user32.dll")]
        internal static extern IntPtr CreateIconIndirect(ref IconInfo icon);
    }
}
