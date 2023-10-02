//forgot 

let forgot = document.getElementById("forgot");
let box = document.querySelector(".box");
let closeWindow = document.getElementById("forgotClose");
let card = document.querySelector(".card");

forgot.addEventListener("click", (event) => {
  card.classList.remove("none");
  event.preventDefault();


});

closeWindow.addEventListener("click", () => {
  card.classList.add("none");

});

//forgot end 




// password see


let password = document.getElementById("passwrd");
let see = document.getElementById("iconSee");
let nosee = document.getElementById("noSee");

function checkInput() {
  if (password.value != "") {
    see.classList.remove("hidden");
  } else {
    see.classList.add("hidden");
  }
}

password.addEventListener("input", checkInput);

see.addEventListener("click", () => {
  if (password.type === "password") {
    password.type = "text";
    nosee.classList.remove("hidden");
    see.classList.add("hidden");
  } else {
    password.type = "password";
  }
});

nosee.addEventListener("click", () => {
  password.type = "password";
  nosee.classList.add("hidden");
  see.classList.remove("hidden");
});

checkInput();


//see end