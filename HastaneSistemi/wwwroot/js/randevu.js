// Global doktor listesi
let doktorVerileri = [];

// Poliklinik listesi
const poliklinikVerileri = [
    { ad: "Acil Tıp", ikon: "uil-ambulance" },
    { ad: "Aile Hekimliği", ikon: "uil-user-md" },
    { ad: "Anesteziyoloji ve Reanimasyon", ikon: "uil-clinic-medical" },
    { ad: "Beyin ve Sinir Cerrahisi", ikon: "uil-brain" },
    { ad: "Cildiye", ikon: "uil-swatchbook" },
    { ad: "Endokrinoloji ve Metabolizma Hastalıkları", ikon: "uil-flask" },
    { ad: "Enfeksiyon Hastalıkları", ikon: "uil-virus-slash" },
    { ad: "Fiziksel Tıp ve Rehabilitasyon", ikon: "uil-wheelchair" },
    { ad: "Gastroenteroloji", ikon: "uil-stethoscope" },
    { ad: "Genel Cerrahi", ikon: "uil-hospital" },
    { ad: "Genel Dahiliye", ikon: "uil-medical-drip" },
    { ad: "Girişimsel Radyoloji", ikon: "uil-flask" },
    { ad: "Göz Hastalıkları", ikon: "uil-eye" },
    { ad: "Göğüs Cerrahisi", ikon: "uil-stethoscope" },
    { ad: "Göğüs Hastalıkları", ikon: "uil-stethoscope" },
    { ad: "Hematoloji", ikon: "uil-syringe" },
    { ad: "Kadin Hastalıkları", ikon: "uil-user" },
    { ad: "Kalp ve Damar Cerrahisi", ikon: "uil-heart" },
    { ad: "Kardiyoloji", ikon: "uil-heartbeat" },
    { ad: "Kulak Burun Boğaz", ikon: "uil-headphones" },
    { ad: "Nefroloji", ikon: "uil-kid" },
    { ad: "Nöroloji", ikon: "uil-brain" },
    { ad: "Nükleer Tıp", ikon: "uil-atom" },
    { ad: "Ortopedi", ikon: "uil-wheelchair" },
    { ad: "Plastik, Rekonstrüktif ve Estetik Cerrahi", ikon: "uil-user-check" },
    { ad: "Radyasyon Onkolojisi", ikon: "uil-atom" },
    { ad: "Radyoloji", ikon: "uil-dna" },
    { ad: "Romatoloji", ikon: "uil-wheelchair" },
    { ad: "Ruh Sağlığı ve Hastalıkları", ikon: "uil-user" },
    { ad: "Tibbi Biyokimya", ikon: "uil-flask" },
    { ad: "Tibbi Genetik", ikon: "uil-dna" },
    { ad: "Tibbi Mikrobiyoloji", ikon: "uil-virus-slash" },
    { ad: "Tibbi Onkoloji", ikon: "uil-dna" },
    { ad: "Tibbi Patoloji", ikon: "uil-notes" },
    { ad: "Yeni Doğan", ikon: "uil-baby-carriage" },
    { ad: "Çocuk Acil", ikon: "uil-ambulance" },
    { ad: "Çocuk Cerrahisi", ikon: "uil-syringe" },
    { ad: "Çocuk Endokrinolojisi", ikon: "uil-baby-carriage" },
    { ad: "Çocuk Enfeksiyon ve Hastalıkları", ikon: "uil-virus-slash" },
    { ad: "Çocuk Gastroenterolojisi", ikon: "uil-stethoscope-alt" },
    { ad: "Çocuk Hematolojisi ve Onkolojisi", ikon: "uil-dna" },
    { ad: "Çocuk Kardiyolojisi", ikon: "uil-heart" },
    { ad: "Çocuk Nefrolojisi", ikon: "uil-kid" },
    { ad: "Çocuk Nörolojisi", ikon: "uil-brain" },
    { ad: "Çocuk Romatolojisi", ikon: "uil-wheelchair-alt" },
    { ad: "Çocuk Sağlığı ve Hastalıkları", ikon: "uil-kid" },
    { ad: "Çocuk ve Ergen Ruh Sağlığı", ikon: "uil-user" },
    { ad: "Çocuk İmmünolojisi ve Alerji Hastalıkları", ikon: "uil-syringe" },
    { ad: "Üroloji", ikon: "uil-medkit" },
    { ad: "İmmünoloji ve Alerji Hastalıkları", ikon: "uil-syringe" }
];

