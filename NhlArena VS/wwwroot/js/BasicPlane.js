class BasicPlane extends THREE.Group {
    constructor() {
        super();
        this.init();
    }
    init() {
        var selfRef = this;
        var geometry = new THREE.BoxGeometry(20, 2, 20);
        var material = new THREE.MeshBasicMaterial({ color: 0xffffff });

        var box = new THREE.Mesh(geometry, material);
        box.position.x = -10;
        box.position.y = 0;
        box.position.z = -100;
        selfRef.add(box);
    }
}