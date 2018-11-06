class UIManager {
    constructor(setMultiplier) {
        //audio
        this.volume = true;
        this.audio;
        //powerup
        this.setMultiplier = setMultiplier;
        this.powerUpTimers = [];
        this.powers = 0;
        this.pheight = 58;
        //scoreboard
        this.scores = document.getElementById("Scoreboard__Content");
        this.endScene = false;
        //menu 
        var scope = this;
        this.tabbedTab = false;
        this.activeMenu = 0;
        //menu: zocht voor keybindings en logica voor het openen van menus
        document.addEventListener('keydown', function (event) {     //https://css-tricks.com/snippets/javascript/javascript-keycodes/
            if (event.keyCode == 82) {  // r - scoreboard
                if (!scope.tabbedTab) {
                    scope.ShowScoreboard(true);
                }
            }
            if (event.keyCode == 85) {  // u - sound
                scope.SwitchSoundOnOff();
            }
            if (event.keyCode == 89) {  // y - exit
                if (scope.activeMenu == event.keyCode) {
                    scope.activeMenu = 0;
                    scope.ShowExitMenu(false);
                } else {
                    scope.activeMenu = event.keyCode;
                    scope.HideMenus();
                    scope.ShowExitMenu(true);
                }                
            }
            if (event.keyCode == 73) {  // i - options
                if (scope.activeMenu == event.keyCode) {
                    scope.activeMenu = 0;
                    scope.ShowOptionMenu(false);
                } else {
                    scope.activeMenu = event.keyCode;
                    scope.HideMenus();
                    scope.ShowOptionMenu(true);
                }   
            }
        });
        document.addEventListener('keyup', function (event) {
            if (event.keyCode == 82) {  // r - scoreboard
                if (!scope.endScene) {
                    scope.tabbedTab = false;
                    scope.ShowScoreboard(false);
                }
            }
        });
    }

    //stats: laat kicks (kills) en dropouts (deaths) rechts boven en ecs (health) en herkansingen (armour) onderin in het midden zien.
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

    //gun: laat de amunatie en de naam van het wapen dat je op het moment in de hand hebt rechts onderin zien.
    UpdateGunName(val) {
        document.getElementById("gunName").innerHTML = val + ":";
    }

    UpdateGunCurAmmo(val) {
        document.getElementById("gunCurAmmo").innerHTML = val;
    }

    UpdateGunMaxAmmo(val) {
        document.getElementById("gunMaxAmmo").innerHTML = val;
    }

    //menus: laat menus en het scoreboard zien.
    HideMenus() {
        this.ShowExitMenu(false);
        this.ShowOptionMenu(false);
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
        this.endScene = true;
        this.ShowScoreboard(true);
        document.getElementById("toggle-Scoreboard").classList.add("endBgr");
    }

    //scoreboard: update de gegevens van alle spelers in het scoreboard.
    UpdateScoreboardScore(score) {
        var row = document.getElementById(score.guid);
        if (row != null) {
            row.cells[2].innerHTML = score.kills;
            row.cells[3].innerHTML = score.deaths;
            console.log("update soreboard:");
            console.log({ updscore: score });
            this.OrderScoreboardScore();
        }
        
    }

    AddScoreboardScore(score) {
        if (document.getElementById(score.guid) == null) {
            var row = this.scores.insertRow(0);
            row.id = score.guid;
            var cell1 = row.insertCell(0);  //index
            cell1.innerHTML = "1.";
            var cell2 = row.insertCell(1);  //username
            cell2.innerHTML = score.username;
            var cell3 = row.insertCell(2);  //kills
            cell3.innerHTML = score.kills;
            var cell4 = row.insertCell(3);  //deaths
            cell4.innerHTML = score.deaths;
            console.log("add soreboard:");
            console.log({ addscore: score });
            this.OrderScoreboardScore(score);
        }
    }

    OrderScoreboardScore() {            //https://www.w3schools.com/howto/howto_js_sort_table.asp
        var rows, switching, i, x, y, shouldSwitch;
        switching = true;
        /*Make a loop that will continue until
        no switching has been done:*/
        while (switching) {
            //start by saying: no switching is done:
            switching = false;
            rows = this.scores.rows;
            /*Loop through all table rows (except the
            first, which contains table headers):*/
            for (i = 1; i < (rows.length - 1); i++) {
                //start by saying there should be no switching:
                shouldSwitch = false;
                /*Get the two elements you want to compare,
                one from current row and one from the next:*/
                x = rows[i].cells[2];
                y = rows[i + 1].cells[2];
                //check if the two rows should switch place:
                if (Number(x.innerHTML) > Number(y.innerHTML)) {
                    //if so, mark as a switch and break the loop:
                    shouldSwitch = true;
                    break;
                }
            }
            if (shouldSwitch) {
                /*If a switch has been marked, make the switch
                and mark that a switch has been done:*/
                rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
                switching = true;
            }
        }
        for (var j = 0; j < rows.length; j++) {
            rows[j].cells[0].innerHTML = String(j+1) + ".";
        }
    }

    RemoveScoreboardScore(guid) {
        if (!this.endScene) {
            var row = document.getElementById(guid);
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
    }

    //sound: zet audio uit en aan.
    SetAudio(src) {
        this.audio = src; 
    }

    SwitchSoundOnOff() {
        var newInnerText;
        if (this.volume) {
            this.volume = false;
            newInnerText = "volume_mute";
        } else {
            this.volume = true;
            newInnerText = "volume_up";
        }
        if (this.audio != null) {
            if (this.volume) {
                this.audio.play();
            } else {
                this.audio.pause();
            }
        }
        var soundSwitchButtons = document.getElementsByClassName("volume");
        for (var i = 0; i < soundSwitchButtons.length; i++) {
            soundSwitchButtons[i].innerText = newInnerText;
        }
    }

    //timer: update de gametimerdisplay met de overige game tijd boven in het midden
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

    //powerup: laat powerups zien en triggerd modifier aanpassing.
    StartPowerUp(id, name, length, value) {
        //remove empty powerdisplay:
        if (this.powers == 0) {
            var emptyElement = document.getElementById("EmptyPower");
            emptyElement.parentElement.removeChild(emptyElement);
            this.pheight -= 43;
        }

        //update powerdisplay:
        var child = document.getElementById(id);
        if (child != null) {
            var parent = child.parentNode;
            var index = Array.prototype.indexOf.call(parent.children, child);   // The equivalent of parent.children.indexOf(child)
            this.powerUpTimers[index].SetTime(length);
            console.log("update powerup");
        //add powerdisplay:
        } else {
            this.powers = this.powers + 1;
            
            var newpowerdisplay = document.createElement("div");
            newpowerdisplay.id = id;
            newpowerdisplay.classList.add("powerup__item");
            newpowerdisplay.innerHTML = "<i class='powerup__icon icon material-icons noselect fleft'>flash_on</i><div class='powerup__timer fleft'></div><label class='powerup__label fleft'></label>";
            // display values:
            var color;
            switch (id) {
                case "jmp":
                    color = "rgb(0, 160, 255)";
                    break;
                case "SpeedBoost":
                    color = "rgb(255, 255, 0)";
                    break;
                case "DamageBoost":
                    color = "rgb(255, 145, 0)";
                    break;
                default:
                    color = "rgb(0, 160, 255)";
                    break;  
            }
            newpowerdisplay.getElementsByClassName("powerup__icon")[0].style.color = color;
            newpowerdisplay.getElementsByClassName("powerup__label")[0].innerText = name;
            this.powerUpTimers.push(new displayTimer(length, newpowerdisplay.getElementsByClassName("powerup__timer")[0], this.StopPowerUp, this));
            this.pheight += 43;
            document.getElementById("powerup").style.setProperty('--part-height', this.pheight+"px");
            document.getElementById("powerup").getElementsByClassName("powerup")[0].appendChild(newpowerdisplay);
            this.setMultiplier(id, value);
            console.log("start powerup");
        }
    }

    StopPowerUp(displayElement, scope) {
        //remove powerdisplay
        var powerElement = displayElement.parentNode;
        var powersElement = powerElement.parentNode;
        var index = Array.prototype.indexOf.call(powersElement.children, powerElement);   // The equivalent of parent.children.indexOf(child)
        scope.powerUpTimers[index] = null;
        scope.powerUpTimers.splice(index, 1);
        powersElement.removeChild(powerElement);
        scope.powers = scope.powers - 1;
        if (scope.powers == 0) {
            //add empty powerdisplay:
            var emptypowerdisplay = document.createElement("div");
            emptypowerdisplay.id = "EmptyPower";
            emptypowerdisplay.classList.add("powerup__item");
            emptypowerdisplay.innerHTML = "<i class='powerup__icon icon material-icons noselect fleft'>flash_on</i>";
            document.getElementById("powerup").getElementsByClassName("powerup")[0].appendChild(emptypowerdisplay);
            scope.pheight += 43;
        }
        scope.pheight -= 43;
        document.getElementById("powerup").style.setProperty('--part-height', scope.pheight + "px");
        var power = powerElement.id;
        scope.setMultiplier(power, 1);
        console.log("stop powerup");
    }

    StopPowers() {
        //remove all powers
        while (this.powerUpTimers.length != 0) {
            this.powerUpTimers[0].StopTimer();
        }
    }
}

//display timer: timer die per seconde een display element met de overige tijd laat zien en eindig functie uitvoert.
class displayTimer {
    constructor(length, element, stopAction, scope) {
        this.timeLeft = length + 1;     // in seconds
        this.displayElement = element;
        var ref = this;
        this.runTimer = setInterval(function () {
            ref.UpdateTimer();
        }, 1000);
        this.UpdateTimer();
        this.scope = scope;
        this.stopAction = stopAction;
        return this.runTimer;
    }

    SetTime(length) {
        this.timeLeft = length + 1;
        this.UpdateTimer();
    }

    UpdateTimer() {
        this.timeLeft = this.timeLeft - 1;
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
        this.displayElement.innerText = displayTime;
        if (this.timeLeft <= 0) {

            this.StopTimer();
        }
    }

    StopTimer() {
        clearInterval(this.runTimer);
        console.log("stop timerDisplay");
        if (this.stopAction != null) {
            this.stopAction(this.displayElement, this.scope);
        } 
    }
}