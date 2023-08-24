using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    /// <summary>
    /// Define a custom <see cref="TreeView"/> with Windows theme
    /// </summary>
    public partial class TreeViewControl : TreeView
    {
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private extern static int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList); 
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        private const int WM_SETREDRAW = 11;

        /// <summary>
        /// Create a new instance of <see cref="TreeViewControl"/>
        /// </summary>
        public TreeViewControl() : base()
        {
            BackColor = SystemColors.Control;
            AfterSelect += TreeViewControl_AfterSelect;
        }

        private void TreeViewControl_AfterSelect(object sender, TreeViewEventArgs e)
        {
            e.Node.NodeFont = new Font(Font, FontStyle.Bold);
            e.Node.Text = e.Node.Text;

            ResetFonts(Nodes, e.Node);
            Focus();
        }

        /// <summary>
        /// Add a new <see cref="TreeNode"/> to <paramref name="root"/>
        /// </summary>
        /// <param name="root">The root <see cref="TreeNode"/></param>
        /// <param name="leafText">The leaf <see cref="TreeNode"/> text</param>
        public void AddNode(TreeNode root, string leafText)
        {
            TreeNode leaf = new TreeNode(leafText);
            root.Nodes.Add(leaf);
        }

        /// <summary>
        /// Suspend the redraw of the control, must be followed by <see cref="ResumeDrawing(Control)"/>
        /// </summary>
        /// <param name="parent"></param>
        public static void SuspendDrawing(Control parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
        }

        /// <summary>
        /// Resume the redraw of the control. Must be preceded by <see cref="SuspendDrawing(Control)"/>
        /// </summary>
        /// <param name="parent"></param>
        public static void ResumeDrawing(Control parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
            parent.Refresh();
        }

        protected override void CreateHandle()
        {
            base.CreateHandle();
            SetWindowTheme(Handle, "explorer", null);
        }

        /// <summary>
        /// Reset all the <see cref="TreeView"/> nodes
        /// </summary>
        /// <param name="nodes">The <see cref="TreeNodeCollection"/></param>
        /// <param name="nodeToExclude">The <see cref="TreeNode"/> to exclude</param>
        private void ResetFonts(TreeNodeCollection nodes, TreeNode nodeToExclude)
        {
            foreach (TreeNode node in nodes)
            {
                if (node != nodeToExclude)
                {
                    node.NodeFont = new Font("Lucida Sans Unicode", 12f, FontStyle.Regular);
                    ResetFonts(node.Nodes, nodeToExclude);
                }
                else
                    ResetFonts(node.Nodes, nodeToExclude);
            }
        }
    }
}
