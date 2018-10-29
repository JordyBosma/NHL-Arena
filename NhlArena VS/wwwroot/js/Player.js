class Player extends THREE.Group {

    constructor(x ,y ,z) {
        super();
        this.init(x,y,z);
    }


    init(x,y,z) {
        var selfref = this;
        var geometry = new THREE.BoxGeometry(1, 1, 1);
        var player = new THREE.Mesh(geometry);
        player.position.y = y;
        player.position.x = x;
        player.position.z = z;
        player.receiveShadow = true;
        player.castShadow = true;
        player.shadowDarkness = 0.5;
        selfref.add(player);
    }
} 