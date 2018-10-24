class Player {
    constructor(camera, scene) {

        this.camera = camera;

        this.prevTime = performance.now();
        this.velocity = new THREE.Vector3();

        this.controls = new THREE.FirstPersonControls(this.camera);
        //scene.add(this.controls.GetPlayer);
    }
    Update() {
        this.playerControls();
    }

    LogPositionGetObject() {
        window.setInterval(1000);
        console.log(this.controls.getPlayer().position.x, this.controls.getPlayer().position.y, this.controls.getPlayer().position.z);
    }

    playerControls() {

        // Check for pointerlock in browser
        if (this.controls.controlsEnabled) {

            // Save the current time
            this.time = performance.now();
            // Create a delta value based on current time
            this.delta = (this.time - this.prevTime) / 1000;
            // Set the velocity.x and velocity.z using the calculated time delta
            this.velocity.x -= this.velocity.x * 10.0 * this.delta;
            this.velocity.z -= this.velocity.z * 10.0 * this.delta;

            // Calculate "gravity" using delta
            this.velocity.y -= 9.8 * 20.0 * this.delta; // middle number = mass

            // Moves the camera (player) forward
            if (this.controls.moveForward) {
                this.velocity.z -= 200.0 * this.delta;
            }

            // Moves the camera (player) backward
            if (this.controls.moveBackward) {
                this.velocity.z += 200.0 * this.delta;
            }

            // Moves the camera (player) left
            if (this.controls.moveLeft) {
                this.velocity.x -= 200.0 * this.delta;
            }

            // Moves the camera (player) right
            if (this.controls.moveRight) {
                this.velocity.x += 200.0 * this.delta;
            }

            // Makes the camera (player) jump
            if (this.controls.jump) {
                if (this.velocity.y > 100) {
                    this.velocity.y = 0;
                }
                this.velocity.y += 50.0;
                this.controls.jump = false;
            }

            // Update the position using the changed delta
            this.controls.GetPlayer().translateX(this.velocity.x * this.delta);
            this.controls.GetPlayer().translateY(this.velocity.y * this.delta);
            this.controls.GetPlayer().translateZ(this.velocity.z * this.delta);

            // Prevent the camera/player from falling out of the 'world'
            if (this.controls.GetPlayer().position.y < 3) {
                this.velocity.y = 0;
                this.controls.GetPlayer().position.y = 3;
            }

            // Save the time for future delta calculations
            this.prevTime = this.time;
        }
    }
}

function 