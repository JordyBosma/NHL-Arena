﻿var weaponArray = [
    {
        id: 0,
        name: "Apple",
        damage: 20,
        velocity: 5,
        fireRate: 300,
        maxAmmo: 100,
        ammo: 100
    },
    {
        id: 1,
        name: "Laptop",
        damage: 75,
        velocity: 2,
        fireRate: 1000,
        maxAmmo: 100,
        ammo: 100
    },
    {
        id: 2,
        name: "Beer",
        damage: 40,
        velocity: 4,
        fireRate: 500,
        maxAmmo: 100,
        ammo: 100
    }
]

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
