
const redScoreIncrease = document.querySelector(".Plus-Red");
const redScoreDecrease = document.querySelector(".Minuse-Red");
const blueScoreIncrease = document.querySelector(".Plus-Blue");
const blueScoreDecrease = document.querySelector(".Minus-Blue");
const redScoreDisplay = document.querySelector(".Points-Red");
const blueScoreDisplaye = document.querySelector(".Points-Blue");
let blueScore = 0;
let redScore = 0;

redScoreIncrease.addEventListener('click', () => {
    redScore++;
    redScoreDisplay.textContent = redScore.toString();
})

document.getElementById("<%=RedScore.ClientID%>").value = "87";