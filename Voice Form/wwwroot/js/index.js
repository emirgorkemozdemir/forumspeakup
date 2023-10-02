//topicCreate 

const konuBtn = document.getElementById("konuBtn");
const create = document.querySelector(".topicCreate");
const record = document.querySelector(".record");
const pause = document.querySelector(".pause");
const stop = document.querySelector(".stop");

const animationRecord = document.querySelector(".record-button");

konuBtn.addEventListener("click", () => {
  create.classList.remove("none");
});

record.addEventListener("click", () => {
  animationRecord.classList.remove("none");
});

pause.addEventListener("click", () => {
  animationRecord.classList.add("none");
});



//topicCreate end


let calcScrollValue = () => {
  let scrollProgress = document.getElementById("progress");
  let progressValue = document.getElementById("progress-value");
  let pos = document.documentElement.scrollTop;
  let calcHeight =
    document.documentElement.scrollHeight -
    document.documentElement.clientHeight;
  let scrollValue = Math.round((pos * 100) / calcHeight);
  if (pos > 100) {
    scrollProgress.style.display = "grid";
  } else {
    scrollProgress.style.display = "none";
  }
  scrollProgress.addEventListener("click", () => {
    document.documentElement.scrollTop = 0;
  });
  scrollProgress.style.background = `conic-gradient(#03cc65 ${scrollValue}%, #001a2e ${scrollValue}%)`;
};

window.onscroll = calcScrollValue;
window.onload = calcScrollValue;


const topicTitle = document.getElementById("topicTitle");
topicTitle.style.display = "none";


konuBtn.addEventListener("click", () => {
  topicTitle.style.display = "block";
});

// konuMobile 

const konuIcon = document.getElementById("konuli");

konuIcon.addEventListener("click", () => {
  create.classList.remove("none");
  topicTitle.style.display = "block";


});




// konuMobile end