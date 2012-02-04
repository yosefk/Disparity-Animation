using AForge;
using AForge.Imaging;
using System;

class BeierNeelyMorph
{
    public static UnmanagedImage morph1(UnmanagedImage srcimg, IntLine[] srclines, IntLine[] dstlines)
    {
        if (srclines.Length != dstlines.Length)
        {
            throw new ArgumentException(string.Format("srclines length ({0}) differs from dstlines length ({1})", srclines.Length, dstlines.Length));
        }
        return null;
    }

    public static UnmanagedImage test_morph1()
    {
        UnmanagedImage srcimg = null;
        IntLine[] srclines = new IntLine[10];
        IntLine[] dstlines = new IntLine[9];
        return morph1(srcimg, srclines, dstlines);
    }
}