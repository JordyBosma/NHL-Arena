class UIManager {
    constructor(x) {
        this.volume = true;
        this.openMenu = "";
        this.timeLeft = 0;  //in seconds
        this.StartTimer(360);
        this.timer = null;
    }

    ShowOptionMenu(b) {
        document.getElementById("toggle-OptionMenu").checked = b;
    }

    ShowExitMenu(b) {
        document.getElementById("toggle-ExitMenu").checked = b;
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

    StartTimer(time) {
        this.timeLeft = time;
        this.timer = setInterval(this.UpdateTimer(), 1000);
    }

    UpdateTimer() {
        this.timeLeft--;
        console.log(this.timeLeft);
        var displayTimeMin = Math.floor(this.timeLeft / 60);
        displayTimeMin = displayTimeMin.toString();
        if (displayTimeMin.length < 2) {
            displayTimeMin = '0' + displayTimeMin;
        }
        var displayTimeSec = String(this.timeLeft % 60);
        if (displayTimeSec.length < 2) {
            displayTimeSec = '0' + displayTimeSec;
        }
        var displayTime = displayTimeMin + ':' + displayTimeSec;
        document.getElementById('gameTimer').innerText = displayTime;
        if (this.timeLeft === 0) {
            //setTimeout(this.UpdateTimer(), 1000000);
            clearInterval(this.Timer);
            console.log("stop");
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