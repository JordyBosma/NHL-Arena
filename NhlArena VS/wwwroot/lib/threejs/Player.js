class Player extends THREE.Group {
    constructor(camera) {
        super();
        this.init();
        CameraControls(camera);
    }

    //Sets all the player variables
    init() {
        var jumpDelay = 500;
        var canJump = true;
        var scope = this;
        var raycaster = new THREE.Raycaster();
        var originVector = new THREE.Vector3();
        var directionVector = new THREE.Vector3();
        var intersects;
        var prevTime = performance.now();
        var velocity = new THREE.Vector3();
    }

    //Updates the things that have to be updated every gametick
    Update = function() {
        this.RayCasting();
        this.PlayerControls();
    }

    //Casts a ray
    RayCasting = function() {
        originVector.x = player.getObject().position.x;
        originVector.y = player.getObject().position.y;
        originVector.z = player.getObject().position.z;
        directionVector.x = 1;

        //scene.add(new THREE.ArrowHelper(raycaster.ray.direction, raycaster.ray.origin, 300, 0xff0000));

        raycaster.set(originVector, originVector);

        intersects = raycaster.intersectObjects(scene.children);

        for (var i = 0; i < intersects.length; i++) {

            intersects[i].object.material.color.set(0xff0000);
        }
    }
    //Sets and enables the player to be controlled
    PlayerControls = function() {
        if (player.controlsEnabled) {

            // Save the current time
            var time = performance.now();
            // Create a delta value based on current time
            var delta = (time - prevTime) / 1000;
            // Set the velocity.x and velocity.z using the calculated time delta
            velocity.x -= velocity.x * 10.0 * delta;
            velocity.z -= velocity.z * 10.0 * delta;

            // Calculate "gravity" using delta
            velocity.y -= 9.8 * 20.0 * delta; // middle number = mass

            // Moves the camera (player) forward
            if (player.moveForward) {
                velocity.z -= 200.0 * delta;
            }

            // Moves the camera (player) backward
            if (player.moveBackward) {
                velocity.z += 200.0 * delta;
            }

            // Moves the camera (player) left
            if (player.moveLeft) {
                velocity.x -= 200.0 * delta;
            }

            // Moves the camera (player) right
            if (player.moveRight) {
                velocity.x += 200.0 * delta;
            }

            // Makes the camera (player) jump
            if (player.jump) {
                if (velocity.y > 100) {
                    velocity.y = 0;
                }
                velocity.y += 50.0;
                player.jump = false;
            }

            // Update the position using the changed delta
            player.getObject().translateX(velocity.x * delta);
            player.getObject().translateY(velocity.y * delta);
            player.getObject().translateZ(velocity.z * delta);

            // Prevent the camera/player from falling out of the 'world'
            if (player.getObject().position.y < 3) {
                velocity.y = 0;
                player.getObject().position.y = 3;
            }

            // Save the time for future delta calculations
            prevTime = time;
        }
    }
}

function PlayerKeys() {

    this.moveForward = false;
    this.moveBackward = false;
    this.moveLeft = false;
    this.moveRight = false;
    this.jump = false;

    //Event when key is pressed down
    var onKeyDown = function (event) {
        switch (event.keyCode) {
            case 38: // up
            case 87: // w
                scope.moveForward = true;
                break;
            case 37: // left
            case 65: // a
                scope.moveLeft = true;
                break;
            case 40: // down
            case 83: // s
                scope.moveBackward = true;
                break;
            case 39: // right
            case 68: // d
                scope.moveRight = true;
                break;
            case 32: // spacebar
                if (canJump) {
                    scope.jump = true;
                    canJump = false;
                    var timer = setTimeout(UpdateCanJump, delay);
                }
                break;
        }
    };

    //Changes the variable that controls the ability to jump to true || false
    function UpdateCanJump() {
        canJump = true;
    }

    //Event when key is let go
    var onKeyUp = function (event) {
        switch (event.keyCode) {
            case 38: // up
            case 87: // w
                scope.moveForward = false;
                break;
            case 37: // left
            case 65: // a
                scope.moveLeft = false;
                break;
            case 40: // down
            case 83: // s
                scope.moveBackward = false;
                break;
            case 39: // right
            case 68: // d
                scope.moveRight = false;
                break;
            case 32: // spacebar
                scope.jump = false;
                break;
        }
    };
    document.addEventListener('keydown', onKeyDown, false);
    document.addEventListener('keyup', onKeyUp, false);
}
/**
 * The camera control function, this function controls all camera movement and locks the cursor in the middle of the camera
 * @param {PerspectiveCamera} camera The camera used for the firstperson perspective.
 */
