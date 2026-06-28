(function () {
    document.getElementById("printBlogBtn")?.addEventListener("click", () => {
        window.print();
    });

    const shareBtn = document.getElementById("shareBlogBtn");
    const shareMenu = document.getElementById("shareMenu");
    const shareCopyLink = document.getElementById("shareCopyLink");
    const shareFeedback = document.getElementById("shareFeedback");

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
        if (shareMenu.classList.contains("hidden")) openShareMenu();
        else closeShareMenu();
    }

    async function copyShareLink() {
        const url = shareBtn?.dataset.shareUrl || window.location.href;
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

    shareBtn?.addEventListener("click", (event) => {
        event.stopPropagation();
        toggleShareMenu();
    });

    shareCopyLink?.addEventListener("click", () => {
        copyShareLink();
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
