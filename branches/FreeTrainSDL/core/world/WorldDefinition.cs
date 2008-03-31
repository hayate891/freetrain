#region LICENSE
/*
 * Copyright (C) 2007 - 2008 FreeTrain Team (http://freetrain.sourceforge.net)
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
#endregion LICENSE

using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using FreeTrain.Contributions.Sound;
using FreeTrain.Contributions.Train;
using FreeTrain.Controllers;
using FreeTrain.Framework;
using FreeTrain.Framework.Plugin;
using FreeTrain.World;
using FreeTrain.World.Rail;
using FreeTrain.Views;
using FreeTrain.World.Development;
using FreeTrain.Framework.Graphics;

namespace FreeTrain.World
{
    /// <summary>
    /// 世界の操作権限定義
    /// god = 神：無制限
    /// player = プレイヤー鉄道会社
    /// com = コンピュータ（ライバル会社、行政）
    /// </summary>
    public enum ControlMode
    {
        /// <summary>
        /// 
        /// </summary>
        God,
        /// <summary>
        /// 
        /// </summary>
        Player,
        /// <summary>
        /// 
        /// </summary>
        Com
    };

    /// <summary>
    /// ゲームデータのルート
    /// </summary>
    [Serializable]
    public sealed class WorldDefinition : IDeserializationCallback
    {
        /// <summary>
        /// 現在ゲーム中の世界
        /// </summary>
        private static WorldDefinition world;

        /// <summary>
        /// 
        /// </summary>
        public static WorldDefinition World
        {
            get { return WorldDefinition.world; }
            set { WorldDefinition.world = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        private Surface satellite;

        /// <summary>
        /// 
        /// </summary>
        public Surface Satellite
        {
            get { return satellite; }
            set { satellite = value; }
        }
        /// <summary>
        /// 世界の大きさ (H,V,D)
        /// </summary>
        private readonly Distance size;

        /// <summary>
        /// 
        /// </summary>
        public Distance Size
        {
            get { return size; }
        } 

        /// <summary>
        /// 
        /// </summary>
        private DevelopmentAlgorithm developmentAlgorithm;

        /// <summary>
        /// 
        /// </summary>
        public DevelopmentAlgorithm DevelopmentAlgorithm
        {
            get { return developmentAlgorithm; }
            set { developmentAlgorithm = value; }
        }

        /// <summary>
        /// Fired after a new world is loaded/created.
        /// </summary>
        public static event EventHandler onNewWorld;

        /// <summary>
        /// 空の世界を作成
        /// </summary>
        /// <param name="sz">世界の大きさ (H,V,D)</param>
        /// <param name="tempolary"></param>
        /// <param name="waterLevel"></param>
        private WorldDefinition(Distance sz, int waterLevel, bool tempolary)
        {
            this.name = "Terra Incognita";
            //! this.name = "ななしさん";
            this.size = sz;
            this.waterLevel = (byte)waterLevel;
            voxels = new SparseVoxelArray(sz.x, sz.y, sz.z);
            groundLevels = new byte[sz.x, sz.y];

            for (int y = 0; y < sz.y; y++)
                for (int x = 0; x < sz.x; x++)
                    groundLevels[x, y] = (byte)waterLevel;

            if (tempolary)
            {
                onVoxelChanged += new VoxelChangeListener(EmptyHandler);
            }
            else
            {
                // add system-defined controllers to the list
                // trainControllers.add( DelegationTrainControllerImpl.theInstance );
                trainControllers.add(SimpleTrainControllerImpl.theInstance);

                developmentAlgorithm = new DevelopmentAlgorithm();
                clock.registerRepeated(new ClockHandler(developmentAlgorithm.handleClock),
                    TimeLength.fromHours(1));

                // for test
                new Train(rootTrainGroup, "3-Car High-Speed Train", 3,
                    //! new Train( rootTrainGroup, "３両高速編成",3,
                    (TrainContribution)Core.plugins.getContribution("{3983B298-ADB1-4905-94E5-03B7AAE5A221}"),
                    SimpleTrainControllerImpl.theInstance);
                new Train(rootTrainGroup, "5-Car Medium-Speed Train", 5,
                    //! new Train( rootTrainGroup, "５両中速編成",5,
                    (TrainContribution)Core.plugins.getContribution("{2C6F6C72-FA4B-4941-84C1-57553C8A5C2A}"),
                    SimpleTrainControllerImpl.theInstance);
                new Train(rootTrainGroup, "7-Car Low-Speed Train", 7,
                    //! new Train( rootTrainGroup, "７両低速編成",7,
                    (TrainContribution)Core.plugins.getContribution("{F7134C8E-6B63-4780-AF16-90D33131CD07}"),
                    SimpleTrainControllerImpl.theInstance);


                // when a voxel is changed, it should be notified to OutlookListeners automatically
                onVoxelChanged += new VoxelChangeListener(OnVoxelUpdated);

                landValue = new Development.LandValue(this);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sz"></param>
        /// <param name="waterLevel"></param>
        public WorldDefinition(Distance sz, int waterLevel)
            : this(sz, waterLevel, false)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="minsizePixel"></param>
        /// <param name="struct_size"></param>
        /// <returns></returns>
        public static WorldDefinition CreatePreviewWorld(Size minsizePixel, Distance struct_size)
        {
            int v = struct_size.x + struct_size.y;
            int h = struct_size.z;
            int mx = (minsizePixel.Width + 33) >> 5;
            int my = (minsizePixel.Height + 9) >> 3;
            Distance sz = new Distance(Math.Max(mx, v / 2 + 2), Math.Max(my, v / 2 + h * 2 + 4), h + 2);
            return new WorldDefinition(sz, 0, true);
        }

        /// <summary>ゲームの名前</summary>
        private string name;

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>Clock object that controls the time of this world.</summary>
        public readonly Clock clock = new Clock();

        /// <summary> Voxels in this World indexed by its position. </summary>
        private readonly SparseVoxelArray voxels;

        /// <summary>
        /// Ground levels. If the value is n, then voxel of the height n-1 would be
        /// underground.
        /// </summary>
        private readonly byte[,] groundLevels;

        /// <summary>
        /// Water level. Voxels whose z axis is smaller than this 
        /// is considered as under-water.
        /// </summary>
        public readonly byte waterLevel;
        /// <summary>
        /// 
        /// </summary>
        public readonly Accounting.AccountManager account = new Accounting.AccountManager();

        /// <summary>
        /// Responsible for computing/maintaining land values for this world.
        /// </summary>
        public readonly Development.LandValue landValue;

        /// <summary>
        /// Other objects associated to this world.
        /// </summary>
        public readonly GlobalViewOptions viewOptions = new GlobalViewOptions();

        /// <summary>
        /// Other objects associated to this world.
        /// </summary>
        public readonly IDictionary otherObjects = new Hashtable();


        #region coordination conversion methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="h"></param>
        /// <param name="v"></param>
        public void toHV(int x, int y, out int h, out int v)
        {
            int xx = x - (size.y - 1) / 2;
            h = (xx + y) >> 1;
            v = y - xx;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="h"></param>
        /// <param name="v"></param>
        public void toHV(Location loc, out int h, out int v)
        {
            toHV(loc.x, loc.y, out h, out v);
        }


        /// <summary>
        /// Converts the (X,Y,Z) coordinates to the (A,B) coordinates.
        /// 
        /// This method returns the top-left corner of the specified location.
        /// </summary>
        public Point fromXYZToAB(int x, int y, int z)
        {
            // collapse z
            x += z; y -= z;
            // change the origin of x
            x -= (WorldDefinition.world.size.y - 1) / 2;

            return new Point(16 * (x + y), 8 * (-x + y));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public Point fromXYZToAB(Location loc)
        {
            return fromXYZToAB(loc.x, loc.y, loc.z);
        }

        /// <summary>
        /// Compute the box in (A,B) coordinates that contains
        /// the given voxel.
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public Rectangle getBoundingBox(Location loc)
        {
            Point topLeft = fromXYZToAB(loc);
            topLeft.Y -= 16;
            return new Rectangle(topLeft, new Size(32, 32));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public Rectangle getBoundingBox(int x, int y, int z)
        {
            return getBoundingBox(new Location(x, y, z));
        }

        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cube"></param>
        /// <returns></returns>
        public bool isOutsideWorld(Cube cube)
        {
            if (isOutsideWorld(new Location(cube.x1, cube.y1, cube.z1))) return true;
            if (isOutsideWorld(new Location(cube.x2, cube.y1, cube.z1))) return true;
            if (isOutsideWorld(new Location(cube.x1, cube.y2, cube.z1))) return true;
            if (isOutsideWorld(new Location(cube.x2, cube.y2, cube.z1))) return true;
            if (isOutsideWorld(new Location(cube.x1, cube.y1, cube.z2))) return true;
            if (isOutsideWorld(new Location(cube.x2, cube.y1, cube.z2))) return true;
            if (isOutsideWorld(new Location(cube.x1, cube.y2, cube.z2))) return true;
            if (isOutsideWorld(new Location(cube.x2, cube.y2, cube.z2))) return true;
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cube"></param>
        /// <returns></returns>
        public bool isInsideWorld(Cube cube)
        {
            return !isOutsideWorld(cube);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public System.Drawing.Size realSize()
        {
            return new Size(this.size.x * 32, this.size.y * 16);
        }

        /// <summary>
        /// Returns true only when the given location is inside the world.
        /// </summary>
        public bool isInsideWorld(Location loc)
        {
            int h, v;
            toHV(loc.x, loc.y, out h, out v);

            return 0 <= h && h < size.x && 0 <= v && v < size.y;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public bool isOutsideWorld(Location loc)
        {
            return !isInsideWorld(loc);
        }

        /// <summary>
        /// Returns true if the given location is outside the visible region.
        /// </summary>
        public bool isBorderOfWorld(Location loc)
        {
            int h, v;
            toHV(loc.x, loc.y, out h, out v);

            return h < 0 || size.x <= h || v < loc.z * 2 || size.y - (size.z - loc.z) * 2 <= v;
        }


        /// <summary>
        /// 指定された位置にあるVoxelを取得
        /// </summary>
        public Voxel this[int x, int y, int z]
        {
            get
            {
                int xx = x - (size.y - 1) / 2; // xの基準軸を移動
                int h = (xx + y) >> 1;
                int v = y - xx;

                try
                {
                    return voxels[h, v, z];
                }
                catch (IndexOutOfRangeException)
                {
                    // this is outside the world, return an empty voxel
                    // so that the caller would think that this voxel is already occupied.
                    return new OutOfWorldVoxel(x, y, z);
                }
            }
            set
            {
                int xx = x - (size.y - 1) / 2; // move the origin of x-axis
                int h = (xx + y) >> 1;	// note that x>>1 is NOT equal to x/2.
                int v = y - xx;

                //       x=3 x=2 x=1 x=0 x=-1 x=-2 x=-3
                // x>>1   1   1   0   0   -1   -1   -2
                // x/2    1   1   0   0    0   -1   -1
                //
                // thus we need to use >>1 to correctly handle negative case

                voxels[h, v, z] = value;
                onVoxelChanged(new Location(x, y, z));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        public static void setWorld(WorldDefinition w)
        {
            world = w;
            if (onNewWorld != null)
                onNewWorld(null, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public Entity getEntityAt(Location loc)
        {
            Voxel v = this[loc];
            if (v == null) return null;
            else return v.entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public int getGroundLevel(Location loc)
        {
            return getGroundLevel(loc.x, loc.y);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int getGroundLevel(int x, int y)
        {
            int xx = x - (size.y - 1) / 2; // move the origin of x-axis
            int h = (xx + y) >> 1;	// note that x>>1 is NOT equal to x/2.
            int v = y - xx;

            try
            {
                return groundLevels[h, v];
            }
            catch (IndexOutOfRangeException)
            {
                return 0;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="h"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public int getGroundLevelFromHV(int h, int v)
        {
            return groundLevels[h, v];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        public void raiseGround(Location loc)
        {
            raiseGround(loc.x, loc.y);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void raiseGround(int x, int y)
        {
            int xx = x - (size.y - 1) / 2; // move the origin of x-axis
            int h = (xx + y) >> 1;	// note that x>>1 is NOT equal to x/2.
            int v = y - xx;

            groundLevels[h, v]++;

            // update voxel listeners
            OnVoxelUpdated(new Location(x, y, groundLevels[h, v] - 1));
            OnVoxelUpdated(new Location(x, y, groundLevels[h, v]));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        public void lowerGround(Location loc)
        {
            lowerGround(loc.x, loc.y);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void lowerGround(int x, int y)
        {
            int xx = x - (size.y - 1) / 2; // move the origin of x-axis
            int h = (xx + y) >> 1;	// note that x>>1 is NOT equal to x/2.
            int v = y - xx;

            groundLevels[h, v]--;

            // update voxel listeners
            OnVoxelUpdated(new Location(x, y, groundLevels[h, v] + 1));
            OnVoxelUpdated(new Location(x, y, groundLevels[h, v]));
        }

        /// <summary>
        /// Returns true if this voxel can be silently re-claimed.
        /// Voxels unused or land voxels are reusable.
        /// </summary>
        public bool isReusable(Location loc)
        {
            Voxel v = this[loc];
            return v == null || v.entity.isSilentlyReclaimable;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public bool isReusable(int x, int y, int z)
        {
            return isReusable(new Location(x, y, z));
        }


        /// <summary>
        /// Used as the invisible wall that fills world outside the world
        /// </summary>
        [Serializable]
        public class OutOfWorldVoxel : Voxel, Entity
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="z"></param>
            public OutOfWorldVoxel(int x, int y, int z) : this(new Location(x, y, z)) { }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="loc"></param>
            public OutOfWorldVoxel(Location loc)
            {
                this.loc = loc;
            }

            private readonly Location loc;
            /// <summary>
            /// 
            /// </summary>
            public override Location location { get { return loc; } }
            /// <summary>
            /// 
            /// </summary>
            public override Entity entity { get { return this; } }

            #region Entity implementation
            /// <summary>
            /// 
            /// </summary>
            public bool isSilentlyReclaimable { get { return false; } }
            /// <summary>
            /// 
            /// </summary>
            public bool isOwned { get { return false; } }
            /// <summary>
            /// 
            /// </summary>
            public void remove()
            {
                // TODO: not sure what to do.
                // can't be removed.
                if (onEntityRemoved != null) onEntityRemoved(this, null);
            }
            /// <summary>
            /// 
            /// </summary>
            public event EventHandler onEntityRemoved;
            /// <summary>
            /// 
            /// </summary>
            public int entityValue { get { return 0; } }

            #endregion
            /// <summary>
            /// 
            /// </summary>
            /// <param name="dc"></param>
            /// <param name="pt"></param>
            /// <param name="heightCutDiff"></param>
            public override void draw(DrawContext dc, Point pt, int heightCutDiff) { }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="display"></param>
            /// <param name="pt"></param>
            protected override void drawFrontFence(DrawContext display, Point pt) { }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="display"></param>
            /// <param name="pt"></param>
            protected override void drawBehindFence(DrawContext display, Point pt) { }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="d"></param>
            /// <param name="f"></param>
            public override void setFence(Direction d, Fence f) { }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="d"></param>
            /// <returns></returns>
            public override Fence getFence(Direction d) { return null; }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override Color getColorOfTile() { return Color.Red; }

        }

        /// <summary>
        /// Translates (h,v,d) co-ordinates into (x,y,z).
        /// </summary>
        /// <param name="h"></param>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public Location toXYZ(int h, int v, int d)
        {
            int xx = h - v / 2;

            return new Location(xx + (size.y - 1) / 2, h + (v + 1) / 2, d);
        }

        /// <summary>
        /// Removes a voxel in the specified location.
        /// </summary>
        public void remove(Location loc) { remove(loc.x, loc.y, loc.z); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        public void remove(Voxel v) { remove(v.location); }

        // deprecated?
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void remove(int x, int y, int z)
        {
            int xx = x - (size.y - 1) / 2; // xの基準軸を移動
            int h = (xx + y) / 2;
            int v = y - xx;

            Voxel vx = voxels[h, v, z];
            Debug.Assert(vx != null);
            voxels.remove(h, v, z);
            vx.onRemoved();
            onVoxelChanged(new Location(x, y, z));
        }

        /// <summary>
        /// 指定された位置にあるVoxelを取得
        /// </summary>
        public Voxel this[Location loc]
        {
            get
            {
                return this[loc.x, loc.y, loc.z];
            }
            set
            {
                this[loc.x, loc.y, loc.z] = value;
            }
        }

        /// <summary>
        /// HVD単位系でのVoxelを取得
        /// </summary>
        public Voxel voxelHVD(int h, int v, int d)
        {
            return voxels[h, v, d];
        }



        /// <summary> Fired when a voxel needs to be redrawn.</summary>
        [NonSerialized]
        public IList voxelOutlookListeners = new ArrayList();

        /// <summary>
        /// 地価計算のためにRoadクラスのコンストラクタから呼べるように用意した。(477)
        /// </summary>
        /// <param name="loc"></param>
        internal void fireOnVoxelChanged(Location loc)
        {
            onVoxelChanged(loc);
        }

        /// <summary>
        /// Notifies the voxel outlook listener, if there is any.
        /// </summary>
        public void OnVoxelUpdated(Location location)
        {
            foreach (IVoxelOutlookListener v in voxelOutlookListeners)
                v.OnUpdateVoxel(location);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        public void OnVoxelUpdated(Voxel voxel)
        {
            OnVoxelUpdated(voxel.location);
            //Point p = new Point((int)(Math.Round(v.location.x * Math.Cos(45) + v.location.y * Math.Sin(45)) - world.size.x / 2),
            //                    (int)(Math.Round(-v.location.x * Math.Sin(45) + v.location.y * Math.Cos(45)) + world.size.y / 2));
            Point p = new Point(voxel.location.x, voxel.location.y);
            if (satellite != null) satellite.SetPixel(p, voxel.getColorOfTile());
        }

        /// <summary>
        /// Notifies all the voxel outlook listeners at once.
        /// </summary>
        public void OnAllVoxelUpdated()
        {
            foreach (IVoxelOutlookListener v in voxelOutlookListeners)
                v.OnUpdateAllVoxels();
        }

        /// <summary>
        /// Notifies all the cubic voxels within the given cube
        /// are updated.
        /// </summary>
        public void OnVoxelUpdated(Cube cube)
        {
            foreach (IVoxelOutlookListener v in voxelOutlookListeners)
                v.OnUpdateVoxel(cube);
        }



        /// <summary> Fired when a voxel is added/removed.</summary>
        public event VoxelChangeListener onVoxelChanged;








        /// <summary> Root train group that holds all the trains in its descendants. </summary>
        public readonly TrainGroup rootTrainGroup = new TrainGroup(null, "My Trains");
        //! public readonly TrainGroup rootTrainGroup = new TrainGroup(null,"保有する列車");

        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        public class TrainControllerCollection : CollectionBase
        {
            internal TrainControllerCollection() { }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="tc"></param>
            public void add(TrainController tc)
            {
                base.List.Add(tc);
            }
            /// <summary>
            /// /
            /// </summary>
            /// <param name="tc"></param>
            public void remove(TrainController tc)
            {
                base.List.Remove(tc);
            }
        }

        /// <summary> All TrainControllers that exist in the World.</summary>
        public readonly TrainControllerCollection trainControllers = new TrainControllerCollection();



        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        public class StationCollection : CollectionBase
        {
            // TODO: this should be set, not list.
            internal StationCollection() { }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="st"></param>
            public void add(Station st)
            {
                base.List.Add(st);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="st"></param>
            public void remove(Station st)
            {
                base.List.Remove(st);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="idx"></param>
            /// <returns></returns>
            public Station get(int idx)
            {
                return (Station)base.List[idx];
            }
        }

        /// <summary> All stations that exist in this orld. </summary>
        public readonly StationCollection stations = new StationCollection();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        public void OnDeserialization(object sender)
        {
            // this field won't be deserialized, so we need to set it manually.
            voxelOutlookListeners = new ArrayList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static WorldDefinition load(IFormatter f, Stream stream)
        {
            using (new Util.LongTask())
            {
                //				SoapFormatter f = new SoapFormatter();
                //				BinaryFormatter f = new BinaryFormatter();
                f.Binder = new PluginSerializationBinder();
                Core.bgmManager.currentBGM = ((BGMContribution[])f.Deserialize(stream))[0];
                return (WorldDefinition)f.Deserialize(stream);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="stream"></param>
        public void save(IFormatter f, Stream stream)
        {
            using (new Util.LongTask())
            {
                // currentBGM can be null, so serialize as an array
                f.Serialize(stream, new BGMContribution[] { Core.bgmManager.currentBGM });
                f.Serialize(stream, this);
            }
        }

        private void EmptyHandler(Location loc)
        {
            // do nothins;
        }
    }


    /// <summary>
    /// Receives notification of a voxel addition/removal.
    /// </summary>
    public delegate void VoxelChangeListener(Location loc);
}
