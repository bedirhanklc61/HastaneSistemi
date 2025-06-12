const csrfToken = document.querySelector('meta[name="csrf-token"]').getAttribute('content');
document.addEventListener('DOMContentLoaded', function () {
    const userIcon = document.getElementById('user-icon');
    const dropdownMenu = document.getElementById('dropdown-menu');

    // Menü aç/kapa
    userIcon.addEventListener('click', (e) => {
        e.stopPropagation();
        dropdownMenu.style.display = (dropdownMenu.style.display === 'block') ? 'none' : 'block';
    });

    document.addEventListener('click', (e) => {
        if (!userIcon.contains(e.target) && !dropdownMenu.contains(e.target)) {
            dropdownMenu.style.display = 'none';
        }
    });

    // Tema uygulama (veritabanından gelen input'a göre)
    const temaInput = document.getElementById("TemaModuInput");
    if (temaInput) {
        if (temaInput.value === "light") {
            document.body.classList.add("light-mode");
        } else {
            document.body.classList.remove("light-mode");
        }
    }

    // Randevu listeleme işlemleri
    const randevuListesi = document.getElementById("randevu-listesi");

    function randevuHTML(r, tip) {
        return `
    <div class="card randevu-box mb-3 ${tip === "gecmis" ? "gecmis" : ""}" data-id="${r.randevuID}">
        <div class="card-body">
            <h5 class="card-title">📅 ${new Date(r.tarih).toLocaleDateString()}</h5>
            <p class="card-text">🏥 ${r.bolum} - Dr. ${r.doktorAd}</p>
            <p class="card-text">🕒 Saat: ${r.saat}</p>
            ${tip === "tum" ? '<button class="btn btn-danger btn-sm mt-2 iptal-btn">Randevuyu İptal Et</button>' : ''}
        </div>
    </div>
    `;
    }


    function aktiflestirButonlar() {
        document.querySelectorAll(".iptal-btn").forEach(button => {
            button.addEventListener("click", function () {
                const kart = this.closest(".randevu-box");
                const id = kart.getAttribute("data-id");


                if (confirm("Randevuyu iptal etmek istediğinize emin misiniz?")) {
                    fetch("/Hasta/RandevuIptalEt", {
                        method: "POST",
                        headers: {
                            'Content-Type': 'application/json',
                            'RequestVerificationToken': csrfToken
                        },
                        body: JSON.stringify({ randevuID: id })
                    })
                        .then(res => {
                            if (res.ok) {
                                kart.remove();
                            } else {
                                alert("Randevu silinemedi.");
                            }
                        });
                }
            });
        });
    }


    function filtrele(tip) {
        fetch("/Hasta/RandevulariGetir")
            .then(res => res.json())
            .then(data => {
                const simdi = new Date();

                let randevular = data.map(r => {
                    const [hour, minute] = r.saat.split(":").map(Number);
                    const tarihSaat = new Date(r.tarih);
                    tarihSaat.setHours(hour, minute, 0, 0);
                    return { ...r, tarihSaat };
                });

                let filtreli = [];
                if (tip === "yaklasan") {
                    filtreli = randevular
                        .filter(r => r.tarihSaat >= simdi)
                        .sort((a, b) => a.tarihSaat - b.tarihSaat)
                        .slice(0, 1);
                } else if (tip === "gecmis") {
                    filtreli = randevular
                        .filter(r => r.tarihSaat < simdi)
                        .sort((a, b) => b.tarihSaat - a.tarihSaat);
                } else if (tip === "tum") {
                    filtreli = randevular
                        .filter(r => r.tarihSaat >= simdi)
                        .sort((a, b) => a.tarihSaat - b.tarihSaat);
                }

                document.querySelectorAll(".btn-group button").forEach(btn => btn.classList.remove("active"));
                const aktifBtn = document.getElementById("btn-" + tip);
                if (aktifBtn) aktifBtn.classList.add("active");

                if (filtreli.length === 0) {
                    randevuListesi.innerHTML = `<p class="text-center mt-4">📋 Gösterilecek randevu bulunamadı.</p>`;
                } else {
                    randevuListesi.innerHTML = filtreli.map(r => randevuHTML(r, tip)).join("");
                    aktiflestirButonlar();
                }
            });
    }


        

    document.getElementById("btn-yaklasan").addEventListener("click", () => filtrele("yaklasan"));
    document.getElementById("btn-tum").addEventListener("click", () => filtrele("tum"));
    document.getElementById("btn-gecmis").addEventListener("click", () => filtrele("gecmis"));

    // Sayfa ilk açıldığında yaklaşanları göster
    filtrele("yaklasan");
});
