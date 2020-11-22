using Utilities;
using System;

namespace Utilities.Integers
{
  public static class Integers
  {
    public static int FirstSetBit(this int number)
    {
      UInt32 c = (UInt32) number;  // your input number
      UInt32 deBrujinIndex = ((UInt32)((c & -c) * (UInt32) 0x077CB531)) >> 27;
      int r = -1;
      if (deBrujinIndex < 32)
      {
        r = Bytes.MultiplyDeBrujinBitPosition[deBrujinIndex];
      }
      Console.WriteLine("set bit in number: " + r);
      Print.IntAsBin(number);
      return r;
    } 
  }
}