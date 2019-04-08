﻿#region Copyright (c) 2012-2019 Pieter Geerkens (email: pgeerkens@users.noreply.github.com)
///////////////////////////////////////////////////////////////////////////////////////////
// THis software may be used under the terms of attached file License.md (The MIT License).
///////////////////////////////////////////////////////////////////////////////////////////
#endregion
using System;
using System.Diagnostics;

namespace PGNapoleonics.HexUtilities.Pathfinding {
    /// <summary>A <see cref="DirectedPathCollection"/>Step with a board location and travel direction.</summary>
    [DebuggerDisplay("NeighbourHex: {Hex.Coords} enters from {HexsideEntry}")]
    public struct DirectedPathStepHex : IEquatable<DirectedPathStepHex> {
        /// <summary>Creates a new <see cref="DirectedPathStepHex"/> instance at <paramref name="coords"/> exiting through <see cref="Hexside.North"/>.</summary>
        /// <remarks>
        /// THis is usually only used as the initial step for a new <see cref="DirectedPathCollection"/>.
        /// </remarks>
        public DirectedPathStepHex(HexCoords coords) : this(coords, Hexside.North) {}

        /// <summary>Creates a new <see cref="DirectedPathStepHex"/> instance at <paramref name="coords"/> exiting through <paramref name="hexsideExit"/>.</summary>
        public DirectedPathStepHex(HexCoords coords, Hexside hexsideExit) : this() {
            Coords      = coords;
            HexsideExit = hexsideExit;
        }

        /// <summary>The <see cref="HexCoords"/> of the <see cref="DirectedPathCollection"/>Step.</summary>
        public HexCoords Coords { get; }

        /// <summary>The hexside of the neighbour through which the agent enters from this hex.</summary>
        public Hexside HexsideEntry => HexsideExit.Reversed;

        /// <summary>The hexside of this hex through which the agent exits to the neighbour.</summary>
        public Hexside HexsideExit { get; }

        #region Value Equality with IEquatable<T> - on Hex field only
        /// <inheritdoc/>
        public override bool Equals(object obj) => (obj is DirectedPathStepHex other) && this.Equals(other);

        /// <inheritdoc/>
        public bool Equals(DirectedPathStepHex other) => Coords == other.Coords;

        /// <inheritdoc/>
        public override int GetHashCode() => Coords.GetHashCode();

        /// <summary>Tests value-inequality.</summary>
        public static bool operator !=(DirectedPathStepHex lhs, DirectedPathStepHex rhs) => ! lhs.Equals(rhs);

        /// <summary>Tests value-equality.</summary>
        public static bool operator ==(DirectedPathStepHex lhs, DirectedPathStepHex rhs) =>  lhs.Equals(rhs);
        #endregion

        /// <inheritdoc/>
        public override string ToString() => $"NeighbourHex: {Coords} enters from {HexsideEntry}";
  }
}
