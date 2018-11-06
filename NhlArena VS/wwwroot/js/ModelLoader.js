class NHLArenaMap extends THREE.Group {
    constructor() {
        super();
        this.init();
    }

    init() {
        var selfRef = this;
        this.name = "NHLArenaMap";
        loadOBJModel("/models/objects/NHLArenaMap/", "NHLArenaMap.obj", "/models/materials/NHLArenaMap/", "NHLArenaMap.mtl", (mesh) => {
            mesh.scale.set(1, 1, 1);
            selfRef.add(mesh);
        });
    }
}

class Player extends THREE.Group {
    constructor(x,y,z,guid) {
        super();
        this.init(x,y,z,guid);
    }

    init(x,y,z,guid) {
        var selfRef = this;
        loadOBJModel("/models/objects/Character/", "CharacterRed.obj", "/models/materials/Character/", "CharacterRed.mtl", (mesh) => {
            mesh.scale.set(1, 1, 1);
            mesh.position.y -= 2;
            mesh.rotation.y = Math.PI;
            selfRef.add(mesh);
        });
        selfRef.position.set(x, y, z);
        this.playerGuid = guid;
    }
}

class Player2 extends THREE.Group {
    constructor(x, y, z, guid) {
        super();
        this.init(x, y, z, guid);
    }

    init(x, y, z, guid) {
        var selfRef = this;
        loadOBJModel("/models/objects/Character/", "CharacterRed.obj", "/models/materials/Character/", "CharacterRed.mtl", (mesh) => {
            mesh.scale.set(1, 1, 1);
            mesh.position.y -= 2;
            mesh.rotation.y = Math.PI;
            selfRef.add(mesh);
        });
        selfRef.position.set(x, y, z);
        this.playerGuid = guid;
    }
}

class Player3 extends THREE.Group {
    constructor(x, y, z, guid) {
        super();
        this.init(x, y, z, guid);
    }

    init(x, y, z, guid) {
        var selfRef = this;
        loadOBJModel("/models/objects/Character/", "CharacterRed.obj", "/models/materials/Character/", "CharacterRed.mtl", (mesh) => {
            mesh.scale.set(1, 1, 1);
            mesh.position.y -= 2;
            mesh.rotation.y = Math.PI;
            selfRef.add(mesh);
        });
        selfRef.position.set(x, y, z);
        this.playerGuid = guid;
    }
}

class Player4 extends THREE.Group {
    constructor(x, y, z, guid) {
        super();
        this.init(x, y, z, guid);
    }

    init(x, y, z, guid) {
        var selfRef = this;
        loadOBJModel("/models/objects/Character/", "CharacterRed.obj", "/models/materials/Character/", "CharacterRed.mtl", (mesh) => {
            mesh.scale.set(1, 1, 1);
            mesh.position.y -= 2;
            mesh.rotation.y = Math.PI;
            selfRef.add(mesh);
        });
        selfRef.position.set(x, y, z);
        this.playerGuid = guid;
    }
}

class Player5 extends THREE.Group {
    constructor(x, y, z, guid) {
        super();
        this.init(x, y, z, guid);
    }

    init(x, y, z, guid) {
        var selfRef = this;
        loadOBJModel("/models/objects/Character/", "CharacterRed.obj", "/models/materials/Character/", "CharacterRed.mtl", (mesh) => {
            mesh.scale.set(1, 1, 1);
            mesh.position.y -= 2;
            mesh.rotation.y = Math.PI;
            selfRef.add(mesh);
        });
        selfRef.position.set(x, y, z);
        this.playerGuid = guid;
    }
}

class Player6 extends THREE.Group {
    constructor(x, y, z, guid) {
        super();
        this.init(x, y, z, guid);
    }

    init(x, y, z, guid) {
        var selfRef = this;
        loadOBJModel("/models/objects/Character/", "CharacterRed.obj", "/models/materials/Character/", "CharacterRed.mtl", (mesh) => {
            mesh.scale.set(1, 1, 1);
            mesh.position.y -= 2;
            mesh.rotation.y = Math.PI;
            selfRef.add(mesh);
        });
        selfRef.position.set(x, y, z);
        this.playerGuid = guid;
    }
}

class Projectile extends THREE.Group {
    constructor() {
        super();
        this.init();
    }

    init() {
        var selfRef = this;
        var material = new THREE.MeshLambertMaterial({ color: 0xff0000, wireframe: true });
        var box = new THREE.Mesh(
            new THREE.SphereGeometry(1),
            material
        );
        box.scale.set(0.1, 0.1, 0.1);
        selfRef.add(box);
    }
}

class DamageBoost extends THREE.Group {
    constructor() {
        super();
        this.init();
    }

    UpdateTime() {
        if (this.materialShader) {

            this.materialShader.uniforms.time.value = performance.now() / 1000;
        }
    }

    init() {
        var selfRef = this;

        loadOBJModel("/models/objects/Pickups/", "Pickup_Damage.obj", "/models/materials/Pickups/", "Pickup_Damage.mtl", (mesh) => {
            //console.log(mesh);
            mesh.children[0].material.onBeforeCompile = function (shader) {

                console.log(shader);

                shader.uniforms.time = { value: 0 };

                shader.vertexShader = 'uniform float time;\n' + shader.vertexShader;
                shader.vertexShader = shader.vertexShader.replace(
                    '#include <begin_vertex>',
                    [
                        'float theta = time * 2.0;',
                        'float c = cos( theta );',
                        'float s = sin( theta );',
                        
                        'mat3 m = mat3( c, 0, s, 0, 1, 0, -s, 0, c );',
                        'vec3 transformed = vec3( position ) * m;',
                        'vNormal = vNormal * m;'
                        
                    ].join('\n')
                );
                selfRef.materialShader = shader;
            };
            mesh.scale.set(2.5, 2.5, 2.5);
            selfRef.add(mesh);
        });
    }
}

