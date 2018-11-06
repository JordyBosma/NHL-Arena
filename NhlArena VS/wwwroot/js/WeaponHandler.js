var weaponArray = [
    {
        id: 0,
        name: "Apple",
        damage: 20,
        velocity: 2.5,
        fireRate: 200,
        maxAmmo: 200,
        ammo: 100
    },
    {
        id: 1,
        name: "Laptop",
        damage: 100,
        velocity: 1.2,
        fireRate: 400,
        maxAmmo: 50,
        ammo: 0
    },
    {
        id: 2,
        name: "Beer",
        damage: 35,
        velocity: 1.8,
        fireRate: 250,
        maxAmmo: 100,
        ammo: 0
    }
]

function resetAmmo() {
    for (var i = 0; i < 3; i++) {
        weaponArray[i].ammo = 0;        
    }
    weaponArray[0].ammo = 50;
}

function AddAmmo(weaponId, Value) {
    if (weaponArray[weaponId].ammo + Value > weaponArray[weaponId].maxAmmo) {
        weaponArray[weaponId].ammo = weaponArray[weaponId].maxAmmo;
    }
    else {
        weaponArray[weaponId].ammo += Value;
    }
}

function GetDefaultWeapon() {
    return weaponArray[0];
}

function GetWeaponByIndex(int) {
    return weaponArray[int];
}

function GetNextWeapon(currentWeapon) {
    for (var i = 0; i < weaponArray.length; i++) {
        if (weaponArray[i].id == (currentWeapon.id +1)) {
            return weaponArray[i];
        }
    }
    return currentWeapon;
}

function GetPreviousWeapon(currentWeapon) {
    for (var i = 0; i < weaponArray.length; i++) {
        if (weaponArray[i].id == (currentWeapon.id - 1)) {
            return weaponArray[i];
        }
    }
    return currentWeapon;
}

