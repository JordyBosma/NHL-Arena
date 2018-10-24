class BasicPlane extends THREE.Group {
    constructor() {
        super();
        this.init();
    }
    init() {
        var selfRef = this;

        var material = new THREE.MeshBasicMaterial({ color: 0xffffff });
        var box = new THREE.Mesh(
            new THREE.BoxGeometry(200, 2, 200),
            material
        );
        
        box.position.x = -10;
        box.position.y = 0;
        box.position.z = -100;
        selfRef.add(box);
    }
}