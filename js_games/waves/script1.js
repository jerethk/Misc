// Copyright Jereth Kok 2021

// JavaScript source code
const waveBtn = document.getElementById("wavebutton");
const canvas = document.getElementById("myCanvas");
const ctx = document.getElementById("myCanvas").getContext("2d");
const wave1Wid = document.getElementById("wave1wid");
const wave1Amp = document.getElementById("wave1amp");
const wave2Wid = document.getElementById("wave2wid");
const wave2Amp = document.getElementById("wave2amp");

waveBtn.onclick = startWaves;
let phase = 0;
let w1w = 0;
let w1a = 0;
let w2w = 0;
let w2a = 0;

function startWaves() {
    waveBtn.disabled = true;
    ctx.fillStyle = "rgba(0, 0, 0, 0.5)";

    /* set limits 1 - 10 */
    if (wave1Wid.value < 1) wave1Wid.value = 1;
    if (wave1Wid.value > 10) wave1Wid.value = 10;
    if (wave1Amp.value < 1) wave1Amp.value = 1;
    if (wave1Amp.value > 10) wave1Amp.value = 10;
    if (wave2Wid.value < 1) wave2Wid.value = 1;
    if (wave2Wid.value > 10) wave2Wid.value = 10;
    if (wave2Amp.value < 1) wave2Amp.value = 1;
    if (wave2Amp.value > 10) wave2Amp.value = 10;

    /* assign width and amplitude values*/
    w1w = wave1Wid.value / 5;
    w1a = 5 * wave1Amp.value;
    w2w = wave2Wid.value / 5;
    w2a = 5 * wave2Amp.value;

    phase = 0;
    doWaves();
}

function doWaves() {
    nextFrame(phase);
    phase = phase + 0.1;
    if (phase <= 50) {
        window.requestAnimationFrame(doWaves);
    } else {
        waveBtn.disabled = false;
    }
}

function nextFrame() {
    const xOffset = 10;
    const yOffsetA = 100;
    const yOffsetB = 220;
    const yOffsetC = 440;

    let x = 0;
    let y = 0;
    let a1 = 0;
    let a2 = 0;
    let b = 0;

    ctx.fillRect(0, 0, canvas.width, canvas.height); // clear canvas

    // First wave
    ctx.strokeStyle = "rgb(0, 240, 0)";
    ctx.beginPath();
    for (let i = 0; i < canvas.width - 20; i++) {
        a1 = Math.round(i / w1w);
        b = Math.sin(((a1 / 180) * Math.PI) - phase) * w1a;
        x = i + xOffset;
        y = b + yOffsetA;
        ctx.lineTo(x, y);
    }   
    ctx.stroke();

    // Second wave
    ctx.strokeStyle = "rgb(80, 80, 255)";
    ctx.beginPath();
    for (let i = 0; i < canvas.width - 20; i++) {
        a2 = Math.round(i / w2w);
        b = Math.sin(((a2 / 180) * Math.PI) + phase) * w2a;
        x = i + xOffset;
        y = b + yOffsetB;
        ctx.lineTo(x, y);
    }
    ctx.stroke();

    
    // Combined wave 
    ctx.strokeStyle = "rgb(240, 240, 0)";
    ctx.beginPath();
    for (let i = 0; i < canvas.width - 20; i++) {
        a1 = Math.round(i / w1w);
        a2 = Math.round(i / w2w);
        b = Math.sin(((a1 / 180) * Math.PI) - phase) * w1a + Math.sin(((a2 / 180) * Math.PI) + phase) * w2a;
        x = i + xOffset;
        y = b + yOffsetC;
        ctx.lineTo(x, y);
    }
    ctx.stroke(); 
}




/* OLD WAVE CODE 
ctx.strokeStyle = "rgb(0, 240, 0)";
ctx.beginPath();
for (i = 0; i < 3000; i++) {
    a = Math.round(i * 1 / 4);
    b1 = Math.sin(((i / 180) * Math.PI) - phase);
    x = a + xOffset;
    y = (b1 * 40) + yOffsetA;
    ctx.lineTo(x, y);
}
ctx.stroke();

ctx.strokeStyle = "rgb(80, 80, 255)";
ctx.beginPath();
for (i = 0; i < 3000; i++) {
    a = Math.round(i * 1 / 4);
    b2 = Math.sin(((i / 180) * Math.PI) + phase);
    x = a + xOffset;
    y = (b2 * 40) + yOffsetB;
    ctx.lineTo(x, y);
}
ctx.stroke();

ctx.strokeStyle = "rgb(240, 240, 0)";
ctx.beginPath();
for (i = 0; i < 3000; i++) {
    a = Math.round(i * 1 / 4);
    b3 = (Math.sin(((i / 180) * Math.PI) - phase)) + (Math.sin(((i / 180) * Math.PI) + phase));
    x = a + xOffset;
    y = (b3 * 40) + yOffsetC;
    ctx.lineTo(x, y);
}
ctx.stroke();

*/ 