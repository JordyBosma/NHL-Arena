﻿class UIManager {
    constructor(x) {
        this.volume = true;
        this.openMenu = "";

        this.timeLeft = 0;  //in seconds
        this.isRunning = false;
        this.runTimer;
        //this.StartTimer(360);
        //this.UpdateGameScores(8, 14);
        //this.UpdatePlayerArmor(70);
        //this.UpdatePlayerHealth(60);
        this.scores = document.getElementById("Scoreboard__Content");
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

    UpdatePlayerArmor(val) {
        document.getElementById("bar--herkansing").style.width = val + "%";
    }
    UpdatePlayerHealth(val) {
        document.getElementById("bar--ecs").style.width = val + "%";
    }

    ShowOptionMenu(b) {
        document.getElementById("toggle-OptionMenu").checked = b;
    }

    ShowExitMenu(b) {
        document.getElementById("toggle-ExitMenu").checked = b;
    }

    ShowScoreboard(b) {
        document.getElementById("toggle-Scoreboard").checked = b;
    }

    EndScene() {
        this.ShowScoreboard(true);
        document.getElementById("toggle-Scoreboard").classList.add("endBgr");
    }
    
    UpdateScoreboardScore(score) {
        /*score.guid;
        score.kills;
        score.deaths;*/
        var row = this.scores.getElementById(score.guid);
        if (row != null) {
            row.cells[2].innerHTML = score.kills;
            row.cells[3].innerHTML = score.deaths;
            console.log("update soreboard:");
            console.log({ updscore: score });
            this.OrderScoreboardScore();
        }
        
    }

    AddScoreboardScore(score) {
        if (this.scores.getElementById(score.guid) == null) {
            var row = this.scores.insertRow(0);
            row.id = score.guid;
            var cell1 = row.insertCell(0);//index
            cell1.innerHTML = "1.";
            var cell2 = row.insertCell(1);//username
            cell2.innerHTML = score.username;
            var cell3 = row.insertCell(2);//kills
            cell3.innerHTML = score.kills;
            var cell4 = row.insertCell(3);//deaths
            cell4.innerHTML = score.deaths;
            console.log("add soreboard:");
            console.log({ addscore: score });
            this.OrderScoreboardScore(score);
        }
    }

    OrderScoreboardScore() {

    }

    RemoveScoreboardScore(guid) {
        var row = this.scores.getElementById(guid);
        if (row != null) {
            for (var i = 0; i < this.scores.rows.length; i++) {
                if (this.scores.rows[i] == row) {
                    this.scores.deleteRow(i);
                    break;
                }
            }
            this.OrderScoreboardScore();
        }
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

    UpdateTimerDisplay(timeLeft) {
        var displayTimeMin = Math.floor(timeLeft / 60);
        displayTimeMin = displayTimeMin.toString();
        if (displayTimeMin.length < 2) {
            displayTimeMin = '0' + displayTimeMin;
        }
        var displayTimeSec = String(timeLeft % 60);
        if (displayTimeSec.length < 2) {
            displayTimeSec = '0' + displayTimeSec;
        }
        var displayTime = displayTimeMin + ':' + displayTimeSec;
        document.getElementById('gameTimer').innerText = displayTime;
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

    
}