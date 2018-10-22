class BasicPlane extends THREE.Group {
    constructor() {
        super();
        this.init();
    }
    init() {
        var selfRef = this;

        var material = new THREE.MeshBasicMaterial({ color: 0xffffff });
        var box = new Physijs.BoxMesh(
            new THREE.BoxGeometry(200, 2, 200),
            material
        );
        var material2 = new THREE.MeshBasicMaterial({ color: 0x888888 });
        var box2 = new Physijs.BoxMesh(
            new THREE.BoxGeometry(200, 10, 1),
            material2
        );
        box2.position.x = 0;
        box2.position.y = 0;
        box2.position.z = 0;
        
        box.position.x = 0;
        box.position.y = -1;
        box.position.z = -100;
        
        selfRef.add(box, box2);
    }
}