(function () {
    const scrollToTopButton = document.getElementById("scrollToTop");
    if (!scrollToTopButton) return;

    scrollToTopButton.addEventListener("click", function () {
        window.scrollTo({ top: 0, behavior: "smooth" });
    });
})();
