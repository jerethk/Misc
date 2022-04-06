// Spaceship game
// Copyright Jereth Kok 2021

// JavaScript source code
const startBtn = document.getElementById("startbutton");
const canvas = document.getElementById("mycanvas");
const ctx = canvas.getContext("2d");

var starArray = [];
var asteroidArray = [];
var alienArray = [];
var laserArray = [];
var explosionArray = [];
var gameOver = true;

startBtn.onclick = startGame;

/* Hitbox */
function Hitbox(x1, x2, y1, y2, willDraw) {
    this.x1 = x1;
    this.x2 = x2;
    this.y1 = y1;
    this.y2 = y2;

    this.willDraw = willDraw;

    this.draw = function () {
        ctx.strokeStyle = "rgb(255, 0, 255)";
        ctx.beginPath();
        ctx.moveTo(this.x1, this.y1);
        ctx.lineTo(this.x1, this.y2);
        ctx.lineTo(this.x2, this.y2);
        ctx.lineTo(this.x2, this.y1);
        ctx.lineTo(this.x1, this.y1);
        ctx.stroke();
    }
}

/* Spaceship constructor ------------------------------------------------------- */
function Spaceship() {
    this.x = (canvas.width / 2) - 5;
    this.y = canvas.height - 150;
    this.vx = 0;
    this.vy = 0;
    this.increment = 2;
    this.hitbox = new Hitbox(this.x - 7, this.x + 7, this.y - 8, this.y + 12, false);
    this.score = 0;
    this.timeKilled = null;

    this.draw = function () {
        ctx.fillStyle = "rgb(0, 0, 240)";
        ctx.strokeStyle = "rgb(200, 200,100)";
        //ctx.lineWidth = 2;
        ctx.beginPath();
        ctx.moveTo(this.x + 2, this.y - 15);
        ctx.lineTo(this.x + 10, this.y + 15);
        ctx.lineTo(this.x - 10, this.y + 15);
        ctx.lineTo(this.x - 2, this.y - 15);
        ctx.closePath();
        ctx.fill();
        ctx.stroke();

        if (this.hitbox.willDraw) this.hitbox.draw();
    }

    this.move = function () {
        this.x = this.x + this.vx;
        this.y = this.y + this.vy;

        // move hitbox
        this.hitbox.x1 += this.vx;
        this.hitbox.x2 += this.vx;
        this.hitbox.y1 += this.vy;
        this.hitbox.y2 += this.vy;

        // hit horizontal boundary 
        if ((this.x >= canvas.width - 30) || (this.x <= 30)) {
            this.vx = 0;
        }

        // hit vertical boundary 
        if ((this.y >= canvas.height - 50) || (this.y <= canvas.height - 300)) {
            this.vy = 0;
        }
    }

    this.moveRight = function () {
        if ((this.vx <= 0) && (this.x < canvas.width - 30)) {
            this.vx = this.increment;
            this.vy = 0;
        } else {
            this.vx = 0;
        }
    }

    this.moveLeft = function () {
        if ((this.vx >= 0) && (this.x > 30)) {
            this.vx = -1 * this.increment;
            this.vy = 0;
        } else {
            this.vx = 0;
        }
    }

    this.moveUp = function () {
        if ((this.vy >= 0) && (this.y > canvas.height - 300)) {
            this.vy = -1 * this.increment;
            this.vx = 0;
        } else {
            this.vy = 0;
        }
    }

    this.moveDown = function () {
        if ((this.vy <= 0) && (this.y < canvas.height - 50)) {
            this.vy = this.increment;
            this.vx = 0;
        } else {
            this.vy = 0;
        }
    }

    this.collisionCheck = function (otherHitbox) {
        let result = false;

        if (this.hitbox.x2 > otherHitbox.x1 && this.hitbox.x1 < otherHitbox.x2) {
            if (this.hitbox.y2 > otherHitbox.y1 && this.hitbox.y1 < otherHitbox.y2) {
                result = true;
            }
        }

        return result;
    }
}

/* Star constructor --------------------------------------------------------------*/
// these are just for background
function Star(ypos) {
    this.x = Math.round(Math.random() * (canvas.width - 20) + 10);
    this.y = ypos;
    this.vel = Math.random() * 0.5 + 0.5;
    this.brightness = Math.round(Math.random() * 145) + 100;
    this.colour = "rgb(" + this.brightness + "," + this.brightness + "," + this.brightness + ")";

    this.drift = function () {
        this.y += this.vel;
    }

    this.draw = function () {
        ctx.fillStyle = this.colour;
        ctx.beginPath();
        ctx.arc(this.x, this.y, 1, 0, 2*Math.PI);
        ctx.fill();
    }
}

/* Star controller */
function controlStars() {
    let i = 0;

    while (i < starArray.length) {
        starArray[i].draw();
        starArray[i].drift();

        if (starArray[i].y >= canvas.height) { // remove star when it has reached the end of the screen
            starArray.splice(i, 1);
        }
        i++;
    }
}