class SpeedBoost extends THREE.Group {
    constructor() {
        super();
        this.init();
    }

    UpdateTime() {
        if (this.materialShader) {

            this.materialShader.uniforms.time.value = performance.now() / 1000;
        }
    }

    init() {
        var selfRef = this;

        loadOBJModel("/models/objects/Pickups/", "Pickup_Speed.obj", "/models/materials/Pickups/", "Pickup_Speed.mtl", (mesh) => {
            mesh.children[0].material.onBeforeCompile = function (shader) {

                console.log(shader);

                shader.uniforms.time = { value: 0 };

                shader.vertexShader = 'uniform float time;\n' + shader.vertexShader;
                shader.vertexShader = shader.vertexShader.replace(
                    '#include <begin_vertex>',
                    [
                        'float theta = time * 2.0;',
                        'float c = cos( theta );',
                        'float s = sin( theta );',

                        'mat3 m = mat3( c, 0, s, 0, 1, 0, -s, 0, c );',
                        'vec3 transformed = vec3( position ) * m;',
                        'vNormal = vNormal * m;'

                    ].join('\n')
                );
                selfRef.materialShader = shader;
            };
            mesh.scale.set(0.8, 0.8, 0.8);
            selfRef.add(mesh);
        });
    }
}

class AmmoItem extends THREE.Group {
    constructor() {
        super();
        this.init();
    }

    UpdateTime() {
        if (this.materialShader) {

            this.materialShader.uniforms.time.value = performance.now() / 1000;
        }
    }

    init() {
        var selfRef = this;

        loadOBJModel("/models/objects/Pickups/", "Pickup_Ammo.obj", "/models/materials/Pickups/", "Pickup_Ammo.mtl", (mesh) => {
            mesh.children[0].material.onBeforeCompile = function (shader) {

                console.log(shader);

                shader.uniforms.time = { value: 0 };

                shader.vertexShader = 'uniform float time;\n' + shader.vertexShader;
                shader.vertexShader = shader.vertexShader.replace(
                    '#include <begin_vertex>',
                    [
                        'float theta = time * 2.0;',
                        'float c = cos( theta );',
                        'float s = sin( theta );',

                        'mat3 m = mat3( c, 0, s, 0, 1, 0, -s, 0, c );',
                        'vec3 transformed = vec3( position ) * m;',
                        'vNormal = vNormal * m;'

                    ].join('\n')
                );
                selfRef.materialShader = shader;
            };
            mesh.scale.set(0.8, 0.8, 0.8);
            selfRef.add(mesh);
        });
    }
}

class HealthItem extends THREE.Group {
    constructor() {
        super();
        this.init();
    }

    UpdateTime() {
        if (this.materialShader) {

            this.materialShader.uniforms.time.value = performance.now() / 1000;
        }
    }

    init() {
        var selfRef = this;

        loadOBJModel("/models/objects/Pickups/", "Pickup_Health.obj", "/models/materials/Pickups/", "Pickup_Health.mtl", (mesh) => {
            mesh.children[0].material.onBeforeCompile = function (shader) {

                console.log(shader);

                shader.uniforms.time = { value: 0 };

                shader.vertexShader = 'uniform float time;\n' + shader.vertexShader;
                shader.vertexShader = shader.vertexShader.replace(
                    '#include <begin_vertex>',
                    [
                        'float theta = time * 2.0;',
                        'float c = cos( theta );',
                        'float s = sin( theta );',

                        'mat3 m = mat3( c, 0, s, 0, 1, 0, -s, 0, c );',
                        'vec3 transformed = vec3( position ) * m;',
                        'vNormal = vNormal * m;'

                    ].join('\n')
                );
                selfRef.materialShader = shader;
            };
            mesh.scale.set(0.8, 0.8, 0.8);
            selfRef.add(mesh);
        });
    }
}

class ArmourItem extends THREE.Group {
    constructor() {
        super();
        this.init();
    }

    UpdateTime() {
        if (this.materialShader) {

            this.materialShader.uniforms.time.value = performance.now() / 1000;
        }
    }

    init() {
        var selfRef = this;

        loadOBJModel("/models/objects/Pickups/", "Pickup_Shield.obj", "/models/materials/Pickups/", "Pickup_Shield.mtl", (mesh) => {
            mesh.children[0].material.onBeforeCompile = function (shader) {

                console.log(shader);

                shader.uniforms.time = { value: 0 };

                shader.vertexShader = 'uniform float time;\n' + shader.vertexShader;
                shader.vertexShader = shader.vertexShader.replace(
                    '#include <begin_vertex>',
                    [
                        'float theta = time * 2.0;',
                        'float c = cos( theta );',
                        'float s = sin( theta );',

                        'mat3 m = mat3( c, 0, s, 0, 1, 0, -s, 0, c );',
                        'vec3 transformed = vec3( position ) * m;',
                        'vNormal = vNormal * m;'

                    ].join('\n')
                );
                selfRef.materialShader = shader;
            };
            mesh.scale.set(0.8, 0.8, 0.8);
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