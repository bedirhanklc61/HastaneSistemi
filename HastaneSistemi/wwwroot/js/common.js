function applyTheme(mode) {
    if (mode === 'light') {
        document.body.classList.add('light-mode');
    } else {
        document.body.classList.remove('light-mode');
    }
}

function getThemeInputValue() {
    const temaInput = document.getElementById('TemaModuInput');
    return temaInput ? temaInput.value : null;
}

function setThemeInputValue(mode) {
    const temaInput = document.getElementById('TemaModuInput');
    if (temaInput) {
        temaInput.value = mode;
    }
}

function applyThemeFromInput() {
    const mode = getThemeInputValue();
    applyTheme(mode);
}