/* Asteroid constructor --------------------------------------------------------------*/
function Asteroid() {
    this.x = Math.round(Math.random() * (canvas.width - 160) + 80);
    this.y = -30;
    this.vel = 1.5;
    this.sizeX = Math.round(Math.random() * 20) + 25;
    this.sizeY = Math.round(Math.random() * 20) + 20;
    this.rot = (Math.random() * 50 - 25) / 180 * 3.14;
    this.brightness = Math.round(Math.random() * 7) + 3;
    this.colour = "rgb(" + (this.brightness * 20) + "," + (this.brightness * 12) + "," + (this.brightness * 4) + ")";
    this.hitbox = new Hitbox(this.x - this.sizeX + 4, this.x + this.sizeX - 4, this.y - this.sizeY + 4, this.y + this.sizeY - 4, false);

    this.drift = function () {
        this.y += this.vel;

        this.hitbox.y1 += this.vel;
        this.hitbox.y2 += this.vel;
    }

    this.draw = function () {
        ctx.fillStyle = this.colour;
        ctx.beginPath();
        ctx.ellipse(this.x, this.y, this.sizeX, this.sizeY, this.rot, 0, 2 * Math.PI);
        ctx.fill();

        if (this.hitbox.willDraw) this.hitbox.draw();
    }
}

/* Asteroid controller */
function controlAsteroids() {
    let i = 0;

    while (i < asteroidArray.length) {
        asteroidArray[i].draw();
        asteroidArray[i].drift();

        if (asteroidArray[i].y >= canvas.height + 30) { // remove asteroid when it has reached the end of the screen
            asteroidArray.splice(i, 1);
        }
        i++;
    }
}

/* Alien constructor --------------------------------------------------------------*/
function Alien() {
    this.x = Math.round(Math.random() * (canvas.width - 80) + 40);
    this.y = -20;
    this.vel = 1.5;
    this.xvel = 0.5;
    this.img = new Image();
    this.img.src = "alien1.png";
    this.hitbox = new Hitbox(this.x - 12, this.x + 12, this.y - 12, this.y + 12, false);

    this.drift = function () {
        this.y += this.vel;
        this.hitbox.y1 += this.vel;
        this.hitbox.y2 += this.vel;

        if (this.x > 20 && this.x < canvas.width - 20) {
            this.x += this.xvel;
            this.hitbox.x1 += this.xvel;
            this.hitbox.x2 += this.xvel;
        }
    }

    this.draw = function () {
        ctx.drawImage(this.img, this.x - 15, this.y - 15);
        if (this.hitbox.willDraw) this.hitbox.draw();
    }
}

/* Alien controller */
function controlAliens() {
    let i = 0;

    while (i < alienArray.length) {
        alienArray[i].draw();
        alienArray[i].drift();

        if (Math.random() > 0.99) {      // randomly change x-direction
            alienArray[i].xvel = -1 * alienArray[i].xvel;
        }

        if (alienArray[i].y >= canvas.height + 30) { // remove alien when it has reached the end of the screen
            alienArray.splice(i, 1);
        }

        // check for collision with laser, remove if true
        for (let las = 0; las < laserArray.length; las++) {
            if (alienArray.length > 0) {
                if (laserArray[las].x > alienArray[i].hitbox.x1 && laserArray[las].x < alienArray[i].hitbox.x2) {
                    if (laserArray[las].y > alienArray[i].hitbox.y1 && laserArray[las].y < alienArray[i].hitbox.y2) {
                        explosionArray.push(new Explosion(alienArray[i].x, alienArray[i].y, 800));
                        alienArray.splice(i, 1);
                        myShip.score += 1;
                        //console.log("Remaining aliens: " + alienArray.length);
                        break;
                    }
                }
            }

        }
        i++;
    }
}

/* Laser constructor --------------------------------------------------------------*/
function Laserbolt() {
    this.x = myShip.x - 1;
    this.y = myShip.y - 35;
    this.speed = 10;

    this.draw = function () {
        ctx.fillStyle = "rgb(250, 0, 0)";
        ctx.fillRect(this.x, this.y, 3, 18);
    }

    this.move = function () {
        this.y -= this.speed;
    }
}

function controlLaserbolts() {
    let i = 0;

    while (i < laserArray.length) {
        laserArray[i].draw();
        laserArray[i].move();

        if (laserArray[i].y <= -15) { // remove laser when it has reached the top of the screen
            laserArray.splice(i, 1);
        }

        // check for collision with asteroid, remove if true
        for (let a = 0; a < asteroidArray.length; a++) {
            if (laserArray.length > 0) {    // this condition is necessary in case the final laserbolt was removed, emptying the laser array
                if (laserArray[i].y < asteroidArray[a].hitbox.y2 - 5 && laserArray[i].y >= asteroidArray[a].hitbox.y2 - 20) {
                    if (laserArray[i].x > asteroidArray[a].hitbox.x1 && laserArray[i].x < asteroidArray[a].hitbox.x2) {
                        laserArray.splice(i, 1);
                        console.log("number of lasers = " + laserArray.length);
                        break;
                    }
                }
            }
        }

        i++;
    }
}

