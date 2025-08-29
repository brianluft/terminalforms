using TurboVision.Outlines;

namespace Tests.TurboVision.Outlines;

[TestClass]
public sealed unsafe class TNodeTests
{
    [TestMethod]
    public void Test_PlacementNew()
    {
        var bytes = stackalloc byte[TNode.PlacementSize];
        using var node = new TNode(bytes, "Test Node");
        Assert.IsNotNull(node);
        Assert.IsFalse(node.IsDisposed);
        Assert.AreEqual("Test Node", node.Text);
        Assert.IsTrue(node.Expanded);
        Assert.IsNull(node.Next);
        Assert.IsNull(node.ChildList);
    }

    [TestMethod]
    public void Test_New()
    {
        // Test basic constructor with text
        using var node = new TNode("Test Node");
        Assert.AreEqual("Test Node", node.Text);
        Assert.IsTrue(node.Expanded);
        Assert.IsNull(node.Next);
        Assert.IsNull(node.ChildList);

        // Test constructor with all parameters
        using var child = new TNode("Child Node");
        using var next = new TNode("Next Node");
        using var parentNode = new TNode("Parent Node", child, next, false);

        Assert.AreEqual("Parent Node", parentNode.Text);
        Assert.IsFalse(parentNode.Expanded);
        Assert.IsNotNull(parentNode.Next);
        Assert.AreEqual("Next Node", parentNode.Next!.Text);
        Assert.IsNotNull(parentNode.ChildList);
        Assert.AreEqual("Child Node", parentNode.ChildList!.Text);
    }

    [TestMethod]
    public void Test_Equals()
    {
        using var node1 = new TNode("Test");
        using var node2 = new TNode("Test");
        using var node3 = new TNode("Different");

        // Test equality with same content
        Assert.IsTrue(node1.Equals(node2));
        Assert.IsTrue(node2.Equals(node1));

        // Test inequality with different content
        Assert.IsFalse(node1.Equals(node3));
        Assert.IsFalse(node3.Equals(node1));

        // Test self-equality
        Assert.IsTrue(node1.Equals(node1));
    }

    [TestMethod]
    public void Test_GetHashCode()
    {
        using var node1 = new TNode("Test");
        using var node2 = new TNode("Test");
        using var node3 = new TNode("Different");

        // Hash codes should be equal for equal objects
        Assert.AreEqual(node1.GetHashCode(), node2.GetHashCode());

        // Hash codes should likely be different for different objects
        Assert.AreNotEqual(node1.GetHashCode(), node3.GetHashCode());
    }

    [TestMethod]
    public void Test_TextProperty()
    {
        using var node = new TNode("Initial Text");
        Assert.AreEqual("Initial Text", node.Text);

        // Test changing text
        node.Text = "New Text";
        Assert.AreEqual("New Text", node.Text);

        // Test empty text
        node.Text = "";
        Assert.AreEqual("", node.Text);
    }

    [TestMethod]
    public void Test_ExpandedProperty()
    {
        using var node = new TNode("Test Node");

        // Default should be expanded
        Assert.IsTrue(node.Expanded);

        // Test changing expanded state
        node.Expanded = false;
        Assert.IsFalse(node.Expanded);

        node.Expanded = true;
        Assert.IsTrue(node.Expanded);
    }

    [TestMethod]
    public void Test_NextProperty()
    {
        using var node1 = new TNode("Node 1");
        using var node2 = new TNode("Node 2");

        // Initially no next node
        Assert.IsNull(node1.Next);

        // Set next node
        node1.Next = node2;
        Assert.IsNotNull(node1.Next);
        Assert.AreEqual("Node 2", node1.Next!.Text);

        // Clear next node
        node1.Next = null;
        Assert.IsNull(node1.Next);
    }

    [TestMethod]
    public void Test_ChildListProperty()
    {
        using var parent = new TNode("Parent");
        using var child = new TNode("Child");

        // Initially no children
        Assert.IsNull(parent.ChildList);

        // Set child
        parent.ChildList = child;
        Assert.IsNotNull(parent.ChildList);
        Assert.AreEqual("Child", parent.ChildList!.Text);

        // Clear child
        parent.ChildList = null;
        Assert.IsNull(parent.ChildList);
    }

    [TestMethod]
    public void Test_ComplexHierarchy()
    {
        // Create a more complex node structure
        using var child1 = new TNode("Child 1");
        using var child2 = new TNode("Child 2");
        using var grandchild = new TNode("Grandchild");

        // Set up hierarchy: parent -> child1 -> child2, and child1 has grandchild
        child1.Next = child2;
        child1.ChildList = grandchild;

        using var parent = new TNode("Parent");
        parent.ChildList = child1;

        // Verify the structure
        Assert.AreEqual("Parent", parent.Text);
        Assert.IsNotNull(parent.ChildList);
        Assert.AreEqual("Child 1", parent.ChildList!.Text);
        Assert.IsNotNull(parent.ChildList.Next);
        Assert.AreEqual("Child 2", parent.ChildList.Next!.Text);
        Assert.IsNotNull(parent.ChildList.ChildList);
        Assert.AreEqual("Grandchild", parent.ChildList.ChildList!.Text);
    }
}
