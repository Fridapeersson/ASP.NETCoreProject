document.addEventListener("DOMContentLoaded", function () {
    var bookmarks = document.querySelectorAll(".bookmark");
    bookmarks.forEach(function (bookmark) {
        var isSaved = bookmark.getAttribute("data-saved") == "True";
        bookmark.setAttribute("title", isSaved ? "Remove" : "Add");
    });
});