// JavaScript source code
const addBtn = document.getElementById("thebutton");
const remBtn = document.getElementById("rembutton");
const startBtn = document.getElementById("startbutton");
const stopBtn = document.getElementById("stopbutton");
const numBalls = document.getElementById("numballs");
const grav = document.getElementById("grav");

const canvas = document.getElementById("myCanvas");
const ctx = document.getElementById("myCanvas").getContext("2d");

let running = false;
let gravity = 0;

addBtn.onclick = addNewBall; 
remBtn.onclick = removeBall;
startBtn.onclick = startRunning;
stopBtn.onclick = stopRunning;
grav.onchange = changeGrav;

/* Ball constructor */
function Ball() {
    this.x = 20 + Math.round(Math.random() * 200);
    this.y = 20 + Math.round(Math.random() * 400);
    this.dx = (Math.random() * 4);
    this.dy = (Math.random() * 4);
    this.size = Math.round(Math.random() * 20) + 5;

    let r = Math.round(Math.random() * 235) + 20; // avoid dark colours < 20
    let g = Math.round(Math.random() * 235) + 20;
    let b = Math.round(Math.random() * 235) + 20;
    this.col = "rgb(" + r + "," + b + "," + g + ")";
    console.log(this.col);

    this.draw = function () {
        ctx.fillStyle = this.col;
        ctx.beginPath();
        ctx.arc(this.x, this.y, this.size, 0, (2 * Math.PI));
        ctx.fill();
    }

    this.move = function () {
        this.x = this.x + this.dx;
        this.y = this.y + this.dy;
        this.dy = this.dy + gravity / 20;

        /* hit a horizontal boundary */
        if ((this.x <= 10) || (this.x >= canvas.width - 10)) {
            this.dx = -1 * this.dx;
        }

        /* hit a vertical boundary */
        if ((this.y <= 10) || (this.y >= canvas.height - 10)) {
            this.dy = -1 * this.dy;
        }
    }
}

let firstBall = new Ball();
let ballCollection = [firstBall];

function addNewBall() {
    if (ballCollection.length < 100) {
        newBall = new Ball();
        ballCollection.push(newBall);
    }

    numBalls.textContent = ballCollection.length;
}

function removeBall() {
    if (ballCollection.length > 1) {
        ballCollection.pop();
    } 

    numBalls.textContent = ballCollection.length;
}

function startRunning() {
    if (!running) {
        running = true;
        animate();
    }
}

function stopRunning() {
    running = false;
}

function changeGrav() {
    if (grav.value % 1 !== 0) {
        grav.value = Math.round(grav.value);
    }

    gravity = grav.value;
    console.log("gravity = " + gravity);
}


function animate() {
    ctx.fillStyle = "rgba(0, 0, 0, 0.2)";
    ctx.fillRect(0, 0, canvas.width, canvas.height);  // clear screen between frames

    for (let i = 0; i < ballCollection.length; i++) { // draw balls, then move to next position
        ballCollection[i].draw();
        ballCollection[i].move();
    }

    if (running) window.requestAnimationFrame(animate);

}