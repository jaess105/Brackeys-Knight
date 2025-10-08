using System.Collections.Generic;
using System.Diagnostics;
using Godot;

namespace Brackeys.Knight.Util;

public static class Util
{
    public static void GameOver(this Node self)
    {
        self.GetTree().ReloadCurrentScene();
    }

    /// <summary>
    /// Gets all nods of type T in the current scene tree starting with the root node.
    /// </summary>
    /// <typeparam name="T">The type of the nodes that should be found.</typeparam>
    /// <param name="self">The node from which the scene tree and thus the root is accessed.</param>
    /// <returns>A list of all nodes of type T in the current scene tree.</returns>
    public static List<T> GetNodesOfType<T>(this Node self)
        => self.GetTree().Root.GetNodesOfTypeRecur<T>();

    /// <summary>
    /// Recursively go through all nodes of the current scene tree adding them to the collection
    /// if they are of the provided type T.
    /// </summary>
    /// <typeparam name="T">The type of which all nodes should be.</typeparam>
    /// <param name="self">The node for which all it's children should be searched.</param>
    /// <param name="coll">The collection into which the children are added.</param>
    /// <returns>coll, mutating it to contain all nodes of type T.</returns>
    private static List<T> GetNodesOfTypeRecur<T>(this Node self, List<T> coll = null)
    {
        coll ??= [];
        if (self is T node) { coll.Add(node); }

        foreach (var child in self.GetChildren())
        {
            child.GetNodesOfTypeRecur(coll);
        }

        return coll;
    }
}

public static class Printer
{
    public static void Print(string message)
    {
        Debug.Print(message);
    }
}

public static class GravityExtension
{
    public static Vector2 ApplyGravity(this CharacterBody2D self, float delta, Vector2? velocity = null)
    {
        velocity ??= self.Velocity;

        // Add the gravity.
        if (!self.IsOnFloor())
        {
            velocity += self.GetGravity() * (float)delta;
        }

        return velocity.Value;
    }
}