using Godot;
using System;
using System.Collections.Generic;

public class Weapon
{
	public string Name;
	public int Ammo;
}

public partial class GlobalWeapon : Node
{
	// Le délégué a été corrigé pour passer un int, ce qui est compatible avec le signal.
	[Signal]
	public delegate void WeaponChangedEventHandler(int newWeaponType);
	
	public enum WeaponType { Gun, Bat, Machinegun, Shotgun, Rocketlauncher }
	public Dictionary<WeaponType, Weapon> weapons = new Dictionary<WeaponType, Weapon>();
	public WeaponType currentWeapon = WeaponType.Bat;

	public override void _Ready()
	{
		weapons.Add(WeaponType.Gun, new Weapon { Name = "gun", Ammo = 10 });
		weapons.Add(WeaponType.Bat, new Weapon { Name = "bat", Ammo = -1 });
		weapons.Add(WeaponType.Machinegun, new Weapon { Name = "machinegun", Ammo = 60 });
		weapons.Add(WeaponType.Shotgun, new Weapon { Name = "shotgun", Ammo = 24 });
		weapons.Add(WeaponType.Rocketlauncher, new Weapon { Name = "rocketlauncher", Ammo = 8 });

	}
	
	public void ChangeWeapon(WeaponType newWeapon)
	{
		if (weapons.ContainsKey(newWeapon))
		{
			currentWeapon = newWeapon;
			GD.Print("Arme changée : " + weapons[currentWeapon].Name);
			
			// Le signal est émis en convertissant l'enum en int.
			EmitSignal(SignalName.WeaponChanged, (int)currentWeapon);
		}
	}
	
	public void CycleNextWeapon()
	{
		Array weaponTypes = Enum.GetValues(typeof(WeaponType));
		
		int currentIndex = Array.IndexOf(weaponTypes, currentWeapon);
		
		int nextIndex = (currentIndex + 1) % weaponTypes.Length;
		
		WeaponType nextWeapon = (WeaponType)weaponTypes.GetValue(nextIndex);
		
		ChangeWeapon(nextWeapon);
	}
}
