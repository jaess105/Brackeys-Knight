using System.Collections.Generic;
using System.Diagnostics;
using Godot;

namespace Brackeys.Knight.Util;

public static class Util
{
    public static void GameOver(this Node2D self)
    {
        self.GetTree().ReloadCurrentScene();
    }

    public static List<T> GetNodesOfType<T>(this Node self)
        where T : Node
        => self.GetTree().Root.GetNodesOfTypeRecur<T>();

    private static List<T> GetNodesOfTypeRecur<T>(this Node self, List<T> coll = null)
        where T : Node
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