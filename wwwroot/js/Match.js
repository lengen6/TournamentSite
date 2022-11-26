
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
    document.getElementById("scoreRedOne").value = redScore;
    document.getElementById("scoreRedTwo").value = redScore;
    document.getElementById("scoreRedThree").value = redScore;
    document.getElementById("scoreRedFour").value = redScore;
})


