﻿using mRemoteNG.Themes;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls.Base
{
    //Themable label to overide the winforms behavior of drawing the forecolor of disabled with a system color
    //This class repaints the control to avoid Disabled state mismatch of the theme
    [ToolboxBitmap(typeof(Label))]
    public class NGLabel : Label
    {
         
        private ThemeManager _themeManager;

        public NGLabel()
        {
            ThemeManager.getInstance().ThemeChanged += OnCreateControl;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (!Tools.DesignModeTest.IsInDesignMode(this))
            {
                _themeManager = ThemeManager.getInstance();
                Invalidate();
            }
        }

  
        protected override void OnPaint(PaintEventArgs e)
        {
            if (Tools.DesignModeTest.IsInDesignMode(this) || !_themeManager.ThemingActive)
            {
                base.OnPaint(e);
                return;
            }

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            if (Enabled)
            {
                TextRenderer.DrawText(e.Graphics, this.Text, Font, ClientRectangle, ForeColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            }
            else
            {
                var disabledtextLabel = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Disabled_Foreground");
                TextRenderer.DrawText(e.Graphics, this.Text, Font, ClientRectangle, disabledtextLabel, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            }
                
        } 
    }
}
