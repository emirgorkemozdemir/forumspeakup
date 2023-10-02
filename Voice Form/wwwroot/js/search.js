// search 
let search = document.getElementById("search");
let mobileSearch = document.getElementById("Mobilesearch");
let overlaySearch = document.querySelector(".overlaySearch");
let searchClose = document.getElementById("searchClose");

search.addEventListener("click", openSearch)
mobileSearch.addEventListener("click", openSearch)


searchClose.addEventListener("click" , ()=>{
  overlaySearch.classList.add("visible");
  document.body.classList.remove("hidden");
});


function openSearch(){
  overlaySearch.classList.remove("visible");
  document.body.classList.add("hidden");
}

//search end


