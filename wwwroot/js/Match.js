﻿
const redScoreIncrease = document.querySelector(".Plus-Red");
const redScoreDecrease = document.querySelector(".Minus-Red");
const blueScoreIncrease = document.querySelector(".Plus-Blue");
const blueScoreDecrease = document.querySelector(".Minus-Blue");
const redScoreDisplay = document.querySelector(".Points-Red");
const blueScoreDisplay = document.querySelector(".Points-Blue");
const sendButtons = document.querySelectorAll(".send");
let blueScore = 0;
let redScore = 0;

//No idea why I can't select based of a class name but it doesn't work unless selected by ID

redScoreIncrease.addEventListener('click', () => {

    redScore++;
    redScoreDisplay.textContent = redScore.toString();

    document.getElementById("scoreRedOne").value = redScore;
    document.getElementById("scoreRedTwo").value = redScore;
    document.getElementById("scoreRedThree").value = redScore;
    document.getElementById("scoreRedFour").value = redScore;
    document.getElementById("scoreRedFive").value = redScore;
    document.getElementById("scoreRedSix").value = redScore;
    document.getElementById("scoreRedSeven").value = redScore;
    document.getElementById("scoreRedEight").value = redScore;
})

redScoreDecrease.addEventListener('click', () => {

    if (redScore > 0) {
        redScore--;
        redScoreDisplay.textContent = redScore.toString();
    }

    document.getElementById("scoreRedOne").value = redScore;
    document.getElementById("scoreRedTwo").value = redScore;
    document.getElementById("scoreRedThree").value = redScore;
    document.getElementById("scoreRedFour").value = redScore;
    document.getElementById("scoreRedFive").value = redScore;
    document.getElementById("scoreRedSix").value = redScore;
    document.getElementById("scoreRedSeven").value = redScore;
    document.getElementById("scoreRedEight").value = redScore;
})

blueScoreIncrease.addEventListener('click', () => {

    blueScore++;
    blueScoreDisplay.textContent = blueScore.toString();

    document.getElementById("scoreBlueOne").value = blueScore;
    document.getElementById("scoreBlueTwo").value = blueScore;
    document.getElementById("scoreBlueThree").value = blueScore;
    document.getElementById("scoreBlueFour").value = blueScore;
    document.getElementById("scoreBlueFive").value = blueScore;
    document.getElementById("scoreBlueSix").value = blueScore;
    document.getElementById("scoreBlueSeven").value = blueScore;
    document.getElementById("scoreBlueEight").value = blueScore;
})

blueScoreDecrease.addEventListener('click', () => {

    if (blueScore > 0) {
        blueScore--;
        blueScoreDisplay.textContent = blueScore.toString();
    }

    document.getElementById("scoreBlueOne").value = blueScore;
    document.getElementById("scoreBlueTwo").value = blueScore;
    document.getElementById("scoreBlueThree").value = blueScore;
    document.getElementById("scoreBlueFour").value = blueScore;
    document.getElementById("scoreBlueFive").value = blueScore;
    document.getElementById("scoreBlueSix").value = blueScore;
    document.getElementById("scoreBlueSeven").value = blueScore;
    document.getElementById("scoreBlueEight").value = blueScore;
})




