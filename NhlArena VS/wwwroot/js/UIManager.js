class UIManager {
    constructor(x) {
        this.volume = true;
    }

    setAngleUIParts() {
        var ps = document.getElementsByClassName("part__container");
        for (var i = 0; i < ps.length; i++) {
            var style = window.getComputedStyle(ps[i]);
            var height = style.getPropertyValue('height');

            ps[i].style.setProperty('--part-angle', Math.acos(height / (height * 0.75)));
        }
    }

    ShowOptionMenu(b) {
        //this.Show(this.menu, b);
        document.getElementById("toggle-1").checked = b;
    }

    
    SwitchSoundOnOff(clicked) {
        var newInnerText;
        if (clicked.innerText === "volume_up") {
            this.volume = false;
            newInnerText = "volume_mute";
        } else {
            this.volume = true;
            newInnerText = "volume_up";
        }
        var soundSwitchButtons = document.getElementsByClassName("volume");
        for (var i = 0; i < soundSwitchButtons.length; i++) {
            soundSwitchButtons[i].innerText = newInnerText;
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