function CameraControls(camera) {
    camera.rotation.set(0, 0, 0);

    //Creating a placeholder crosshairbox and material
    var crosshairMaterial = new THREE.MeshBasicMaterial({ color: 0x000000, side: THREE.DoubleSide });
    var crosshair = new Physijs.BoxMesh(
        new THREE.BoxGeometry(0.1, 0.1, 0.1),
        crosshairMaterial
    );
    //Giving the crosshairbox a position
    crosshair.position.set(0, 0, -10);

    //pitchobject controls horizontal camera movement
    var pitchObject = new THREE.Object3D();
    pitchObject.add(camera);

    //yawObject controls vertical camera movement
    var yawObject = new THREE.Object3D();
    yawObject.position.y = 10;
    yawObject.add(pitchObject);
    pitchObject.add(crosshair);

    //Helps with calculating the rotations
    var PI_2 = Math.PI / 2;

    //Event triggers when mouse is moved
    var onMouseMove = function (event) {
        var movementX = event.movementX || event.mozMovementX || event.webkitMovementX || 0;
        var movementY = event.movementY || event.mozMovementY || event.webkitMovementY || 0;

        //Rotates based on whether the mouse is moved along the X or Y axis.
        yawObject.rotation.y -= movementX * 0.002;
        pitchObject.rotation.x -= movementY * 0.002;

        pitchObject.rotation.x = Math.max(- PI_2, Math.min(PI_2, pitchObject.rotation.x));
    };
    //Allows the camera to be controlled
    this.enabled = false;

    //Gets and returns the object that controls the camera movement.
    this.getObject = function () {
        return yawObject;
    };

    //Variable to save the cursorlock in
    var havePointerLock = 'pointerLockElement' in document ||
        'mozPointerLockElement' in document ||
        'webkitPointerLockElement' in document;
    //Dunno what this does, probably along the lines of: if browser does not support *cursor lock method* then try different *cursor lock method*
    if (havePointerLock) {
        var element = document.body;
        var pointerlockchange = function (event) {
            if (document.pointerLockElement === element ||
                document.mozPointerLockElement === element ||
                document.webkitPointerLockElement === element) {
                scope.controlsEnabled = true;
                document.addEventListener('mousemove', onMouseMove, false);
            } else {
                scope.controlsEnabled = false;
            }
        };
        //Gives an error if cursor lock is not supported by browser
        var pointerlockerror = function (event) {
            //There was an error
        };

        // Hook pointer lock state change events
        document.addEventListener('pointerlockchange', pointerlockchange, false);
        document.addEventListener('mozpointerlockchange', pointerlockchange, false);
        document.addEventListener('webkitpointerlockchange', pointerlockchange, false);
        document.addEventListener('pointerlockerror', pointerlockerror, false);
        document.addEventListener('mozpointerlockerror', pointerlockerror, false);
        document.addEventListener('webkitpointerlockerror', pointerlockerror, false);

    } else {
        document.body.innerHTML = 'Your browser doesn\'t seem to support Pointer Lock API';
    }

    //Gets the direction the camera is rotating
    this.getDirection = function () {

        // assumes the camera itself is not rotated

        var direction = new THREE.Vector3(0, 0, - 1);
        var rotation = new THREE.Euler(0, 0, 0, "YXZ");

        return function (v) {

            rotation.set(pitchObject.rotation.x, yawObject.rotation.y, 0);

            v.copy(direction).applyEuler(rotation);

            return v;

        };

    }();

    //var newDiv = document.createElement("div");
    //newDiv.innerHTML = "Click to play";
    //document.body.appendChild (newDiv);
    document.body.addEventListener("click", function () {
        var element = document.body;
        element.requestPointerLock = element.requestPointerLock || element.mozRequestPointerLock || element.webkitRequestPointerLock;
        element.requestPointerLock();
    });
}

