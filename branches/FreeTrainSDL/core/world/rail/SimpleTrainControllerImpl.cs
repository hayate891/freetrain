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
using System.Runtime.Serialization;
using System.Windows.Forms;
using freetrain.framework.plugin;
using freetrain.contributions.rail;

namespace freetrain.world.rail
{
    /// <summary>
    /// Default TrainController implementation that doesn't do anything
    /// interesting.
    /// </summary>
    [Serializable]
    public class SimpleTrainControllerImpl : TrainController, ISerializable
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly TrainController theInstance = new SimpleTrainControllerImpl();

        private SimpleTrainControllerImpl()
        {
            name = "Default Diagram";
            //! name = "ディフォルトダイヤグラム";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="train"></param>
        /// <param name="rr"></param>
        /// <returns></returns>
        public override JunctionRoute onJunction(Train train, JunctionRailRoad rr)
        {
            return JunctionRoute.Straight;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="train"></param>
        /// <param name="harbor"></param>
        /// <param name="callCount"></param>
        /// <returns></returns>
        public override TimeLength getStopTimeSpan(Train train, TrainHarbor harbor, int callCount)
        {
            // stop 1 hour and go
            if (!(harbor is Station)) return TimeLength.ZERO;	// ignore everything but a station

            if (callCount == 0) return TimeLength.fromMinutes(30);
            else return TimeLength.ZERO;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        public override void config(IWin32Window owner)
        {
            MessageBox.Show(owner, "This diagram has no setting item", Application.ProductName,
                //! MessageBox.Show( owner, "このダイヤには設定項目はありません", Application.ProductName,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        // we don't have any contribution and this controller is only used implicitly by default.
        /// <summary>
        /// 
        /// </summary>
        public override TrainControllerContribution contribution { get { return null; } }

        //		/// <summary> The sole instance of the contribution. </summary>
        //		private static TrainControllerContribution theContribution = new SimpleTrainControllerContribution();
        //
        //		private class SimpleTrainControllerContribution : TrainControllerContribution {
        //		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.SetType(typeof(ReferenceImpl));
        }
        [Serializable]
        internal sealed class ReferenceImpl : IObjectReference
        {
            public object GetRealObject(StreamingContext context)
            {
                return theInstance;
            }
        }
    }
}
