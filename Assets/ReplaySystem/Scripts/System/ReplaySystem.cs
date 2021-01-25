using System;

public static class ReplaySystem
{
    public static float GlobalTime = 0;
    public static int GlobalFrame = 0;
    public static EventHandler FrameHandler;
    public static string RecordDataPath = "";
    public static void UpdateFrame()
    {
        if (FrameHandler != null)
        {
            FrameHandler(typeof(ReplaySystem), EventArgs.Empty);
        }
    }
}
