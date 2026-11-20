using System.Linq;
using Godot;
using Godot.Collections;
using Array = System.Array;

public partial class Main : Node
{

    [Export] private PackedScene _startupScene;
    

    public override void _Ready()
    {
        // ... register stuff, etc.
        CallDeferred(nameof(GoToGame));
    }

    private void GoToGame()
    {
        if (_startupScene != null) {
            GetTree().ChangeSceneToPacked(_startupScene);
        }
        else {
            Logger.Error("Startup Scene is null");
        }
    }
    
}