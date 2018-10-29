class UIManager {
    constructor(x) {
        this.volume = true;
        this.openMenu = "";

        this.timeLeft = 0;  //in seconds
        this.isRunning = false;
        this.runTimer;
        this.StartTimer(360);
        this.UpdateGameScores(10, 10);
    }

    UpdateGameScores(K, D) {
        this.UpdateGameScore(K, "gameScoreK", "K: ");
        this.UpdateGameScore(D, "gameScoreD", "D: ");
    }
    UpdateGameScore(Val, Type, Prompt) {
        Val = String(Val);
        if (Val.length < 2) {
            Val = "0" + Val;
        }
        document.getElementById(Type).innerText = Prompt + Val;
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
        this.StopTimer();
        this.timeLeft = time;
        var cls = this;
        this.runTimer = setInterval(function () {
            cls.UpdateTimer(cls);
        }, 1000);
        this.isRunning = true;
        console.log("start");
    }

    UpdateTimer(cls) {
        cls.timeLeft = cls.timeLeft - 1;
        var displayTimeMin = Math.floor(cls.timeLeft / 60);
        displayTimeMin = displayTimeMin.toString();
        if (displayTimeMin.length < 2) {
            displayTimeMin = '0' + displayTimeMin;
        }
        var displayTimeSec = String(cls.timeLeft % 60);
        if (displayTimeSec.length < 2) {
            displayTimeSec = '0' + displayTimeSec;
        }
        var displayTime = displayTimeMin + ':' + displayTimeSec;
        document.getElementById('gameTimer').innerText = displayTime;
        if (cls.timeLeft <= 0) {
            
            cls.StopTimer();
        }
    }

    StopTimer() {
        if (this.isRunning) {
            this.isRunning = false;
            clearInterval(this.runTimer);
            console.log("stop");
        }
    }

    /*
    Show(x, b) {
        if (b) {
            x.classList.remove("hiddenEffect");
            x.classList.add("showEffect");
        } else {
            x.classList.remove("showEffect");
            x.classList.add("hiddenEffect");
        }
        
    }*/
}