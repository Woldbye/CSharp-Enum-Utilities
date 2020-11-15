using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Utilities.Enumerators
{
  // Max size of enum can be 32
  // This class is a bool container for enums. 
  //  For each bit b in _info with index i: b denotes whether this container contains the enumerator Enum.GetValue(i).
  public class HasEnum32Container<TEnum> : IEquatable<HasEnum32Container<TEnum>> where TEnum : struct, IConvertible, IComparable, IFormattable
  { 
    // index converter
    private static EnumToIntConverter<TEnum> _Converter = EnumToIntConverter<TEnum>.Instance;
    public const int CAST_FAILURE = Int32.MinValue;
    protected static BitMasks _Masks = BitMasks.Instance;
    // implement all operations with bitarrays :)
    protected BitVector32 _info;

    // Constructor
    public HasEnum32Container() 
    { 
      // constructor receives initial value of vector
      this._info = new BitVector32(0); 
    }

    // getters
    protected int Data { get { return this._info.Data; } }
    public int AsInt { get { return this.Data;} }
    public Type TypeOfEnum { get { return typeof(TEnum); } }
    public int Size { get { return HasEnum32Container<TEnum>._Converter.Size; } }
    public bool IsEmpty { get { return (this.AsInt != 0) ? false : true; } }

    public bool this[TEnum i]
    {
      get { return this._info[HasEnum32Container<TEnum>._Converter[i]]; }
      set { this._info[HasEnum32Container<TEnum>._Converter[i]] = value; }
    }

    protected int GetMask(int i)
    {
      return HasEnum32Container<TEnum>._Masks.GetAs<Int32>(i); 
    }

    public TEnum GetFirstInstance(int start = 0)
    {
      return this._info.FirstInstanceAsEnum<TEnum>();
    }

    public void Clear()
    {
      for (int i=0; i < this.Size; i++)
      {
        if (this._info[i]) { this._info[i] = false; }
      }
    }

    // force atleast one argument
    public void Add(TEnum[] values)
    {
      foreach (TEnum val in values)
      {
        this[val] = true;
      }
    }    

    // force atleast one argument
    public void Add(TEnum fst, params TEnum[] values)
    {
      this[fst] = true;
      foreach (TEnum val in values)
      {
        this[val] = true;
      }
    }

    // force atleast one argument
    public bool Contains(TEnum[] values) 
    {
      foreach (TEnum val in values)
      {
        if (!this[val])
        {
          return false;
        }
      }
      return true;
    }

    // force atleast one argument
    public bool Contains(TEnum fst, params TEnum[] values) 
    {
      if (!this[fst])
      {
        return false;
      }

      foreach (TEnum val in values)
      {
        if (!this[val])
        {
          return false;
        }
      }
      return true;
    }

    // force atleast one argument
    public void Remove(TEnum[] values)
    {
      foreach (TEnum val in values)
      {
        this[val] = false;
      }
    }
    // force atleast one argument
    public void Remove(TEnum fst, params TEnum[] values)
    {
      this[fst] = false;
      foreach (TEnum val in values)
      {
        this[val] = false;
      }
    }

    public bool Equals(HasEnum32Container<TEnum> other)
    {
      if (other == null) 
      {
        return false;
      }

      return this._info.Equals(other._info);
    }

    public override bool Equals(object obj)
    {
      if (obj == null) 
      {
        return false;
      }
      HasEnum32Container<TEnum> objAsContainer = obj as HasEnum32Container<TEnum>;
      if (objAsContainer == null)
      {
        return false;
      } else {
        return Equals(objAsContainer);
      } 
    }

    public override int GetHashCode() { return this._info.GetHashCode(); }
  }
}