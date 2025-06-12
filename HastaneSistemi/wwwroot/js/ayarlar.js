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

document.addEventListener('DOMContentLoaded', function () {
    applyThemeFromInput();
    if (temaSwitch) {
        temaSwitch.checked = getThemeInputValue() === 'light';
    }
});

if (temaSwitch) {
    temaSwitch.addEventListener('change', function () {
        const mode = temaSwitch.checked ? 'light' : 'dark';
        applyTheme(mode);
        setThemeInputValue(mode);
    });
}

// Yazı büyüklüğü değişimi
const yaziBuyukSwitch = document.getElementById('yaziBuyukSwitch');

if (yaziBuyukSwitch) {
    yaziBuyukSwitch.addEventListener('change', function () {
        if (yaziBuyukSwitch.checked) {
            document.body.classList.add('large-text');
        } else {
            document.body.classList.remove('large-text');
        }
    });
}
