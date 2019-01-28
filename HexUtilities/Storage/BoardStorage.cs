﻿#region The MIT License - Copyright (C) 2012-2015 Pieter Geerkens
/////////////////////////////////////////////////////////////////////////////////////////
//                PG Software Solutions Inc. - Hex-Grid Utilities
/////////////////////////////////////////////////////////////////////////////////////////
// The MIT License:
// ----------------
// 
// Copyright (c) 2012-2015 Pieter Geerkens (email: pgeerkens@hotmail.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, 
// merge, publish, distribute, sublicense, and/or sell copies of the Software, and to 
// permit persons to whom the Software is furnished to do so, subject to the following 
// conditions:
//     The above copyright notice and this permission notice shall be 
//     included in all copies or substantial portions of the Software.
// 
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//     EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
//     OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
//     NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
//     HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
//     WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
//     FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
//     OTHER DEALINGS IN THE SOFTWARE.
/////////////////////////////////////////////////////////////////////////////////////////
#endregion
using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;

using PGNapoleonics.HexUtilities.Common;
using PGNapoleonics.HexUtilities.FastLists;

namespace PGNapoleonics.HexUtilities.Storage {
    using HexSize     = System.Drawing.Size;

    /// <summary>Abstract specification and partial implementation of the <c>BoardStorage</c> required by <c>HexBoard</c>.</summary>
    /// <typeparam name="T">The type of the information being stored. 
    /// If {T} implements IDisposable then the Dispose() method will dispose all elements.
    /// </typeparam>
    [ContractClass(typeof(BoardStorageContract<>))]
    public abstract class BoardStorage<T> : IBoardStorage<T>, IForEachable<T>, IForEachable2<T>, IDisposable {
        /// <summary>Initializes a new instance with the specified hex extent.</summary>
        /// <param name="sizeHexes"></param>
        protected BoardStorage(HexSize sizeHexes) {
              MapSizeHexes   = sizeHexes;
        }

        /// <inheritdoc/>
        public       HexSize MapSizeHexes           { get; }

        /// <summary>Returns the <c>THex</c> instance at the specified coordinates.</summary>
        [Pure, SuppressMessage("Microsoft.Design", "CA1043:UseIntegralOrStringArgumentForIndexers")]
        public          T    this[HexCoords coords] => this[coords.User];

        /// <summary>Returns the <c>THex</c> instance at the specified user coordinates.</summary>
        [Pure, SuppressMessage("Microsoft.Design", "CA1043:UseIntegralOrStringArgumentForIndexers")]
        public virtual  T    this[IntVector2D userCoords] {
            get { 
              //  Contract.Ensures( ! MapSizeHexes.IsOnboard(userCoords)  ||  Contract.Result<T>() != null);
                return MapSizeHexes.IsOnboard(userCoords) ? ItemInner (userCoords.X,userCoords.Y) : default(T);
            }
        }

        #pragma warning disable 3008
        /// <summary>TODO</summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        [Pure]
        protected abstract T ItemInner(int x, int y);
        #pragma warning restore 3008

        /// <inheritdoc/>
        public          void ForAllNeighbours(HexCoords coords, Action<T,Hexside> action) {
            action.RequiredNotNull("action");

            Hexside.ForEach( hexside =>
                action(this[coords.GetNeighbour(hexside)], hexside)
            );
        }
        /// <inheritdoc/>
        void IBoardStorage<T>.ForAllNeighbours(HexCoords coords, Action<T, Hexside> action) =>
            ForAllNeighbours(coords, action);

        /// <inheritdoc/>
        public          T    Neighbour(HexCoords coords, Hexside hexside) =>
           this[coords.GetNeighbour(hexside)]; 


        /// <summary>Perform the specified <paramref name="action"/> in parallel on all hexes.</summary>
        public abstract void ForEach(Action<T> action);
        /// <summary>Perform the Invoke action of the specified <paramref name="functor"/> in parallel on all hexes.</summary>
        public abstract void ForEach(FastIteratorFunctor<T> functor);

        /// <summary>Perform the specified <paramref name="action"/> in parallel on all hexes.</summary>
        public abstract void ForEachSerial(Action<T> action);
        /// <summary>Perform the Invoke action of the specified <paramref name="functor"/> in parallel on all hexes.</summary>
        public abstract void ForEachSerial(FastIteratorFunctor<T> functor);

        /// <summary>Sets the location to the specified value.</summary>
        /// <remarks>Use carefully - can interfere with iterators.</remarks>
        internal abstract void SetItem(HexCoords coords, T value);

        #region IDisposable implementation with Finalizeer
        /// <summary>Clean up any resources being used, and suppress finalization.</summary>
        public void Dispose() { Dispose(true); GC.SuppressFinalize(this); }
        /// <summary>True if already Disposed.</summary>
        private bool _isDisposed = false;
        /// <summary>Clean up any resources being used.</summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected virtual void Dispose(bool disposing) {
            if (!_isDisposed) {
                if (disposing) {
                    if (typeof(T).GetInterfaces().Contains(typeof(IDisposable))) {
                        ForEach(i => { var item = i as IDisposable; if(item != null) item.Dispose(); i = default(T);} );
                    }
                }
                _isDisposed = true;
            }
        }
        /// <summary>Finalize this instance.</summary>
        ~BoardStorage() { Dispose(false); }
        #endregion
    }

    [ContractClassFor(typeof(BoardStorage<>))]
    internal abstract class BoardStorageContract<T> : BoardStorage<T> {
        private BoardStorageContract(HexSize sizeHexes) : base(sizeHexes) { }

        [Pure]protected override T ItemInner(int x, int y) {
        //  Contract.Requires(MapSizeHexes.IsOnboard(x,y));
        //  Contract.Ensures(Contract.Result<T>() != null);

          return default(T);
        }

        public override void ForEachSerial(Action<T> action) =>
            action.RequiredNotNull("action");

        /// <summary>TOTO</summary>
        public override void ForEachSerial(FastIteratorFunctor<T> functor) =>
            functor.RequiredNotNull("functor");

    }
}
