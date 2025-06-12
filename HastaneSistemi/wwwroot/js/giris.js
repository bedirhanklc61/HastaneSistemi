
const toggleBtn = document.getElementById('theme-toggle');



function typeWriter(element, text, delay = 100) {
  let i = 0;
  function type() {
    if (i < text.length) {
      element.setAttribute("placeholder", text.substring(0, i + 1));
      i++;
      setTimeout(type, delay);
    }
  }
  type();
}

window.addEventListener("DOMContentLoaded", () => {
  const emailInput = document.getElementById("email");
  if (emailInput) {
    typeWriter(emailInput, "Email adresinizi giriniz...", 80);
  }
});


document.getElementById("accessibility-toggle").addEventListener("click", function () {
  document.body.classList.toggle("accessible-mode");
});

