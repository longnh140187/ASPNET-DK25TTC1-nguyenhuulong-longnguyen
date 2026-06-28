(function () {
    const scrollToTopButton = document.getElementById("scrollToTop");
    if (scrollToTopButton) {
        scrollToTopButton.addEventListener("click", () => {
            window.scrollTo({ top: 0, behavior: "smooth" });
        });
    }

    const observerOptions = { threshold: 0.1 };
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

    document.getElementById("printRecipeBtn")?.addEventListener("click", () => {
        window.print();
    });

    const shareBtn = document.getElementById("shareRecipeBtn");
    const shareMenu = document.getElementById("shareMenu");
    const shareCopyLink = document.getElementById("shareCopyLink");
    const shareNativeBtn = document.getElementById("shareNativeBtn");
    const shareFeedback = document.getElementById("shareFeedback");

    const canNativeShare = typeof navigator.share === "function";

    if (canNativeShare && shareNativeBtn) {
        shareNativeBtn.classList.remove("hidden");
    }

    function showShareFeedback(message) {
        if (!shareFeedback) return;
        shareFeedback.textContent = message;
        shareFeedback.classList.remove("hidden");
        setTimeout(() => shareFeedback.classList.add("hidden"), 2500);
    }

    function closeShareMenu() {
        if (!shareMenu || !shareBtn) return;
        shareMenu.classList.add("hidden");
        shareBtn.setAttribute("aria-expanded", "false");
    }

    function openShareMenu() {
        if (!shareMenu || !shareBtn) return;
        shareMenu.classList.remove("hidden");
        shareBtn.setAttribute("aria-expanded", "true");
    }

    function toggleShareMenu() {
        if (!shareMenu) return;
        if (shareMenu.classList.contains("hidden")) {
            openShareMenu();
        } else {
            closeShareMenu();
        }
    }

    function getShareData() {
        return {
            title: shareBtn?.dataset.shareTitle || document.title,
            text: shareBtn?.dataset.shareText || "",
            url: shareBtn?.dataset.shareUrl || window.location.href
        };
    }

    async function copyShareLink() {
        const { url } = getShareData();
        try {
            if (navigator.clipboard?.writeText) {
                await navigator.clipboard.writeText(url);
            } else {
                const input = document.createElement("input");
                input.value = url;
                document.body.appendChild(input);
                input.select();
                document.execCommand("copy");
                document.body.removeChild(input);
            }
            showShareFeedback("Đã sao chép liên kết!");
        } catch {
            showShareFeedback("Không thể sao chép liên kết.");
        }
    }

    async function nativeShare() {
        const data = getShareData();
        try {
            await navigator.share({
                title: data.title,
                text: data.text,
                url: data.url
            });
            closeShareMenu();
        } catch (err) {
            if (err?.name !== "AbortError") {
                showShareFeedback("Không thể chia sẻ lúc này.");
            }
        }
    }

    shareBtn?.addEventListener("click", (event) => {
        event.stopPropagation();
        toggleShareMenu();
    });

    shareCopyLink?.addEventListener("click", async () => {
        await copyShareLink();
    });

    shareNativeBtn?.addEventListener("click", () => {
        nativeShare();
    });

    document.addEventListener("click", (event) => {
        if (!shareMenu || !shareBtn) return;
        if (shareMenu.contains(event.target) || shareBtn.contains(event.target)) return;
        closeShareMenu();
    });

    document.addEventListener("keydown", (event) => {
        if (event.key === "Escape") closeShareMenu();
    });
})();
