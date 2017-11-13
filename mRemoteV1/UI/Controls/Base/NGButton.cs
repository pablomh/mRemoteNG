﻿using mRemoteNG.Themes;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;


namespace mRemoteNG.UI.Controls.Base
{
    [ToolboxBitmap(typeof(Button))]
    //Extended button class, the button onPaint completely repaint the control
    public class NGButton : Button
    {
        private ThemeManager _themeManager ;

        public enum MouseState
        {
            HOVER,
            DOWN,
            OUT
        }

        public NGButton()
        {
            ThemeManager.getInstance().ThemeChanged += OnCreateControl;
        }

        public MouseState _mice { get; set; }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (Tools.DesignModeTest.IsInDesignMode(this)) return;
            _themeManager = ThemeManager.getInstance();
            if (_themeManager.ThemingActive)
            {
                _mice = MouseState.OUT;
                MouseEnter += (sender, args) =>
                {
                    _mice = MouseState.HOVER;
                    Invalidate();
                };
                MouseLeave += (sender, args) =>
                {
                    _mice = MouseState.OUT;
                    Invalidate();
                };
                MouseDown += (sender, args) =>
                {
                    if (args.Button == MouseButtons.Left)
                    {
                        _mice = MouseState.DOWN;
                        Invalidate();
                    }
                };
                MouseUp += (sender, args) =>
                {
                    _mice = MouseState.OUT;

                    Invalidate();
                };
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
            Color back;
            Color fore;
            Color border;
            if (Enabled)
            {

                switch (_mice)
                {
                    case  MouseState.HOVER:
                        back = _themeManager.ActiveTheme.ExtendedPalette.getColor("Button_Hover_Background");
                        fore = _themeManager.ActiveTheme.ExtendedPalette.getColor("Button_Hover_Foreground");
                        border = _themeManager.ActiveTheme.ExtendedPalette.getColor("Button_Hover_Border");
                        break;
                    case MouseState.DOWN:
                        back = _themeManager.ActiveTheme.ExtendedPalette.getColor("Button_Pressed_Background");
                        fore = _themeManager.ActiveTheme.ExtendedPalette.getColor("Button_Pressed_Foreground");
                        border = _themeManager.ActiveTheme.ExtendedPalette.getColor("Button_Pressed_Border");
                        break;
                    default:
                        back = _themeManager.ActiveTheme.ExtendedPalette.getColor("Button_Background");
                        fore = _themeManager.ActiveTheme.ExtendedPalette.getColor("Button_Foreground");
                        border = _themeManager.ActiveTheme.ExtendedPalette.getColor("Button_Border");
                        break;
                } 
            }
            else
            {
                back = _themeManager.ActiveTheme.ExtendedPalette.getColor("Button_Disabled_Background"); 
                fore = _themeManager.ActiveTheme.ExtendedPalette.getColor("Button_Disabled_Foreground");
                border = _themeManager.ActiveTheme.ExtendedPalette.getColor("Button_Disabled_Border");
            }



            e.Graphics.FillRectangle(new SolidBrush(back), e.ClipRectangle);
            e.Graphics.DrawRectangle(new Pen(border, 1), 0, 0, Width - 1, Height - 1);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            //Warning. the app doesnt use many images in buttons so this positions are kinda tailored just for the used by the app
            //not by general usage of iamges in buttons
            if (Image != null)
            {
                SizeF stringSize = e.Graphics.MeasureString(Text, Font);

                e.Graphics.DrawImageUnscaled(Image, Width / 2 - (int)stringSize.Width / 2  - Image.Width  , Height / 2 - Image.Height/2);
            }
            TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, fore, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
    }
}
