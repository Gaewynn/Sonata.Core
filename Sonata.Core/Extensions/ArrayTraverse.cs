using System;

namespace Sonata.Core.Extensions
{
    internal class ArrayTraverse
    {
        #region Members

        private readonly int[] _maxLengths;

        #endregion

        #region Properties

        public int[] Position { get; set; }

        #endregion

        #region Constructors

        public ArrayTraverse(Array array)
        {
            _maxLengths = new int[array.Rank];
            for (var i = 0; i < array.Rank; ++i)
                _maxLengths[i] = array.GetLength(i) - 1;

            Position = new int[array.Rank];
        }

        #endregion

        #region Methods

        public bool Step()
        {
            for (var i = 0; i < Position.Length; ++i)
            {
                if (Position[i] >= _maxLengths[i]) 
                    continue;

                Position[i]++;
                for (var j = 0; j < i; j++)
                    Position[j] = 0;

                return true;
            }

            return false;
        }

        #endregion
    }
}
