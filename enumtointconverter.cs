using System;
using System.Collections.Generic;

namespace Utilities.Enumerators
{
  // WARNING: 
  // The index is based on the array returned by Enum.GetValues(typeof(TEnum)). This array is sorted by 
  // it's binary value NOT textual order.
  // Forexample: If we have enum TesterBin: 
  //  [Flags]
  //  public enum TesterBin : byte   
  //  {
  //    fst = 0xFF,
  //    snd = 0xF0,
  //    thrd = 0x00
  //  }
  // The converter would map TesterBin.thrd to index 0 and fst to index 2.
  public sealed class EnumToIntConverter<TEnum> where TEnum : struct, IConvertible, IComparable, IFormattable
  {
    private Dictionary<TEnum, Int32> _indexLookUp;
    private int _size;
    private static EnumToIntConverter<TEnum> _Instance;

    // avoid setting beforeFieldInit
    static EnumToIntConverter() {}

    private EnumToIntConverter()
    {
      if (!typeof(TEnum).IsEnum)
      {
        throw new ArgumentException(String.Format("The generic type EnumToIntConverter<T> only accepts enumerators."));
      }
      int i = 0;
      this._size = Enum.GetValues(typeof(TEnum)).Length;
      this._indexLookUp = new Dictionary<TEnum, Int32>(this._size);
      foreach (TEnum valAsEnum in Enum.GetValues(typeof(TEnum)))
      {
        this._indexLookUp.Add(valAsEnum, i);
        #if (DEBUG)
          Console.WriteLine("added mapping between: " + valAsEnum + " and " + i);
        #endif
        i++;
      }
    }

    // no set for obvious reasons :)
    public Int32 this[TEnum iterator]
    {
      get 
      {
        return this._indexLookUp[iterator];
      }
    }

    public int Size
    {
      get 
      {
        return this._size;
      }
    }

    public static EnumToIntConverter<TEnum> Instance
    {
      get 
      {
        if (EnumToIntConverter<TEnum>._Instance == null)
        {
          return new EnumToIntConverter<TEnum>();
        }
        return EnumToIntConverter<TEnum>._Instance;
      }
    }
  }
}