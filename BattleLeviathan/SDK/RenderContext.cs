﻿using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using Vector2 = SharpDX.Vector2;

class RenderContext
{
    public WindowRenderTarget target;

    public RenderContext(WindowRenderTarget target)
    {
        this.target = target;
    }

    /// <summary>
    /// Get solidcolorbrush of colour
    /// </summary>
    /// <param name="colour"></param>
    /// <returns></returns>
    public SolidColorBrush GetBrush(Color4 colour)
    {
        return new SolidColorBrush(target, colour);
    }

    /// <summary>
    /// Convert a pos & size to RawRectangleF
    /// </summary>
    public RawRectangleF GetRect(Vector2 pos, Vector2 size)
    {
        RectangleF rectDims = new RectangleF
        {
            Location = pos,
            Size = new Size2F(size.X, size.Y)
        };

        RawRectangleF rect = new RawRectangleF();

        rect.Left = rectDims.Left;
        rect.Right = rectDims.Right;
        rect.Top = rectDims.Top;
        rect.Bottom = rectDims.Bottom;

        return rect;
    }

    /// <summary>
    /// Get font of size
    /// </summary>
    public TextFormat GetFont(string font, int size)
    {
        return new TextFormat(new SharpDX.DirectWrite.Factory(), font, size);
    }

    /// <summary>
    /// Begin the drawing of the next frame
    /// </summary>
    public void Begin() => target.BeginDraw();

    /// <summary>
    /// end the drawing of the next frame then present it
    /// </summary>
    public void End() => target.EndDraw();

    /// <summary>
    /// Clear the screen with the specified colour
    /// </summary>
    public void Clear(Color4 colour) => target.Clear(colour);

    /// <summary>
    /// Draw a straight line from pos1 to pos2 with x width & colour
    /// </summary>
    public void DrawLine(Vector2 pos1, Vector2 pos2, Color4 colour, int width = 2)
    {
        target.DrawLine(
            pos1,
            pos2,
            GetBrush(colour),
            width
        );
    }

    /// <summary>
    /// Draw a simple string of text at pos of color colour with font & size
    /// </summary>
    public void DrawString(string text, Vector2 pos, Color4 colour, string font, int size)
    {
        target.DrawText(
            text,
            GetFont(font, size),
            new RectangleF(pos.X, pos.Y, target.PixelSize.Width, target.PixelSize.Height),
            GetBrush(colour)
        );
    }

    /// <summary>
    /// Draw a filled rectangle at pos1 with size of pos2 and colour
    /// </summary>
    public void FillRectangle(Vector2 pos1, Vector2 pos2, Color4 colour)
    {
        target.FillRectangle(
            GetRect(pos1, pos2),
            GetBrush(colour)
        );
    }

    /// <summary>
    /// Draw a filled rectangle at pos1 with size of pos2 with rounded corners of radius and colour
    /// </summary>
    public void FillRoundedRectangle(Vector2 pos1, Vector2 pos2, Vector2 radius, Color4 colour)
    {
        RoundedRectangle rect = new RoundedRectangle();

        rect.Rect = GetRect(pos1, pos2);
        rect.RadiusX = radius.X;
        rect.RadiusY = radius.Y;

        target.FillRoundedRectangle(
            rect,
            GetBrush(colour)
        );
    }
}