
const redScoreIncrease = document.querySelector(".Plus-Red");
const redScoreDecrease = document.querySelector(".Minuse-Red");
const blueScoreIncrease = document.querySelector(".Plus-Blue");
const blueScoreDecrease = document.querySelector(".Minus-Blue");
const redScoreDisplay = document.querySelector(".Points-Red");
const blueScoreDisplaye = document.querySelector(".Points-Blue");
const sendButtons = document.querySelectorAll(".send");
let blueScore = 7;
let redScore = 0;

redScoreIncrease.addEventListener('click', () => {
    redScore++;
    redScoreDisplay.textContent = redScore.toString();
    document.getElementById("scoreRed").value = "My value";
})

let obj = {
    RedScore: redScore,
    BlueScore: blueScore
};

