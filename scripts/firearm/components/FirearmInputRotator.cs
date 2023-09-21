using Godot;
using System;

namespace NXRFirearm; 

[Tool]
[GlobalClass]
public partial class FirearmInputRotator : Node3D
{
    [Export]
    private Vector3 _start = new();
    [Export]
    private Vector3 _end = new();

    [ExportGroup("tool settings")]
    [Export]
    private bool _setStart = false;
    [Export]
    private bool _setEnd = false;
    [Export]
    private bool _goStart = false;
    [Export]
    private bool _goEnd = false;


    public float inputDelta = 0.0f; 

    public override void _Process(double delta)
    {
      if (Engine.IsEditorHint()) { 

            if (_setStart)
            {
                _start = Rotation;
                _setStart = false;
                GD.Print(_start); 
            }
            if (_setEnd)
            {
                _end = Rotation;
                _setEnd = false;
            }

            if (_goStart)
            {
                Rotation = _start; 
                _goStart = false;
            }
            if (_goEnd)
            {
                Rotation = _end; 
                _goEnd = false;
            }
            return; 
        }
        
        Basis = Basis.FromEuler(_start).Slerp(
            Basis.FromEuler(_end),
            inputDelta
        ); 
        
    }
}