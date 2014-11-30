﻿#region The MIT License - Copyright (C) 2012-2014 Pieter Geerkens
/////////////////////////////////////////////////////////////////////////////////////////
//                PG Software Solutions Inc. - Hex-Grid Utilities
/////////////////////////////////////////////////////////////////////////////////////////
// The MIT License:
// ----------------
// 
// Copyright (c) 2012-2013 Pieter Geerkens (email: pgeerkens@hotmail.com)
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
using System.Drawing;
using System.Drawing.Drawing2D;

using PGNapoleonics.HexgridPanel;
using PGNapoleonics.HexUtilities;

namespace PGNapoleonics.HexgridExampleCommon {

  /// <summary>Abstract class for <c>MapGridHex</c> as used in the TerrainGridHex example.</summary>
  internal abstract class TerrainGridHex : MapGridHex {

    /// <summary>Initializes a new instance of a <see cref="TerrainGridHex"/>.</summary>
    /// <param name="board"></param>
    /// <param name="coords">Board location of this hex.</param>
    protected TerrainGridHex(HexBoard<MapGridHex,GraphicsPath> board, HexCoords coords) 
      : base(board,coords) {
      Elevation = 0;
      Board = board;
    }

    public new HexBoard<MapGridHex,GraphicsPath> Board { get; private set; }

    ///  <inheritdoc/>
    public    override GraphicsPath HexgridPath   { get {return Board.HexgridPath;} }
    /// <summary>TODO</summary>
    protected virtual  Brush        HexBrush      { get { return Brushes.Transparent; } }
    ///  <inheritdoc/>
    public    override int          HeightTerrain { get { return ElevationASL; } }

    ///  <inheritdoc/>
    public    override void         Paint(Graphics g) { 
      if (g==null) throw new ArgumentNullException("g");
      g.FillPath(HexBrush, HexgridPath);
    }
    ///  <inheritdoc/>
    public    override int          StepCost(Hexside direction) { return  4; }
  }
  /// <summary>A <see cref="TerrainGridHex"/> representing clear terrain.</summary>
  internal sealed class ClearTerrainGridHex    : TerrainGridHex {
    /// <summary>Creates a new instance of a clear <see cref="TerrainGridHex"/>.</summary>
    public ClearTerrainGridHex(HexBoard<MapGridHex,GraphicsPath> board, HexCoords coords) 
      : base(board, coords) { }

    /// <inheritdoc/>
    public    override void Paint(Graphics g) { ; }
  }

  /// <summary>A <see cref="TerrainGridHex"/> representing a river ford.</summary>
  internal sealed class FordTerrainGridHex     : TerrainGridHex {
    /// <summary>Creates a new instance of a ford <see cref="TerrainGridHex"/>.</summary>
    public FordTerrainGridHex(HexBoard<MapGridHex,GraphicsPath> board, HexCoords coords) 
      : base(board, coords) { }

    /// <inheritdoc/>
    protected override Brush HexBrush { get { return Brushes.Brown; } }
    /// <inheritdoc/>
    public    override int   StepCost(Hexside direction) { return  5; }
  }

  /// <summary>A <see cref="TerrainGridHex"/> representing a river (impassable terrain).</summary>
  internal sealed class RiverTerrainGridHex    : TerrainGridHex {
    /// <summary>Creates a new instance of a river <see cref="TerrainGridHex"/>.</summary>
    public RiverTerrainGridHex(HexBoard<MapGridHex,GraphicsPath> board, HexCoords coords) 
      : base(board, coords) { }

    /// <inheritdoc/>
    protected override Brush HexBrush { get { return Brushes.DarkBlue; } }

    /// <summary>This hex type is always impassable.</summary>
    public    override int   StepCost(Hexside direction) { return -1; }
  }

  /// <summary>A <see cref="TerrainGridHex"/> representing a pike (ie a major road).</summary>
  internal sealed class PikeTerrainGridHex     : TerrainGridHex {
    public PikeTerrainGridHex(HexBoard<MapGridHex,GraphicsPath> board, HexCoords coords) 
      : base(board, coords) { }

    /// <inheritdoc/>
    public    override int   StepCost(Hexside direction) { return  2; }
    /// <inheritdoc/>
    public    override void  Paint(Graphics g) { 
      if (g==null) throw new ArgumentNullException("g");
      using(var brush = new SolidBrush(Color.FromArgb(78,Color.DarkGray)))
        g.FillPath(brush, HexgridPath); 
    }
  }

  /// <summary>A <see cref="TerrainGridHex"/> representing a (secondary) road..</summary>
  internal sealed class RoadTerrainGridHex     : TerrainGridHex {
    public RoadTerrainGridHex(HexBoard<MapGridHex,GraphicsPath> board, HexCoords coords) 
      : base(board, coords) { }

    /// <inheritdoc/>
    public    override int   StepCost(Hexside direction) { return  3; }
    /// <inheritdoc/>
    public    override void  Paint(Graphics g) { 
      if (g==null) throw new ArgumentNullException("g");
      using(var brush = new SolidBrush(Color.FromArgb(78,Color.SaddleBrown)))
        g.FillPath(brush, HexgridPath); 
    }
  }

  /// <summary>A <see cref="TerrainGridHex"/> representing elevated terrain.</summary>
  internal sealed class HillTerrainGridHex     : TerrainGridHex {
    public HillTerrainGridHex(HexBoard<MapGridHex,GraphicsPath> board, HexCoords coords) 
      : base(board, coords) { 
      Elevation = 1;
    }

    /// <inheritdoc/>
    protected override Brush HexBrush  { get { return Brushes.Khaki; } }
    /// <inheritdoc/>
    public    override int   StepCost(Hexside direction) { return  5; }
  }

  /// <summary>A <see cref="TerrainGridHex"/> representing double elevated terrain.</summary>
  internal sealed class MountainTerrainGridHex : TerrainGridHex {
    public MountainTerrainGridHex(HexBoard<MapGridHex,GraphicsPath> board, HexCoords coords) 
      : base(board, coords) {
      Elevation = 2;
    }

    /// <inheritdoc/>
    protected override Brush HexBrush  { get { return Brushes.DarkKhaki; } }
    /// <inheritdoc/>
    public    override int   StepCost(Hexside direction) { return  6; }
  }

  /// <summary>A <see cref="TerrainGridHex"/> representing forested terrain.</summary>
  internal sealed class WoodsTerrainGridHex    : TerrainGridHex {
    public WoodsTerrainGridHex(HexBoard<MapGridHex,GraphicsPath> board, HexCoords coords) 
      : base(board, coords) { }

    /// <inheritdoc/>
    public    override int   HeightTerrain { get { return ElevationASL + 7; } }
    /// <inheritdoc/>
    protected override Brush HexBrush      { get { return Brushes.Green; } }
    /// <inheritdoc/>
    public    override int   StepCost(Hexside direction) { return  8; }
  }
}
