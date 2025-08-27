/**
 * Bootstrap tablosu için client-side pagination
 * @param {Array} data - Tabloya basılacak veri listesi (JSON array)
 * @param {Array} props - Sütun adları (string array)
 * @param {string} tableId - Tablo elementinin id değeri
 * @param {string} paginationId - Sayfalama container'ının id değeri
 * @param {number} rowsPerPage - Her sayfada gösterilecek satır sayısı
 */
function initTablePagination(data, paginationId, rowsPerPage = 5) {

    function displayTable(page) {
        const tbody = document.querySelector(`#${tableId} tbody`);
        tbody.innerHTML = "";

        const startIndex = (page - 1) * rowsPerPage;
        const endIndex = startIndex + rowsPerPage;
        const slicedData = data.slice(startIndex, endIndex);

        slicedData.forEach(item => {
            const tr = document.createElement("tr");
            props.forEach(propName => {
                const td = document.createElement("td");
                td.textContent = item[propName] ?? ""; // Eğer property yoksa boş string
                tr.appendChild(td);
            });
            tbody.appendChild(tr);
        });
        updatePagination(page);
    }

    function updatePagination(page) {
        const pageCount = Math.ceil(data.length / rowsPerPage);
        const paginationContainer = document.getElementById(paginationId);
        paginationContainer.innerHTML = "";

        const nav = document.createElement("nav");
        nav.setAttribute("aria-label", "Page navigation");

        const ul = document.createElement("ul");
        ul.classList.add("pagination", "justify-content-center");

        // Previous
        const prevLi = document.createElement("li");
        prevLi.classList.add("page-item");
        if (page === 1) prevLi.classList.add("disabled");
        const prevLink = document.createElement("a");
        prevLink.classList.add("page-link");
        prevLink.href = "#";
        prevLink.innerText = "Önceki";
        prevLink.onclick = function (e) {
            e.preventDefault();
            if (page > 1) displayTable(page - 1);
        };
        prevLi.appendChild(prevLink);
        ul.appendChild(prevLi);

        // Sayfa numaraları
        for (let i = 1; i <= pageCount; i++) {
            const li = document.createElement("li");
            li.classList.add("page-item");
            if (i === page) li.classList.add("active");
            const a = document.createElement("a");
            a.classList.add("page-link");
            a.href = "#";
            a.innerText = i;
            a.onclick = function (e) {
                e.preventDefault();
                displayTable(i);
            };
            li.appendChild(a);
            ul.appendChild(li);
        }

        // Next
        const nextLi = document.createElement("li");
        nextLi.classList.add("page-item");
        if (page === pageCount) nextLi.classList.add("disabled");
        const nextLink = document.createElement("a");
        nextLink.classList.add("page-link");
        nextLink.href = "#";
        nextLink.innerText = "Sonraki";
        nextLink.onclick = function (e) {
            e.preventDefault();
            if (page < pageCount) displayTable(page + 1);
        };
        nextLi.appendChild(nextLink);
        ul.appendChild(nextLi);

        nav.appendChild(ul);
        paginationContainer.appendChild(nav);
    }

    // Başlat
    displayTable(1);
}
