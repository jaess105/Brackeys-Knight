using System.Diagnostics;
using Godot;

namespace Brackeys.Knight.Util;

public static class Util
{
    public static void GameOver(this Node2D self)
    {
        self.GetTree().ReloadCurrentScene();
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