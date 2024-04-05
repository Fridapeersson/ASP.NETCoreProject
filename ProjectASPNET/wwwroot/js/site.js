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


//changing symbol based on if course is saved or not
document.addEventListener("DOMContentLoaded", function () {
    var bookmarks = document.querySelectorAll(".bookmark");
    bookmarks.forEach(function (bookmark) {
        var isSaved = bookmark.getAttribute("data-saved") == "True";
        bookmark.setAttribute("title", isSaved ? "Remove" : "Add");
    });
});



// category dropdown
document.addEventListener("DOMContentLoaded", () => {
    Select()
    SearchQuery()
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

            updateCoursesByFilter(category, searchQuery);
        })
    }
    catch {
        console.error('Error in SearchQuery function:', error);
        }
}

//function UpdateCourses() {
//    const category = document.querySelector(".select ")
//}

function updateCoursesByFilter(category, searchValue = "") {
    const query = searchValue || "";
    console.log(`Category: ${category}, SearchQuery: ${searchValue}`);
    fetch(`/courses?category=${encodeURIComponent(category)}&searchQuery=${encodeURIComponent(query)}`)
        .then(res => res.text())
        .then(data => {
            const parser = new DOMParser();
            const dom = parser.parseFromString(data, "text/html");
            document.querySelector("#boxes").innerHTML = dom.querySelector("#boxes").innerHTML;
        })
        .catch(error => console.error('Error:', error));
}
