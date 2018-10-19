class UIManager {
    constructor(x) {
        this.menu = document.getElementById("MainMenu");
    }

    ShowMenu(b) {
        //this.Show(this.menu, b);
        document.getElementById("toggle-1").checked = b;
    }

    Show(x, b) {
        if (b) {
            x.classList.remove("hiddenEffect");
            x.classList.add("showEffect");
        } else {
            x.classList.remove("showEffect");
            x.classList.add("hiddenEffect");
        }
        
    }
}