document.addEventListener('DOMContentLoaded', function () {
    const userIcon = document.getElementById('user-icon');
    const dropdownMenu = document.getElementById('dropdown-menu');

    // Sağ üst kullanıcı menüsü aç/kapa
    userIcon?.addEventListener('click', (e) => {
        e.stopPropagation();
        dropdownMenu.style.display = (dropdownMenu.style.display === 'block') ? 'none' : 'block';
    });

    document.addEventListener('click', (e) => {
        if (!userIcon?.contains(e.target) && !dropdownMenu?.contains(e.target)) {
            dropdownMenu.style.display = 'none';
        }
    });

    // Tema uygulaması
    const temaInput = document.getElementById("TemaModuInput");
    if (temaInput) {
        if (temaInput.value === "light") {
            document.body.classList.add("light-mode");
        } else {
            document.body.classList.remove("light-mode");
        }
    }
});

document.addEventListener('DOMContentLoaded', function () {
    const aramaKutusu = document.getElementById('aramaKutusu');

    if (aramaKutusu) {
        aramaKutusu.addEventListener('input', function () {
            const aranan = this.value.toLowerCase();
            const kartlar = document.querySelectorAll('.poliklinik-col');

            kartlar.forEach(kart => {
                const baslik = kart.querySelector('h5').textContent.toLowerCase();
                kart.style.display = baslik.includes(aranan) ? 'block' : 'none';
            });
        });
    }
});



// Sayfa yüklendiğinde doktorları al ve kartları oluştur
window.addEventListener("DOMContentLoaded", () => {
    fetch('/Hasta/DoktorlariGetir')
        .then(res => res.json())
        .then(data => {
            doktorVerileri = data;
            kartlariOlustur();
        });
});

