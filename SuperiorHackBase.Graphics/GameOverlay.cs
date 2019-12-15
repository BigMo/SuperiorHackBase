using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX.Windows;
using SuperiorHackBase.Core;
using SuperiorHackBase.Core.Maths;
using SuperiorHackBase.Core.ProcessInteraction;
using SuperiorHackBase.Graphics.Controls;
using SuperiorHackBase.Input;

namespace SuperiorHackBase.Graphics
{
    public class GameOverlay : RenderForm
    {
        public Renderer Renderer { get; private set; }
        public bool DrawOnlyWhenInForeground { get; set; }
        public event EventHandler<RenderingEventArgs> Drawing;
        public float RefreshRate { get => drawingTimer.Tickrate; set => drawingTimer.Tickrate = value; }
        public bool DrawingEnabled => drawingTimer.Enabled;
        public Control RootControl => rootControl;
        public new Vector2 Location => new Vector2(base.Location.X, base.Location.Y);
        public new Vector2 Size => new Vector2(base.Width, base.Height);
        public Rectangle OverlayRectangle => new Rectangle(Vector2.Zero, Size);
        public Rectangle ScreenRectangle => new Rectangle(Location.X, Location.Y, Width, Height);

        private TickrateTimer drawingTimer;
        private IHackContext hackContext;
        private IntPtr handle;
        private Control rootControl, mouseOverControl;
        private DateTime lastFrame;

        public GameOverlay(IHackContext hackContext, string text) : base(text)
        {
            lastFrame = DateTime.Now;
            this.hackContext = hackContext;
            handle = Handle;
            rootControl = new Control();
            Renderer = new Renderer();
            drawingTimer = new TickrateTimer();
            drawingTimer.Tick += DrawForm;
            drawingTimer.Enabled = true;
            drawingTimer.Interval = 10;
            UserResized += GameOverlay_UserResized;
            CreateResources();

            //Setup form-properties
            DoubleBuffered = true;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            BackColor = System.Drawing.Color.Blue;
            TransparencyKey = BackColor;
            TopMost = true;
            TopLevel = true;

            int initialStyle = WinAPI.GetWindowLong(handle, (int)WinAPI.GetWindowLongFlags.GWL_EXSTYLE);
            WinAPI.SetWindowLong(handle, (int)WinAPI.GetWindowLongFlags.GWL_EXSTYLE, initialStyle | (int)WinAPI.ExtendedWindowStyles.WS_EX_LAYERED | (int)WinAPI.ExtendedWindowStyles.WS_EX_TRANSPARENT);
            WinAPI.SetWindowPos(handle, (IntPtr)WinAPI.SetWindpwPosHWNDFlags.TopMost, 0, 0, 0, 0, (uint)WinAPI.SetWindowPosFlags.NOMOVE | (uint)WinAPI.SetWindowPosFlags.NOSIZE);
            WinAPI.SetLayeredWindowAttributes(handle, 0, 255, (uint)WinAPI.LayeredWindowAttributesFlags.LWA_ALPHA);

            ExtendForm();

            DrawOnlyWhenInForeground = true;
        }

        public void RegisterHooks(KeyboardHook key, MouseHook mouse)
        {
            key.KeyDown += KeyDownEvent;
            key.KeyUp += KeyUpEvent;
            mouse.MouseEvent += MouseEvent;
        }

        public void UnregisterHooks(KeyboardHook key, MouseHook mouse)
        {
            key.KeyDown -= KeyDownEvent;
            key.KeyUp -= KeyUpEvent;
            mouse.MouseEvent -= MouseEvent;
        }

        private void BroadcastMessage(UIMessage msg)
        {
            if (msg.HasMouseEvent)
            {
                mouseOverControl = rootControl.GetMouseControl(msg.MouseEvent.Position);
                rootControl.ProcessMouseEvent(mouseOverControl, msg.MouseEvent);     
            }
        }

        private void MouseEvent(object sender, MouseEventExtArgs e)
        {
            if (!ScreenRectangle.Intersects(e.Position)) return; //Discard events outside of the form
            BroadcastMessage(new UIMessage(e.MakeRelative(Location)));
        }

        private void KeyUpEvent(object sender, KeyEventExtArgs e)
        {
            BroadcastMessage(new UIMessage(e));
        }

        private void KeyDownEvent(object sender, KeyEventExtArgs e)
        {
            BroadcastMessage(new UIMessage(e));
        }

        private void DrawForm(object sender, EventArgs e)
        {
            TrackWindow();

            var now = DateTime.Now;

            Renderer.Device.BeginDraw();
            Renderer.Device.Clear(null);

            if (DrawOnlyWhenInForeground && !hackContext.Process.InForeground)
            {
                Renderer.Device.EndDraw();
                return;
            }

            rootControl.Draw(Renderer);

            Drawing?.Invoke(this, new RenderingEventArgs(Renderer, this, now - lastFrame));

            lastFrame = DateTime.Now;
            Renderer.Device.EndDraw();
        }

        private void TrackWindow()
        {
            WinAPI.WINDOWINFO info = new WinAPI.WINDOWINFO();
            if (WinAPI.GetWindowInfo(hackContext.Process.MainWindow, ref info))
            {
                var width = info.rcWindow.Right - info.rcWindow.Left;
                var height = info.rcWindow.Bottom - info.rcWindow.Top;
                if (base.Location.X != info.rcClient.Left ||
                    base.Location.Y != info.rcClient.Top)
                {
                    base.Location = new System.Drawing.Point(info.rcClient.Left, info.rcClient.Top);
                }
                if (Width != info.rcClient.Right - info.rcClient.Left ||
                    Height != info.rcClient.Bottom - info.rcClient.Top)
                {
                    base.Size = new System.Drawing.Size(info.rcClient.Right - info.rcClient.Left, info.rcClient.Bottom - info.rcClient.Top);
                }
                WinAPI.SetWindowPos(handle, handle, info.rcWindow.Left, info.rcWindow.Top, width, height, 0);
            }
        }

        private void GameOverlay_UserResized(object sender, EventArgs e)
        {
            CreateResources();
            rootControl.Size = Size;
            ExtendForm();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            ExtendForm();
        }

        private void ExtendForm()
        {
            WinAPI.MARGINS margins = new WinAPI.MARGINS();
            margins.topHeight = 0;
            margins.bottomHeight = 0;
            margins.leftWidth = Left;
            margins.rightWidth = Right;
            WinAPI.DwmExtendFrameIntoClientArea(handle, ref margins);
        }

        protected virtual void CreateResources()
        {
            Renderer.Initialize(handle, ClientSize.Width, ClientSize.Height);
        }
    }
}
