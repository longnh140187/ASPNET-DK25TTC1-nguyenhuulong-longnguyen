// Recipe detail page interactions
(function () {
    const scrollToTopButton = document.getElementById("scrollToTop");
    if (scrollToTopButton) {
        scrollToTopButton.addEventListener("click", () => {
            window.scrollTo({top: 0, behavior: "smooth"});
        });
    }

    const observerOptions = {threshold: 0.1};
    const observer = new IntersectionObserver((entries) => {
        entries.forEach((entry) => {
            if (entry.isIntersecting) {
                entry.target.classList.add("opacity-100", "translate-y-0");
                entry.target.classList.remove("opacity-0", "translate-y-8");
            }
        });
    }, observerOptions);

    document.querySelectorAll(".recipe-detail section").forEach((el) => {
        el.classList.add("transition-all", "duration-700", "opacity-0", "translate-y-8");
        observer.observe(el);
    });
})();
