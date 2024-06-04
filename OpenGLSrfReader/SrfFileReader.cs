using System;
using System.IO;
using System.Linq;
using System.Text;

namespace OpenGLSrfReader;

public class SrfFileReader
{
    private string filePath;
    private const int SizeHeader = 0x100;

    public SrfFileReader(string filePath)
    {
        this.filePath = filePath;
    }

    public SrfFileData ReadSrfFile()
    {
        var srfData = new SrfFileData();

        if (!System.IO.File.Exists(filePath))
        {
            filePath = Path.ChangeExtension(filePath, ".raw");
        }
        if (!System.IO.File.Exists(filePath))
        {
            filePath = Path.ChangeExtension(filePath, ".srf");
        }
        if (!System.IO.File.Exists(filePath))
        {
            throw new FileNotFoundException("Файл снимка не найден");
        }

        using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        using (var binaryReader = new BinaryReader(fileStream))
        {
            byte[] headerBytes = binaryReader.ReadBytes(SizeHeader);

            srfData.Prefix = Encoding.ASCII.GetString(headerBytes.Take(4).ToArray());
            srfData.FrameWidth = BitConverter.ToInt32(headerBytes, 4);
            srfData.FrameHeight = BitConverter.ToInt32(headerBytes, 8);
            srfData.Contrast = BitConverter.ToInt32(headerBytes, 12);
            srfData.Brightness = BitConverter.ToInt32(headerBytes, 16);
            srfData.IsNegative = headerBytes[20] == 1;
            srfData.ContextVisionLUT = Encoding.ASCII.GetString(headerBytes.Skip(21).Take(4).ToArray());
            srfData.ContextVisionGOP = Encoding.ASCII.GetString(headerBytes.Skip(25).Take(4).ToArray());
            srfData.KalimatorLeft = BitConverter.ToInt16(headerBytes, 29);
            srfData.KalimatorTop = BitConverter.ToInt16(headerBytes, 31);
            srfData.KalimatorRight = BitConverter.ToInt16(headerBytes, 33);
            srfData.KalimatorBottom = BitConverter.ToInt16(headerBytes, 35);
            srfData.BitDepth = headerBytes[37];
            srfData.PixelSize = BitConverter.ToInt16(headerBytes, 38);
            srfData.VerticalFlip = headerBytes[40] == 1;
            srfData.HorizontalFlip = headerBytes[41] == 1;
            srfData.RotationDegree = BitConverter.ToInt16(headerBytes, 42);
            srfData.NormalizationIndex = BitConverter.ToInt32(headerBytes, 44);
            srfData.MinimalAdjustmentLevel = BitConverter.ToInt16(headerBytes, 48);
            srfData.OrientationString = Encoding.ASCII.GetString(headerBytes.Skip(50).Take(4).ToArray());

            if (!(srfData.Contrast >= -8191 && srfData.Contrast <= 8191))
                srfData.Contrast = 0;

            if (!(srfData.Brightness >= -8191 && srfData.Brightness <= 8191))
                srfData.Brightness = 0;

            var pixelDataCount = srfData.FrameWidth * srfData.FrameHeight;
            srfData.PixelData = new ushort[pixelDataCount];
            for (int i = 0; i < pixelDataCount; i++)
            {
                srfData.PixelData[i] = binaryReader.ReadUInt16();
            }
        }

        return srfData;
    }
}
