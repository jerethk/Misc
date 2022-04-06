// JavaScript source code

const startBtn = document.getElementById("startbutton");
const canvas = document.getElementById("mycanvas");
const ctx = canvas.getContext("2d");
const score = document.getElementById("score");

let myPaddle;
let theBall;
let brickArray = [];
let gameOver = true;

startBtn.addEventListener("click", startGame);

/* Paddle constructor --------------------------------------------- */
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

/* Ball constructor --------------------------------------------- */
function Ball() {
    this.x = (canvas.width / 2);
    this.y = (canvas.height / 2);
    this.radius = 6;
    this.colour = "rgb(100, 100, 255)";
    this.vx = Math.random() - 0.3; //0.8;
    this.vy = 3;

    this.draw = function () {
        ctx.fillStyle = this.colour;
        ctx.beginPath();
        ctx.arc(this.x, this.y, this.radius, 0, 2 * Math.PI);
        ctx.fill();
    }

    this.move = function () {
        this.x = this.x + this.vx;
        this.y = this.y + this.vy;

        this.brickCollisionCheck();

        /* bounce off side of screen */
        if ((this.x <= 10) || (this.x >= canvas.width - 10)) {
            this.vx = -1 * this.vx;
        }

        /* bounce off top of screen */
        if (this.y <= 10) {
            this.vy = -1 * this.vy;
        }
    }
    
    this.paddleBounce = function(paddle) {
        this.vy = -1 * this.vy;

        // calculate vel-x of ball based on where on paddle it hits 
        let relativeXPos = this.x - paddle.x;
        this.vx = 6 * (relativeXPos / paddle.length) - 3;
    }

    this.brickCollisionCheck = function () {

        if (this.vy < 0) {  // ball moving up, check hit bottom of brick
            for (b = 0; b < brickArray.length; b++) {
                if ((this.y > brickArray[b].y + 12 + 2) && (this.y < brickArray[b].y + 12 + 7)) {
                    if ((this.x > brickArray[b].x - 5) && (this.x < brickArray[b].x + 56 + 5)) {

                        brickArray.splice(b, 1);
                        myPaddle.score += 1;
                        score.textContent = "Score: " + myPaddle.score;
                        this.vy = -1 * this.vy;
                        console.log(brickArray.length);
                        break;
                    }
                }
            }
        }

        if (this.vy > 0) {  // ball moving down, check hit top of brick
            for (b = 0; b < brickArray.length; b++) {
                if ((this.y < brickArray[b].y - 2) && (this.y > brickArray[b].y - 7)) {
                    if ((this.x > brickArray[b].x - 5) && (this.x < brickArray[b].x + 56 + 5)) {
                        
                        brickArray.splice(b, 1);
                        myPaddle.score += 1;
                        score.textContent = "Score: " + myPaddle.score;
                        this.vy = -1 * this.vy;
                        console.log(brickArray.length);
                        break;
                    }
                }
            }
        }

        if (this.vx > 0) {   // ball moving right, check hit left of brick
            for (b = 0; b < brickArray.length; b++) {
                if ((this.x < brickArray[b].x - 2) && (this.x > brickArray[b].x - 7)) {
                    if ((this.y > brickArray[b].y - 5) && (this.y < brickArray[b].y + 12 + 5)) {

                        brickArray.splice(b, 1);
                        myPaddle.score += 1;
                        score.textContent = "Score: " + myPaddle.score;
                        this.vx = -1 * this.vx;
                        break;
                    }
                }
            }
        }

        if (this.vx < 0) {  // ball moving left, check hit right of brick
            for (b = 0; b < brickArray.length; b++) {
                if ((this.x > brickArray[b].x + 56 + 2) && (this.x < brickArray[b].x + 56 + 7)) {
                    if ((this.y > brickArray[b].y - 5) && (this.y < brickArray[b].y + 12 + 5)) {
                        
                        brickArray.splice(b, 1);
                        myPaddle.score += 1;
                        score.textContent = "Score: " + myPaddle.score;
                        this.vx = -1 * this.vx;
                        break;
                    }
                }
            }
        }
    }
}

/* Brick constructur  --------------------------------------------- */
function Brick(x, y) {
    this.x = x;
    this.y = y;
    this.width = 56;
    this.height = 12;
    this.colour = "rgb(180, 0, 0)";
    this.borderColour = "rgb(100, 0, 0)";

    this.draw = function () {
        ctx.fillStyle = this.colour;
        ctx.strokeStyle = this.borderColour;
        ctx.lineWidth = 2;
        ctx.beginPath();
        ctx.rect(this.x, this.y, this.width, this.height);
        ctx.fill();
        ctx.stroke();
    }
}

/* ------------------------------------------------------------------------------------------ */

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
    myPaddle = new Paddle(560, "rgb(120, 200, 0)", 10);
    theBall = new Ball();
    generateBricks();

    gameOver = false;
    startBtn.disabled = true;
    score.textContent = "Score: " + myPaddle.score;
    runGame();
}

function generateBricks() {
    for (a = 0; a < 12; a++) {
        for (b = 0; b < 10; b++) {
            let newBrick = new Brick(a * 60 + 40, b * 18 + 40);
            brickArray.push(newBrick);
        }
    }

    console.log(brickArray.length);
}

function runGame() {
    ctx.fillStyle = "rgba(0, 0, 0, 0.5)";  // refresh canvas area
    ctx.fillRect(0, 0, canvas.width, canvas.height); 

    myPaddle.draw();
    theBall.draw();
    theBall.move();

    for (let b = 0; b < brickArray.length; b++) {
        brickArray[b].draw();
    }

    /* contact with player paddle */
    if (theBall.y >= (myPaddle.y - 5) && theBall.y <= (myPaddle.y - 2)) {   // ball at Y level of my paddle
        if (theBall.x >= myPaddle.x && theBall.x <= (myPaddle.x + myPaddle.length)) {  // paddle in correct X position
            theBall.paddleBounce(myPaddle);
        }
    }

    if (theBall.y >= (canvas.height - 10)) { // ball at bottom of screen, player loses
        roundOver("Bugger!");
    }

    if (!gameOver)
        window.requestAnimationFrame(runGame);
}

function roundOver(message) {
    gameOver = true;
    theBall.vx = 0;
    theBall.vy = 0;

    score.textContent = "Score: " + myPaddle.score;
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
    theBall = new Ball();

    gameOver = false;
    startBtn.disabled = true;
    document.removeEventListener("keydown", newRound);
    document.addEventListener("keydown", movePaddle);
    runGame();
}

