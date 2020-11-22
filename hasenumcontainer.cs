using System;
using Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Utilities.Enumerators
{
  // Max size of enum can be 32
  // This class is a bool container for enums. 
  //  For each bit b in _info with index i: b denotes whether this container contains the enumerator Enum.GetValue(i).
  public class HasEnum32Container<TEnum> : IEquatable<HasEnum32Container<TEnum>>, IEnumerable<TEnum> 
    where TEnum : struct, IConvertible, IComparable, IFormattable 
  { 
    // index converter
    private static EnumToIntConverter<TEnum> _Converter = EnumToIntConverter<TEnum>.Instance;
    public const int CAST_FAILURE = Int32.MinValue;
    // implement all operations with bitarrays :)
    protected BitVector32 _info;
    protected static BitMasks _masks = BitMasks.Instance;
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
      get 
      {
        int index = this.GetMask(HasEnum32Container<TEnum>._Converter[i] + 1); 
        return this._info[index]; 
      }
      set 
      {
        int index = this.GetMask(HasEnum32Container<TEnum>._Converter[i] + 1); 
        this._info[index] = value; 
      }
    }

    public void PrintRaw()
    {
      Console.WriteLine("Printing BitVector as 32Binary: ");
      Print.IntAsBin(this._info.Data);
    }

    protected int GetMask(int index)
    {
      return HasEnum32Container<TEnum>._masks.GetAs<Int32>(index);
    }

    public TEnum GetFirstInstance(TEnum start)
    {
      return this.GetFirstInstance(HasEnum32Container<TEnum>._Converter[start]);
    }

    public TEnum GetFirstInstance(int start = 0)
    {
      return this._info.FirstInstanceAsEnum<TEnum>(start);

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
      Console.WriteLine("Trying to add " + fst + " to " + this._info.Data);
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

    public IEnumerator<TEnum> GetEnumerator()
    {
      return new HasEnum32Enumerator<TEnum>(this._info);
    }

    private IEnumerator GetEnumerator1()
    {
        return this.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator1();
    }
  }

  // We only want to get objects that are currently set.
  // so foreach should go return each enum that are set, starting at smallest in terms of numerical value
  public class HasEnum32Enumerator<TEnum> : IEnumerator<TEnum>
    where TEnum : struct, IConvertible, IComparable, IFormattable
  {
    private int _current;
    private static EnumToIntConverter<TEnum> _intConverter = EnumToIntConverter<TEnum>.Instance;
    private readonly BitVector32 _container;
    private int _size;

    public HasEnum32Enumerator(BitVector32 container)
    {
      this._container = container;
      this._size = Enum.GetValues(typeof(TEnum)).Length;
      this._current = -1;
    }



    public TEnum Current
    {
      get 
      {
        /*
        if ((this._container.Data == 0) || (this._current == -1))
        {
          throw new InvalidOperationException("There is no set set bits, the Enum container might be empty");
        }
        */
        return HasEnum32Enumerator<TEnum>._intConverter.IntToEnum(this._current);
      }
    }

    private object Current1 { get { return this.Current; } } 
    object IEnumerator.Current { get { return Current1; } } 
    
    public bool MoveNext()
    { 
      bool MoveSuccess = false;
      if (this._container.Data != 0 && (this._current < this._size))
      {
        try 
        {
          TEnum nxt = this._container.FirstInstanceAsEnum<TEnum>(this._current);
          this._current = HasEnum32Enumerator<TEnum>._intConverter[nxt];
          MoveSuccess = true;
        } catch (ArgumentNullException)
        {
          return false;
        } // return false
      }
      return MoveSuccess;
    }

    public void Reset()
    {
      this._current = -1;
    }

    public void Dispose(){}
  }


}