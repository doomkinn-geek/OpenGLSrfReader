using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLSrfReader;

public class SrfFileData
{
    public string Prefix { get; set; }
    public int FrameWidth { get; set; }
    public int FrameHeight { get; set; }
    public int Contrast { get; set; }
    public int Brightness { get; set; }
    public bool IsNegative { get; set; }
    public string ContextVisionLUT { get; set; }
    public string ContextVisionGOP { get; set; }
    public short KalimatorLeft { get; set; }
    public short KalimatorTop { get; set; }
    public short KalimatorRight { get; set; }
    public short KalimatorBottom { get; set; }
    public byte BitDepth { get; set; }
    public short PixelSize { get; set; }
    public bool VerticalFlip { get; set; }
    public bool HorizontalFlip { get; set; }
    public short RotationDegree { get; set; }
    public int NormalizationIndex { get; set; }
    public short MinimalAdjustmentLevel { get; set; }
    public string OrientationString { get; set; }
    public ushort[] PixelData { get; set; } // 2-byte unsigned short for pixel data
}
