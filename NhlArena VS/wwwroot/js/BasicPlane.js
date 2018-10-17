class BasicPlane extends THREE.Group {
    constructor() {
        super();
        this.init();
    }
    init() {
        var selfRef = this;
        var geometry = new THREE.BoxGeometry(200, 2, 200);
        var material = new THREE.MeshBasicMaterial({ color: 0x111555});

        var box = new THREE.Mesh(geometry, material);
        box.position.x = -10;
        box.position.y = 0;
        box.position.z = -100;
        selfRef.add(box);
    }
}