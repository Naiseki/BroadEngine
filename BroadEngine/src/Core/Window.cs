using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Mathematics;
using MathKit;
using Broad.Graphics;
using Broad.Audio;

namespace Broad
{  
    class Window: GameWindow
    {
        public static Float2x2 AspectMatrix { get; private set; }
        public static float AspectRatio { get; private set; }


        internal Window(Int2 windowSize, string title, int renderFrequency, int updateFrequency, Type firstScene) : base(
            new GameWindowSettings() { RenderFrequency = renderFrequency, UpdateFrequency = updateFrequency },
            new NativeWindowSettings() {
                 APIVersion = new Version(3, 2), 
                 Flags = ContextFlags.ForwardCompatible,
                 Title = title,
                 Size = new Vector2i(windowSize.x, windowSize.y)
        }) 
        {
            AspectRatio = new Float2(windowSize).Divided;
            AspectMatrix = Float2x2.CreateScale(new Float2(1f / AspectRatio, 1f));
        }


        protected override void OnLoad() 
        {
            Time.Start();
            Input.Init();
            Camera.Update();
            GLProgram.Init();
            ALProgram.Init();
            Interval.Set(Assets.UnloadUnusedAssets, 60f);
        }


        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            Camera.Update();
            Game.ActiveScene.Render();
            Context.SwapBuffers();         
        }


        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            Time.Update();
            Input.Update(KeyboardState);
            Interval.Update();
            Updater.Run();
            Game.ActiveScene.Update();
            ActorCommands.Execute();
        }


        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            float sizeX = (float)Size.X / Window.AspectRatio;
            float sizeY = (float)Size.Y * Window.AspectRatio;

            if (sizeX > sizeY) {
                GL.Viewport(0, 0, (int)((float)Size.Y * Window.AspectRatio), Size.Y);
            }
            else {
                GL.Viewport(0, 0, Size.X, (int)((float)Size.X / Window.AspectRatio));
            }
        }


        protected override void OnMaximized(MaximizedEventArgs e)
        {
            base.OnMaximized(e);
            WindowState = WindowState.Fullscreen;
        }


        protected override void OnClosed()
        {
            GC.Collect();
            base.OnClosed();
        }


        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            Input.MustUpdate = true;
        }


        protected override void OnUnload()
        {
            ALProgram.Deinit();
            GLProgram.Deinit();
        }
    }
}
