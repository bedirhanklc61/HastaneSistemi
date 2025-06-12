
const csrfToken = document.querySelector('meta[name="csrf-token"]').getAttribute('content');
// Arama filtresi
document.getElementById('search-input').addEventListener('keyup', function () {
    const filter = this.value.toLowerCase();
    document.querySelectorAll('.randevu-card').forEach(card => {
        const patient = card.getAttribute('data-patient').toLowerCase();
        const time = card.getAttribute('data-time').toLowerCase();
        const date = card.getAttribute('data-date').toLowerCase();
        card.style.display = (patient.includes(filter) || time.includes(filter) || date.includes(filter)) ? '' : 'none';
    });
});

// Buton eventleri
function attachListeners() {
    document.querySelectorAll('.cancel').forEach(btn => {
        btn.addEventListener('click', function () {
            const id = this.dataset.id;
            const card = this.closest('.randevu-card');
            fetch('/Doktor/RandevuIptalEt', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json', 'RequestVerificationToken': csrfToken },
                body: JSON.stringify({ randevuID: id })
            }).then(res => {
                if (res.ok) card.remove();
            });
        });
    });

    document.querySelectorAll('.view-details').forEach(btn => {
        btn.addEventListener('click', function () {
            const card = this.closest('.randevu-card');
            const hasta = card.dataset.patient;
            const tarih = card.dataset.date;
            const saat = card.dataset.time;
            document.getElementById('patient-details').textContent = `
                        Hasta Adı: ${hasta}
                        Tarih: ${tarih}
                        Saat: ${saat}
                    `;
            $('#detailsModal').modal('show');
        });
    });

    document.querySelectorAll('.cancel').forEach(btn => {
        btn.addEventListener('click', function () {
            const card = this.closest('.randevu-card');
            const randevuId = this.dataset.id;

            fetch('/Doktor/RandevuIptalEt', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json', 'RequestVerificationToken': csrfToken },
                body: JSON.stringify(randevuId)
            })
                .then(res => {
                    if (res.ok) {
                        card.remove();
                        const kalan = document.querySelectorAll('.randevu-card').length;
                        if (kalan === 0) {
                            const noEl = document.getElementById('no-appointments');
                            if (noEl) noEl.style.display = 'block';
                        }
                        location.reload();
                    } else {
                        alert("❌ Randevu iptal edilemedi.");
                    }
                });
        });
    });

}

attachListeners();

document.addEventListener('DOMContentLoaded', function () {
    applyThemeFromInput();
    const userIcon = document.getElementById('user-icon');
    const dropdownMenu = document.getElementById('dropdown-menu');

    userIcon?.addEventListener('click', (e) => {
        e.stopPropagation();
        dropdownMenu.style.display = (dropdownMenu.style.display === 'block') ? 'none' : 'block';
    });

    document.addEventListener('click', (e) => {
        if (!userIcon?.contains(e.target) && !dropdownMenu?.contains(e.target)) {
            dropdownMenu.style.display = 'none';
        }
    });
});