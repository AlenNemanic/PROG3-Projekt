using System;

namespace Projekt
{
    public class Iskalno_dvojisko_drevo<T> : Dvojisko_drevo<T> where T : IComparable<T>
    {
        public Iskalno_dvojisko_drevo(T vrednost) : base(vrednost)
        {
        }
    }
}