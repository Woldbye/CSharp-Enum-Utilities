using System;
using System.Linq;
using System.Numerics;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace Utilities.Enumerators
{
  public static class EnumExtension
  { 
    // runs in O(n) where n is the amount of defined enums
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
      if (start < 0)
      {
        throw new ArgumentException("The start parameter must be larger than one for BitVector32Extension.FirstInstanceAsEnum");
      }
      int count = start;
      uint infoToInt = (uint) bitvector.Data;
      uint valueOnlyRight = 0;
      while (count >= 0)
      {
        if (infoToInt == 0)
        {
          throw new ArgumentNullException("There is no classes in the container");
        }
        // Value with only the first bit on the right side set.
        valueOnlyRight = infoToInt - (infoToInt - (uint) 1);
        // remove
        infoToInt &= ~valueOnlyRight;
        count--;
      }
      // the index of the first bit set
      // value only right will always be 1 or even number, so Math.Log2 is 
      // castable to uint
      int bitSet = (int) Math.Log((double) valueOnlyRight, 2);
      return (T) Enum.GetValues(typeof(T)).GetValue(bitSet);
    }
  }
}