/* Explosion */
function Explosion(x, y, duration) {
    this.x = x;
    this.y = y;
    this.start = Date.now();
    this.duration = duration;
    this.expImg1 = new Image();
    this.expImg1.src = "explosion1.png";
    this.expImg2 = new Image();
    this.expImg2.src = "explosion2.png";

    this.draw1 = function () {
        ctx.drawImage(this.expImg1, this.x - 20, this.y - 20);
    }

    this.draw2 = function () {
        ctx.drawImage(this.expImg2, this.x - 20, this.y - 20);
    }
}

function controlExplosions() {
    let i = 0;
    while (i < explosionArray.length) {
        let expDuration = Date.now() - explosionArray[i].start;

        if (expDuration % 200 < 100) {
            explosionArray[i].draw1();
        } else {
            explosionArray[i].draw2();
        }

        if (expDuration > explosionArray[i].duration) {   
            explosionArray.splice(i, 1);
        }

        i++;
    }
}

/* -------------------------------------------------------------------------*/
function startGame() {
    console.log("Game started");
    myShip = new Spaceship();
    startBtn.disabled = true;
    gameOver = false;

    // generate a field of 100 stars
    for (let i = 0; i < 100; i++) {
        starArray.push(new Star(Math.round(Math.random() * canvas.height)));
    }

    document.addEventListener("keydown", controlShip);
    runGame();
}

function controlShip(e) {
    //console.log(e.code);

    if (e.code === "ArrowLeft") {
        myShip.moveLeft();
    }

    if (e.code === "ArrowRight") {
        myShip.moveRight();
    }

    if (e.code === "ArrowUp") {
        myShip.moveUp();
    }

    if (e.code === "ArrowDown") {
        myShip.moveDown();
    }

    if (e.code === "Space") {
        if (laserArray.length < 5) {    // max 5 laser bolts at a time
            laserArray.push(new Laserbolt());
            //console.log("number of lasers: " + laserArray.length);
        }
    }
}

function runGame() {
    ctx.fillStyle = "rgba(0, 0, 0, 0.5)";
    ctx.fillRect(0, 0, canvas.width, canvas.height);

    let n = Math.random() * 100;    // random number
    if (n > 90) {
        starArray.push(new Star(0));
        //console.log("number of stars: " + starArray.length);
    }

    if (n > 99.2) { // add asteroid
        asteroidArray.push(new Asteroid());
        //console.log("number of asteroids: " + asteroidArray.length);
    }

    if ((n > 49.5 && n < 50) && (alienArray.length < 8)) { // add alien, max 8 at a time
        alienArray.push(new Alien());
        //console.log("number of aliens: " + alienArray.length);
    }

    controlStars();
    controlAsteroids();
    controlAliens();
    controlLaserbolts();
    controlExplosions();
    myShip.draw();
    myShip.move();

    ctx.font = "16px sans-serif";
    ctx.fillStyle = "rgb(240, 240, 80)";
    ctx.fillText("Aliens destroyed: " + myShip.score, 20, canvas.height - 20);

    // test for collision
    for (let i = 0; i < asteroidArray.length; i++) {
        if (myShip.collisionCheck(asteroidArray[i].hitbox)) {
            shipCrash();
        }
    }

    for (let i = 0; i < alienArray.length; i++) {
        if (myShip.collisionCheck(alienArray[i].hitbox)) {
            shipCrash();
        }
    }

    if (!gameOver) window.requestAnimationFrame(runGame);
}

function shipCrash() {
    gameOver = true;
    myShip.timeKilled = Date.now();
    explosionArray.push(new Explosion(myShip.x, myShip.y, 2000));
    document.removeEventListener("keydown", controlShip);

    endGame();
}

function endGame() {
    ctx.fillStyle = "rgba(0, 0, 0, 0.5)";
    ctx.fillRect(0, 0, canvas.width, canvas.height);
    
    controlStars();
    controlAsteroids();
    controlAliens();
    controlLaserbolts();
    controlExplosions();
    
    ctx.font = "16px sans-serif";
    ctx.fillStyle = "rgb(240, 240, 80)";
    ctx.fillText("Aliens destroyed: " + myShip.score, 20, canvas.height - 20);

    if (Date.now() - myShip.timeKilled < 2000) {
        window.requestAnimationFrame(endGame);
    } else {
        ctx.font = "48px sans-serif";
        ctx.fillStyle = "rgb(240, 240, 80)";
        ctx.fillText("You died!", canvas.width / 2 - 100, 50);
        startBtn.disabled = false;

        // clear everything
        asteroidArray.splice(0, asteroidArray.length);
        alienArray.splice(0, alienArray.length);
        laserArray.splice(0, laserArray.length);
        starArray.splice(0, starArray.length);
    }
}