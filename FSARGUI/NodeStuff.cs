using System;
using System.Linq;
using System.Windows.Forms;

namespace FSARGUI
{
    public class NodeStuff
    {
        public static void GenTreeNodes(OpenedArchive openedArchive)
        {
            foreach (var entry in openedArchive.FileEntries)
            {
                string path = entry.Path;

                //split by ~~dot~~ backslashes to get nodes names
                var nodeNames = path.Split('\\');
                //TreeNode to remember node level
                TreeNode lastNode = null;

                //iterate through all node names
                foreach (string nodeName in nodeNames)
                {
                    //values for name and tag (tag is empty string by default)
                    string name = nodeName;
                    string tagValue = string.Empty;

                    //var used for finding existing node
                    TreeNode existingNode = null;
                    //new node to add to tree
                    TreeNode newNode = new TreeNode(name);
                    newNode.Tag = tagValue;
                    //collection of subnodes to search for node name (to check if node exists)
                    //in first pass, that collection is collection of treeView's nodes (first level)
                    TreeNodeCollection nodesCollection = openedArchive.tree.Nodes;

                    //with first pass, this will be null, but in every other, this will hold last added node.
                    if (lastNode != null)
                    {
                        nodesCollection = lastNode.Nodes;
                    }

                    //look into collection if node is already there (method checks only first level of node collection)
                    existingNode = FindNode(nodesCollection, name);
                    //node is found? In that case, skip it but mark it as last "added"
                    if (existingNode != null)
                    {
                        lastNode = existingNode;
                        continue;
                    }
                    else //not found so add it to collection and mark node as last added.
                    {
                        nodesCollection.Add(newNode);
                        lastNode = newNode;
                    }
                }
            }
        }
        private static TreeNode FindNode(TreeNodeCollection nodeCollectionToSearch, string nodeText)
        {
            var nodesToSearch = nodeCollectionToSearch.Cast<TreeNode>();
            var foundNode = nodesToSearch.FirstOrDefault(n => n.Text == nodeText);
            return foundNode;
        }
    }
}