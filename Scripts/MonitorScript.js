newsBox = document.getElementsByClassName("newsBox");
images = document.getElementsByClassName("postImage");
currentSlide = 0;

window.onload = () => {
    for (var j = 0; j < images.length; j++) {
        if (images[j].naturalHeight > images[j].naturalWidth) {
            images[j].style.height = "40%";
        } else if (images[j].naturalHeight == images[j].naturalWidth) {
            images[j].style.height = "40vh";
        } else {
            images[j].style.height = "40vh";
        }
    }

    for (var i = 0; i < newsBox.length; i++) {
        Object.assign(newsBox[i].style, {
            left: "-100vw"
        });
    }
    newsBox[0].style.left = "50vw";
};

setInterval(() => {
    if (newsBox[currentSlide + 1] == null) {
        window.location.href = "http://infomedia.orebro.se/matsmat";
    }
    newsBox[currentSlide].style.left = "150vw";
    newsBox[currentSlide + 1].style.left = "50vw";
    ++currentSlide;
}, 10000);
