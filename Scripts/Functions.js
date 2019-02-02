menuIcon = document.getElementById("hamMenuIcon");
container = document.getElementById("container");
menu = document.getElementById("menu");
header = document.getElementsByTagName("header");

textArea = document.getElementsByName("FeedbackText")[0];
CurrentWordCount = document.getElementById("CurrentWordCount");

AnonLabel = document.getElementById("AnonLabel");

checkBoxes = document.getElementsByClassName("checkbox");

window.onload = () => {
    checkBoxes[1].style.display = "none";
};


window.addEventListener("click", (event) => {
    if (menuIcon.contains(event.target)) {
        //Requiered
    } else if (container.contains(event.target)) {
        menu.classList.remove("open");
    }
});

function toggleMenu() {
    if (menu.classList.contains("open")) {
        menu.classList.remove("open");
    } else {
        menu.classList.add("open");

    }
}

function verifyText() {
    var charLimit = 250;
    if (textArea.value.length > charLimit) {
        textArea.value = textArea.value.slice(0, 250);
    }
    CurrentWordCount.innerHTML = textArea.value.length + "/" + charLimit;
}

function toggleAnon(x) {
    AnonLabel.style.backgroundColor = x.checked ? "#282D43" : "white";
    AnonLabel.style.color = x.checked ? "white" : "#282D43";
    Author = document.getElementById("Author");

    if (x.checked) {
        checkBoxes[1].style.display = "block";
        checkBoxes[0].style.display = "none";
        Author.style.display = "none";
    } else {
        checkBoxes[1].style.display = "none";
        checkBoxes[0].style.display = "block";
        Author.style.display = "block";
    }
}

var loadFile = function (event) {
    var output = document.getElementById('output');
    output.src = URL.createObjectURL(event.target.files[0]);
};