//THREE.FirstPersonControls = function (camera) {
    //var delay = 500;
    //var canJump = true;
    //var scope = this;

    //camera.rotation.set(0, 0, 0);

    //var material = new THREE.MeshBasicMaterial({ color: 0x000000, side: THREE.DoubleSide });
    //var box = new Physijs.BoxMesh(
    //    new THREE.BoxGeometry(0.1, 0.1, 0.1),
    //    material
    //);

    //box.position.set(0, 0, -10);

    //var pitchObject = new THREE.Object3D();
    //pitchObject.add(camera);

    //var yawObject = new THREE.Object3D();
    //yawObject.position.y = 10;
    //yawObject.add(pitchObject);
    //pitchObject.add(box);

    //var PI_2 = Math.PI / 2;

    //var onMouseMove = function (event) {
    //    var movementX = event.movementX || event.mozMovementX || event.webkitMovementX || 0;
    //    var movementY = event.movementY || event.mozMovementY || event.webkitMovementY || 0;

    //    yawObject.rotation.y -= movementX * 0.002;
    //    pitchObject.rotation.x -= movementY * 0.002;

    //    pitchObject.rotation.x = Math.max(- PI_2, Math.min(PI_2, pitchObject.rotation.x));
    //};
    //this.enabled = false;

    //this.getObject = function () {
    //    return yawObject;
    //};

    //this.moveForward = false;
    //this.moveBackward = false;
    //this.moveLeft = false;
    //this.moveRight = false;
    //this.jump = false;

    //var onKeyDown = function (event) {
    //    switch (event.keyCode) {
    //        case 38: // up
    //        case 87: // w
    //            scope.moveForward = true;
    //            break;
    //        case 37: // left
    //        case 65: // a
    //            scope.moveLeft = true;
    //            break;
    //        case 40: // down
    //        case 83: // s
    //            scope.moveBackward = true;
    //            break;
    //        case 39: // right
    //        case 68: // d
    //            scope.moveRight = true;
    //            break;
    //        case 32: // spacebar
    //            if (canJump) {
    //                scope.jump = true;
    //                canJump = false;
    //                var timer = setTimeout(UpdateCanJump, delay);
    //            }
    //            break;
    //    }
    //};

    //function UpdateCanJump() {
    //    canJump = true;
    //}

    //var onKeyUp = function (event) {
    //    switch (event.keyCode) {
    //        case 38: // up
    //        case 87: // w
    //            scope.moveForward = false;
    //            break;
    //        case 37: // left
    //        case 65: // a
    //            scope.moveLeft = false;
    //            break;
    //        case 40: // down
    //        case 83: // s
    //            scope.moveBackward = false;
    //            break;
    //        case 39: // right
    //        case 68: // d
    //            scope.moveRight = false;
    //            break;
    //        case 32: // spacebar
    //            scope.jump = false;
    //            break;
    //    }
    //};

    //document.addEventListener('keydown', onKeyDown, false);
    //document.addEventListener('keyup', onKeyUp, false);

    //var havePointerLock = 'pointerLockElement' in document ||
    //    'mozPointerLockElement' in document ||
    //    'webkitPointerLockElement' in document;

    //if (havePointerLock) {
    //    var element = document.body;
    //    var pointerlockchange = function (event) {
    //        if (document.pointerLockElement === element ||
    //            document.mozPointerLockElement === element ||
    //            document.webkitPointerLockElement === element) {
    //            scope.controlsEnabled = true;
    //            document.addEventListener('mousemove', onMouseMove, false);
    //        } else {
    //            scope.controlsEnabled = false;
    //        }
    //    };
    //    var pointerlockerror = function (event) {
    //        //There was an error
    //    };
    //    // Hook pointer lock state change events
    //    document.addEventListener('pointerlockchange', pointerlockchange, false);
    //    document.addEventListener('mozpointerlockchange', pointerlockchange, false);
    //    document.addEventListener('webkitpointerlockchange', pointerlockchange, false);
    //    document.addEventListener('pointerlockerror', pointerlockerror, false);
    //    document.addEventListener('mozpointerlockerror', pointerlockerror, false);
    //    document.addEventListener('webkitpointerlockerror', pointerlockerror, false);

    //} else {
    //    document.body.innerHTML = 'Your browser doesn\'t seem to support Pointer Lock API';
    //}

    //this.getDirection = function () {

    //    // assumes the camera itself is not rotated

    //    var direction = new THREE.Vector3(0, 0, - 1);
    //    var rotation = new THREE.Euler(0, 0, 0, "YXZ");

    //    return function (v) {

    //        rotation.set(pitchObject.rotation.x, yawObject.rotation.y, 0);

    //        v.copy(direction).applyEuler(rotation);

    //        return v;

    //    };

    //}();

    ////var newDiv = document.createElement("div");
    ////newDiv.innerHTML = "Click to play";
    ////document.body.appendChild (newDiv);
    //document.body.addEventListener("click", function () {
    //    var element = document.body;
    //    element.requestPointerLock = element.requestPointerLock || element.mozRequestPointerLock || element.webkitRequestPointerLock;
    //    element.requestPointerLock();
    //});
//};