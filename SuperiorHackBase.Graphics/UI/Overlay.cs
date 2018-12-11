using SuperiorHackBase.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperiorHackBase.Graphics.UI
{
    public class Overlay : Form
    {
        #region VARIABLES
        private Timer windowUpdater;
        #endregion

        #region PROPERTIES
        public IRenderer Renderer { get; private set; }
        public IntPtr Hwnd { get; private set; }
        public new List<Controls.Control> Controls { get; private set; }
        #endregion

        #region CONSTRUCTORS
        public Overlay(IRenderer renderer)
        {
            Controls = new List<Controls.Control>();
            Renderer = renderer;
            windowUpdater = new Timer();
            windowUpdater.Interval = (int)(1000f / 60f);
            windowUpdater.Tick += UpdateWindowPosSize;

            BackColor = System.Drawing.Color.Black;
            //TransparencyKey = System.Drawing.Color.Black;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "";
            Name = "";
            TopMost = true;
            TopLevel = true;

            //Make form transparent and fully topmost
            int initialStyle = WinAPI.GetWindowLong(this.Handle, (int)WinAPI.GetWindowLongFlags.GWL_EXSTYLE);
            WinAPI.SetWindowLong(this.Handle, (int)WinAPI.GetWindowLongFlags.GWL_EXSTYLE, initialStyle | (int)WinAPI.ExtendedWindowStyles.WS_EX_LAYERED | (int)WinAPI.ExtendedWindowStyles.WS_EX_TRANSPARENT);
            WinAPI.SetWindowPos(this.Handle, (IntPtr)WinAPI.SetWindpwPosHWNDFlags.TopMost, 0, 0, 0, 0, (uint)WinAPI.SetWindowPosFlags.NOMOVE | (uint)WinAPI.SetWindowPosFlags.NOSIZE);
            WinAPI.SetLayeredWindowAttributes(this.Handle, 0, 255, (uint)WinAPI.LayeredWindowAttributesFlags.LWA_ALPHA);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            windowUpdater.Start();
        }

        private void UpdateWindowPosSize(object sender, EventArgs e)
        {
            if (Hwnd == IntPtr.Zero)
                return;

            WinAPI.WINDOWINFO info = new WinAPI.WINDOWINFO();
            if (!WinAPI.GetWindowInfo(Hwnd, ref info))
                return;

            Location = new System.Drawing.Point(info.rcClient.Left, info.rcClient.Top);
            Size = new System.Drawing.Size(info.rcClient.Right - info.rcClient.Left, info.rcClient.Bottom - info.rcClient.Top);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Renderer.Dispose();
        }
        #endregion

        #region METHODS
        public void Attach(IntPtr hWnd)
        {
            WinAPI.WINDOWINFO info = new WinAPI.WINDOWINFO();
            if (!WinAPI.GetWindowInfo(hWnd, ref info))
                throw new Exception();

            Hwnd = hWnd;
            Renderer.Initialize(hWnd, info.rcClient.Right - info.rcClient.Left, info.rcClient.Bottom - info.rcClient.Top);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (Hwnd != IntPtr.Zero)
                Renderer.Reset(Width, Height);

            WinAPI.MARGINS margins = new WinAPI.MARGINS();
            margins.topHeight = 0;
            margins.bottomHeight = 0;
            margins.leftWidth = Left;
            margins.rightWidth = Right;
            WinAPI.DwmExtendFrameIntoClientArea(this.Handle, ref margins);
        }
        #endregion
    }
}
