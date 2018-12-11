using SuperiorHackBase.Core.ProcessInteraction;
using SuperiorHackBase.Graphics.UI;
using SuperiorHackBase.Graphics.UI.Controls;
using SuperiorHackBase.Rendering.SharpDX.D2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SuperiorHackBase.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var ctx = LocalHackContext.CreateContext("notepad");
            var renderer = new D2D1Renderer() { BackgroundColor = new Graphics.Color4f(0f, 0f, 0f) };
            var overlay = new Overlay(renderer);

            overlay.Attach(ctx.Process.WindowHandle);

            var panel = new Panel() { Position = new Core.Maths.Vector2(100, 100), Size = new Core.Maths.Vector2(200, 200) };
            var label = new Label() { Position = new Core.Maths.Vector2(50, 50), Size = new Core.Maths.Vector2(100, 100), Text = "Test!" };

            panel.AddChild(label);

            overlay.Controls.Add(panel);
            var thread = new Thread(() =>
            {
                while (true)
                {
                    if (renderer.Initialized)
                    {
                        renderer.StartFrame();
                        panel.Draw(renderer);
                        renderer.EndFrame();
                    }
                    Thread.Sleep(1);
                }
            });
            overlay.Shown += (o, e) => thread.Start();
            overlay.ShowDialog();
        }
    }
}
