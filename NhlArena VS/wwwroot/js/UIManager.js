class UIManager {
    constructor(x) {
        this.optionM = document.getElementById("OptionMenu");
        this.volume = true;
    }

    ShowOptionMenu(b) {
        //this.Show(this.menu, b);
        document.getElementById("toggle-1").checked = b;
    }

    
    SwitchSoundOnOff(document) {
        if (document.innerText === "volume_up") {
            this.volume = false;
            document.innerText = "volume_mute";
        } else {
            this.volume = true;
            document.innerText = "volume_up";
        }
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