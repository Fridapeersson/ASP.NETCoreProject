const toggleMenu = () => {
    const container = document.querySelector(".container");
    const menuOpened = container.classList.contains("toggle-mobile-menu");

    document.querySelector("#btn-switch").classList.toggle("hide");
    document.querySelector(".nav").classList.toggle("hide");
    document.querySelector(".account-buttons").classList.toggle("hide");

    //changing bars-icon to X-icon
    document.querySelector("#mobile-icon").classList.toggle("fa-bars");
    document.querySelector("#mobile-icon").classList.toggle("fa-x");

    container.classList.toggle("toggle-mobile-menu");
    container.setAttribute("aria-expanded", !menuOpened);

}

const checkScreenSize = () => {
    if (window.innerWidth >= 992) {
        if (document.querySelector(".container").classList.contains("toggle-mobile-menu")) {
            toggleMenu();
        }
    }
} 

window.addEventListener("resize", checkScreenSize);


document.addEventListener("DOMContentLoaded", function () {
    let switchBtn = document.querySelector("#switch-mode")

    switchBtn.addEventListener("change", function () {
        let theme = this.checked ? "dark" : "light"


        //AJAX - renderar inte om sidan
        fetch(`/settings/changetheme?theme=${theme}`)
            .then(res => {
                if (res.ok) {
                    window.location.reload();

                }
                else {
                    console.log("Wrong, try again");
                }
            })
        console.log(theme);
    })
})

//Ändra symbol baserat på om kurs är sparad eller inte
document.addEventListener("DOMContentLoaded", function () {
    var bookmarks = document.querySelectorAll(".bookmark");
    bookmarks.forEach(function (bookmark) {
        var isSaved = bookmark.getAttribute("data-saved") == "True";
        bookmark.setAttribute("title", isSaved ? "Remove" : "Add");
    });
});



// category dropdown
document.addEventListener("DOMContentLoaded", () => {
    Select();
    SearchQuery();
    //SetupPagination();
});

function Select() {
    try {
        const select = document.querySelector(".select");
        console.log(select);
        const selected = select.querySelector(".selected");
        const selectOptions = select.querySelector(".select-options");

        selected.addEventListener("click", () => {
            selectOptions.classList.toggle("show");
        });

        select.querySelectorAll(".option").forEach(option => {
            option.addEventListener("click", () => {
                selected.textContent = option.textContent;
                selected.setAttribute('data-value', option.getAttribute('data-value'));
                selectOptions.classList.remove("show");

                let category = option.getAttribute("data-value");
                let searchValue = document.querySelector("#searchQuery").value || "";
                updateCoursesByFilter(category, searchValue);
            });
        });

        window.addEventListener("click", (e) => {
            if (!select.contains(e.target)) {
                selectOptions.classList.remove("show");
            }
        });
    }
    catch { }

}

function SearchQuery() {
    try {
        document.querySelector(".searchQuery").addEventListener("keyup", function(event) {
            const searchQuery = event.target.value || "";
            const category = document.querySelector(".select .selected").getAttribute("data-value") || "all";
            //console.log(`SEARCHQUERY: ${searchQuery}`);
            //console.log(`CATEGORY: ${category}`);
            updateCoursesByFilter(category, searchQuery);
        })
    }
    catch { }
}

function updateCoursesByFilter(category, searchValue = "") {
    const query = searchValue || "";
    //console.log(`CATEGORY: ${category}, SEARCHQUERY: ${searchValue}`);
    fetch(`/courses?category=${encodeURIComponent(category)}&searchQuery=${encodeURIComponent(query)}`)
        .then(res => res.text())
        .then(data => {
            const parser = new DOMParser();
            const dom = parser.parseFromString(data, "text/html");

            const newBoxesContent = dom.querySelector("#boxes");

            if (newBoxesContent) {
                document.querySelector("#boxes").innerHTML = newBoxesContent.innerHTML;
            }

            const newPaginationContent = dom.querySelector(".pagination");
            const pagination = document.querySelector(".pagination");
            if (pagination) {
                pagination.innerHTML = newPaginationContent ? newPaginationContent.innerHTML : "";
            }
        })
}

//document.addEventListener("DOMContentLoaded", function () {
//    var messages = document.querySelectorAll('.alert-message');
//    messages.forEach(function (message) {
//        console.log(message);

//        setTimeout(function () {
//            message.style.display = 'none';
//        }, 5000); 
//    });
//});