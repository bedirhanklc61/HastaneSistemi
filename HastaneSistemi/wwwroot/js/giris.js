
const toggleBtn = document.getElementById('theme-toggle');
const currentTheme = localStorage.getItem('theme');

if (currentTheme === 'light') {
  document.body.classList.add('light-mode');
  toggleBtn.textContent = '🌙';
} else {
  toggleBtn.textContent = '☀️';
}

toggleBtn.addEventListener('click', () => {
  document.body.classList.toggle('light-mode');
  const isLight = document.body.classList.contains('light-mode');
  toggleBtn.textContent = isLight ? '🌙' : '☀️';
  localStorage.setItem('theme', isLight ? 'light' : 'dark');
});


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

