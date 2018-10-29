class NHLArenaMap extends THREE.Group {
    constructor() {
        super();
        this.init();
    }

    init() {
        var selfRef = this;
        loadOBJModel("/models/objects/", "Ramp.obj", "/models/materials/", "Ramp.mtl", (mesh) => {
            mesh.scale.set(1, 1, 1);
            selfRef.add(mesh);
        });
    }
}

class Player extends THREE.Group {
    constructor() {
        super();
        this.init();
    }

    init() {
        var selfRef = this;
        loadOBJModel("/models/objects/", "CharacterBase.obj", "/models/materials/", "CharacterBase.mtl", (mesh) => {
            mesh.scale.set(1, 1, 1);
            selfRef.add(mesh);
        });
    }
} 

/**
 * Load an OBJ model from the server
 * @param {string} objPath The path to the model (.obj) on the server
 * @param {string} objName The name of the model inside the path (OBJ file)
 * @param {string} materialPath The path to the texture (.mtl) of the model
 * @param {string} materialName The name of the texture of the mdoel (MTL File)
 * @param {function(THREE.Mesh): void} onload The function to be called once the model is loaded and available
 */
function loadOBJModel(objPath, objName, materialPath, materialName, onload) {
    new THREE.MTLLoader()
        .setPath(materialPath)
        .load(materialName, function (materials) {
            materials.preload();
            new THREE.OBJLoader()
                .setPath(objPath)
                .setMaterials(materials)
                .load(objName, function (object) {
                    onload(object);
                }, function () { }, function (e) { console.log("Error loading model"); console.log(e); });
        });
}