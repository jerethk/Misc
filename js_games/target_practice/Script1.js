// Target shooting game
// By Jereth Kok

// JavaScript source code
const startBtn = document.getElementById("startbutton");
const canvas = document.getElementById("mycanvas");
const ctx = canvas.getContext("2d");

const gameDuration = 20;  // time in seconds

let bg = new Image();
//bg.src = "background.png";

let baddies = [];
baddieImg = new Image();
baddieImg.src = "target.png";

startBtn.onclick = startGame;

let player = {
    score: 0
};


function Baddie() {
    this.x = Math.random() * (canvas.width - 80) + 40;
    this.y = Math.random() * (canvas.height - 80) + 40;
    this.startTime = Date.now();
    this.stayTime = Math.random() * 1000 + 2000;    // will stay for between 2 and 3 seconds

    this.draw = function () {
        ctx.drawImage(baddieImg, this.x, this.y);
    }

}

function startGame() {
    player.score = 0;
    startBtn.disabled = true;
    canvas.addEventListener("click", shoot);

    runGame(Date.now() + (gameDuration * 1000));
}

function runGame(finishTime) {
    let nowTime = Date.now();

    if (nowTime < finishTime) {
        //ctx.drawImage(bg, 0, 0);
        ctx.fillStyle = "rgba(0, 0, 0, 1)";
        ctx.fillRect(0, 0, canvas.width, canvas.height);  // clear screen between frames

        // Add baddies at random interval
        let rand = 100 * Math.random();
        if (rand > 97 && baddies.length < 3) {
            baddies.push(new Baddie());
        }

        // Remove baddies when their time has expired
        if (baddies !== null) {
            baddies.forEach(function (n, index) { baddies[index].draw() });

            /*for (let i = 0; i < baddies.length; i++) {
                baddies[i].draw();
            }*/

            for (let i = 0; i < baddies.length; i++) {
                if (nowTime - baddies[i].startTime > baddies[i].stayTime) {
                    baddies.splice(i, 1);
                }
            }
        }

        // Display score
        ctx.fillStyle = "rgb(40, 200, 120)";
        ctx.font = "20px sans-serif";
        ctx.fillText(`Score: ${player.score}`, 20, canvas.height - 25);
        window.requestAnimationFrame(() => runGame(finishTime));
    }
    else {
        // Game is finished
        gameFinished();
    }
}

function shoot(e) {
    console.log(`${e.offsetX} , ${e.offsetY}`);

    for (let i = 0; i < baddies.length; i++) {
        if ((e.offsetX > baddies[i].x + 5) && (e.offsetX < baddies[i].x + 35)) {
            if ((e.offsetY > baddies[i].y + 5) && (e.offsetY < baddies[i].y + 35)) {
                baddies.splice(i, 1);   // killed one!
                player.score += 1;
            }
        }
            
    }
}

function gameFinished() {
    ctx.fillStyle = "rgb(40, 200, 120)";
    ctx.font = "40px sans-serif";
    ctx.fillText(`Finished!`, (canvas.width / 2) - 75, (canvas.height / 2) - 25);

    window.setTimeout(function () { startBtn.disabled = false; }, 1500);
}
