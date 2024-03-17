using Veldrid.Sdl2;
using Veldrid;
using System.Diagnostics;
using Veldrid.StartupUtilities;
using System.Numerics;
using ImGuiNET;
using Melanchall.DryWetMidi.Multimedia;

namespace IMidi;

class Program
{
    public static Sdl2Window _window;
    private static GraphicsDevice _gd;
    private static CommandList _cl;
    private static ImGuiController _controller;
    private static Vector3 _clearColor = new(0.45f, 0.55f, 0.6f);

    static void Main(string[] args)
    {
        VeldridStartup.CreateWindowAndGraphicsDevice(
            new WindowCreateInfo(50, 50, 1280, 720, WindowState.Maximized, $"IMidi"),
            new GraphicsDeviceOptions(false, null, true, ResourceBindingModel.Improved, true, true),
            out _window,
            out _gd);
        _window.Resized += () =>
        {
            _gd.MainSwapchain.Resize((uint)_window.Width, (uint)_window.Height);
            _controller.WindowResized(_window.Width, _window.Height);
        };
        _cl = _gd.ResourceFactory.CreateCommandList();
        _controller = new ImGuiController(_gd, _gd.MainSwapchain.Framebuffer.OutputDescription, _window.Width, _window.Height);

        var stopwatch = Stopwatch.StartNew();
        float deltaTime = 0f;

        ImGuiController.LoadImages(_gd, _controller);
        ProgramData.Initialize();

        while (_window.Exists)
        {
            deltaTime = stopwatch.ElapsedTicks / (float)Stopwatch.Frequency;
            stopwatch.Restart();
            InputSnapshot snapshot = _window.PumpEvents();
            if (!_window.Exists) { break; }
            _controller.Update(deltaTime, snapshot);

            if (_window.WindowState == WindowState.Minimized)
            {
                Thread.Sleep(100);
                continue;
            }
            
            RenderUI();

            ImGuiController.UpdateMouseCursor();

            _cl.Begin();
            _cl.SetFramebuffer(_gd.MainSwapchain.Framebuffer);
            _cl.ClearColorTarget(0, new RgbaFloat(_clearColor.X, _clearColor.Y, _clearColor.Z, 1f));
            _controller.Render(_gd, _cl);
            _cl.End();
            _gd.SubmitCommands(_cl);
            _gd.SwapBuffers(_gd.MainSwapchain);
        }

        PlaybackCurrentTimeWatcher.Instance.Dispose();

        _gd.WaitForIdle();
        _controller.Dispose();
        _cl.Dispose();
        _gd.Dispose();
    }

    static void RenderUI()
    {
        ImGui.SetNextWindowPos(Vector2.Zero, ImGuiCond.Once);
        ImGui.SetNextWindowSize(ImGui.GetIO().DisplaySize, ImGuiCond.Once);
        ImGui.Begin("Main", ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.MenuBar);

        DevicesManager.RenderDevicesWindow();
        About.RenderWindow();

        MenuBar.RenderMenubar();

        ImGui.BeginChild("Screen", new(ImGui.GetContentRegionAvail().X, ImGui.GetIO().DisplaySize.Y - (ImGui.GetIO().DisplaySize.Y * 32f / 100)));
        ScreenCanvas.RenderScreen();
        ImGui.EndChild();

        ImGui.GetForegroundDrawList().AddLine(new(0, ImGui.GetCursorPos().Y), new(ImGui.GetIO().DisplaySize.X, ImGui.GetCursorPos().Y), ImGui.GetColorU32(new Vector4(1, 0, 0, 1)), 2);

        ImGui.BeginChild("Keyboard", ImGui.GetContentRegionAvail());
        PianoRenderer.RenderKeyboard();
        ImGui.EndChild();

        ImGui.End();
    }
}