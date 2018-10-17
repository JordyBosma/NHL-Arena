class Character extends THREE.Group {



    constructor(camera) {
        super();
        this.init(camera);
    }

    init(camera) {
        var selfRef = this;
        selfRef.controls = new THREE.FirstPersonControls(camera);
        var box = new Physijs.BoxMesh(
            new THREE.BoxGeometry(1, 1, 1),
            new THREE.MeshBasicMaterial({ color: 0x111555 })
        );

        selfRef.add(box);
        selfRef.add(selfRef.controls.getObject());
    }

    updatePlayerMovement() {
        var selfRef = this;
        var controls = selfRef.controls;
        var prevTime = performance.now();
        var velocity = new THREE.Vector3();
        // Check of de browser pointerlock ondersteund
        if (controls.controlsEnabled) {

            // Save the current time
            var time = performance.now();
            // Create a delta value based on current time
            var delta = (time - prevTime) / 1000;

            // Set the velocity.x and velocity.z using the calculated time delta
            velocity.x -= velocity.x * 10.0 * delta;
            velocity.z -= velocity.z * 10.0 * delta;

            // As velocity.y is our "gravity," calculate delta
            velocity.y -= 9.8 * 20.0 * delta; // 100.0 = mass

            if (controls.moveForward) {
                velocity.z -= 400.0 * delta;
            }

            if (controls.moveBackward) {
                velocity.z += 400.0 * delta;
            }

            if (controls.moveLeft) {
                velocity.x -= 400.0 * delta;
            }

            if (controls.moveRight) {
                velocity.x += 400.0 * delta;
            }

            if (controls.jump) {
                if (velocity.y > 200) {
                    velocity.y = 0;
                }
                velocity.y += 50.0;
                controls.jump = false;
            }

            // Update the position using the changed delta
            selfRef.translateX(velocity.x * delta);
            selfRef.translateY(velocity.y * delta);
            selfRef.translateZ(velocity.z * delta);

            // Prevent the camera/player from falling out of the 'world'
            if (controls.getObject().position.y < 3) {
                velocity.y = 0;
                controls.getObject().position.y = 3;
            }

            // Save the time for future delta calculations
            prevTime = time;
        }
    }
}