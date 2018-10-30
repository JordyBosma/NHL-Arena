class BasicPlane extends THREE.Group {
    constructor() {
        super();
        this.init();
    }
    init() {
        var selfRef = this;
        selfRef.name = "basicplane";
        var material = new THREE.MeshBasicMaterial({ color: 0xffffff });
        var box = new THREE.Mesh(
            new THREE.BoxGeometry(200, 2, 200),
            material
        );
        var material2 = new THREE.MeshBasicMaterial({ color: 0x888888, side:THREE.DoubleSide });
        var box2 = new THREE.Mesh(
            new THREE.BoxGeometry(200, 10, 1),
            material2
        );

        var material3 = new THREE.MeshBasicMaterial({ color: 0x888888, side: THREE.DoubleSide });
        var box3 = new THREE.Mesh(
            new THREE.BoxGeometry(1, 10, 20),
            material3
        );
        box2.position.x = 0;
        box2.position.y = 0;
        box2.position.z = 0;
        

        box.position.x = 0;
        box.position.y = -1;
        box.position.z = -100;
        
        selfRef.add(box, box2,box3);
    }
}