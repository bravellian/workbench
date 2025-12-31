using Terminal.Gui;
using Workbench.Core.VoiceViz;

namespace Workbench.Tui.VoiceViz;

public sealed class EqualizerView : View
{
    private readonly EqualizerModel model;
    private readonly float[] bandBuffer;

    public EqualizerView(EqualizerModel model)
    {
        this.model = model;
        bandBuffer = new float[model.BandCount];
        CanFocus = false;
    }

    public bool ShowLabel { get; set; } = true;

    public override void Redraw(Rect bounds)
    {
        base.Redraw(bounds);
        if (bounds.Width <= 0 || bounds.Height <= 0)
        {
            return;
        }

        var labelHeight = ShowLabel ? 1 : 0;
        var barHeight = Math.Max(1, bounds.Height - labelHeight);
        var level = model.CopySnapshot(bandBuffer);

        if (ShowLabel)
        {
            var label = $"REC {Math.Round(level * 100):0}%";
            Driver.Move(bounds.X, bounds.Y);
            Driver.AddStr(label.PadRight(bounds.Width));
        }

        var bandsToDraw = Math.Min(bandBuffer.Length, bounds.Width);
        if (bandsToDraw <= 0)
        {
            return;
        }

        var bandStep = bandBuffer.Length / (float)bandsToDraw;
        for (var column = 0; column < bandsToDraw; column++)
        {
            var sourceIndex = (int)MathF.Round(column * bandStep);
            if (sourceIndex >= bandBuffer.Length)
            {
                sourceIndex = bandBuffer.Length - 1;
            }
            var value = bandBuffer[sourceIndex];
            var filled = (int)MathF.Round(value * (barHeight - 1));
            for (var row = 0; row < barHeight; row++)
            {
                var y = bounds.Y + labelHeight + (barHeight - 1 - row);
                Driver.Move(bounds.X + column, y);
                Driver.AddRune(row <= filled ? '█' : ' ');
            }
        }

        for (var column = bandsToDraw; column < bounds.Width; column++)
        {
            for (var row = 0; row < barHeight; row++)
            {
                var y = bounds.Y + labelHeight + (barHeight - 1 - row);
                Driver.Move(bounds.X + column, y);
                Driver.AddRune(' ');
            }
        }
    }
}
