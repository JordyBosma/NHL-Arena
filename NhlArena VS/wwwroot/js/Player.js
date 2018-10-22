class Player extends THREE.Group {

    constructor(x ,z) {
        super();
        this.init(x,z);
    }


    init(x,z) {
        var selfref = this;
        var geometry = new THREE.BoxGeometry(0.9, 0.9, 0.9);
        var player = new THREE.Mesh(geometry);
        player.position.y = 2;
        player.position.x = x;
        player.position.z = z;
        player.receiveShadow = true;
        player.castShadow = true;
        player.shadowDarkness = 0.5;
        selfref.add(player);
    }
} 