function kartlariOlustur() {
    const poliklinikListesi = document.getElementById("poliklinik-listesi");
    const popup = document.getElementById("popup");

    poliklinikVerileri.forEach(p => {
        const col = document.createElement("div");
        col.className = "col-md-4 mb-4 poliklinik-col";

        const card = document.createElement("div");
        card.className = "poliklinik-card";

        card.innerHTML = `
            <button class="geri-btn"><i class="uil uil-arrow-left"></i></button>
            <i class="uil ${p.ikon}"></i>
            <h5>${p.ad}</h5>
            <form>
                <div class="form-group">
                    <label>Doktor Seçiniz</label>
                    <select class="form-control doktorSec" required>
                        <option value="">Doktor Seçiniz</option>
                    </select>
                </div>
                <div class="form-group">
                    <label>Tarih</label>
                    <input type="date" class="form-control" required>
                </div>
                <div class="form-group">
                    <label>Saat</label>
                    <select class="form-control saatSec" required>
                        <option value="">Saat Seçiniz</option>
                    </select>
                </div>
                <button type="submit" class="btn btn-primary btn-block">Randevuyu Oluştur</button>
            </form>
        `;

        col.appendChild(card);
        poliklinikListesi.appendChild(col);

        const doktorSelect = card.querySelector(".doktorSec");
        const ilgiliDoktorlar = doktorVerileri.filter(d => d.bolum.toLowerCase().includes(p.ad.toLowerCase()));

        ilgiliDoktorlar.forEach(doktor => {
            const option = document.createElement("option");
            option.value = doktor.doktorID;
            option.textContent = doktor.adSoyad;
            doktorSelect.appendChild(option);
        });

        const form = card.querySelector("form");
        const geriBtn = card.querySelector(".geri-btn");

        doktorSelect.addEventListener("change", () => {
            const doktorID = doktorSelect.value;
            const tarihInput = form.querySelector('input[type="date"]');
            const saatSelect = form.querySelector('.saatSec');

            tarihInput.disabled = true;
            saatSelect.disabled = true;
            tarihInput.value = "";
            saatSelect.innerHTML = "";

            if (!doktorID) return;

            fetch(`/Hasta/GetDoktorUygunluk?doktorID=${doktorID}`)
                .then(res => res.json())
                .then(uygunluk => {
                    if (!uygunluk) return;

                    const baslangic = new Date(uygunluk.baslangicTarih);
                    const bitis = new Date(uygunluk.bitisTarih);
                    const bugun = new Date();
                    bugun.setHours(0, 0, 0, 0);

                    let minGecerliTarih = new Date(baslangic);
                    let tarihListesi = [];

                    while (minGecerliTarih <= bitis) {
                        const dateStr = minGecerliTarih.toISOString().split("T")[0];
                        const saat17 = new Date(minGecerliTarih);
                        saat17.setHours(17, 0, 0, 0);

                        if (
                            minGecerliTarih.toDateString() === new Date().toDateString() &&
                            new Date() > saat17
                        ) {
                            // bugünün 17:00 sonrası ➜ geçersiz
                        } else if (minGecerliTarih < bugun) {
                            // geçmiş gün ➜ geçersiz
                        } else {
                            tarihListesi.push(dateStr);
                        }

                        minGecerliTarih.setDate(minGecerliTarih.getDate() + 1);
                    }

                    if (tarihListesi.length === 0) {
                        tarihInput.innerHTML = "<option>Uygun gün kalmadı</option>";
                        tarihInput.disabled = true;
                        return;
                    }

                    tarihInput.min = tarihListesi[0];
                    tarihInput.max = tarihListesi[tarihListesi.length - 1];
                    tarihInput.disabled = false;

                    tarihInput.addEventListener("change", () => {
                        const secilenTarih = tarihInput.value;
                        saatSelect.innerHTML = "";
                        saatSelect.disabled = true;

                        if (!secilenTarih) return;

                        fetch(`/Hasta/DoluSaatleriGetir?doktorID=${doktorID}&tarih=${secilenTarih}`)
                            .then(res => res.json())
                            .then(doluSaatler => {
                                const start = uygunluk.baslangicSaat.split(":").map(Number);
                                const end = uygunluk.bitisSaat.split(":").map(Number);

                                let saat = start[0];
                                let dakika = start[1];
                                const saatBit = end[0] * 60 + end[1];

                                const bugunStr = new Date().toISOString().split("T")[0];
                                const bugunSaat = new Date();
                                const isToday = secilenTarih === bugunStr;

                                saatSelect.innerHTML = '<option value="">Saat Seçiniz</option>';

                                while ((saat * 60 + dakika) < saatBit) {
                                    const toplamDakika = saat * 60 + dakika;

                                    // 12:00–13:30 öğle arası
                                    if (toplamDakika >= 720 && toplamDakika < 810) {
                                        dakika += 15;
                                        if (dakika >= 60) { dakika = 0; saat++; }
                                        continue;
                                    }

                                    const hh = String(saat).padStart(2, '0');
                                    const mm = String(dakika).padStart(2, '0');
                                    const timeStr = `${hh}:${mm}`;

                                    // Eğer bugünün geçmiş saatleri ise listeleme
                                    if (
                                        isToday &&
                                        parseInt(hh + mm) <= parseInt(
                                            bugunSaat.getHours().toString().padStart(2, '0') +
                                            bugunSaat.getMinutes().toString().padStart(2, '0')
                                        )
                                    ) {
                                        dakika += 15;
                                        if (dakika >= 60) { dakika = 0; saat++; }
                                        continue;
                                    }

                                    const opt = document.createElement("option");
                                    opt.value = timeStr;

                                    if (doluSaatler.includes(timeStr)) {
                                        opt.disabled = true;
                                        opt.textContent = `${timeStr} (Dolu)`;
                                    } else {
                                        opt.textContent = timeStr;
                                    }

                                    saatSelect.appendChild(opt);

                                    dakika += 15;
                                    if (dakika >= 60) {
                                        dakika = 0;
                                        saat++;
                                    }
                                }

                                if (saatSelect.children.length === 1) {
                                    saatSelect.innerHTML = '<option>Uygun saat bulunamadı</option>';
                                    saatSelect.disabled = true;
                                } else {
                                    saatSelect.disabled = false;
                                }
                            });

                    });
                });
        });



        form.addEventListener("submit", (e) => {
            e.preventDefault();

            const doktorID = doktorSelect.value;
            const tarih = form.querySelector('input[type="date"]').value;
            const saat = form.querySelector('.saatSec').value; // ✅ select'ten alıyoruz
            const bolum = card.querySelector("h5").textContent;

            if (!doktorID || !tarih || !saat) {
                alert("Lütfen tüm alanları doldurunuz.");
                return;
            }

            fetch('/Hasta/RandevuOlustur', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    bolum: bolum,
                    doktorID: doktorID,
                    tarih: tarih,
                    saat: saat
                })
            })
                .then(res => {
                    if (res.ok) {
                        popup.classList.add("show");
                        setTimeout(() => {
                            popup.classList.remove("show");
                            window.location.href = "/Hasta/HastaPanel"; // ✅ başarılıysa yönlendir
                        }, 2000);
                    } else {
                        alert("❌ Randevu oluşturulamadı.");
                    }
                })
                .catch(err => {
                    console.error("Hata:", err);
                });
        });


        card.addEventListener("click", (e) => {
            if (!card.classList.contains("expanded") && !e.target.closest(".geri-btn")) {
                const rect = card.getBoundingClientRect();
                const scrollTop = window.scrollY || document.documentElement.scrollTop;
                const scrollLeft = window.scrollX || document.documentElement.scrollLeft;

                const x = rect.left + rect.width / 2;
                const y = rect.top + rect.height / 2;
                const centerX = window.innerWidth / 2;
                const centerY = window.innerHeight / 2;
                const dx = centerX - x;
                const dy = centerY - y;

                card.style.position = 'fixed';
                card.style.top = `${y + scrollTop - rect.height / 2}px`;
                card.style.left = `${x + scrollLeft - rect.width / 2}px`;
                card.style.width = `${rect.width}px`;
                card.style.height = `${rect.height}px`;
                card.style.zIndex = '1050';
                card.classList.add("moving");

                setTimeout(() => {
                    card.style.transform = `translate(${dx}px, ${dy}px) scale(1.05)`;
                }, 10);

                document.querySelectorAll(".poliklinik-card").forEach(c => {
                    if (c !== card) c.classList.add("blurred");
                });

                setTimeout(() => {
                    card.classList.remove("moving");
                    card.classList.add("expanded");
                    card.style.top = '50%';
                    card.style.left = '50%';
                    card.style.transform = 'translate(-50%, -50%)';
                    card.style.width = '90%';
                    card.style.maxWidth = '600px';
                    card.style.height = 'auto';
                    geriBtn.style.display = "block";
                    setTimeout(() => card.classList.add("show-form"), 200);
                }, 650);
            }
        });

        geriBtn.addEventListener("click", (e) => {
            e.stopPropagation();
            card.classList.remove("show-form");
            setTimeout(() => {
                card.classList.remove("expanded", "moving");
                card.removeAttribute("style");
                document.querySelectorAll(".poliklinik-card").forEach(c => c.classList.remove("blurred"));
                geriBtn.style.display = "none";
            }, 300);
        });
    });

    




}
