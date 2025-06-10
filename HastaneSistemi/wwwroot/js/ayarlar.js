// Şifre göster/gizle
const sifreToggle = document.getElementById('sifre-toggle');
const sifreInput = document.getElementById('sifre');

sifreToggle.addEventListener('click', () => {
    if (sifreInput.type === 'password') {
        sifreInput.type = 'text';
        sifreToggle.classList.remove('uil-eye');
        sifreToggle.classList.add('uil-eye-slash');
    } else {
        sifreInput.type = 'password';
        sifreToggle.classList.remove('uil-eye-slash');
        sifreToggle.classList.add('uil-eye');
    }
});

// Tema değişimi
const temaSwitch = document.getElementById('temaSwitch');
const temaInput = document.getElementById('TemaModuInput');

document.addEventListener('DOMContentLoaded', function () {
    if (temaInput && temaInput.value === 'light') {
        document.body.classList.add('light-mode');
        temaSwitch.checked = true;
    } else {
        document.body.classList.remove('light-mode');
        temaSwitch.checked = false;
    }
});

temaSwitch.addEventListener('change', function () {
    if (temaSwitch.checked) {
        document.body.classList.add('light-mode');
        temaInput.value = 'light';
    } else {
        document.body.classList.remove('light-mode');
        temaInput.value = 'dark';
    }
});
