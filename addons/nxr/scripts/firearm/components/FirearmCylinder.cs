using Godot;
using NXR;
using NXRFirearm;
using System;

[Tool]
[GlobalClass]
public partial class FirearmCylinder : FirearmMovable
{
	[Export]
	private Firearm _firearm; 
	
	[Export]
	private Node3D _bulletQueue; 

    public override void _Ready()
    {

        if (Util.NodeIs((Node)GetParent(), typeof(Firearm)))
        {
            _firearm = (Firearm)GetParent();
        }
    }

    public override void _Process(double delta) { 
		
		if (Engine.IsEditorHint()) { 
			RunTool();
		}

		if (_firearm == null) return; 

		if (IsClosed() && GetOpenInput()) { 
			Open(); 
		}

		if (!IsClosed() && GetCloseInput()) { 
			Close(); 
		}

		if (!IsClosed()) { 
			_firearm.BlockFire = true; 
		} else{ 
			_firearm.BlockFire = false; 
		}

		if(_bulletQueue != null) { 
			if (!IsClosed() && -GlobalTransform.Basis.Z.Dot(Vector3.Up) > 0.8) { 
				FirearmBulletZoneQueue queue = (FirearmBulletZoneQueue)_bulletQueue; 
				queue.EjectAll(Vector3.Zero,  Vector3.Zero, true); 
			}
		}
	}

	private bool IsClosed() { 
		return Target.Transform.IsEqualApprox(StartXform); 
	}
	

	public void Open() { 
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(Target, "transform", EndXform, 0.1f); 
	}

	public void Close() { 
		Tween tween = GetTree().CreateTween(); 
		tween.TweenProperty(Target, "transform", StartXform, 0.1f); 
	}
	
	private bool GetOpenInput() { 
		if (_firearm.GetPrimaryInteractor() == null) return false; 
		
		Controller controller = _firearm.GetPrimaryInteractor().Controller; 

		return _firearm.GetPrimaryInteractor().Controller.ButtonOneShot("by_button"); 
	}

	private bool GetCloseInput() { 
		if (_firearm.GetPrimaryInteractor() == null) return false; 
		
		Controller controller = _firearm.GetPrimaryInteractor().Controller; 

		return controller.LocalVelMatches(GlobalTransform.Basis.X, 30); 
	}
}
