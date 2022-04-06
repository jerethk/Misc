// JavaScript source code

const startBtn = document.getElementById("startbutton");
const canvas = document.getElementById("mycanvas");
const ctx = canvas.getContext("2d");
const score = document.getElementById("score");

let myPaddle;
let theBall;
let gameOver = true;

startBtn.addEventListener("click", startGame);

/* Paddle constructor */
function Paddle(ypos, col, incre) {
    this.length = 80;
    this.x = (canvas.width / 2) - 40;
    this.y = ypos;
    this.colour = col;
    this.increment = incre;
    this.score = 0;

    this.draw = function () {
        ctx.fillStyle = this.colour;
        ctx.fillRect(this.x, this.y, this.length, 10);
    }

    this.moveLeft = function () {
        if (this.x >= 20) {
            this.x -= this.increment;
        }
    }
    this.moveRight = function () {
        if (this.x <= (canvas.width - this.length - 20)) {
            this.x += this.increment;
        }
    }
}

/* Ball constructor */
function Ball() {
    this.x = (canvas.width / 2);
    this.y = (canvas.height / 2);
    this.colour = "rgb(100, 100, 255)";
    this.vx = Math.random() * 2 - 1;
    this.vy = 3;

    this.draw = function () {
        ctx.fillStyle = this.colour;
        ctx.beginPath();
        ctx.arc(this.x, this.y, 8, 0, 2 * Math.PI);
        ctx.fill();
    }

    this.move = function () {
        this.x = this.x + this.vx;
        this.y = this.y + this.vy;

        /* bounce off side of screen */
        if ((this.x <= 10) || (this.x >= canvas.width - 10)) {
            this.vx = -1 * this.vx;
        }
    }

    this.paddleBounce = function(paddle) {
        this.vy = -1 * this.vy;

        // calculate vel-x of ball based on where on paddle it hits 
        let relativeXPos = this.x - paddle.x;
        this.vx = 8 * (relativeXPos / paddle.length) - 4;
    }
}

function movePaddle(e) {
    if (e.code === "ArrowLeft") {
        myPaddle.moveLeft();
    }

    if (e.code === "ArrowRight") {
        myPaddle.moveRight();
    }
}
function startGame() {
    document.addEventListener("keydown", movePaddle);
    myPaddle = new Paddle(560, "rgb(240, 0, 0)", 10);
    comPaddle = new Paddle(40, "rgb(0, 240, 0)", 2);
    theBall = new Ball();

    gameOver = false;
    startBtn.disabled = true;
    score.textContent = "Score: " + myPaddle.score + " - " + comPaddle.score;
    runGame();
}

function runGame() {
    ctx.fillStyle = "rgba(0, 0, 0, 0.5)";  // refresh canvas area
    ctx.fillRect(0, 0, canvas.width, canvas.height); 

    myPaddle.draw();
    comPaddle.draw();
    theBall.draw();
    theBall.move();

    /* com paddle chases ball */
    if ((comPaddle.x + comPaddle.length / 2) < theBall.x) { 
        comPaddle.moveRight();
    } else if ((comPaddle.x + comPaddle.length / 2) > theBall.x) {
        comPaddle.moveLeft();
    }

    /* contact with player paddle */
    if (theBall.y >= (myPaddle.y - 5) && theBall.y <= (myPaddle.y - 2)) {   // ball at Y level of my paddle
        if (theBall.x >= myPaddle.x && theBall.x <= (myPaddle.x + myPaddle.length)) {  // paddle in correct X position
            theBall.paddleBounce(myPaddle);
        }
    }

    /* contact with com paddle */
    if (theBall.y <= (comPaddle.y + 16) && theBall.y >= (comPaddle.y + 13)) {   // ball at Y level of com paddle
        if (theBall.x >= comPaddle.x && theBall.x <= (comPaddle.x + comPaddle.length)) {  // paddle in correct X position
            theBall.paddleBounce(comPaddle);
        }
    }

    if (theBall.y >= (canvas.height - 10)) { // ball at bottom of screen, player loses
        comPaddle.score += 1;
        roundOver("Too bad!");
    }

    if (theBall.y <= 10) { // ball at top of screen, player wins
        myPaddle.score += 1;
        roundOver("Nice work!");
    }

    if (!gameOver)
        window.requestAnimationFrame(runGame);
}

function roundOver(message) {
    gameOver = true;
    theBall.vx = 0;
    theBall.vy = 0;

    score.textContent = "Score: " + myPaddle.score + " - " + comPaddle.score;
    ctx.font = "64px sans-serif";
    ctx.fillStyle = "rgb(200, 120, 50)";
    ctx.fillText(message, canvas.width / 2 - 150, canvas.height / 2);

    window.setTimeout(keyToContinue, 1000);

}

function keyToContinue() {
    ctx.font = "32px sans-serif";
    ctx.fillText("Press any key to continue", canvas.width / 2 - 200, canvas.height / 2 + 80);

    document.removeEventListener("keydown", movePaddle);
    document.addEventListener("keydown", newRound);
}

function newRound() {
    myPaddle.x = (canvas.width / 2) - 40;
    comPaddle.x = (canvas.width / 2) - 40;
    theBall = new Ball();

    gameOver = false;
    startBtn.disabled = true;
    document.removeEventListener("keydown", newRound);
    document.addEventListener("keydown", movePaddle);
    runGame();
}

