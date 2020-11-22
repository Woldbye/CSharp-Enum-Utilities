using System;
using System.Collections.Generic;

namespace Utilities.Enumerators
{




  // TEnum should be a Enum integer type
  // T is the type the list should hold
  // The constructor will initialize a EnumIteratedList with all values the enum span set to default(T)
  public class EnumIteratedList<T, TEnum> : List<T> 
    where TEnum : struct, IConvertible, IComparable, IFormattable
    // TO:DO implement equality ICompareable<EnumIteratedList<T>>, IEqua
  {  
    private static EnumToIntConverter<TEnum> _Converter = EnumToIntConverter<TEnum>.Instance;

    public EnumIteratedList() : base(EnumIteratedList<T, TEnum>._Converter.Size)
    {
        for (int i=0; i < EnumIteratedList<T, TEnum>._Converter.Size; i++)
        {
          base.Add(default(T));
        }
    }

    public EnumIteratedList(ICollection<T> initial) : base(initial)
    {
      if (initial.Count != base.Count)
      {
        Console.WriteLine("Invalid length of received initial values");
      }
    } 

    // if specific default value
    public EnumIteratedList(T initial) : base(EnumIteratedList<T, TEnum>._Converter.Size)
    {
      for (int i=0; i < EnumIteratedList<T, TEnum>._Converter.Size; i++)
      {
        base.Add(initial);
      }
    }

    public int Length { get { return base.Count; } }
    public Type TypeOfEnum { get { return typeof(TEnum); } }
    public List<T> AsRegularList() { return this as List<T>; }

    public T this[TEnum iterator] 
    { 
      get { return base[EnumIteratedList<T, TEnum>._Converter[iterator]]; } 
      set { base[EnumIteratedList<T, TEnum>._Converter[iterator]] = value; }
    }

    public bool Contains(T[] items)
    {
      foreach (T item in items)
      {
        if (!base.Contains(item))
        {
          return false;
        }
      }
      return true;
    }


    public void Insert(TEnum iterator, T item)
    {
      base.Insert(EnumIteratedList<T, TEnum>._Converter[iterator], item); 
    }

    public void RemoveAt(TEnum iterator)
    {
      this.RemoveAt(EnumIteratedList<T, TEnum>._Converter[iterator]);
    }
  }
}