using System;
using System.Linq;
using System.Numerics;
using System.Collections;
using Utilities.Integers;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace Utilities.Enumerators
{
  public static class EnumExtension
  { 
    public static int IndexOfIgnoreCase(this char[] charArr, char value)
    {
      int j = -1;
      for (int i=0; i < charArr.Length; i++)
      {
        bool same = char.ToUpper(charArr[i]).Equals(char.ToUpper(value));
        if (same) 
        {
          j = i;
          return j;
        }
      }
      return j;
    }


    public static int IndexOfIgnoreCase<TEnum>(this EnumIteratedList<char, TEnum> chars, char value)
      where TEnum : struct, IConvertible, IComparable, IFormattable
    {
      int j = -1;
      for (int i=0; i < chars.Length; i++)
      {
        string charAsStr = chars[i].ToString();
        bool same = String.Equals(value.ToString(), charAsStr, StringComparison.OrdinalIgnoreCase);
        if (same) 
        {
          j = i;
          return j;
        }
      }
      return j;
    }


    public static int IndexOfIgnoreCase<TEnum>(this EnumIteratedList<char, TEnum> chars, string value)
      where TEnum : struct, IConvertible, IComparable, IFormattable
    {
      int j = -1;
      for (int i=0; i < chars.Length; i++)
      {
        string charAsStr = chars[i].ToString();
        bool same = String.Equals(value, charAsStr, StringComparison.OrdinalIgnoreCase);
        if (same) 
        {
          j = i;
          return j;
        }
      }
      return j;
    }

    public static int IndexOfIgnoreCase<TEnum>(this EnumIteratedList<string, TEnum> strings, char value)
      where TEnum : struct, IConvertible, IComparable, IFormattable
    {
      int j = -1;
      for (int i=0; i < strings.Length; i++)
      {
        bool same = String.Equals(value.ToString(), strings[i], StringComparison.OrdinalIgnoreCase);
        if (same)
        {
          j = i;
          return j;
        }
      }
      return j;
    }

    // runs in O(n) where n is the amount of defined enums
    // Should use class EnumToIntConverter for faster calculations
    public static int ToArrayIndex(this Enum instance)
    {
      int i = 0;
      foreach (Enum instance_ in Enum.GetValues(instance.GetType()))
      {
        if (instance_.Equals(instance))
        {
          break;
        }
        i++;
      }
      return i;
    }

    // Returns the maximum integer value of the input enumerator.
    public static int Max(this Enum eType)
    {
      return Enum.GetValues(eType.GetType()).Cast<int>().Max();
    }

    // Returns the maximum integer value of the input enumerator.
    public static int Min(this Enum eType)
    {
      return Enum.GetValues(eType.GetType()).Cast<int>().Min();
    }

    // returns the first value in set in the bitvector as an enum. Starts from position ddefined by parameter "start"
    public static T FirstInstanceAsEnum<T> (this BitVector32 bitvector, int start = 0)
    {
      if (start > 32)
      {
        throw new ArgumentException("The start parameter must be larger than one for BitVector32Extension.FirstInstanceAsEnum");
      }
      int count = (start < 0) ? 0 : start;
      int bitSetShiftedVector = (bitvector.Data >> count).FirstSetBit();
      if (bitSetShiftedVector == -1)
      {
        throw new ArgumentException("No set bit for the vector starting at position " + start);
      }
      int bitSet = bitSetShiftedVector + count;

      return (T) Enum.GetValues(typeof(T)).GetValue(bitSet);
    }
  }
}