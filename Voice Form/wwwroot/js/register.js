const contract = document.getElementById("contractPop");
const popup = document.querySelector(".contract") ;
contract.addEventListener("click", (event) => {
    event.preventDefault();
    popup.classList.remove("none");
  
  });