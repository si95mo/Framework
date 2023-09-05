using System.Windows.Forms;

namespace UserInterface.Controls.Tests
{
    public partial class TreeViewTestForm : Form
    {
        public TreeViewTestForm()
        {
            InitializeComponent();

            TreeNode root = new TreeNode("Root");
            treeViewControl.AddNode(root, "First leaf");

            TreeNode child = new TreeNode("First child");
            child.Nodes.Add("Second leaf");
            root.Nodes.Add(child);

            treeViewControl.AddNode(root, "Third leaf");
            treeViewControl.Nodes.Add(root);
        }
    }
}
