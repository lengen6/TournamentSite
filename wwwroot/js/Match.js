
const redScoreIncrease = document.querySelector(".Plus-Red");
const redScoreDecrease = document.querySelector(".Minus-Red");
const blueScoreIncrease = document.querySelector(".Plus-Blue");
const blueScoreDecrease = document.querySelector(".Minus-Blue");
const redScoreDisplay = document.querySelector(".Points-Red");
const blueScoreDisplay = document.querySelector(".Points-Blue");
const start = document.getElementById('start');
const reset = document.getElementById('reset');
const minute = document.getElementById("minute");
const second = document.getElementById("second");
const pause = document.getElementById("pause");
const play = document.getElementById("play");
const sendButtons = document.querySelectorAll("send");
let blueScore = 0;
let redScore = 0;
let isPaused = false;

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

start.addEventListener('click', function startFunction () {
   
    function startInterval() {
        startTimer = setInterval(function () {
            if (!isPaused) {
                timer();
            }

            if (minute.value == 0 && second.value == 0) {
                stopInterval();
                isPaused = false;
            }

            leadingZeros(minute);
            leadingZeros(second);

        }, 1000);

        //Start Minutes
        document.getElementById("startMinutesOne").value = minute.value;
        document.getElementById("startMinutesTwo").value = minute.value;
        document.getElementById("startMinutesThree").value = minute.value;
        document.getElementById("startMinutesFour").value = minute.value;
        document.getElementById("startMinutesFive").value = minute.value;
        document.getElementById("startMinutesSix").value = minute.value;
        document.getElementById("startMinutesSeven").value = minute.value;
        document.getElementById("startMinutesEight").value = minute.value;

        //Start Seconds
        document.getElementById("startSecondsOne").value = second.value;
        document.getElementById("startSecondsTwo").value = second.value;
        document.getElementById("startSecondsThree").value = second.value;
        document.getElementById("startSecondsFour").value = second.value;
        document.getElementById("startSecondsFive").value = second.value;
        document.getElementById("startSecondsSix").value = second.value;
        document.getElementById("startSecondsSeven").value = second.value;
        document.getElementById("startSecondsEight").value = second.value;
    }   
    startInterval();
    start.setAttribute('disabled', '');
})

reset.addEventListener('click', function () {
    minute.value = 0;
    second.value = 0;
    stopInterval()
    isPaused = false;
    start.removeAttribute('disabled');
    
})

function timer() {
    if (minute.value == 0 && second.value == 0) {
        minute.value = 0;
        second.value = 0;
    } else if (second.value != 0) {
        second.value--;
    } else if (minute.value != 0 && second.value == 0) {
        second.value = 59;
        minute.value--;
    }

    //End Minutes
    document.getElementById("endMinutesOne").value = minute.value;
    document.getElementById("endMinutesTwo").value = minute.value;
    document.getElementById("endMinutesThree").value = minute.value;
    document.getElementById("endMinutesFour").value = minute.value;
    document.getElementById("endMinutesFive").value = minute.value;
    document.getElementById("endMinutesSix").value = minute.value;
    document.getElementById("endMinutesSeven").value = minute.value;
    document.getElementById("endMinutesEight").value = minute.value;

    //End Seconds            
    document.getElementById("endSecondsOne").value = second.value;
    document.getElementById("endSecondsTwo").value = second.value;
    document.getElementById("endSecondsThree").value = second.value;
    document.getElementById("endSecondsFour").value = second.value;
    document.getElementById("endSecondsFive").value = second.value;
    document.getElementById("endSecondsSix").value = second.value;
    document.getElementById("endSecondsSeven").value = second.value;
    document.getElementById("endSecondsEight").value = second.value;

    return;
}

function stopInterval() {
    clearInterval(startTimer);
}

pause.addEventListener('click', function(){
    isPaused = true;
})

play.addEventListener('click', function () {
    isPaused = false;
})

function leadingZeros(input) {
    if (!isNaN(input.value) && input.value.length === 1) {
        input.value = '0' + input.value;
    }
}



