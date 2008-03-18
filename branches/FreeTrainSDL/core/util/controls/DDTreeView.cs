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
using System.Drawing;
using System.Windows.Forms;

namespace FreeTrain.Util.Controls
{
    /// <summary>
    /// TreeView control with drag-n-drop functionality
    /// </summary>
    public class DDTreeView : TreeView
    {
        /// <summary>
        /// 
        /// </summary>
        public DDTreeView()
        {
            AllowDrop = true;
        }

        private TreeNode getNodeFrom(DragEventArgs e)
        {
            return GetNodeAt(PointToClient(new Point(e.X, e.Y)));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            base.OnItemDrag(e);
            // e.Item is a TreeNode
            DoDragDrop(new DataObject("treeNode", e.Item), DragDropEffects.Move);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDragDrop(DragEventArgs e)
        {
            base.OnDragDrop(e);

            TreeNode n = e.Data.GetData("treeNode") as TreeNode;
            if (n == null) return;

            // move this node
            OnItemMoved(n, getNodeFrom(e));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);

            DDTreeNode target = getNodeFrom(e) as DDTreeNode;

            if (target != null && target.canAcceptDrop)
            {
                // make sure the data is correct and it belongs to us.
                TreeNode data = e.Data.GetData("treeNode") as TreeNode;
                if (data != null && data.TreeView == this)
                {
                    // make sure that data is not a parent of target
                    TreeNode n = target;
                    while (n != null)
                    {
                        if (n == data) break;
                        n = n.Parent;
                    }

                    if (n == null)
                    {
                        e.Effect = DragDropEffects.Move;
                        return;
                    }
                }
            }
            e.Effect = DragDropEffects.None;
        }


        /// <summary>
        /// This method is called when a drag-n-drop is completed.
        /// </summary>
        /// <param name="node">A node being dragged</param>
        /// <param name="newParent">new parent node</param>
        protected virtual void OnItemMoved(TreeNode node, TreeNode newParent)
        {
            ItemMoved(node, newParent);
        }
        /// <summary>
        /// 
        /// </summary>
        public ItemMovedHandler ItemMoved;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="node"></param>
    /// <param name="newParent"></param>
    public delegate void ItemMovedHandler(TreeNode node, TreeNode newParent);

    /// <summary>
    /// 
    /// </summary>
    public class DDTreeNode : TreeNode
    {
        /// <summary>
        /// 
        /// </summary>
        public DDTreeNode() : base() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public DDTreeNode(string text) : base(text) { }
        /// <summary>
        /// 
        /// </summary>
        public bool canAcceptDrop;
    }
}
